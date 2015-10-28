using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.DataModels
{
    class DataBusLine
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public String Time { get; set; }
        public DataBusLine()
        {
            //empty constructor  
        }
        public DataBusLine(string name, string data)
        {
            Name = name;
            Data = data;
            Time = DateTime.Now.ToString();
        }
    }
}
