namespace Famoser.SqliteWrapper.Converters.Interface
{
    public interface IEntityMappingConverter
    {
        object Convert(object val);

        object ConvertBack(object val);
    }
}
