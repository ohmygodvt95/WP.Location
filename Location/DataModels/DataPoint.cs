using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Location.DataModels
{
    [Table("DataPoint")]
    class DataPoint
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public int LineId { get; set; }
        public String Name { get; set; }
        public String Long { get; set; }
        public String Lat { get; set; }
        public DataPoint()
        {
            //empty constructor  
        }
        public DataPoint(string name, int id, String log, String lat)
        {
            Name = name;
            LineId = id;
            Long = log;
            Lat = lat;
        }
    }
}
