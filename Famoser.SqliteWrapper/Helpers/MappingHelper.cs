using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Famoser.SqliteWrapper.Attributes;
using Famoser.SqliteWrapper.Enums;
using Famoser.SqliteWrapper.Exceptions;

namespace Famoser.SqliteWrapper.Helpers
{
    public class MappingHelper
    {
        /// <summary>
        /// Writes all properties which are marked with the EntityMapAttribute in the Business Object from the Entity to the Business object.
        /// </summary>
        /// <param name="entity">the Entity object, the "source"</param>
        /// <param name="model"></param>
        /// <returns>the Model</returns>
        public static TModel ConvertToModel<TModel, TEntity>(TEntity entity, TModel model = null)
            where TEntity : class, new()
            where TModel : class, new()
        {
            if (model == null)
                model = new TModel();

            if (entity == null)
                return model;

            //gets all properties of the entity
            var entityType = entity.GetType();
            var entityProps = entityType.GetRuntimeProperties().ToList();

            //gets all properties of the business
            var businessProps = typeof(TModel).GetRuntimeProperties().ToList();

            foreach (var propertyInfo in businessProps)
            {
                //check for the attribute
                var attribute = propertyInfo.GetCustomAttribute(typeof(EntityMapAttribute), false) as EntityMapAttribute;

                if (attribute != null)
                {
                    //check if I am allowed to write properties of entity to this object
                    if (attribute.Procedure == EntityMappingProcedure.OnlyRead || attribute.Procedure == EntityMappingProcedure.ReadAndWrite)
                    {
                        var prop = entityProps.FirstOrDefault(e => e.Name == propertyInfo.Name);
                        if (prop != null)
                        {
                            propertyInfo.SetValue(model, GetValue(propertyInfo, false, prop.GetValue(entity)));
                        }
                        else
                        {
                            throw new PropertyNotFoundException(typeof(TEntity), typeof(TModel), propertyInfo.Name);
                        }
                    }
                }
            }

            return model;
        }

        public static List<TModel> ConvertAllToModel<TModel, TEntity>(List<TEntity> entity)
            where TModel : class, new()
            where TEntity : class, new()
        {
            return entity.Select(t => ConvertToModel<TModel, TEntity>(t)).ToList();
        }

        public static List<TEntity> ConvertAllToEntity<TEntity, TModel>(List<TModel> business)
            where TEntity : class, new()
            where TModel : class, new()
        {
            return business.Select(t => ConvertToEntity<TEntity, TModel>(t)).ToList();
        }

        /// <summary>
        /// Writes all properties which are marked with the EntityMapAttribute from the Business object to the Entity object.
        /// </summary>
        /// <param name="model">the Business object, the "source"</param>
        /// <param name="entity"></param>
        /// <returns>the Entity object (same instance as passed)</returns>
        public static TEntity ConvertToEntity<TEntity, TModel>(TModel model, TEntity entity = null)
            where TEntity : class, new()
            where TModel : class, new()
        {
            if (entity == null)
                entity = new TEntity();
            if (model == null)
                return entity;

            //gets all properties of the business
            var businessType = model.GetType();
            var businessProps = businessType.GetRuntimeProperties();


            //gets all properties of the entity
            var entityType = typeof(TEntity);
            var entityProps = entityType.GetRuntimeProperties().ToList();

            foreach (var propertyInfo in businessProps)
            {
                //check for the attribute
                var attribute = propertyInfo.GetCustomAttribute(typeof(EntityMapAttribute), false) as EntityMapAttribute;
                if (attribute != null)
                {
                    //check if I am allowed to write properties to the entity from this object
                    if (attribute.Procedure == EntityMappingProcedure.ReadAndWrite ||
                        attribute.Procedure == EntityMappingProcedure.OnlyWrite)
                    {
                        var prop = entityProps.FirstOrDefault(e => e.Name == propertyInfo.Name);
                        if (prop != null)
                        {
                            prop.SetValue(entity, GetValue(propertyInfo, true, propertyInfo.GetValue(model)));
                        }
                        else
                        {
                            throw new PropertyNotFoundException(typeof(TEntity), typeof(TModel), propertyInfo.Name);

                        }
                    }
                }
            }

            return entity;
        }

        public static TEntity WriteAllProperties<TEntity>(TEntity targetentity, TEntity sourcentity)
            where TEntity : class
        {
            if (targetentity == null || sourcentity == null)
                return targetentity;

            //gets all properties of the business
            var businessType = targetentity.GetType();
            var businessProps = businessType.GetRuntimeProperties();

            foreach (var propertyInfo in businessProps)
            {
                propertyInfo.SetValue(targetentity, propertyInfo.GetValue(sourcentity));
            }

            return targetentity;
        }

        private static object GetValue(PropertyInfo propertyInfo, bool fromBuiness, object value)
        {
            var conversionAttribute = propertyInfo.GetCustomAttribute(typeof(EntityConversionAttribute), false) as EntityConversionAttribute;
            if (conversionAttribute != null)
            {
                value = fromBuiness ? conversionAttribute.Converter.ConvertBack(value) : conversionAttribute.Converter.Convert(value);
            }

            return value;
        }
    }
}
