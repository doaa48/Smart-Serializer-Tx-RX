using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_CompanyData
    {
        public int id { get; set; }
        public string CompanyName { get; set; }
        public byte[] image { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string website { get; set; }

    }
}
