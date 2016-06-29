using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Famoser.SqliteWrapper.Entities;

namespace Famoser.SqliteWrapper.Example.Entities
{
    public class MyBaseEntity : BaseEntity
    {
        public string MyStringProp { get; set; }
        public int MyIntProp { get; set; }
        public int VisibilityEnum { get; set; }
        public string GuidProperty { get; set; }
        public string StringList { get; set; }
    }
}
