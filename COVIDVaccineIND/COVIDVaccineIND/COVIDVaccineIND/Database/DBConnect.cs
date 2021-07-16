using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using COVIDVaccineIND.Models;

namespace COVIDVaccineIND.Database
{
    public class DBConnect
    {
        private SQLiteAsyncConnection _connection;
        private static DBConnect _dbConnection;
        private string _dbPath;
        private Action DBCreated;
        public bool IsDBCreated = false;

        private DBConnect(Action DBCreated)
        {
            this.DBCreated = DBCreated;
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Database");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            _dbPath = Path.Combine(folderPath, "MainDB.db3");

            _connection = new SQLiteAsyncConnection(_dbPath);

            CreateTable();

        }

        public static DBConnect GetDBConnect(Action DBCreated = null)
        {
            if (_dbConnection == null)
            {
                _dbConnection = new DBConnect(DBCreated);
            }

            return _dbConnection;
        }

        private async void CreateTable()
        {
            await _connection.CreateTableAsync<APIResponseCache>();
            DBCreated?.Invoke();
            IsDBCreated = true;
        }

        public async Task InsertRecord<E>(E item)
        {
            await _connection.InsertAsync(item);
        }

        public async Task UpdateRecord<E>(E item)
        {
            await _connection.UpdateAsync(item);
        }

        public async Task DeleteRecord<E>(E item)
        {
            await _connection.DeleteAsync(item);
        }

        public async Task<List<APIResponseCache>> GetAPIResponses()
        {
            return await _connection.Table<APIResponseCache>().ToListAsync();
        }

        public async Task ClearCache()
        {
            await _connection.DeleteAllAsync<APIResponseCache>();
        }

    }
}
