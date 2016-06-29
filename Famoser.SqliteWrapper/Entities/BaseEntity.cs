using SQLite.Net.Attributes;

namespace Famoser.SqliteWrapper.Entities
{
    public class BaseEntity
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
    }
}
