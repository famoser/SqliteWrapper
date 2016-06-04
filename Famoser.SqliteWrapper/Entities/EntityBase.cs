using SQLite.Net.Attributes;

namespace Famoser.SqliteWrapper.Entities
{
    public class EntityBase
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
    }
}
