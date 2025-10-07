using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{

  
    public class SEL_RetrivalCard
    {
        public BLL_RetrivalCard BLLRetrivalData = new BLL_RetrivalCard();
        public BLL_CardIssues BLL_CardIssues_Data = new BLL_CardIssues();
        DAL_RetrivalCard    DAL_RetrivalCard_Object = new DAL_RetrivalCard();
        unifyWaterCard_Comm UnifyCard_Object = new unifyWaterCard_Comm();
         public  List<BLL_RetrivalCard_ReturnRead> RetrivalReturn_Data = new List<BLL_RetrivalCard_ReturnRead>();
         public  List<BLL_CompanyCards_ReturnRead> MainReturn_Data = new List<BLL_CompanyCards_ReturnRead>();

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
        public bool Set_RetrivslCard()
        {
            bool status = false; //this function using for write 
                                 //read card first 
                                 //show if card not clear
                                 //set data to card 
                                 //with two certificates 
            status = UnifyCard_Object.RetrivalCard_WriteCard(BLLRetrivalData);  //update card 

           

            return status;
        }
        public bool Read_RetrivslCard()
        {
            bool status = false;


            status = UnifyCard_Object.RetrivalCard_ReadCard(ref RetrivalReturn_Data, ref MainReturn_Data);  //update card 

            

            BLL_CardIssues_Data= UnifyCard_Object.BLLCardIssues_Data;
            BLLRetrivalData = UnifyCard_Object.BLL_RetrivalCard_Data;
            return status;

        }

    }
}
