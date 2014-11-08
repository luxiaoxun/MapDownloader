using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public abstract class GMapAnimateMarker : GMapMarker
    {
        private Guid id;
        public Guid ID
        {
            get
            {
                if (this.id == Guid.Empty)
                {
                    this.id = Guid.NewGuid();
                }
                return this.id;
            }
        }

        private GMapControl control;
        public GMapControl Control
        {
            get
            {
                return this.control;
            }
            set
            {
                this.control = value;
            }
        }

        public GMapAnimateMarker(GMapControl map, PointLatLng pos) : base(pos)
        {
            this.control = map;
            if(ThreadManager.instance!=null)
            {
                if (map != null && ThreadManager.instance.ControlHT[control] == null)
                {
                    Hashtable hashtables = new Hashtable();
                    ThreadManager.instance.ControlHT.Add(control, hashtables);
                }
                ThreadManager.instance.increase();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            ThreadManager.instance.decrease();
            lock (ThreadManager.instance.ControlHT.SyncRoot)
            {
                Hashtable item = (Hashtable)ThreadManager.instance.ControlHT[control];
                item.Remove(this.ID.ToString());
            }
        }

        public void RefreshAnimateMarkerRegion(Rectangle rect)
        {
            lock (ThreadManager.instance.ControlHT.SyncRoot)
            {
                Hashtable item = (Hashtable)ThreadManager.instance.ControlHT[control];
                if (item != null)
                {
                    if (item[this.ID.ToString()] == null)
                    {
                        item.Add(this.ID.ToString(), rect);
                    }
                    else
                    {
                        item[this.ID.ToString()] = rect;
                    }
                }
            }
        }
    }
}
