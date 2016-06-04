namespace Famoser.SqliteWrapper.Attributes.Interface
{
    public class EnumConverter<T> : IEntityMappingConverter
    {
        public object Convert(object val)
        {
            if (val == null)
                return default(T);
            return (T)val;
        }

        public object ConvertBack(object val)
        {
            if (val == null)
                return 0;
            return (int)val;
        }
    }
}
