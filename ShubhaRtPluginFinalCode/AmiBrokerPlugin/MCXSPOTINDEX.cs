using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace AmiBrokerPlugin
{
    


        [DelimitedRecord(","), IgnoreFirst(1), IgnoreEmptyLines(true)]
        public class SpaceRemove
        {
            public string   ticker;
            public string name;
            public string date;
            public string open;
            public string high;
            public string low;
            public string close;
            public string volume;
            [FieldNullValue(typeof(long), "0")]
            public Nullable<long> openint;
        }


    
}
