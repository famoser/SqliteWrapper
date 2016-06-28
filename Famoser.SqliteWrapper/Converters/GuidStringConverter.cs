using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.SqliteWrapper.Converters.Base;
using Famoser.SqliteWrapper.Converters.Interface;

namespace Famoser.SqliteWrapper.Converters
{
    public class GuidStringConverter : BaseStringConverter
    {
        public override object ConvertToModelFormat(object entityFormat)
        {
            var str = entityFormat as string;
            if (str != null)
                return Guid.Parse(str);
            return Guid.Empty;
        }
    }
}
