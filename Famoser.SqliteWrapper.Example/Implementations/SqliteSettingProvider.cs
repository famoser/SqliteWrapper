using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Famoser.SqliteWrapper.Services.Interfaces;
using SQLite.Net.Async;

namespace Famoser.SqliteWrapper.Example.Implementations
{
   public class SqliteServiceSettingsProvider : ISqliteServiceSettingsProvider
    {
       public async Task<string> GetFullPathOfDatabase()
       {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("database.sqlite", CreationCollisionOption.OpenIfExists);
            return file.Path;
        }

       public int GetApplicationId()
       {
           return 870706572;
       }

       public async Task DoMigration(SQLiteAsyncConnection connection)
       {
            //check for database version
           if (await connection.ExecuteAsync("PRAGMA user_version") == 1)
           {
                //do migration
               await connection.ExecuteAsync("PRAGMA user_version = 2");
           }
       }
    }
}
