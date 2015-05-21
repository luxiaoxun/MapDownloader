using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Cells;

namespace NetUtil
{
    public static class ExcelHelper
    {
        public static int DataTableToExcel(DataTable data, string fileName, string sheetName, bool isColumnNameWritten)
        {
            int num = -1;
            try
            {
                Workbook workBook;
                Worksheet worksheet = null;

                if (File.Exists(fileName))
                    workBook = new Workbook(fileName);
                else
                    workBook = new Workbook();

                if (sheetName == null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        worksheet = workBook.Worksheets[0];
                    }
                    else
                    {
                        sheetName = "Sheet1";
                        workBook.Worksheets.RemoveAt(sheetName);
                        worksheet = workBook.Worksheets.Add(sheetName);
                    }
                }
                if (worksheet != null)
                {
                    num = worksheet.Cells.ImportDataTable(data, isColumnNameWritten, 0, 0, false);
                    workBook.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return num;
        }

        public static void AddOneRowToExcel(string[] dataArray, string fileName, string sheetName)
        {
            try
            {
                Workbook workBook;

                if (File.Exists(fileName))
                    workBook = new Workbook(fileName);
                else
                    workBook = new Workbook();

                Worksheet worksheet=null;

                if (sheetName == null)
                {
                    worksheet = workBook.Worksheets[0];
                }
                else
                {
                    worksheet = workBook.Worksheets[sheetName];
                }
                if (worksheet != null)
                {
                    worksheet.Cells.ImportArray(dataArray, worksheet.Cells.MaxDataRow+1, 0, false);
                    workBook.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumnName)
        {
            DataTable data = new DataTable();

            try
            {
                Workbook workbook = null;

                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Extension.ToLower().Equals(".xlsx"))
                    workbook = new Workbook(fileName, new LoadOptions(LoadFormat.Xlsx));
                else if (fileInfo.Extension.ToLower().Equals(".xls"))
                    workbook = new Workbook(fileName, new LoadOptions(LoadFormat.Excel97To2003));
                if (workbook != null)
                {
                    Worksheet worksheet = null;
                    if (sheetName != null)
                    {
                        worksheet = workbook.Worksheets[sheetName];
                    }
                    else
                    {
                        worksheet = workbook.Worksheets[0];
                    }
                    if (worksheet != null)
                    {
                        data = worksheet.Cells.ExportDataTable(0, 0, worksheet.Cells.MaxRow+1, worksheet.Cells.MaxColumn+1,
                            isFirstRowColumnName);

                        return data;
                    }
                }
                else
                {
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
    }
}
