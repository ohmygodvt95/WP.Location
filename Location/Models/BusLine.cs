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
        public String data;
        public String name;

        public BusLine()
        {
            this.ListPoints = new ObservableCollection<MyPoint>();
            this.data = "";
            this.name = "";
        }

        public void AddPoint(MyPoint point)
        {
            this.ListPoints.Add(point);
        }

        public void AddData(Geopoint point)
        {
            this.data += point.Position.Latitude + "|" + point.Position.Longitude + " ";
        }

        public void AddName(String str)
        {
            this.name = str;
        }
        public void Reset()
        {
            while (this.ListPoints.Count != 0)
            {
                this.ListPoints.Remove(this.ListPoints.Last<MyPoint>());
            }
            this.data = "";
            this.name = "";
        }
    }
}
