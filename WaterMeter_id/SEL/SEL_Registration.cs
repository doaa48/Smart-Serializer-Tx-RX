using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace WaterMeter_id
{
    internal class SEL_Registration
    {  //this layer used to set form of registration and dealing with hardware and data type 

		public BLL_HoldingComp   HoldingCompany_DataStr;
		public BLL_MeterManf     MeterManf_DataStr;
		public BLL_CardProd      CardProd_DataStr;
		public BLL_WaterComp     WaterComp_DataStr;
		Secuirty Secuirty_obj = new Secuirty();

		


		private DAL_HoldingComp HoldingCompany_DAL = new DAL_HoldingComp();
		private DAL_MeterManf meterManufacture_DAL = new DAL_MeterManf();
		private DAL_CardProd cardProducer_DAL = new DAL_CardProd();
		private DAL_WaterComp WaterCompany_DAL = new DAL_WaterComp();
		private void GenerateEntityRandomData(string entity,string name)
		{
			string Publickey = "";
			string Private_Key = "";
			Secuirty_obj.Secuirty_GenerateKeyPair(ref Publickey, ref Private_Key);
			if (!(entity == "H"))
			{
				if (!(entity == "X"))
				{
					if (!(entity == "F"))
					{
						if (entity == "CP")
						{
							
							this.CardProd_DataStr = new BLL_CardProd
							{
								CardProd_name = name
							};
							CardProd_DataStr.CardProd_KUCP = Publickey;
							CardProd_DataStr.CardProd_KPCP = Private_Key;
							
							//this.cardProducer.HCWWCertificate = "";
							//this.cardProducer.Kuh = "";
							//this.cardProducer = this._TestToolRepository.AddOrUpdateCardProducer(this.cardProducer);
							//this.FillCardProducerControles();
						}
					}
					else
					{
						this.MeterManf_DataStr = new BLL_MeterManf
						{
							MeterManf_Name = name
						};
						MeterManf_DataStr.MeterManf_KUF = Publickey;
						MeterManf_DataStr.MeterManf_KPF = Private_Key;
						
						//this.manufacturer.HCWWCertificate = "";
						//this.manufacturer.HCWWPK = "";
						//this.manufacturer = this._TestToolRepository.AddOrUpdateMeterManfactur(this.manufacturer);
						//this.FillManufacturerControles();
					}
				}
				else
				{

					this.WaterComp_DataStr = new BLL_WaterComp
					{
						WaterComp_Name = name
					};
					WaterComp_DataStr.WaterComp_KUW = Publickey;
					WaterComp_DataStr.WaterComp_KPW = Private_Key;
						

					//this.waterCompany.WaterCompanyCertificateByHCWW = this.hCWW.Certify(this.waterCompany.Name, this.waterCompany.KeyPair.PublicKey);
					//this.waterCompany.Kuh = this.hCWW.KeyPair.PublicKey;
					//this.waterCompany.Kuf = "";
					//this.waterCompany.Kucp = "";


					//.waterCompany = this._TestToolRepository.AddOrUpdateWaterCompany(this.waterCompany);
					//.FillWaterCompanyControles();
					//this.waterCompanyTableAdapter.Fill(this.testToolDBDataSet1.WaterCompany);

				}
			}
			else
			{
				this.HoldingCompany_DataStr = new BLL_HoldingComp
				{
					Holding_Comp_Name = name
				};
				
				
				HoldingCompany_DataStr.Holding_Comp_KUH = Publickey;
				HoldingCompany_DataStr.Holding_Comp_KPH= Private_Key;
			
				
			}
		}


		public void Generate_meterManufacture(string companyname,ref string pblickKey,ref string PrivateKey)

		{
			GenerateEntityRandomData("F", companyname);
			pblickKey = MeterManf_DataStr.MeterManf_KUF;
			PrivateKey = MeterManf_DataStr.MeterManf_KPF ;
			
				
		}
		public bool Register_meterManufacture(string companyname, string pblickKey, string PrivateKey)
		{
			MeterManf_DataStr.MeterManf_KUF = pblickKey;
			MeterManf_DataStr.MeterManf_KPF = PrivateKey;
			MeterManf_DataStr.MeterManf_Name = companyname;
			
			DataTable table = HoldingCompany_DAL.Select();
			MeterManf_DataStr.MeterManf_HoldingCompId = Convert.ToInt32(table.Rows[0]["HoldingComp_ID"]);
			string signer_Name = table.Rows[0]["Holding_Comp_Name"].ToString();
			String signer_PrivateKey = table.Rows[0]["Holding_Comp_KPH"].ToString();
			MeterManf_DataStr.MeterManf_CKUF = Secuirty_obj.CreateCertificate(signer_Name, signer_PrivateKey, MeterManf_DataStr.MeterManf_Name, MeterManf_DataStr.MeterManf_KUF);
			return meterManufacture_DAL.Insert(MeterManf_DataStr);


		}
		public DataTable GetTable_meterManufacture()
		{
			
			return meterManufacture_DAL.Select();
			
		}

		public void Generate_cardProducere(string companyname, ref string pblickKey, ref string PrivateKey)

		{
			GenerateEntityRandomData("CP", companyname);
			pblickKey = CardProd_DataStr.CardProd_KUCP;
			PrivateKey = CardProd_DataStr.CardProd_KPCP;


		}
		public BLL_HoldingComp GetHoldingDataByName ()
		{
			return HoldingCompany_DAL.GetHoldingCompData();

        }

		public bool Register_cardProducer(string companyname, string pblickKey, string PrivateKey)
		{
			CardProd_DataStr.CardProd_KUCP = pblickKey;
			CardProd_DataStr.CardProd_KPCP = PrivateKey;
			CardProd_DataStr.CardProd_name = companyname;

			DataTable table = HoldingCompany_DAL.Select();
			CardProd_DataStr.CardProd_HoldingCompId = Convert.ToInt32(table.Rows[0]["HoldingComp_ID"]);
			string signer_Name = table.Rows[0]["Holding_Comp_Name"].ToString();
			String signer_PrivateKey = table.Rows[0]["Holding_Comp_KPH"].ToString();
			CardProd_DataStr.CardProd_CKUCP = Secuirty_obj.CreateCertificate(signer_Name, signer_PrivateKey, CardProd_DataStr.CardProd_name, CardProd_DataStr.CardProd_KUCP);
			return cardProducer_DAL.Insert(CardProd_DataStr);
		}
		public DataTable GetTable_cardProducer()
		{
		   return  cardProducer_DAL.Select();
			
		}
		public void Register_HoldingCompany(string companyname)
		{
			GenerateEntityRandomData("H", companyname);
			HoldingCompany_DAL.Insert(HoldingCompany_DataStr);
		}
		public DataTable GetTable_HoldingCompany()
		{
			return HoldingCompany_DAL.Select();
		
		}
		public void Register_WaterCompany(string companyname)
		{
			GenerateEntityRandomData("X", companyname);

			DataTable table=HoldingCompany_DAL.Select();
			WaterComp_DataStr.WaterComp_HoldingCompId = Convert.ToInt32(table.Rows[0]["HoldingComp_ID"]);
			string signer_Name =table.Rows[0]["Holding_Comp_Name"].ToString(); 
			String signer_PrivateKey = table.Rows[0]["Holding_Comp_KPH"].ToString();
			WaterComp_DataStr.WaterComp_CertHoldingComp_TO_Watercomp = Secuirty_obj.CreateCertificate(signer_Name, signer_PrivateKey, WaterComp_DataStr.WaterComp_Name, WaterComp_DataStr.WaterComp_KUW);
			WaterCompany_DAL.Insert(WaterComp_DataStr);
		}
		public DataTable GetTable_WaterCompany()
		{
			return  WaterCompany_DAL.Select();
			
		}

		public BLL_WaterComp GetSelectedWaterCompany(int WaterCompID)
		{
			return WaterCompany_DAL.GetWaterCompDataByID(WaterCompID);
		}

        public BLL_WaterComp GetSelectedWaterCompany(string WaterCompName)
        {
            return WaterCompany_DAL.GetWaterCompDataByName(WaterCompName);
        }

		public BLL_MeterManf GetSelectedMeterManufacture(string MeterManfName)
		{
			return meterManufacture_DAL.GetMeterManfData(MeterManfName);

        }

		public BLL_CardProd GetSelectedCardProduct(string CardProd)
		{
			return cardProducer_DAL.GetCardProdDataByName(CardProd);

        }
    }
}
