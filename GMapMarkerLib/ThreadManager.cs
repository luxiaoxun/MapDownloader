using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.Drawing;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class ThreadManager
    {
        private int refCount;

        private Thread thread;

        private Hashtable controlHT;

        private Hashtable refleshRegion;

        private int sleeptime;

        public static readonly ThreadManager instance;

        static ThreadManager()
        {
            ThreadManager.instance = new ThreadManager();
        }

        public Hashtable ControlHT
        {
            get
            {
                if (this.controlHT == null)
                {
                    this.controlHT = new Hashtable();
                }
                return this.controlHT;
            }
        }

        public Hashtable RefleshRegion
        {
            get
            {
                if (this.refleshRegion == null)
                {
                    this.refleshRegion = new Hashtable();
                }
                return this.refleshRegion;
            }
        }
        
        private ThreadManager()
        {
            this.refCount = 0;
            this.sleeptime = 100;
        }

        public void decrease()
        {
            ThreadManager threadManager = this;
            threadManager.refCount = threadManager.refCount - 1;
            if (this.refCount == 0)
            {
                if (this.thread.ThreadState == ThreadState.Running)
                {
                    this.thread.Suspend();
                }
                this.thread.Abort();
            }
        }

        public void increase()
        {
            if (this.refCount == 0)
            {
                this.thread = new Thread(new ThreadStart(this.run));
                if (this.thread.ThreadState == ThreadState.Unstarted)
                {
                    this.thread.Start();
                }
                else if (this.thread.ThreadState == ThreadState.Suspended)
                {
                    this.thread.Resume();
                }
            }
            ThreadManager threadManager = this;
            threadManager.refCount = threadManager.refCount + 1;
        }

        private void run()
        {
            while (true)
            {
                if (this.controlHT != null)
                {
                    lock (this.ControlHT.SyncRoot)
                    {
                        try
                        {
                            foreach (DictionaryEntry controlHT in this.ControlHT)
                            {
                                GMapControl key = (GMapControl)controlHT.Key;
                                Hashtable value = (Hashtable)controlHT.Value;
                                foreach (Rectangle rectangle in value.Values)
                                {
                                    key.Invalidate(rectangle);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                        }
                    }
                }
                Thread.Sleep(this.sleeptime);
            }
        }
    }
}
