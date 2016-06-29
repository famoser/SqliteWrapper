using System;

namespace Famoser.SqliteWrapper.Exceptions
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(Type entityType, Type modelType, string propertyName) : base("requested property not found! Check your mapping attributes")
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
