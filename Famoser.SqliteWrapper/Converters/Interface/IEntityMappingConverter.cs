namespace Famoser.SqliteWrapper.Converters.Interface
{
    public interface IEntityMappingConverter
    {
        object ConvertToModelFormat(object entityFormat);

        object ConvertToEntityFormat(object modelFormat);
    }
}
