using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifyWaterCard.DataModels;
using UnifyWaterCard.Entities;
using UnifyWaterCard.Applets;

using UnifyWaterCard.Helpers;
using UnifyWaterCard.Security;
using System.Data;

namespace WaterMeter_id
{
	
    public class SEL_CardIssues
    {
		
		Secuirty Secuirty_obj = new Secuirty();
		unifyWaterCard_Comm unifyWaterCard_Comm_Obj = new unifyWaterCard_Comm();
		private DAL_CardIssues CardIssues_DAL = new DAL_CardIssues();
		public BLL_CardIssues BLL_CardIssues_Data = new BLL_CardIssues();
		public string[] GetCardproducer_CardIssues()
        {
			DAL_CardProd cardProducer_DAL = new DAL_CardProd();
			DataTable table = cardProducer_DAL.Select();
			string[] data = new string[table.Rows.Count];

			for (int index = 0; index < table.Rows.Count; index++)
			{
				data[index] = table.Rows[index]["CardProd_name"].ToString();
			}
			return data;

        }
		public string[] GetCardType_CardIssues()
        {
			DAL_CardType CardType_DAL = new DAL_CardType();
			DataTable table = CardType_DAL.Select();
			string[] data = new string[table.Rows.Count];

			for (int index = 0; index < table.Rows.Count; index++)
			{
				data[index] = table.Rows[index]["CardType_Code"].ToString();
			}
			return data;
		}
		
		public bool Create_CardIssues(string cardProducerName,string CardTypeCode, DateTime ExpirationDate)
        {
			
			
			bool status = false;
			try
			{
				unifyWaterCard_Comm_Obj.BLLCardIssues_Data.ExpiratonDate = ExpirationDate;
				bool  flage = unifyWaterCard_Comm_Obj.CardIssues_GetPK();
                if (flage)
                {
					DAL_CardProd CardProdcuerData = new DAL_CardProd();
					DataTable table = CardProdcuerData.Search(cardProducerName);
					string CardproduerPVK = "";
					int cardproducerid = 0;
					DAL_HoldingComp HoldingCompany_DAL = new DAL_HoldingComp();
					string HoldingPK = "";
					DAL_CardType CardType_DAL = new DAL_CardType();
					int CardTypeid = 0;
					if (table.Rows.Count >=1)
                    {
						CardproduerPVK= table.Rows[0]["CardProd_KPCP"].ToString();
						cardproducerid = Convert.ToInt32(table.Rows[0]["CardProd_ID"]);

					}
					 table = HoldingCompany_DAL.Select();
					if (table.Rows.Count >= 1)
					{
						HoldingPK = table.Rows[0]["Holding_Comp_KUH"].ToString();

					}

					table = CardType_DAL.SearchId(CardTypeCode);
					if (table.Rows.Count >= 1)
					{
						CardTypeid = Convert.ToInt32(table.Rows[0]["CardType_ID"]);

					}

					flage = HoldingPK != "" && CardproduerPVK != "";
					if (flage) {

						unifyWaterCard_Comm_Obj.BLLCardIssues_Data.CardNum = (int)(CardIssues_DAL.GetLastID()+1);

						if (unifyWaterCard_Comm_Obj.CardIssues_SetCardProducerCertificate(cardProducerName, CardproduerPVK, HoldingPK))
                        {  //if data writeen to card write data base 
							unifyWaterCard_Comm_Obj.BLLCardIssues_Data.CardProdID = cardproducerid;
							unifyWaterCard_Comm_Obj.BLLCardIssues_Data.CardTypeID = CardTypeid;
							status = CardIssues_DAL.Insert(unifyWaterCard_Comm_Obj.BLLCardIssues_Data);
							

						}



					}

				}

			



			}
			catch (Exception ex)
			{
                //	this.Log(ex);
                MessageBox.Show($"Failed to initialize card. Exception details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return status;
	    }


		public DataTable GetTable_Cardissues()
        {
			return CardIssues_DAL.SelectSpecificCardVariables();

		}
		public bool Clear_CardIssues()
        {
			return unifyWaterCard_Comm_Obj.Card_Clear();

		}
		public bool Read_CardIssues( )
        {
			bool status = false;
            if (unifyWaterCard_Comm_Obj.card_ReadBasic())
            {
				BLL_CardIssues_Data=unifyWaterCard_Comm_Obj.BLLCardIssues_Data;
				status = true;

			}
			return status;
        }
		public bool GetCardIssuesByNum(int num)
        {
			BLL_CardIssues_Data= CardIssues_DAL.CardIssueDataSelection(num);

            if (BLL_CardIssues_Data == null)
            {
				return false;
            }
			return true;
		}
	
		public DataTable GetCardIssueByCardNum(int cardnum)
		{
			return CardIssues_DAL.GetCardIssuanceByNum(cardnum);
		}
	}
}
