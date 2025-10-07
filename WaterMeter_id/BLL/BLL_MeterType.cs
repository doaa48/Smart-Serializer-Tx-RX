using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
   public class BLL_MeterType
    {
        public int id                   { get; set; }
        public string Code              { get; set; }
        public string ManfName          { get; set; }
        public string Model             { get; set; }
        public string MeterVersionNum   { get; set; }
        public string Desc              { get; set; }
    }
}
