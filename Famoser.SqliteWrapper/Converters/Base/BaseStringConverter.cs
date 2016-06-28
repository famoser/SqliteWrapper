using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.SqliteWrapper.Converters.Interface;

namespace Famoser.SqliteWrapper.Converters.Base
{
    public abstract class BaseStringConverter : IEntityMappingConverter
    {
        public abstract object ConvertToModelFormat(object entityFormat);

        public object ConvertToEntityFormat(object modelFormat)
        {
           return modelFormat.ToString();
        }
    }
}
