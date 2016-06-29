using System.Threading.Tasks;
using SQLite.Net.Async;

namespace Famoser.SqliteWrapper.Services.Interfaces
{
    public interface ISqliteServiceSettingsProvider
    {
        /// <summary>
        /// The full path of the database file. if no file exists, a new one is generated
        /// </summary>
        /// <returns></returns>
        Task<string> GetFullPathOfDatabase();

        /// <summary>
        /// a unique id for your application written to the database 
        /// </summary>
        /// <returns></returns>
        int GetApplicationId();

        /// <summary>
        /// When a new connection is created this method is invoked. Do updates / migration if needed. Also you may overwrite PRAGMA statements
        /// Tables are already created by the SqliteService implementation provided with this library, no need to worry about that
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        Task DoMigration(SQLiteAsyncConnection connection);
    }
}
