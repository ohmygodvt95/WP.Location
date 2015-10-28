using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Location.DataModels
{
    class DataBusLineHelper
    {
        public SQLiteConnection DbConn;
        public String DbName = "DataBusLine.sqlite";
        public string DbPath = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "DataBusLine.sqlite"));
        //Create Tabble   
        public async Task<bool> OnCreate(string DB_PATH)
        {
            try
            {
                if (!CheckFileExists(DB_PATH).Result)
                {
                    using (DbConn = new SQLiteConnection(DB_PATH))
                    {
                        DbConn.CreateTable<DataBusLine>();
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
        public DataBusLine ReadBusLine(int id)
        {
            using (var dbConn = new SQLiteConnection(DbPath))
            {
                var existingconact = dbConn.Query<DataBusLine>("SELECT * FROM DataBusLine WHERE Id =" + id).FirstOrDefault();
                return existingconact;
            }
        }
        public DataBusLine GetNewLine()
        {
            using (var dbConn = new SQLiteConnection(DbPath))
            {
                List<DataBusLine> myCollection = dbConn.Table<DataBusLine>().ToList<DataBusLine>();
                ObservableCollection<DataBusLine> data = new ObservableCollection<DataBusLine>(myCollection);
                return data.Last();
            }
        }
        //
        public ObservableCollection<DataBusLine> ReadAllBusLines()
        {
            using (var dbConn = new SQLiteConnection(DbPath))
            {
                List<DataBusLine> myCollection = dbConn.Table<DataBusLine>().ToList<DataBusLine>();
                ObservableCollection<DataBusLine> data = new ObservableCollection<DataBusLine>(myCollection);
                return data;
            }
        }
        public void Insert(DataBusLine newS)
        {
            using (var dbConn = new SQLiteConnection(DbPath))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newS);
                });
            }
        }
        //
        public void DeleteBusLine(int Id)
        {
            using (var dbConn = new SQLiteConnection(DbPath))
            {
                var existingconact = dbConn.Query<DataBusLine>("SELECT * FROM DataBusLine WHERE Id =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingconact);
                    });
                }
            }
        }
        //
        public void DeleteAllContact()
        {
            using (var dbConn = new SQLiteConnection(DbPath))
            { 
                dbConn.DropTable<DataBusLine>();
                dbConn.CreateTable<DataBusLine>();
                dbConn.Dispose();
                dbConn.Close();  
            }
        }
    }
}
