using System;

namespace Famoser.SqliteWrapper.Exceptions
{
    public class ConverterNotFoundException : Exception
    {
        public ConverterNotFoundException(Type entityType, Type modelType) : base("No converter could be found for those types")
        {
            EntityType = entityType;
            ModelType = modelType;
        }

        public Type EntityType { get;  }
        public Type ModelType { get; }
    }
}
