using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_CardIssues
    {
        public int id      { get; set; }
        public string PK { get; set; }
        public int CardTypeID     { get; set; }
        public int CardProdID  { get; set; }
        public string CertCardProd_TO_Card { get; set; }
        public int WaterCompID { get; set; }
        public string CertWaterComp_TO_Card { get; set; }
        public int CardNum { get; set; }
        public byte Func { get; set; }
        public byte Mode { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime LastTranscationDate { get; set; }
        public DateTime ExpiratonDate { get; set; }
        public byte LastUploadBy { get; set; }
        public string CardProd_KUCP { get; set; }
        public string WaterComp_KUW { get; set; }
        public string WaterComp_KPW { get; set; }

        public string CardType_Code { get; set; }
        public string CardProd_name { get; set; }
        public string WaterComp_Name { get; set; }

    }
}
