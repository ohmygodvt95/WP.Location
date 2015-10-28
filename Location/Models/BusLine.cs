using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Location.Models
{
    class BusLine
    {
        public ObservableCollection<MyPoint> ListPoints;
        public String Data;
        public String Name;

        public BusLine()
        {
            this.ListPoints = new ObservableCollection<MyPoint>();
            this.Data = "";
            this.Name = "";
        }

        public void AddPoint(MyPoint point)
        {
            this.ListPoints.Add(point);
        }

        public void AddData(Geopoint point)
        {
            this.Data += point.Position.Latitude + "|" + point.Position.Longitude + " ";
        }

        public void AddName(String str)
        {
            this.Name = str;
        }
        public void Reset()
        {
            while (this.ListPoints.Count != 0)
            {
                this.ListPoints.Remove(this.ListPoints.Last<MyPoint>());
            }
            this.Data = "";
            this.Name = "";
        }
    }
}
