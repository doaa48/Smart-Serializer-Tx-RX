using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class TLVTagAttribute : Attribute
    {
        public byte Tag
        {
            get;
            private set;
        }

        public TLVTagAttribute(byte tag)
        {
            Tag = tag;
        }
    }
}
