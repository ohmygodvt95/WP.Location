using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Location.DataModels
{
    class DatapointHelper
    {
        public SQLiteConnection dbConn;
        public String dbName = "DataPoint.sqlite";
        public string DB_PATH = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "DataPoint.sqlite"));
        //Create Tabble   
        public async Task<bool> onCreate(string DB_PATH)
        {
            try
            {
                if (!CheckFileExists(DB_PATH).Result)
                {
                    using (dbConn = new SQLiteConnection(DB_PATH))
                    {
                        dbConn.CreateTable<DataPoint>();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        public async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        public ObservableCollection<DataPoint> ReadPointsOfBusLine(int LineId)
        {
            using (var dbConn = new SQLiteConnection(DB_PATH))
            {
                List<DataPoint> myList = dbConn.Query<DataPoint>("SELECT * FROM DataPoint WHERE LineId =" + LineId);
                ObservableCollection<DataPoint> data = new ObservableCollection<DataPoint>(myList);
                return data;
            }
        }
        public void Insert(DataPoint newS)
        {
            using (var dbConn = new SQLiteConnection(DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newS);
                });
            }
        }
    }
}
