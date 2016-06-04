using System;
using Famoser.SqliteWrapper.Enums;

namespace Famoser.SqliteWrapper.Attributes
{
    /// <summary>
    /// This attribute defines, which properties of an entity are supposed to be written into the business object and vice versa
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityMapAttribute : System.Attribute
    {
        public EntityMappingProcedure Procedure { get; private set; }

        public EntityMapAttribute(EntityMappingProcedure procedure = EntityMappingProcedure.ReadAndWrite)
        {
            Procedure = procedure;
        }
    }
}