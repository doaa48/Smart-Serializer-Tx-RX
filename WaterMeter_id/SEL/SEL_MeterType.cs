using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class SEL_MeterType
    {

        private DAL_MeterType DAL_MeterType_Obj = new DAL_MeterType();
        public BLL_MeterType MeterType_Data = new BLL_MeterType();
        public bool AddNew()
        {
            bool status = false;
            if (MeterType_Data.ManfName != "" )  //add any condtion can lead to destory data 
            {
                status = DAL_MeterType_Obj.Insert(MeterType_Data);
            }
            return status;
        }
        public bool Update()
        {
            bool status = false;
            if (MeterType_Data.ManfName != "" && MeterType_Data.id > 0)  //add any condtion can lead to destory data 
            {
                status = DAL_MeterType_Obj.Update(MeterType_Data);
            }
            return status;
        }

        public bool Delet()
        {
            bool status = false;
            if (MeterType_Data.ManfName != "" && MeterType_Data.id >0)  //add any condtion can lead to destory data 
            {
                status = DAL_MeterType_Obj.Delete(MeterType_Data);
            }
            return status;
        }
        public DataTable GetTable()
        {
            return DAL_MeterType_Obj.Select();
        }
    }
}
