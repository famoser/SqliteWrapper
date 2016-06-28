using Famoser.SqliteWrapper.Converters.Interface;

namespace Famoser.SqliteWrapper.Converters
{
    public class EnumIntConverter<T> : IEntityMappingConverter
    {
        public object ConvertToModelFormat(object entityFormat)
        {
            if (entityFormat == null)
                return default(T);
            return (T)entityFormat;
        }

        public object ConvertToEntityFormat(object modelFormat)
        {
            if (modelFormat == null)
                return 0;
            return (int)modelFormat;
        }
    }
}
