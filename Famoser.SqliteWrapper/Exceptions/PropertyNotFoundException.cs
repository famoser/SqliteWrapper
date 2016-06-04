using System;

namespace Famoser.SqliteWrapper.Exceptions
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(Type entityType, Type modelType, string propertyName) : base("No converter could be found for those types")
        {
            EntityType = entityType;
            ModelType = modelType;
            PropertyName = propertyName;
        }

        public Type EntityType { get; }
        public Type ModelType { get; }
        public string PropertyName { get; }
    }
}
