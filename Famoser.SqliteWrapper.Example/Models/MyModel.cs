using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Famoser.SqliteWrapper.Attributes;
using Famoser.SqliteWrapper.Models.Interfaces;

namespace Famoser.SqliteWrapper.Example.Models
{
    public class MyModel : ISqliteModel
    {
        private int DatabaseId { get; set; }

        [EntityMap]
        public string MyStringProp { get; set; }
        [EntityMap]
        public int MyIntProp { get; set; }

        [EntityMap, EntityConversion(typeof(int), typeof(Visibility))]
        public Visibility VisibilityEnum { get; set; }

        [EntityMap, EntityConversion(typeof(string), typeof(Guid))]
        public Guid GuidProperty { get; set; }

        [EntityMap, EntityConversion(typeof(string), typeof(List<string>))]
        public List<string> StringList { get; set; }

        public int GetId()
        {
            return DatabaseId;
        }

        public void SetId(int id)
        {
            DatabaseId = id;
        }
    }
}
