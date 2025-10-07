using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class BLL_HoldingComp
    {
        public int HoldingComp_Id                  { get; set; }
        public string Holding_Comp_Name            { get; set; }
        public string Holding_Comp_KUH             { get; set; }
        public string Holding_Comp_KPH             { get; set; }
        
    }
    public class BLL_MeterManf
    {
        public int MeterManf_Id                   { get; set; }
        public string MeterManf_Name              { get; set; }
        public string MeterManf_KUF               { get; set; }
        public string MeterManf_KPF               { get; set; }
        public string MeterManf_CKUF              { get; set; }
        public int MeterManf_HoldingCompId         { get; set; }
    }
    public class BLL_CardProd
    {
        public int CardProd_Id                   { get; set; }
        public string CardProd_name              { get; set; }
        public string CardProd_KUCP              { get; set; }
        public string CardProd_KPCP              { get; set; }
        public string CardProd_CKUCP             { get; set; }
        public int CardProd_HoldingCompId       { get; set; }
    }
    public class BLL_WaterComp
    { 
        public int     WaterComp_Id                                        { get; set; }
        public string  WaterComp_Name                                   { get; set; }
        public string  WaterComp_KUW                                    { get; set; }
        public string  WaterComp_KPW                                    { get; set; }
        public int    WaterComp_HoldingCompId                             { get; set; }
        public string WaterComp_CertHoldingComp_TO_Watercomp           { get; set; }
        public int   WaterComp_CardProdId                                { get; set; }
        public int   WaterComp_MeterManfId                               { get; set; }
    }
}
