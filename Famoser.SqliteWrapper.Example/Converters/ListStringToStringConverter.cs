using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.SqliteWrapper.Converters.Interface;
using Newtonsoft.Json;

namespace Famoser.SqliteWrapper.Example.Converters
{
    public class ListStringToStringConverter : IEntityMappingConverter
    {
        public object ConvertToModelFormat(object entityFormat)
        {
            var str = (string)entityFormat;
            if (!string.IsNullOrWhiteSpace(str))
                return JsonConvert.DeserializeObject<List<string>>(str);
            return new List<string>();
        }

        public object ConvertToEntityFormat(object modelFormat)
        {
            var list = modelFormat as List<string>;
            if (list != null)
                return JsonConvert.SerializeObject(list);
            return null;
        }
    }
}
