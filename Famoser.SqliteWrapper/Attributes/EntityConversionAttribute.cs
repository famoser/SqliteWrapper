using System;
using System.Collections.Generic;
using System.Reflection;
using Famoser.SqliteWrapper.Converters;
using Famoser.SqliteWrapper.Converters.Interface;
using Famoser.SqliteWrapper.Exceptions;

namespace Famoser.SqliteWrapper.Attributes
{
    /// <summary>
    /// This attribute defines, how a property of an entity is supposed to be converted before writing it the business object and vice versa
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityConversionAttribute : System.Attribute
    {
        public Type EntityType { get; set; }
        public Type ModelType { get; set; }
        public IEntityMappingConverter Converter { get; private set; }

        private static readonly Dictionary<Type, Dictionary<Type, IEntityMappingConverter>> Converters = new Dictionary<Type, Dictionary<Type, IEntityMappingConverter>>();

        public static bool RegisterConverter(IEntityMappingConverter converter, Type entityType, Type modelType)
        {
            if (!Converters.ContainsKey(entityType))
                Converters[entityType] = new Dictionary<Type, IEntityMappingConverter>();

            if (!Converters[entityType].ContainsKey(modelType))
            {
                Converters[entityType][modelType] = converter;
                return true;
            }
            return false;
        }

        public EntityConversionAttribute(Type entityType, Type modelType)
        {
            EntityType = entityType;
            ModelType = modelType;

            if (entityType == typeof(int) && modelType.GetTypeInfo().IsEnum)
            {
                Type repo = typeof(EnumIntConverter<>);
                Type[] args = { modelType };
                Type constructed = repo.MakeGenericType(args);

                Converter = Activator.CreateInstance(constructed) as IEntityMappingConverter;
            }
            else if (Converters.ContainsKey(entityType) && Converters[entityType].ContainsKey(modelType))
                Converter = Converters[entityType][modelType];
            else
                throw new ConverterNotFoundException(entityType, modelType);
        }
    }
}
