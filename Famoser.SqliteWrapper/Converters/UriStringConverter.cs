using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.SqliteWrapper.Converters.Base;

namespace Famoser.SqliteWrapper.Converters
{
    public class UriStringConverter : BaseStringConverter
    {
        public override object ConvertToModelFormat(object entityFormat)
        {
            var str = entityFormat as string;
            if (entityFormat != null)
                return new Uri(str);
            return null;
        }
    }
}
