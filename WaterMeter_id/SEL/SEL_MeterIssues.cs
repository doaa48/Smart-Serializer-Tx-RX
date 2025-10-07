using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
   
    public class SEL_MeterIssues
    {
        private DAL_MeterIssues DAL_MeterIssues_obj = new DAL_MeterIssues();
        public BLL_MeterIssues MeterIssues_Data = new BLL_MeterIssues();
   

        private unifyWaterCard_Comm unifyWaterCard_Comm_Obj = new unifyWaterCard_Comm();

        public string[] GetMeterType_Code()
        {

            DAL_MeterType DAL_MeterType_Obj = new DAL_MeterType();
            DataTable table = DAL_MeterType_Obj.Select();
            string[] data = new string[table.Rows.Count];

            for (int index = 0; index < table.Rows.Count; index++)
            {
                data[index] = table.Rows[index]["MeterType_Code"].ToString();
            }
            return data;

        }
        public string[] GetMeterManfS_Name()
        {
            DAL_MeterManf DAL_MeterManf_Obj = new DAL_MeterManf();
           
            DataTable table = DAL_MeterManf_Obj.Select();
            string[] data = new string[table.Rows.Count];

            for (int index = 0; index < table.Rows.Count; index++)
            {
                data[index] = table.Rows[index]["MeterManf_Name"].ToString();
            }
            return data;

        }

        public string[] GetMeterNum()
        {

           
            DataTable table = DAL_MeterIssues_obj.Select();
            string[] data = new string[table.Rows.Count];

            for (int index = 0; index < table.Rows.Count; index++)
            {
                data[index] = table.Rows[index]["Meter_MeterNum"].ToString();
            }
            return data;

        }
        public bool SetMeterIssues()
        {
            bool status = false;
            status=unifyWaterCard_Comm_Obj.Meter_InitIssues();

            return status;

        }
        public bool ReadMeterIssues()
        {
            bool status = false;
            status=unifyWaterCard_Comm_Obj.Meter_ReadIssues();
            MeterIssues_Data = unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data;
            return status;

        }

        public DataTable GetTable()
        {
            return DAL_MeterIssues_obj.SelectTheMeters();
        }
        public bool VerifyAndInitData(string MeterType,string MeterMAnfsName)
        {
            bool Status = false;
            ///extract id of meter type 
            DAL_MeterType DAL_MeterType_Obj = new DAL_MeterType();
            DAL_MeterManf DAL_MeterManf_Obj = new DAL_MeterManf();

            DataTable Table = DAL_MeterType_Obj.Search(MeterType);
           
            if (Table.Rows.Count >= 1)
            {
                // CardproduerPVK = table.Rows[0]["MeterType_ID"].ToString();
                MeterIssues_Data.Meter_MeterTypeID = Convert.ToInt32(Table.Rows[0]["MeterType_ID"]);

            }
            //extract id  metermanfsName 
            //public key of mater manfacture 

             Table = DAL_MeterManf_Obj.Search(MeterMAnfsName);
            string MAnfacture_PuK = "";
            if (Table.Rows.Count >= 1)
            {
                MAnfacture_PuK = Table.Rows[0]["MeterManf_KUF"].ToString();
                MeterIssues_Data.Meter_MeterManfID = Convert.ToInt32(Table.Rows[0]["MeterManf_ID"]);

            }
            MeterIssues_Data.Meter_CertWaterComp_TO_Meter = null;
            MeterIssues_Data.Meter_IssueDate = DateTime.Now;

            if (MAnfacture_PuK != "")
            {
                if (unifyWaterCard_Comm_Obj.Meter_VerifyCetrtManfactureToMeter(MeterIssues_Data.Meter_CertMeterManf_TO_Meter, MAnfacture_PuK))
                {
                    //save data to data base 

                    Status = DAL_MeterIssues_obj.Insert(MeterIssues_Data);

                }
            }




            return Status;
        }

        public bool SetMeterWithWaterCompany(string MeterNum)
        {
            bool status = false;
            //watercompanydata
            BLL_WaterComp BLL_WaterComp_Data = new BLL_WaterComp();
            DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();

            DataTable table = DAL_WaterComp_Obj.Select();
            BLL_WaterComp_Data.WaterComp_Id = Convert.ToInt32(table.Rows[0]["WaterComp_ID"]);
            BLL_WaterComp_Data.WaterComp_Name = table.Rows[0]["WaterComp_Name"].ToString();
            BLL_WaterComp_Data.WaterComp_KUW  = table.Rows[0]["WaterComp_KUW"].ToString();
            BLL_WaterComp_Data.WaterComp_KPW  = table.Rows[0]["WaterComp_KPW"].ToString();
            BLL_WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp= table.Rows[0]["WaterComp_CertHoldingComp_TO_Watercomp"].ToString();
            //cardbasicdata
            // BLLCardIssues_Data

            //meterdata
            table = DAL_MeterIssues_obj.Search(MeterNum);
           

            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_ID            = Convert.ToInt32(table.Rows[0]["Meter_ID"]);
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_PK             = table.Rows[0]["Meter_PK"].ToString();
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_MeterTypeID = Convert.ToInt32(table.Rows[0]["Meter_MeterTypeID"]);
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_MeterManfID = Convert.ToInt32(table.Rows[0]["Meter_MeterManfID"]);
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_CertMeterManf_TO_Meter = table.Rows[0]["Meter_CertMeterManf_TO_Meter"].ToString();
          //  unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_WaterCompID = Convert.ToInt32(table.Rows[0]["Meter_WaterCompID"]);
          //  unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_CertWaterComp_TO_Meter = table.Rows[0]["Meter_CertWaterComp_TO_Meter"].ToString();
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_SerialNumber = table.Rows[0]["Meter_SerialNumber"].ToString();
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_IssueDate = (DateTime)table.Rows[0]["Meter_IssueDate"];
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_MeterNum = Convert.ToInt32(table.Rows[0]["Meter_MeterNum"]);
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_Diameter = Convert.ToInt32(table.Rows[0]["Meter_Diameter"]);
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_Origin = Convert.ToInt32(table.Rows[0]["Meter_Origin"]);
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_Model = table.Rows[0]["Meter_Model"].ToString();
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_Man = table.Rows[0]["Meter_Man"].ToString();
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_ChargeMode = Convert.ToInt32(table.Rows[0]["Meter_ChargeMode"]);
            unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data.Meter_Satatus = Convert.ToInt32(table.Rows[0]["Meter_Satatus"]);


            if (unifyWaterCard_Comm_Obj.Meter_SetWaterComany(BLL_WaterComp_Data))
            {
                //save last data to meter 
                status = DAL_MeterIssues_obj.Update(unifyWaterCard_Comm_Obj.BLL_MeterIssues_Data);

            }
            return status;
        }

        public DataTable GetSelectedMeter(int meterNum)
        {
            return DAL_MeterIssues_obj.SelectedMeter(meterNum);
        }
    }
}
