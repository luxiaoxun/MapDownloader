using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Threading.Tasks;

namespace GMapExport
{
    public partial class ArcGISTileToBundleForm : Form
    {
        public ArcGISTileToBundleForm()
        {
            InitializeComponent();

            this.backgroundWorker1.WorkerReportsProgress = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogSrc.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialogSrc.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogDest.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialogDest.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先设置切片所在目录！", "提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("请先设置Bundle输出目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                button3.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void TilesConvert2Bundles()
        {
            StreamWriter log = new StreamWriter("ArcGISTileToBundle_log.txt",true);
            log.WriteLine(DateTime.Now + ":Begin to convert to bundles.");
            log.Flush();
            DirectoryInfo srcDir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] levelDirs=srcDir.GetDirectories("L*");
            int levelLength = levelDirs.Length;
            if (levelLength== 0)
            {
                string msg = srcDir.FullName+ "目录中找不到'L'开头的子目录！";
                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //输出目录
            string destDir = textBox2.Text + "\\Layers\\_alllayer";
            for (int i=0; i<levelLength; ++i)
            {
                DirectoryInfo levelDir = levelDirs[i];
                log.WriteLine(DateTime.Now + ":Begin to convert " + levelDir.FullName);
                log.Flush();
                IEnumerable<DirectoryInfo> rowDirs = levelDir.EnumerateDirectories("R*");
                Dictionary<long, string[]> rowBlock = new Dictionary<long, string[]>();
                Dictionary<long, string[]> colBlock = new Dictionary<long, string[]>();
                log.WriteLine(DateTime.Now + ":Begin to get block by row and column");
                log.Flush();

                #region get block by row and column
                //DirectoryInfo dir = null;
                foreach(DirectoryInfo rowDir in rowDirs)
                {
                    //获取行块
                    long name = Convert.ToInt64(rowDir.Name.Substring(1),16);
                    long rowBlockKey = (name / 128)*128;
                    int index = (int)(name % 128);
                    if (!rowBlock.ContainsKey(rowBlockKey))
                    {
                        rowBlock.Add(rowBlockKey, new string[128]);
                    }
                    rowBlock[rowBlockKey][index] = rowDir.FullName;
                    //获取列块
                    IEnumerable<FileInfo> pngfiles = rowDir.EnumerateFiles("*.png");
                    foreach (FileInfo pngfile in pngfiles)
                    {
                        int len = pngfile.Name.Length;
                        name = Convert.ToInt64(pngfile.Name.Substring(1, len - 5), 16);
                        long colBlockKey = (name / 128) * 128;
                        index = (int)(name % 128);
                        if (!colBlock.ContainsKey(colBlockKey))
                        {
                            colBlock.Add(colBlockKey, new string[128]);
                        }
                        colBlock[colBlockKey][index] = pngfile.Name;
                    }
                    //dir = rowDir;
                }
                //获取列块
                //IEnumerable<FileInfo> pngfiles = dir.EnumerateFiles("*.png");
                //foreach (FileInfo pngfile in pngfiles)
                //{
                //    int len = pngfile.Name.Length;
                //    long name = Convert.ToInt64(pngfile.Name.Substring(1, len - 5), 16);
                //    long colBlockKey = (name / 128) * 128;
                //    int index = (int)(name % 128);
                //    if (!colBlock.ContainsKey(colBlockKey))
                //    {
                //        colBlock.Add(colBlockKey, new string[128]);
                //    }
                //    colBlock[colBlockKey][index] = pngfile.Name;
                //}

                log.WriteLine(DateTime.Now + ":Get block by row and column completed.");
                log.Flush();

                #endregion

                //创建输出级别目录
                DirectoryInfo destLevelDir = new DirectoryInfo(destDir + "\\" + levelDir.Name);
                if (!destLevelDir.Exists)
                {
                    destLevelDir.Create();
                }
                log.WriteLine(DateTime.Now + ":Begin to process by Parallel.ForEach.");
                log.Flush();
                //并行计算逻辑
                Parallel.ForEach(colBlock.Keys, (key) =>
                {
                    log.WriteLine(DateTime.Now + ":Begin to process block on column:" + String.Format("C{0:x4}", key));
                    log.Flush();
                    //按Bundle块大小128*128进行转换输出
                    foreach (long rowkey in rowBlock.Keys)
                    {
                        //判断该Bundlx,Bundle是否已经存在，如果不存在，则继续执行，否则都存在，且Bundlx的字节数为81952，则跳过此次合并
                        FileInfo bdx = new FileInfo(destLevelDir + "\\" + String.Format("R{0:x4}C{1:x4}.bundlx", new object[] { rowkey, key }));
                        FileInfo bdl = new FileInfo(destLevelDir + "\\" + String.Format("R{0:x4}C{1:x4}.bundle", new object[] { rowkey, key }));
                        if (bdx.Exists && bdx.Length == 81952&&bdl.Exists)
                        {
                            log.WriteLine(DateTime.Now + ": bundle:" + destLevelDir + "\\" + String.Format("R{0:x4}C{1:x4}.bundle is exists.", new object[] { rowkey, key }));
                            log.Flush();
                            continue;
                        }

                        FileStream bundlx = new FileStream(destLevelDir + "\\" + String.Format("R{0:x4}C{1:x4}.bundlx", new object[] { rowkey, key }), FileMode.Create);
                        byte[] bundlxbytes = createEmptyBundlx();
                        FileStream bundle = new FileStream(destLevelDir + "\\" + String.Format("R{0:x4}C{1:x4}.bundle", new object[] { rowkey, key }), FileMode.Create);
                        BufferedStream bufbundle = new BufferedStream(bundle, 10485760);//构造缓冲区为10MB大小的数据流
                        byte[] bundleHeader = createBundleHeader((int)rowkey,(int)key);
                        bufbundle.Write(bundleHeader, 0, 0x01003C);
                        int bundleSize = bundleHeader.Length;
                        string[] filenames = colBlock[key];
                        string[] rowdirs=rowBlock[rowkey];
                        int bundlxindex = 16;
                        long bundlxEmptyValue = 0x3C-4;//空数据情况下索引中的值
                        long bundlxNoEmptyValue = 0x1003C;//有数据情况下索引中的值
                        int tileNum = 0;
                        int firstsize = 0;
                        foreach(string filename in filenames)
                        {
                            foreach (string rowdir in rowdirs)
                            {
                                bundlxEmptyValue += 4;
                                //没有切片的情况
                                if (filename == null || rowdir == null)
                                {
                                    byte[] valuebytes = BitConverter.GetBytes(bundlxEmptyValue);
                                    bundlxbytes[bundlxindex++] = valuebytes[0];
                                    bundlxbytes[bundlxindex++] = valuebytes[1];
                                    bundlxbytes[bundlxindex++] = valuebytes[2];
                                    bundlxbytes[bundlxindex++] = valuebytes[3];
                                    bundlxbytes[bundlxindex++] = valuebytes[4];
                                    continue;
                                }
                                FileInfo tile = new FileInfo(rowdir + "\\" + filename);
                                if(!tile.Exists)
                                {
                                    byte[] valuebytes = BitConverter.GetBytes(bundlxEmptyValue);
                                    bundlxbytes[bundlxindex++] = valuebytes[0];
                                    bundlxbytes[bundlxindex++] = valuebytes[1];
                                    bundlxbytes[bundlxindex++] = valuebytes[2];
                                    bundlxbytes[bundlxindex++] = valuebytes[3];
                                    bundlxbytes[bundlxindex++] = valuebytes[4];
                                    continue;
                                }
                                int len = filename.Length;
                                int colNum = (int)Convert.ToInt64(filename.Substring(1, len - 5), 16) % 128;
                                int size = (int)tile.Length;
                                byte[] Little_Endian = BitConverter.GetBytes(size);//获取切片数据字节长度，主机字节序
                                BinaryReader pngreader = new BinaryReader(File.OpenRead(tile.FullName));
                                byte[] filebytes =new byte[4+size];
                                //将切片数据字节长度，网络字节序转为主机字节序
                                filebytes[0] = Little_Endian[0];
                                filebytes[1] = Little_Endian[1];
                                filebytes[2] = Little_Endian[2];
                                filebytes[3] = Little_Endian[3];
                                pngreader.Read(filebytes, 4, size);
                                pngreader.Close();
                                bundleSize += 4 + size;
                                //将PNG数据写入Bundle中
                                bufbundle.Write(filebytes, 0, 4 + size);
                                //将切片索引地址写入Bundlx中
                                byte[] addressbytes = BitConverter.GetBytes(bundlxNoEmptyValue);
                                bundlxbytes[bundlxindex++] = addressbytes[0];
                                bundlxbytes[bundlxindex++] = addressbytes[1];
                                bundlxbytes[bundlxindex++] = addressbytes[2];
                                bundlxbytes[bundlxindex++] = addressbytes[3];
                                bundlxbytes[bundlxindex++] = addressbytes[4];
                                bundlxNoEmptyValue += 4 + size;
                                tileNum++;
                                if (firstsize == 0)
                                {
                                    firstsize = size;
                                }
                            }
                        }
                        //8-11字节为第一张非空、非完全透明图片的大小
                        byte[] firsttilessizebytes = BitConverter.GetBytes(firstsize);
                        bundleHeader[8] = firsttilessizebytes[0];
                        bundleHeader[9] = firsttilessizebytes[1];
                        bundleHeader[10] = firsttilessizebytes[2];
                        bundleHeader[11] = firsttilessizebytes[3];
                        //16-19字节为非空图片数量*4
                        byte[] tileNumbytes = BitConverter.GetBytes(tileNum*4);
                        bundleHeader[16] = tileNumbytes[0];
                        bundleHeader[17] = tileNumbytes[1];
                        bundleHeader[18] = tileNumbytes[2];
                        bundleHeader[19] = tileNumbytes[3];
                        //24-27字节为Bundle文件的大小
                        byte[] bundleSizebytes = BitConverter.GetBytes(bundleSize);
                        bundleHeader[24] = bundleSizebytes[0];
                        bundleHeader[25] = bundleSizebytes[1];
                        bundleHeader[26] = bundleSizebytes[2];
                        bundleHeader[27] = bundleSizebytes[3];
                        //将新的Bundle文件头写入到文件Bundle中
                        bufbundle.Seek(0, SeekOrigin.Begin);
                        bufbundle.Write(bundleHeader, 0, 0x01003C);
                        bufbundle.Flush();
                        bufbundle.Close();
                        bundle.Close();
                        //将Bundlx数据写入到文件Bundlx中
                        bundlx.Write(bundlxbytes, 0, 81952);
                        bundlx.Close();
                        //删除空的Bundle数据
                        if (bundleSize <= 0x01003C)
                        {
                            FileInfo empty_bundlx = new FileInfo(destLevelDir + "\\" + String.Format("R{0:x4}C{1:x4}.bundlx", new object[] { rowkey, key }));
                            empty_bundlx.Delete();
                            FileInfo empty_bundle = new FileInfo(destLevelDir + "\\" + String.Format("R{0:x4}C{1:x4}.bundle", new object[] { rowkey, key }));
                            empty_bundle.Delete();
                        }
                        else
                        {
                            log.WriteLine(DateTime.Now + ":Output bundle:" + destLevelDir + "\\" + String.Format("R{0:x4}C{1:x4}.bundle", new object[] { rowkey, key }));
                            log.Flush();
                        }
                    }
                    log.WriteLine(DateTime.Now + ":Process block on column:" + String.Format("C{0:x4}", key) + " completed.");
                    log.Flush();
                });
                log.WriteLine(DateTime.Now + ":Convert " + levelDir.FullName + " completed.");
                log.Flush();

                int progress = ((i+1)*100)/levelLength;
                this.backgroundWorker1.ReportProgress(progress);
            }
            log.WriteLine(DateTime.Now + ":Convert to bundles completed.");
            log.Flush();
            log.Close();
        }

        private byte[] bundlxBegin = new byte[16] { 0x03,0x00,0x00,0x00,
                                                    0x10,0x00,0x00,0x00,
                                                    0x00,0x40,0x00,0x00,
                                                    0x05,0x00,0x00,0x00};
        private byte[] bundlxEnd = new byte[16] {   0x00,0x00,0x00,0x00,
                                                    0x10,0x00,0x00,0x00,
                                                    0x10,0x00,0x00,0x00,
                                                    0x00,0x00,0x00,0x00};
        private byte[] createEmptyBundlx()
        {
            byte[] bundlxbytes = new byte[81952];
            for (int i = 0; i < 16;i++ )
            {
                bundlxbytes[i]=bundlxBegin[i];
                bundlxbytes[i + 81936] = bundlxEnd[i];
            }
            return bundlxbytes;
        }

        private byte[] bundleBegin = new byte[16] { 0x03,0x00,0x00,0x00,
                                                    0x00,0x40,0x00,0x00,
                                                    0x00,0x00,0x00,0x00,
                                                    0x05,0x00,0x00,0x00};
        private byte[] bundleBegin2 = new byte[12] {    0x28,0x00,0x00,0x00,
                                                        0x00,0x00,0x00,0x00,
                                                        0x10,0x00,0x00,0x00};
        
        private byte[] createBundleHeader(int rowBegin,int colBegin)
        {
            byte[] bundlebytes=new byte[0x01003C];
            for (int i = 0; i < 16;i++ )
            {
                bundlebytes[i] = bundleBegin[i];
            }
            for (int i = 0; i < 12;i++ )
            {
                bundlebytes[i+0x20] = bundleBegin2[i];
            }
            //设置起始行列号，结束行列号
            byte[] rowBeginbytes = BitConverter.GetBytes(rowBegin);
            byte[] colBeginbytes = BitConverter.GetBytes(colBegin);
            byte[] rowEndbytes = BitConverter.GetBytes(rowBegin+0x7F);
            byte[] colEndbytes = BitConverter.GetBytes(colBegin+0x7F);
            for (int i = 0; i < 4;i++ )
            {
                bundlebytes[i + 0x2C] = rowBeginbytes[i];
                bundlebytes[i + 0x30] = rowEndbytes[i];
                bundlebytes[i + 0x34] = colBeginbytes[i];
                bundlebytes[i + 0x38] = colEndbytes[i];
            }
            return bundlebytes;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                TilesConvert2Bundles();
                e.Result = "Completed";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                e.Result = "Failed";
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null)
            {
                this.progressBar1.Value = e.ProgressPercentage;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error!=null)
            {
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else
            {
                string msg = String.Format("Result = {0}", e.Result);
                MessageBox.Show(msg);
            }
        }
    }
}
