using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaterMeter_id
{
    public class SEL_ConfigurationCard
    {
        public BLL_ConfigCard BLLConfigCardData = new BLL_ConfigCard();
        public BLL_CardIssues BLL_CardIssues_Data = new BLL_CardIssues();
        DAL_ConfigCard DALConfigCardObj = new DAL_ConfigCard();
        unifyWaterCard_Comm UnifyCard_Object = new unifyWaterCard_Comm();
        public List<BLL_CompanyCards_ReturnRead> MeterList= new List<BLL_CompanyCards_ReturnRead>();

        public int GetMinStartCustomerID()
        {
            DAL_Client OBject = new DAL_Client();
            return OBject.GetMINClientNum();
        }
        public int GetMaxEndCustomerID()
        {
            DAL_Client OBject = new DAL_Client();
            return OBject.GetMAXClientNum();
        }
        public bool Set_ConfigCard()
        {
            bool status = false; //this function using for write 
                                 //read card first 
                                 //show if card not clear
                                 //set data to card 
                                 //with two certificates 
            status = UnifyCard_Object.ConfigurationCard_WriteCard(BLLConfigCardData);  //update card 

            return status;
        }
        public bool Read_ConfigCard()
        {
            bool status = false;
            status= UnifyCard_Object.ConfigurationCard_ReadCard(ref MeterList);
            BLLConfigCardData = UnifyCard_Object.BLL_Configuration_Data;
            BLL_CardIssues_Data = UnifyCard_Object.BLLCardIssues_Data;
            return status;

        }

    }
}
