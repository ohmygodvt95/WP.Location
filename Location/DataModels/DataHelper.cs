using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Location.DataModels
{
    
    class DataHelper { 
        public static string DbName = "DataBus.db";
        public static bool IsConnect = false;
        private static SQLiteAsyncConnection _conn;

        private static async Task ConnectDb()
        {
            if (IsConnect) return;

            bool isExist = await DoesDbExist();
            if (!isExist)
            {
                System.Diagnostics.Debug.WriteLine("Chua co Data");
                await CreateData();
            }

            _conn = new SQLiteAsyncConnection(DbName);
            IsConnect = true;
        }

        public async Task OnCreate()
        {
            await ConnectDb();
        }
        public static async Task<bool> DoesDbExist()
        {
            bool dbexist = true;
            try
            {
               
                StorageFile StorageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DbName);
            }
            catch
            {
                dbexist = false;
            }
            return dbexist;
        }

        public static async Task CreateData()
        {
            SQLiteAsyncConnection connection = new SQLiteAsyncConnection(DbName);
            await connection.CreateTableAsync<DataBusLine>();
            await connection.CreateTableAsync<DataPoint>();
            System.Diagnostics.Debug.WriteLine("Create DB");
        }

        public async Task<ObservableCollection<DataBusLine>> ReadAllBusLines()
        {
            await ConnectDb();
            List<DataBusLine> myCollection = await _conn.Table<DataBusLine>().ToListAsync();
            ObservableCollection<DataBusLine> data = new ObservableCollection<DataBusLine>(myCollection);
            return data;
        }

        public async void InsertNewBusLine(DataBusLine newS)
        {
            await ConnectDb();
            await _conn.InsertAsync(newS);
        }

        public async Task<DataBusLine> GetNewLine()
        {
            await ConnectDb();
            List<DataBusLine> b = await _conn.QueryAsync<DataBusLine>("SELECT * FROM DataBusLine ORDER BY Id DESC LIMIT 1");
            if (b.Count == 0) return null;
           // System.Diagnostics.Debug.WriteLine("get ok");
            return b[0];
        }
        public async Task<DataBusLine> ReadBusLine(int id)
        {
            await ConnectDb();
            List<DataBusLine> b = await _conn.QueryAsync<DataBusLine>("SELECT * FROM DataBusLine WHERE Id =" + id);
            if (b.Count == 0) return null;
            return b[0];
        }

        public async void InsertNewPoint(DataPoint newS)
        {
            await ConnectDb();
            await _conn.InsertAsync(newS);
        }
        public async Task<List<DataPoint>> ReadPointsOfBusLine(int LineId)
        {
            await ConnectDb();
            List<DataPoint> myList = await _conn.QueryAsync<DataPoint>("SELECT * FROM DataPoint WHERE LineId =" + LineId);
            if (myList.Count == 0) return null;
            return myList;
        }

    }
}
