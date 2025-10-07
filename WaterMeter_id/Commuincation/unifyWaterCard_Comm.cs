using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifyWaterCard.DataModels;
using UnifyWaterCard.Entities;
using UnifyWaterCard.Helpers;

namespace WaterMeter_id
{

	
	
	public class unifyWaterCard_Comm
    {
		//this function used to setup main data of card in card issues and setup function of it 

		private CardBase cardBase;
		private byte[] System_Nounce = new byte[2];
		private byte[] Card_Nounce = new byte[2];
		Secuirty Secuirty_Obj = new Secuirty();


		public BLL_Client BLL_Client_Data = new BLL_Client();
		public BLL_CardIssues BLLCardIssues_Data = new BLL_CardIssues();
		public BLL_MeterIssues BLL_MeterIssues_Data = new BLL_MeterIssues();
		public BLL_ClientInfo BLL_ClientInfo_Data = new BLL_ClientInfo();
		public BLL_ChargeBasicInf BLL_ChargeBasicInf_Data = new BLL_ChargeBasicInf();
		public BLL_PriceScheduler BLL_PriceScheduler_Data = new BLL_PriceScheduler();
		public BLL_Offtimes BLL_Offtimes_Obj = new BLL_Offtimes();
		public BLL_Deductions BLL_Deductions_Data = new BLL_Deductions();
		public BLL_ConfigCard BLL_Configuration_Data = new BLL_ConfigCard();
		public BLL_MaintCard BLL_MaintCard_Data = new BLL_MaintCard();
		public BLL_Readings BLL_Readings_Data = new BLL_Readings();
		public BLL_CreditBalance BLL_CreditBalance_Data = new BLL_CreditBalance();
		public BLL_MeterState BLL_MeterState_Data = new BLL_MeterState();
		public BLL_RetrivalCard BLL_RetrivalCard_Data = new BLL_RetrivalCard();
		public bool CardIssues_GetPK( )
        {
            bool Status = false;
			DAL_CardIssues CardIssues_DAL = new DAL_CardIssues();

			int randomNumber = (int)(CardIssues_DAL.GetLastID() + 1);  //hint if you want to setup any numb er as you want 

			BLLCardIssues_Data.CardNum = randomNumber;
			BLLCardIssues_Data.Func = CardFunctionEnum.CardInitialization.ToByte();
			BLLCardIssues_Data.Mode = (byte)CardModeEnum.New;
			BLLCardIssues_Data.IssueDate = DateTime.Now;
			BLLCardIssues_Data.LastTranscationDate = DateTime.Now;
			BLLCardIssues_Data.LastUploadBy = (byte)LastUpdatedBy.System;
			try
			{				
				cardBase = new CardBase
				{
					MainCardInfo = new MainCardInfo
					{
						CardFun = BLLCardIssues_Data.Func,
						Id = (uint)BLLCardIssues_Data.CardNum,
						Mode = BLLCardIssues_Data.Mode,
						IssueDate = DateModel.Parse(BLLCardIssues_Data.IssueDate.ToString()),
						LastTransactionDate = DateModel.Parse(BLLCardIssues_Data.LastTranscationDate.ToString()),
						LastUpdatedBy = (LastUpdatedBy)BLLCardIssues_Data.LastUploadBy,


						 ExpirationDate= DateModel.Parse(BLLCardIssues_Data.ExpiratonDate.ToString())
					}
				};
				byte[] response = APDUHELPER.ExecuteAPDU(214, 58, BERTLVPayloadBuilder.GeneratePayload(BitConverter.GetBytes(cardBase.MainCardInfo.Id), null, new ModelBase[]
				{
					cardBase.MainCardInfo
				}));
				bool flag = APDUHELPER.GetApduResponseEnum(response) == ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					byte[] array = APDUHELPER.ExecuteAPDU(176, 66, null);
					bool flag2 = APDUHELPER.GetApduResponseEnum(array) != ApduResponseEnum.SuccessResponse;
					if (flag2)
					{
						MessageBox.Show("Fail To Init Card");
					}
					else
					{
						APDUData apdudata = APDUData.ParseTLV(Enumerable.ToArray<byte>(Enumerable.Take<byte>(array, array.Length - 2)));
						Tlv tlv = Enumerable.FirstOrDefault<Tlv>(apdudata.Models, (Tlv m) => m.Tag == 216);
						CardPublicKey cardPublicKey = CardPublicKey.Deserialize((tlv != null) ? tlv.Value : null);
						cardBase.PublicKey = GeneralUtility.ByteArrayToHex(cardPublicKey.PublicKey);
						cardBase.PublicKey = (cardBase.PublicKey.StartsWith("04") ? cardBase.PublicKey.Substring(2) : cardBase.PublicKey);
						///pass pblic key of card 
						BLLCardIssues_Data.PK = cardBase.PublicKey;

						Status = true;
					}
				}
			}
			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("Fail To Init Card");
			}




			return Status;

        }

		public bool CardIssues_SetCardProducerCertificate( string cardProducerName,string cardproducerPVK,string HoldingPuk)
		{

			//
			bool Status=false;
			try
			{



				BLLCardIssues_Data.CertCardProd_TO_Card = Signer.GenerateX509Certificate(cardProducerName, cardproducerPVK, "Card", cardBase.PublicKey, null);
				CardProducerToCardCertificate cardProducerToCardCertificate = new CardProducerToCardCertificate
				{
					Certificate = GeneralUtility.HexToByteArray(BLLCardIssues_Data.CertCardProd_TO_Card)
				};

				
				cardBase.CardProducerId = (uint)BLLCardIssues_Data.CardProdID;
				cardBase.CardCertificateByCardProducer = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);

				
				HCWWPublicKey hcwwpublicKey = new HCWWPublicKey
				{
					PublicKey = GeneralUtility.HexToByteArray(HoldingPuk)
				};
				cardBase.HCWWPublicKey = HoldingPuk;
				BLLCardIssues_Data.Mode=cardBase.MainCardInfo.Mode = (byte)DataEnum.ECard_Mode.Initialized_Mode;
				BLLCardIssues_Data.Func=cardBase.MainCardInfo.CardFun = (byte)DataEnum.ECard_Function.Initialization_Card;
				cardBase.MainCardInfo.Id =(uint) BLLCardIssues_Data.CardNum;


				byte[] response2 = APDUHELPER.ExecuteAPDU(214, 57, BERTLVPayloadBuilder.GeneratePayload(BitConverter.GetBytes(cardBase.MainCardInfo.Id), null, new ModelBase[]
				{
							cardBase.MainCardInfo,
							cardProducerToCardCertificate,
							hcwwpublicKey
				}));
				bool flag3 = APDUHELPER.GetApduResponseEnum(response2) == ApduResponseEnum.SuccessResponse;
				if (flag3)
				{
					byte[] array2 = APDUHELPER.ExecuteAPDU(176, 15, null);
					bool flag4 = APDUHELPER.GetApduResponseEnum(array2) != ApduResponseEnum.SuccessResponse;
					if (flag4)
					{
						MessageBox.Show("Fail To Read Card");
					}
					else
					{
						APDUData apdudata2 = APDUData.ParseTLV(Enumerable.ToArray<byte>(Enumerable.Take<byte>(array2, array2.Length - 2)));
						CardBase ReturncardBase = cardBase;
						Tlv tlv2 = Enumerable.FirstOrDefault<Tlv>(apdudata2.Models, (Tlv m) => m.Tag == 197);
						ReturncardBase.MainCardInfo = MainCardInfo.Deserialize((tlv2 != null) ? tlv2.Value : null);
						Status = true;

						//	this.cardBase = this._TestToolRepository.AddOrUpdateCard(this.cardBase);
						//	this.cardsTableAdapter.Fill(this.testToolDBDataSet1.Cards);
						//	this.IDc.Text = this.cardBase.MainCardInfo.Id.ToString();
						//	this.Kuc.Text = this.cardBase.PublicKey;
						//	this.CKuc.Text = this.cardBase.CardCertificateByCardProducer;
						//	this.CKuh.Text = this.cardBase.HCWWPublicKey;
						//	MessageBox.Show("Card Initialized By Card Producer Successfully");
					}

				}
			}
			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("Fail To Init Card");
			}



			return Status;
		}

		public bool Card_Clear()
        {
			bool status = false;
			byte[] response = APDUHELPER.ExecuteAPDU(224, 0, null);
			bool flag = APDUHELPER.GetApduResponseEnum(response) == ApduResponseEnum.SuccessResponse;
			if (flag)
			{
				
				MessageBox.Show("Clear Card Successfully ");
			}
			else
			{
				bool flag2 = APDUHELPER.GetApduResponseEnum(response) == ApduResponseEnum.CanNotConnectToCard;
				if (flag2)
				{
					MessageBox.Show("Please put card on reader");
				}
				else
				{
					MessageBox.Show("Failed To Clear Card");
				}
			}
			return status;
		}


		private int GetRandomNumber()
		{
			return new Random().Next(1, int.MaxValue);
		}
		public bool card_ReadBasic()
        {
			bool status = false;

			
				try
				{
					int Rannum = GetRandomNumber();

					this.System_Nounce[0] = (byte)(Rannum & 0xff);
					this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
					//mostafa
					byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);
					byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);

					bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
					if (flag)
					{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());
						this.cardBase = new CardBase();
						CardBase cardBase = this.cardBase;
						Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
						cardBase.MainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
						Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
						CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);

						Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
						Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

						Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
						SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);



					     BLLCardIssues_Data.PK = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
					     BLLCardIssues_Data.CertCardProd_TO_Card = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);


					    BLLCardIssues_Data.CardNum = (int)this.cardBase.MainCardInfo.Id;

					DAL_CardIssues Object = new DAL_CardIssues();
					BLLCardIssues_Data = Object.CardIssueDataSelection((int)this.cardBase.MainCardInfo.Id);
					if (BLLCardIssues_Data ==null)
					{

						MessageBox.Show("this card not inserted befor  to databse...re-install ");
						return false;
					}
				


					BLLCardIssues_Data.Func = this.cardBase.MainCardInfo.CardFun;
					    BLLCardIssues_Data.Mode = this.cardBase.MainCardInfo.Mode;
					    BLLCardIssues_Data.CardProdID = this.cardBase.MainCardInfo.CompanyId;
					    BLLCardIssues_Data .IssueDate= DateModel.GetSystemDate(this.cardBase.MainCardInfo.IssueDate);
					    BLLCardIssues_Data .ExpiratonDate= DateModel.GetSystemDate(this.cardBase.MainCardInfo.ExpirationDate);
					    BLLCardIssues_Data.LastTranscationDate = DateModel.GetSystemDate(this.cardBase.MainCardInfo.LastTransactionDate);
					    BLLCardIssues_Data.LastUploadBy = (byte)this.cardBase.MainCardInfo.LastUpdatedBy;
					    status = true;
						
					}
					else
					{
						bool flag2 = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.CanNotConnectToCard;
						if (flag2)
						{
							MessageBox.Show("Please put card on reader");
						}
						else
						{
							MessageBox.Show("Fail To Read Issue Card");
						}
					}
				}
				catch (Exception ex)
				{
					//this.Log(ex);
					MessageBox.Show("Fail To Read Issue Card");
				}
			
			return status;
		}

		///meter issues functions 
		///private void InitMeter_Click(object sender, EventArgs e)


		private bool IsCardCleared(MainCardInfo mainCardInfo)
		{
			return mainCardInfo.CardFun == 0 && mainCardInfo.Mode == 0;
		}
		public bool CardIsClear(BLL_CardIssues BLLCardIssues_Data)
		{
			return BLLCardIssues_Data.Func == 0 && BLLCardIssues_Data.Mode == 0;
		}
		public bool Meter_InitIssues()
        {
			bool Status = false;
			byte[] array = APDUHELPER.ExecuteAPDU(176, 15, null);
		     bool flag = APDUHELPER.GetApduResponseEnum(array) != ApduResponseEnum.SuccessResponse;
			if (flag)
			{
				MessageBox.Show("Fail To Read Card");
			}
			else
			{
				APDUData apdudata = APDUData.ParseTLV(Enumerable.ToArray<byte>(Enumerable.Take<byte>(array, array.Length - 2)));
	            Tlv tlv = Enumerable.FirstOrDefault<Tlv>(apdudata.Models, (Tlv m) => m.Tag == 197);
	            MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
	            bool flag2 = !this.IsCardCleared(mainCardInfo);
				if (flag2)
				{
					MessageBox.Show("Please Clear Card First");
				}
				else
				{
	               mainCardInfo.CardFun = CardFunctionEnum.MeterIssuing.ToByte();
	               mainCardInfo.Mode = CardModeEnum.New.ToByte();
	               mainCardInfo.IssueDate = DateModel.Parse(DateTime.Now.ToString());
	               mainCardInfo.LastTransactionDate = DateModel.Parse(DateTime.Now.ToString());
	               mainCardInfo.LastUpdatedBy = LastUpdatedBy.System;
	               byte[] response = APDUHELPER.ExecuteAPDU(214, 40, BERTLVPayloadBuilder.GeneratePayload(BitConverter.GetBytes(mainCardInfo.Id), null, new ModelBase[]
	               {
						mainCardInfo
	               }));
	               bool flag3 = APDUHELPER.GetApduResponseEnum(response) == ApduResponseEnum.SuccessResponse;
	               if (flag3)
	               {
		              MessageBox.Show("Data Written Successfully to Card");
						Status = true;

				   }
                }
			}
			return Status;
		}

		public bool Meter_ReadIssues()
        {
			bool status = false;
			try
			{
				byte[] array = APDUHELPER.ExecuteAPDU(176, 15, null);
				bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					APDUData apdudata = APDUData.ParseTLV(Enumerable.ToArray<byte>(Enumerable.Take<byte>(array, array.Length - 2)));
					Tlv tlv = Enumerable.FirstOrDefault<Tlv>(apdudata.Models, (Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);

					bool flag2 = mainCardInfo.CardFun == 6;
					if (flag2)
					{
						bool flag3 = mainCardInfo.Mode == 7;
						if (flag3)
						{
							byte[] array2 = APDUHELPER.ExecuteAPDU(176, 64, null);
							bool flag4 = APDUHELPER.GetApduResponseEnum(array2) == ApduResponseEnum.SuccessResponse;
							if (flag4)
							{
								apdudata = APDUData.ParseTLV(Enumerable.ToArray<byte>(Enumerable.Take<byte>(array2, array2.Length - 2)));
								Tlv tlv2 = Enumerable.FirstOrDefault<Tlv>(apdudata.Models, (Tlv m) => m.Tag == 199);
								MeterInfo meterInfo = MeterInfo.Deserialize((tlv2 != null) ? tlv2.Value : null);
								UnifyWaterCard.Entities.Meter meter = new UnifyWaterCard.Entities.Meter();
								meter.MeterInfo = meterInfo;

								BLL_MeterIssues_Data.Meter_MeterNum = (int)meterInfo.MeterId;

								BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
								BLL_MeterIssues_Data.Meter_Origin = meterInfo.MeterOrigin;
								BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
								BLL_MeterIssues_Data.Meter_Model = meterInfo.MeterModel.ToString();
								BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;
								Tlv tlv3 = Enumerable.FirstOrDefault<Tlv>(apdudata.Models, (Tlv m) => m.Tag == 213);
								ManufacturerToMeterCertificate manufacturerToMeterCertificate = ManufacturerToMeterCertificate.Deserialize((tlv3 != null) ? tlv3.Value : null);
								meter.ManufacturerToMeterCertificate = GeneralUtility.ByteArrayToHex(manufacturerToMeterCertificate.Certificate);
								BLL_MeterIssues_Data.Meter_CertMeterManf_TO_Meter = meter.ManufacturerToMeterCertificate;
								meter.PublicKey = Signer.ExtractPublicKeyFromCertificate(manufacturerToMeterCertificate.Certificate);
								BLL_MeterIssues_Data.Meter_PK = meter.PublicKey;
								status = true;
							}
							else
							{
								MessageBox.Show("Fail To Read Card " + APDUHELPER.GetApduResponseEnum(array2).ToString());
							}
						}
						else
						{
							MessageBox.Show("Put Card On meter");
						}
					}
					else
					{
						MessageBox.Show("Fail Card Not Issue Meter Card");
					}
				}
				else
				{
					MessageBox.Show("Fail To Read Card " + APDUHELPER.GetApduResponseEnum(array).ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Fail To Read Issue Meter Card");
			}

			return status;
        } 
	   
	   public bool Meter_VerifyCetrtManfactureToMeter(string CertToVerify, string SingerPublickey)
        {
			bool Status = false;
			if (SingerPublickey != "" && CertToVerify != "")
			{
				Status = Signer.VerifyX509Certificate(CertToVerify, SingerPublickey);
			}
			return Status;
        }
	
	     public bool ConfigurationCard_WriteCard(BLL_ConfigCard ConfigData)
        {
			bool Status = false;
			try
			{
				int Rannum = GetRandomNumber();

				this.System_Nounce[0] = (byte)(Rannum & 0xff);
				this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
				//mostafa
				byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);
				byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);

				bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());
					
					Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
					CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);

					Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
					Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

					Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
					SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);




			
					
					//CardPublicKey of card from number that received in data batabase
					DAL_CardIssues CardIssues_DAL = new DAL_CardIssues();
					BLLCardIssues_Data = CardIssues_DAL.CardIssueDataSelection((int)mainCardInfo.Id);

					bool flag3 = !Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLLCardIssues_Data.CardProd_KUCP);
					if (flag3)
					{
						MessageBox.Show("Fail To Verify CardProducerToCardCertificate Against Card Producer Public Key");
					}
                      flag3 = !this.IsCardCleared(mainCardInfo);
					if (flag3)
					{
						MessageBox.Show("Please Clear Card First");
					}
					else
					{
						string targetEntityPublicKey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
						string text = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);
						DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();
						BLL_WaterComp BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();

						HCWWToWaterCompanyCertificate hcwwtoWaterCompanyCertificate2 = new HCWWToWaterCompanyCertificate
						{
							Certificate = GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp)
						};


					
					BLLCardIssues_Data.CertWaterComp_TO_Card = Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_Name, BLL_WaterComp_Data.WaterComp_KPW, "card", targetEntityPublicKey);
					WaterCompanyToCardCertificate waterCompanyToCardCertificate = new WaterCompanyToCardCertificate
					{
						Certificate = GeneralUtility.HexToByteArray(BLLCardIssues_Data.CertWaterComp_TO_Card)
						//GeneralUtility.HexToByteArray(waterCompanyById.Certify((string)"waterComp", CardPublickey)) 
					};
						BLLCardIssues_Data.Func= mainCardInfo.CardFun = CardFunctionEnum.ConfigureMeter.ToByte();
						mainCardInfo.Mode = CardModeEnum.Active.ToByte();
						mainCardInfo.IssueDate = DateModel.Parse(BLLCardIssues_Data.IssueDate.ToString());

						mainCardInfo.LastTransactionDate = DateModel.Parse(DateTime.Now.ToString());
						mainCardInfo.ExpirationDate = DateModel.Parse(BLLCardIssues_Data.ExpiratonDate.ToString());
						mainCardInfo.LastUpdatedBy = LastUpdatedBy.System;
						mainCardInfo.CompanyId = (byte)BLL_WaterComp_Data.WaterComp_Id;

						BLLCardIssues_Data.Func = mainCardInfo.CardFun;
						BLLCardIssues_Data.Mode = mainCardInfo.Mode;
						BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
						BLLCardIssues_Data.LastUploadBy = (byte)mainCardInfo.LastUpdatedBy;
						BLLCardIssues_Data.LastTranscationDate = DateTime.Now;
						

						ConfigurationCardInfo configurationCardInfo = new ConfigurationCardInfo
						{
							Actions = (ushort)ConfigData.ConfigCard_MeterAction,
							StartConsumerID = (uint)ConfigData.ConfigCard_StartConsumerID,
							EndConsumerID = (uint)ConfigData.ConfigCard_EndConsumerID,
							ResetDate = DateModel.Parse(ConfigData.ConfigCard_RestDate.ToString()),
							TimesEffective = (ushort)ConfigData.ConfigCard_TimeEffective
						};

						byte[] payloadDataCertification = BERTLVPayloadBuilder.GeneratePayload(null, null, null, null, BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), new ModelBase[]
									  { 
										  
								    mainCardInfo,
									configurationCardInfo,
									waterCompanyToCardCertificate,
									hcwwtoWaterCompanyCertificate2

									  });


						byte[] response2 =APDUHELPER.ExecuteAPDU(214, 64, payloadDataCertification);
						bool flag7 = APDUHELPER.GetApduResponseEnum(response2) == ApduResponseEnum.SuccessResponse;
						if (flag7)
						{
							CardIssues_DAL.Update(BLLCardIssues_Data);
							ConfigData.ConfigCard_CardID = BLLCardIssues_Data.id;
							ConfigData.ConfigCard_IssueDate = DateTime.Now;
							DAL_ConfigCard Object = new DAL_ConfigCard();
							Status = Object.Insert(ConfigData);
						}
						else
						{
							MessageBox.Show("Fail to Data Write Card");
						}
						//complete data and update last card
						//

					}
				}
			}
			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("SYSTEM_ERROR");
			}
			return Status;
		


		}



		public bool RetrivalCard_WriteCard(BLL_RetrivalCard RetrivalData)
		{
			bool Status = false;
			try
			{
				int Rannum = GetRandomNumber();

				this.System_Nounce[0] = (byte)(Rannum & 0xff);
				this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
				//mostafa
				byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);
				byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);

				bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());

					Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
					CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);

					Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
					Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

					Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
					SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);






					//CardPublicKey of card from number that received in data batabase
					DAL_CardIssues CardIssues_DAL = new DAL_CardIssues();
					BLLCardIssues_Data = CardIssues_DAL.CardIssueDataSelection((int)mainCardInfo.Id);

					bool flag3 = !Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLLCardIssues_Data.CardProd_KUCP);
					if (flag3)
					{
						MessageBox.Show("Fail To Verify CardProducerToCardCertificate Against Card Producer Public Key");
					}
					flag3 = !this.IsCardCleared(mainCardInfo);
					if (flag3)
					{
						MessageBox.Show("Please Clear Card First");
					}
					else
					{
						string targetEntityPublicKey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
						string text = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);
						DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();
						BLL_WaterComp BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();

						HCWWToWaterCompanyCertificate hcwwtoWaterCompanyCertificate2 = new HCWWToWaterCompanyCertificate
						{
							Certificate = GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp)
						};



						BLLCardIssues_Data.CertWaterComp_TO_Card = Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_Name, BLL_WaterComp_Data.WaterComp_KPW, "card", targetEntityPublicKey);
						WaterCompanyToCardCertificate waterCompanyToCardCertificate = new WaterCompanyToCardCertificate
						{
							Certificate = GeneralUtility.HexToByteArray(BLLCardIssues_Data.CertWaterComp_TO_Card)
							//GeneralUtility.HexToByteArray(waterCompanyById.Certify((string)"waterComp", CardPublickey)) 
						};
						mainCardInfo.CardFun = CardFunctionEnum.RetrieveMeterInfo.ToByte();
						mainCardInfo.Mode = CardModeEnum.Active.ToByte();
						mainCardInfo.IssueDate = DateModel.Parse(BLLCardIssues_Data.IssueDate.ToString());

						mainCardInfo.LastTransactionDate = DateModel.Parse(DateTime.Now.ToString());
						mainCardInfo.ExpirationDate = DateModel.Parse(BLLCardIssues_Data.ExpiratonDate.ToString());
						mainCardInfo.LastUpdatedBy = LastUpdatedBy.System;
						mainCardInfo.CompanyId = (byte)BLL_WaterComp_Data.WaterComp_Id;

						BLLCardIssues_Data.Func = mainCardInfo.CardFun;
						BLLCardIssues_Data.Mode = mainCardInfo.Mode;
						BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
						BLLCardIssues_Data.LastUploadBy = (byte)mainCardInfo.LastUpdatedBy;
						BLLCardIssues_Data.LastTranscationDate = DateTime.Now;


						RetrievalCardInfo retrievalCardInfo = new RetrievalCardInfo
						{
							Actions = (ushort)RetrivalData.RetrivalCard_RequiredData,
							StartConsumerID = (uint)RetrivalData.RetrivalCard_StartConsumerID,
							EndConsumerID = (uint)RetrivalData.RetrivalCard_EndConsumerID,
							TimesEffective = (ushort)RetrivalData.RetrivalCard_TimeEffective
						};

						byte[] payloadDataCertification = BERTLVPayloadBuilder.GeneratePayload(null, null, null, null, BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), new ModelBase[]
									  {

									mainCardInfo,
									retrievalCardInfo,
									waterCompanyToCardCertificate,
									hcwwtoWaterCompanyCertificate2
								
									

									  });


						byte[] response2 = APDUHELPER.ExecuteAPDU(214, 71, payloadDataCertification);
						bool flag7 = APDUHELPER.GetApduResponseEnum(response2) == ApduResponseEnum.SuccessResponse;
						if (flag7)
						{
							CardIssues_DAL.Update(BLLCardIssues_Data);
							RetrivalData.RetrivalCard_CardID = BLLCardIssues_Data.id;
							RetrivalData.RetrivalCard_IssueDate = DateTime.Now;

							DAL_RetrivalCard Object = new DAL_RetrivalCard();

							Status = Object.Insert(RetrivalData);
						
						}
						else
						{
							MessageBox.Show("Fail to Data Write Card");
						}
						//complete data and update last card
						//

					}
				}
			}
			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("SYSTEM_ERROR");
			}
			return Status;



		}

		public bool MaintCard_WriteCard(BLL_MaintCard MainData)
		{
			bool Status = false;
			try
			{
				int Rannum = GetRandomNumber();

				this.System_Nounce[0] = (byte)(Rannum & 0xff);
				this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
				//mostafa
				byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);
				byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);

				bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());

					Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
					CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);

					Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
					Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

					Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
					SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);






					//CardPublicKey of card from number that received in data batabase
					DAL_CardIssues CardIssues_DAL = new DAL_CardIssues();
					BLLCardIssues_Data = CardIssues_DAL.CardIssueDataSelection((int)mainCardInfo.Id);

					bool flag3 = !Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLLCardIssues_Data.CardProd_KUCP);
					if (flag3)
					{
						MessageBox.Show("Fail To Verify CardProducerToCardCertificate Against Card Producer Public Key");
					}
					flag3 = !this.IsCardCleared(mainCardInfo);
					if (flag3)
					{
						MessageBox.Show("Please Clear Card First");
					}
					else
					{
						string targetEntityPublicKey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
						string text = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);
						DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();
						BLL_WaterComp BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();

						HCWWToWaterCompanyCertificate hcwwtoWaterCompanyCertificate2 = new HCWWToWaterCompanyCertificate
						{
							Certificate = GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp)
						};



						BLLCardIssues_Data.CertWaterComp_TO_Card = Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_Name, BLL_WaterComp_Data.WaterComp_KPW, "card", targetEntityPublicKey);
						WaterCompanyToCardCertificate waterCompanyToCardCertificate = new WaterCompanyToCardCertificate
						{
							Certificate = GeneralUtility.HexToByteArray(BLLCardIssues_Data.CertWaterComp_TO_Card)
							//GeneralUtility.HexToByteArray(waterCompanyById.Certify((string)"waterComp", CardPublickey)) 
						};
						mainCardInfo.CardFun = CardFunctionEnum.MaintainMeter.ToByte();
						mainCardInfo.Mode = CardModeEnum.Active.ToByte();
						mainCardInfo.IssueDate = DateModel.Parse(BLLCardIssues_Data.IssueDate.ToString());

						mainCardInfo.LastTransactionDate = DateModel.Parse(DateTime.Now.ToString());
						mainCardInfo.ExpirationDate = DateModel.Parse(BLLCardIssues_Data.ExpiratonDate.ToString());
						mainCardInfo.LastUpdatedBy = LastUpdatedBy.System;
						mainCardInfo.CompanyId = (byte)BLL_WaterComp_Data.WaterComp_Id;

						BLLCardIssues_Data.Func = mainCardInfo.CardFun;
						BLLCardIssues_Data.Mode = mainCardInfo.Mode;
						BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
						BLLCardIssues_Data.LastUploadBy = (byte)mainCardInfo.LastUpdatedBy;
						BLLCardIssues_Data.LastTranscationDate = DateTime.Now;





						PriceSchedule priceSchedule = new PriceSchedule();
						priceSchedule.AppDate = DateModel.Parse(BLL_PriceScheduler_Data.PriceSchedule_APPDate.ToString());

						priceSchedule.MonthFee1 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee1;
						priceSchedule.MonthFee2 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee2;

						priceSchedule.MonthFeesOptions = (byte)BLL_PriceScheduler_Data.PriceSchedule_MonthFeeOption;
						priceSchedule.NoOfUnitsInCalc = ((byte)(BLL_PriceScheduler_Data.PriceSchedule_NoOFUintsCalc));
						priceSchedule.PerMeterFees = (ushort)BLL_PriceScheduler_Data.PriceSchedule_PerMeterFee;

						priceSchedule.Pricing = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Pricing;

						priceSchedule.SewagePrice = (ushort)BLL_PriceScheduler_Data.PriceSchedule_SWGPrice;
						priceSchedule.SewagePercentage = (byte)BLL_PriceScheduler_Data.PriceSchedule_SWGPercent;
						try
						{
							priceSchedule.Levels = new PriceScheduleLevel[]
							{
							new PriceScheduleLevel
							{
								StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Fee
							},
							new PriceScheduleLevel
							{
								StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Fee
							}
								};
						}
						catch
						{
							MessageBox.Show("Enter a valid PriceScheduleLevel.", "Error");
							return Status;
						}


						OffTimes offTimes = new OffTimes();
						offTimes.CutoffTime = (byte)BLL_Offtimes_Obj.OFFTime_CutOffTime;
						offTimes.GracePeriod = (byte)BLL_Offtimes_Obj.OFFTime_GracePeriod;
						offTimes.WorkStart = (byte)BLL_Offtimes_Obj.OFFTime_WorkStart;
						offTimes.WorkEnd = (byte)BLL_Offtimes_Obj.OFFTime_WorkEnd;

						offTimes.WorkingDays = (byte)BLL_Offtimes_Obj.OFFTime_WorkingDays;
						List<Holiday> list = new List<Holiday>();

						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Day
						});


						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Day
						});
						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Day
						});

						list.Add(new Holiday
						{
							Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Month,
							Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Day
						});

						offTimes.Holidays = list.ToArray();



						Deductions deductions = new Deductions();
						deductions.AppDate = DateModel.Parse(BLL_Deductions_Data.ToString());
						deductions.Month = (byte)BLL_Deductions_Data.Deductions_Month;
						deductions.MonthFees = BLL_Deductions_Data.Deductions_MonthFees;
						CreditInfo creditInfo = new CreditInfo();
						creditInfo.ChargeAmount = BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount;
						creditInfo.ChargeNo = (uint)BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo;
						creditInfo.CutoffWarningLimit = (byte)BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit;
						creditInfo.MaxOverdraftCredit = (byte)BLL_ChargeBasicInf_Data.ChargeBasicInf_MaxOverdraftCredit;

						ChargeDateInfo chargeDateInfo = new ChargeDateInfo();
						chargeDateInfo.ChargeDate = DateModel.Parse(BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeDate.ToString());
						chargeDateInfo.EnableValvePeriod = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_EnabledValvePeriod;










						MaintenanceCardInfo maintenanceCardInfo = new MaintenanceCardInfo
						{
							Actions = (ushort)MainData.MaintCard_MeterAction,
							ClientCategory = (byte)MainData.MaintCard_ClientCateg,
							SewageToApply = (byte)MainData.MaintCard_SwegToAPPly,
							ActionDate = DateModel.Parse(MainData.MaintCard_ActionDate.ToString()),
							CategoryToApply = (byte)MainData.MaintCard_CategToapply,
							StartConsumerID = (uint)MainData.MaintCard_StartConsumerID,
							EndConsumerID = (uint)MainData.MaintCard_EndConsumerID,
							TimesEffective = (ushort)MainData.MaintCard_TimeEffective

						


						};

						byte[] payloadDataCertification = BERTLVPayloadBuilder.GeneratePayload(null, null, null, null, BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), new ModelBase[]
									  {

									mainCardInfo,
									hcwwtoWaterCompanyCertificate2,
									waterCompanyToCardCertificate,

									priceSchedule,
							        deductions,
							        offTimes,
							        chargeDateInfo,
							       
									maintenanceCardInfo



									  });


						byte[] response2 = APDUHELPER.ExecuteAPDU(214, 68, payloadDataCertification);
						bool flag7 = APDUHELPER.GetApduResponseEnum(response2) == ApduResponseEnum.SuccessResponse;
						if (flag7)
						{
							CardIssues_DAL.Update(BLLCardIssues_Data);
							MainData.MaintCard_CardNID = BLLCardIssues_Data.id;
							MainData.MaintCard_IssueDate = DateTime.Now;

							DAL_MaintCard Object =new  DAL_MaintCard();

							Status = Object.Insert(MainData);
						}
						else
						{
							MessageBox.Show("Fail to Data Write Card");
						}
						//complete data and update last card
						//

					}
				}
			}
			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("SYSTEM_ERROR");
			}
			return Status;



		}
		public bool Meter_SetWaterComany(BLL_WaterComp BLL_WaterComp_Data)
        {
			bool Status = false;
			try
			{
				
				byte[] array = APDUHELPER.ExecuteAPDU(176, 15, null);
				bool flag2 = APDUHELPER.GetApduResponseEnum(array) != ApduResponseEnum.SuccessResponse;
				if (flag2)
				{
					MessageBox.Show("Fail To Read Card");
				}
				else
				{
					APDUData apdudata = APDUData.ParseTLV(array);
					Tlv tlv = Enumerable.FirstOrDefault<Tlv>(apdudata.Models, (Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					Tlv tlv2 = Enumerable.FirstOrDefault<Tlv>(apdudata.Models, (Tlv m) => m.Tag == 217);
					CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);
					string targetEntityPublicKey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
					string text = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);
					bool flag3 = !this.IsCardCleared(mainCardInfo);
					if (flag3)
					{
						MessageBox.Show("Please Clear Card First");
					}
					else
					{
					
							mainCardInfo.CardFun = CardFunctionEnum.MeterIssuing.ToByte();
							mainCardInfo.Mode = CardModeEnum.MeterIssued.ToByte();
							//mainCardInfo.IssueDate = DateModel.Parse(DateTime.Now.ToString());
							mainCardInfo.LastTransactionDate = DateModel.Parse(DateTime.Now.ToString());
							mainCardInfo.LastUpdatedBy = LastUpdatedBy.System;
							mainCardInfo.CompanyId = Convert.ToByte(BLL_WaterComp_Data.WaterComp_Id);
						   // mainCardInfo.Id = (uint)BLLCardIssues_Data.CardNum; //number of card 
						   
						//
							HCWWToWaterCompanyCertificate hcwwtoWaterCompanyCertificate = new HCWWToWaterCompanyCertificate
							{
								Certificate = GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp)
							};
							WaterCompanyToMeterCertificate waterCompanyToMeterCertificate = new WaterCompanyToMeterCertificate();
						//UnifyWaterCard.Entities.WaterCompany waterCompany = this.waterCompany;
						
						string	targetEntityName = BLL_MeterIssues_Data.Meter_ID.ToString();


						//BLL_MeterIssues_Data.Meter_CertWaterComp_TO_Meter = Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_KPW, BLL_MeterIssues_Data.Meter_PK, (uint)BLL_MeterIssues_Data.Meter_MeterNum, (uint)BLLCardIssues_Data.CardNum);

						BLL_MeterIssues_Data.Meter_WaterCompID = BLL_WaterComp_Data.WaterComp_Id;
						BLL_MeterIssues_Data.Meter_CertWaterComp_TO_Meter = Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_Name, BLL_WaterComp_Data.WaterComp_KPW, targetEntityName, BLL_MeterIssues_Data.Meter_PK);
							waterCompanyToMeterCertificate.Certificate = GeneralUtility.HexToByteArray(BLL_MeterIssues_Data.Meter_CertWaterComp_TO_Meter);
							WaterCompanyToMeterCertificate waterCompanyToMeterCertificate2 = waterCompanyToMeterCertificate;
			
							
						byte[] payloadDataCertification = BERTLVPayloadBuilder.GeneratePayload(null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), new ModelBase[]
									  {

								mainCardInfo,
								hcwwtoWaterCompanyCertificate,
								waterCompanyToMeterCertificate2

									  });


						byte[] response2 = APDUHELPER.ExecuteAPDU(214, 50, payloadDataCertification);
						bool flag7 = APDUHELPER.GetApduResponseEnum(response2) == ApduResponseEnum.SuccessResponse;
						if (flag7)
							{
								MessageBox.Show("Data Written Successfully to Card");
							Status = true;
							}
							else
							{
								MessageBox.Show("SetConfigCard Fail");
							}
						}
						
					
				}
			}
			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("SYSTEM_ERROR");
			}
			return Status;
        }

		string ConvertByteArrayToHexString(byte[] byteArray)
		{
			// Convert byte array to hexadecimal string
			StringBuilder hexStringBuilder = new StringBuilder(byteArray.Length * 2);
			foreach (byte b in byteArray)
			{
				hexStringBuilder.Append(b.ToString("X2")); // Represent each byte as two hex characters
			}

			return hexStringBuilder.ToString();
		}

		public bool RechargeMeter()
        {
			bool Status = false;
     		try
				{
				
					Meter meter2 =new Meter();
				//	WaterCompany waterCompanyById = this._TestToolRepository.GetWaterCompanyById(Convert.ToInt32(this.comboBoxClientCardCompany.SelectedValue));
				meter2.MeterInfo = new MeterInfo();
				meter2.MeterInfo.MeterId     = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
				byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
				if (bytes.Length > 0)
				{
				meter2.MeterInfo.MeterMan = bytes[0];
				}
					meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
					meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

				bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
				if (bytes.Length > 0)
				{
					meter2.MeterInfo.MeterModel = bytes[0];
				}
			
				
				
				Client client = new Client();
					client.SubscriberId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID;
					client.SubscriberName =( (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID).ToString();
					client.MeterId = meter2.MeterInfo.MeterId;

					///generate nounce system and parse to code 
					///
					int Rannum = GetRandomNumber();

					this.System_Nounce[0] = (byte)(Rannum & 0xff);
					this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
				//mostafa
				DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();
				BLL_WaterComp BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();

					byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), null);
					byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);
					bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				// Group every two characters
				//string hexString = ConvertByteArrayToHexString(array);
				//MessageBox.Show(hexString, "Hexadecimal String");

				//Console.WriteLine(hexString);
				if (flag)
					{
					    APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());
						Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
						MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
						bool flag2 = mainCardInfo.CardFun != CardFunctionEnum.CustomerCard.ToByte() && !this.IsCardCleared(mainCardInfo);
						if (flag2)
						{
							MessageBox.Show("Please Clear Card First");
						}
						else
						{
						mainCardInfo.Mode = ((mainCardInfo.CardFun == CardFunctionEnum.CustomerCard.ToByte() && mainCardInfo.Mode == CardModeEnum.Active.ToByte()) ? mainCardInfo.Mode : CardModeEnum.CardInitialized.ToByte());
						mainCardInfo.CardFun = CardFunctionEnum.CustomerCard.ToByte();
						//mainCardInfo.IssueDate = DateModel.Parse(DateTime.Now.ToString());
				         mainCardInfo.LastTransactionDate = DateModel.Parse(DateTime.Now.ToString());
			
						mainCardInfo.LastUpdatedBy = LastUpdatedBy.System;
						
						//mainCardInfo.ExpirationDate = DateModel.Parse(CardExpirationDate.ToString());
					     mainCardInfo.CompanyId = (byte)BLL_WaterComp_Data.WaterComp_Id;
						
						DAL_CardIssues Objet = new DAL_CardIssues();
						BLLCardIssues_Data = Objet.CardIssueDataSelection((int)mainCardInfo.Id);

                        BLLCardIssues_Data.LastTranscationDate = DateModel.GetSystemDate(mainCardInfo.LastTransactionDate);
						BLLCardIssues_Data.LastUploadBy = (byte)mainCardInfo.LastUpdatedBy;
						BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
						client.CardId = mainCardInfo.Id;
						/// set client card //TODO here is the function of setting data 


							Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
							Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

							Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
							SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);



							//Serialize();
							Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
							CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);

					

							 DAL_CardProd DAL_CardProd_Obj  = new DAL_CardProd();
						     BLL_CardProd BLL_CardProd_Data = DAL_CardProd_Obj.GetCardProdData(BLLCardIssues_Data.CardProdID);

						bool flag3 = Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLL_CardProd_Data.CardProd_KUCP);
							if (flag3)
							{
								string CardPublickey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
								//	byte[] signdata =Signer.Sign(waterCompanyById.KeyPair.PrivateKey, System_Nounce);
								//  bool flagSignouncT = Signer.ValidateSignature(System_Nounce, signdata, waterCompanyById.KeyPair.PublicKey);

								bool flagSignounc = Signer.ValidateSignature(this.System_Nounce, SignNonce.NonceSignature, CardPublickey);

								if (flagSignounc)
								{
									ClientCardInfo clientCardInfo = new ClientCardInfo();
								clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
								clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
								clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
								clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
								clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());

								



							    	ChargeDateInfo chargeDateInfo = new ChargeDateInfo();
									chargeDateInfo.ChargeDate = DateModel.Parse(BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeDate.ToString());
									chargeDateInfo.EnableValvePeriod = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_EnabledValvePeriod;


								
								SystemActions systemActions = new SystemActions
								{
									Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
								};
									
		                     PriceSchedule priceSchedule = new PriceSchedule();
									priceSchedule.AppDate = DateModel.Parse(BLL_PriceScheduler_Data.PriceSchedule_APPDate.ToString());

									priceSchedule.MonthFee1 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee1;
									priceSchedule.MonthFee2 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee2;
					
									priceSchedule.MonthFeesOptions = (byte)BLL_PriceScheduler_Data.PriceSchedule_MonthFeeOption;
									priceSchedule.NoOfUnitsInCalc = ((byte)(BLL_PriceScheduler_Data.PriceSchedule_NoOFUintsCalc));
									priceSchedule.PerMeterFees = (ushort)BLL_PriceScheduler_Data.PriceSchedule_PerMeterFee;
								
									priceSchedule.Pricing = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Pricing;

									priceSchedule.SewagePrice = (ushort)BLL_PriceScheduler_Data.PriceSchedule_SWGPrice;
									priceSchedule.SewagePercentage = (byte)BLL_PriceScheduler_Data.PriceSchedule_SWGPercent;
									try
									{
						priceSchedule.Levels = new PriceScheduleLevel[]
						{
							new PriceScheduleLevel
							{
								StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Fee
							},
							new PriceScheduleLevel
							{
								StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Fee
							},
							new PriceScheduleLevel 
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Fee 
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Fee
							}
							};
							}
									catch
									{
										MessageBox.Show("Enter a valid PriceScheduleLevel.", "Error");
										return Status;
									}


									OffTimes offTimes = new OffTimes();
									offTimes.CutoffTime = (byte)BLL_Offtimes_Obj.OFFTime_CutOffTime;
									offTimes.GracePeriod = (byte)BLL_Offtimes_Obj.OFFTime_GracePeriod;
									offTimes.WorkStart = (byte)BLL_Offtimes_Obj.OFFTime_WorkStart;
									offTimes.WorkEnd = (byte)BLL_Offtimes_Obj.OFFTime_WorkEnd;
								
									offTimes.WorkingDays = (byte)BLL_Offtimes_Obj.OFFTime_WorkingDays;
									List<Holiday> list = new List<Holiday>();
											
										list.Add(new Holiday
										{
											Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Month,
											Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Day
										});
								          list.Add(new Holiday
								         {
									      Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Month,
									      Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Day
								             });
							             list.Add(new Holiday
								         {
								 	     Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Month,
								 	     Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Day
								         });
							        	list.Add(new Holiday
							        	{
							        		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Month,
							        		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Day
							        	});
							         	list.Add(new Holiday
							         	{
							         		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Month,
							         		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Day
							         	});
							        	list.Add(new Holiday
							        	{
							        		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Month,
							        		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Day
							        	});
								        list.Add(new Holiday
								        {
								        	Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Month,
								        	Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Day
								        });
								        list.Add(new Holiday
										{
											Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Month,
											Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Day
										});
							         	list.Add(new Holiday
							         	{
							         		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Month,
							         		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Day
							         	});
								
								
								        list.Add(new Holiday
								        		{
								        			Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Month,
								        			Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Day
								        		});
							         	list.Add(new Holiday
							         	{
							         		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Month,
							         		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Day
							         	});
							        	list.Add(new Holiday
							        	{
							        		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Month,
							        		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Day
							        	});
							        	list.Add(new Holiday
							        	{
							        		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Month,
							        		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Day
							        	});
							           	list.Add(new Holiday
							           	{
							           		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Month,
							           		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Day
							           	});
									    list.Add(new Holiday
									    {
									    	Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Month,
									    	Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Day
									    });
								        list.Add(new Holiday
								        {
								        	Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Month,
								        	Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Day
								        });
							        	list.Add(new Holiday
							        	{
							        		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Month,
							        		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Day
							        	});
							          	list.Add(new Holiday
							          	{
							          		Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Month,
							          		Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Day
							          	});
										list.Add(new Holiday
										{
											Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Month,
											Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Day
										});
					          			list.Add(new Holiday
					          			{
					          				Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Month,
					          				Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Day
					          			});
					          			list.Add(new Holiday
					          			{
					          				Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Month,
					          				Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Day
					          			});
					          			list.Add(new Holiday
					          			{
					          				Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Month,
					          				Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Day
					          			});
					          			list.Add(new Holiday
					          			{
					          				Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Month,
					          				Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Day
					          			});
					          			list.Add(new Holiday
					          			{
					          				Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Month,
					          				Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Day
					          			});
					          
					          			list.Add(new Holiday
					          			{
					          				Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Month,
					          				Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Day
					          			});
					          
								 	offTimes.Holidays = list.ToArray();



									CreditInfo creditInfo = new CreditInfo();
									creditInfo.ChargeAmount = BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount;
									creditInfo.ChargeNo = (uint)BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo;
									creditInfo.CutoffWarningLimit = (byte)BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit;
									creditInfo.MaxOverdraftCredit = (byte)BLL_ChargeBasicInf_Data.ChargeBasicInf_MaxOverdraftCredit;
								
								
								Deductions deductions = new Deductions();
									deductions.AppDate = DateModel.Parse(BLL_Deductions_Data.Deductions_AppDate.ToString());
									deductions.Month =(byte) BLL_Deductions_Data.Deductions_Month;
									deductions.MonthFees = BLL_Deductions_Data.Deductions_MonthFees;

									HCWWToWaterCompanyCertificate hcwwtoWaterCompanyCertificate = new HCWWToWaterCompanyCertificate
									{
										Certificate =  GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp) 
									};
								BLLCardIssues_Data.CertWaterComp_TO_Card= Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_Name, BLL_WaterComp_Data.WaterComp_KPW, "card", CardPublickey);
								WaterCompanyToCardCertificate waterCompanyToCardCertificate = new WaterCompanyToCardCertificate
								{
									Certificate = GeneralUtility.HexToByteArray(BLLCardIssues_Data.CertWaterComp_TO_Card)
										//GeneralUtility.HexToByteArray(waterCompanyById.Certify((string)"waterComp", CardPublickey)) 
									};

								BLL_ClientInfo_Data.CertWaterCompToMeterSubscriber = Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_KPW, CardPublickey, meter2.MeterInfo.MeterId, client.SubscriberId);
								WaterCompanyToMeterAndSubscriberCertificate waterCompanyToMeterAndSubscriberCertificate = new WaterCompanyToMeterAndSubscriberCertificate
									{
									//	(string signerPrivate_key, string entityPublicKey, uint meterId, uint subscriberCardId)
										Certificate = GeneralUtility.HexToByteArray(BLL_ClientInfo_Data.CertWaterCompToMeterSubscriber)
									//waterCompanyById.Certify(CardPublickey, meter2.MeterInfo.MeterId, client.SubscriberId));
								};

									byte[] payloadDataCertification = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(client.SubscriberId), BitConverter.GetBytes(meter2.MeterInfo.MeterId), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), new ModelBase[]
									  {
								mainCardInfo,
								hcwwtoWaterCompanyCertificate,
								waterCompanyToCardCertificate,

									  });
									byte[] response = APDUHELPER.ExecuteAPDU(214, 78, payloadDataCertification);
									bool flag5 = APDUHELPER.GetApduResponseEnum(response) == ApduResponseEnum.SuccessResponse;
							      	
									if (flag5)
									{



										byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, (CardNonce.Serialize()), BitConverter.GetBytes(client.SubscriberId), BitConverter.GetBytes(meter2.MeterInfo.MeterId), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), new ModelBase[]
													{

							                          waterCompanyToMeterAndSubscriberCertificate,
							                          mainCardInfo,
							                          clientCardInfo,
							                          priceSchedule,
							                          deductions,
							                          offTimes,
							                          creditInfo,
							                          systemActions,
							                          chargeDateInfo,
							                          meter2.MeterInfo
													});
										byte[] responsex = APDUHELPER.ExecuteAPDU(214, 60, payloadData);
										bool flag4 = APDUHELPER.GetApduResponseEnum(responsex) == ApduResponseEnum.SuccessResponse;
										if (flag4)
										{
											client.MeterId = meter2.MeterInfo.MeterId;
											client.CardId = mainCardInfo.Id;
										BLLCardIssues_Data.LastTranscationDate = DateModel.GetSystemDate(mainCardInfo.LastTransactionDate);
										/*	waterCompanyById.RegisterCard(new CardPublicInfo
											{
												Id = mainCardInfo.Id,
												PublicKey = CardPublickey
											});*/
										Status = true;
										
										}
									}
								}

							}
							else
							{
								MessageBox.Show("Failed To Check Certification");
							}
						}
					}
					else
					{
						MessageBox.Show("Set Client Card Failed");
					}
				}
				catch (Exception ex)
				{
					//	this.Log(ex.ToString());
					MessageBox.Show("Set Client Card Failed");
				}

			


			return Status;

        }


		public bool SetNewClientCard()
		{
			bool Status = false;
			try
			{

				Meter meter2 = new Meter();
				//	WaterCompany waterCompanyById = this._TestToolRepository.GetWaterCompanyById(Convert.ToInt32(this.comboBoxClientCardCompany.SelectedValue));
				meter2.MeterInfo = new MeterInfo();
				meter2.MeterInfo.MeterId = (uint)BLL_MeterIssues_Data.Meter_MeterNum;
				byte[] bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Man);
				if (bytes.Length > 0)
				{
					meter2.MeterInfo.MeterMan = bytes[0];
				}
				meter2.MeterInfo.MeterDim = (byte)BLL_MeterIssues_Data.Meter_Diameter;
				meter2.MeterInfo.MeterOrigin = (ushort)BLL_MeterIssues_Data.Meter_Origin;

				bytes = Encoding.UTF8.GetBytes(BLL_MeterIssues_Data.Meter_Model);
				if (bytes.Length > 0)
				{
					meter2.MeterInfo.MeterModel = bytes[0];
				}



				Client client = new Client();
				client.SubscriberId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID;
				client.SubscriberName = ((uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID).ToString();
				client.MeterId = meter2.MeterInfo.MeterId;

				///generate nounce system and parse to code 
				///
				int Rannum = GetRandomNumber();

				this.System_Nounce[0] = (byte)(Rannum & 0xff);
				this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
				//mostafa
				DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();
				BLL_WaterComp BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();

				byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), null);
				byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);
				bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				// Group every two characters
				//string hexString = ConvertByteArrayToHexString(array);
				//MessageBox.Show(hexString, "Hexadecimal String");

				//Console.WriteLine(hexString);
				if (flag)
				{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());
					Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					bool flag2 = mainCardInfo.CardFun != CardFunctionEnum.CustomerCard.ToByte() && !this.IsCardCleared(mainCardInfo);
					if (flag2)
					{
						MessageBox.Show("Please Clear Card First");
					}
					else
					{
						mainCardInfo.Mode = ((mainCardInfo.CardFun == CardFunctionEnum.CustomerCard.ToByte() && mainCardInfo.Mode == CardModeEnum.Active.ToByte()) ? mainCardInfo.Mode : CardModeEnum.CardInitialized.ToByte());
						mainCardInfo.CardFun = CardFunctionEnum.CustomerCard.ToByte();
						//mainCardInfo.IssueDate = DateModel.Parse(DateTime.Now.ToString());
						mainCardInfo.LastTransactionDate = DateModel.Parse(DateTime.Now.ToString());

						mainCardInfo.LastUpdatedBy = LastUpdatedBy.System;

						//mainCardInfo.ExpirationDate = DateModel.Parse(CardExpirationDate.ToString());
						mainCardInfo.CompanyId = (byte)BLL_WaterComp_Data.WaterComp_Id;

						DAL_CardIssues Objet = new DAL_CardIssues();
						BLLCardIssues_Data = Objet.CardIssueDataSelection((int)mainCardInfo.Id);

						BLLCardIssues_Data.LastTranscationDate = DateModel.GetSystemDate(mainCardInfo.LastTransactionDate);
						BLLCardIssues_Data.LastUploadBy = (byte)mainCardInfo.LastUpdatedBy;
						BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
						client.CardId = mainCardInfo.Id;
						/// set client card //TODO here is the function of setting data 


						Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
						Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

						Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
						SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);



						//Serialize();
						Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
						CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);



						DAL_CardProd DAL_CardProd_Obj = new DAL_CardProd();
						BLL_CardProd BLL_CardProd_Data = DAL_CardProd_Obj.GetCardProdData(BLLCardIssues_Data.CardProdID);

						bool flag3 = Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLL_CardProd_Data.CardProd_KUCP);
						if (flag3)
						{
							string CardPublickey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
							//	byte[] signdata =Signer.Sign(waterCompanyById.KeyPair.PrivateKey, System_Nounce);
							//  bool flagSignouncT = Signer.ValidateSignature(System_Nounce, signdata, waterCompanyById.KeyPair.PublicKey);

							bool flagSignounc = Signer.ValidateSignature(this.System_Nounce, SignNonce.NonceSignature, CardPublickey);

							if (flagSignounc)
							{
								ClientCardInfo clientCardInfo = new ClientCardInfo();
								clientCardInfo.ClientId = (uint)BLL_ClientInfo_Data.ClientInfo_SubscriberID; //uint.Parse(this.txtSetClient_ClientId.Text);
								clientCardInfo.Activity = (byte)BLL_ClientInfo_Data.ClientInfo_Activity; //byte.Parse(this.txtSetClient_ClientActivity.Text);
								clientCardInfo.Category = (byte)BLL_ClientInfo_Data.ClientInfo_Category;//byte.Parse(this.txtSetClient_ClientCategory.Text);
								clientCardInfo.NoOfUnits = (byte)BLL_ClientInfo_Data.ClientInfo_NumOFUnit;// byte.Parse(this.txtSetClient_NumberOfUnits.Text);
								clientCardInfo.SewageService = (byte)BLL_ClientInfo_Data.ClientInfo_SwGServices; //byte.Parse(this.cbSetClient_SwgService.SelectedValue.ToString());





								ChargeDateInfo chargeDateInfo = new ChargeDateInfo();
								chargeDateInfo.ChargeDate = DateModel.Parse(BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeDate.ToString());
								chargeDateInfo.EnableValvePeriod = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_EnabledValvePeriod;



								SystemActions systemActions = new SystemActions
								{
									Actions = (ushort)BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction
								};

								PriceSchedule priceSchedule = new PriceSchedule();
								priceSchedule.AppDate = DateModel.Parse(BLL_PriceScheduler_Data.PriceSchedule_APPDate.ToString());

								priceSchedule.MonthFee1 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee1;
								priceSchedule.MonthFee2 = (ushort)BLL_PriceScheduler_Data.PriceSchedule_MonthFee2;

								priceSchedule.MonthFeesOptions = (byte)BLL_PriceScheduler_Data.PriceSchedule_MonthFeeOption;
								priceSchedule.NoOfUnitsInCalc = ((byte)(BLL_PriceScheduler_Data.PriceSchedule_NoOFUintsCalc));
								priceSchedule.PerMeterFees = (ushort)BLL_PriceScheduler_Data.PriceSchedule_PerMeterFee;

								priceSchedule.Pricing = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Pricing;

								priceSchedule.SewagePrice = (ushort)BLL_PriceScheduler_Data.PriceSchedule_SWGPrice;
								priceSchedule.SewagePercentage = (byte)BLL_PriceScheduler_Data.PriceSchedule_SWGPercent;
								try
								{
									priceSchedule.Levels = new PriceScheduleLevel[]
									{
							new PriceScheduleLevel
							{
								StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level1_Fee
							},
							new PriceScheduleLevel
							{
								StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level2_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level3_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level4_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level5_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level6_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level7_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level8_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level9_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level10_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level11_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level12_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level13_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level14_Fee
							},
							new PriceScheduleLevel
							{
									StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level15_Fee
							},
							new PriceScheduleLevel
							{
										StepMax = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_StepMax,
								Price = (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Price,
								Fee =  (ushort)BLL_PriceScheduler_Data.PriceSchedule_Level16_Fee
							}
										};
								}
								catch
								{
									MessageBox.Show("Enter a valid PriceScheduleLevel.", "Error");
									return Status;
								}


								OffTimes offTimes = new OffTimes();
								offTimes.CutoffTime = (byte)BLL_Offtimes_Obj.OFFTime_CutOffTime;
								offTimes.GracePeriod = (byte)BLL_Offtimes_Obj.OFFTime_GracePeriod;
								offTimes.WorkStart = (byte)BLL_Offtimes_Obj.OFFTime_WorkStart;
								offTimes.WorkEnd = (byte)BLL_Offtimes_Obj.OFFTime_WorkEnd;

								offTimes.WorkingDays = (byte)BLL_Offtimes_Obj.OFFTime_WorkingDays;
								List<Holiday> list = new List<Holiday>();

								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday1_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday2_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday3_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday4_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday5_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday6_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday7_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday8_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday9_Day
								});


								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday10_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday11_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday12_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday13_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday14_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday15_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday16_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday17_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday18_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday19_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday20_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday21_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday22_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday23_Day
								});
								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday24_Day
								});

								list.Add(new Holiday
								{
									Month = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Month,
									Day = (byte)BLL_Offtimes_Obj.OFFTime_Holiday25_Day
								});

								offTimes.Holidays = list.ToArray();



								CreditInfo creditInfo = new CreditInfo();
								creditInfo.ChargeAmount = BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount;
								creditInfo.ChargeNo = (uint)BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo;
								creditInfo.CutoffWarningLimit = (byte)BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit;
								creditInfo.MaxOverdraftCredit = (byte)BLL_ChargeBasicInf_Data.ChargeBasicInf_MaxOverdraftCredit;


								Deductions deductions = new Deductions();
								deductions.AppDate = DateModel.Parse(BLL_Deductions_Data.Deductions_AppDate.ToString());
								deductions.Month = (byte)BLL_Deductions_Data.Deductions_Month;
								deductions.MonthFees = BLL_Deductions_Data.Deductions_MonthFees;

								HCWWToWaterCompanyCertificate hcwwtoWaterCompanyCertificate = new HCWWToWaterCompanyCertificate
								{
									Certificate = GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_CertHoldingComp_TO_Watercomp)
								};
								BLLCardIssues_Data.CertWaterComp_TO_Card = Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_Name, BLL_WaterComp_Data.WaterComp_KPW, "card", CardPublickey);
								WaterCompanyToCardCertificate waterCompanyToCardCertificate = new WaterCompanyToCardCertificate
								{
									Certificate = GeneralUtility.HexToByteArray(BLLCardIssues_Data.CertWaterComp_TO_Card)
									//GeneralUtility.HexToByteArray(waterCompanyById.Certify((string)"waterComp", CardPublickey)) 
								};

								BLL_ClientInfo_Data.CertWaterCompToMeterSubscriber = Secuirty_Obj.CreateCertificate(BLL_WaterComp_Data.WaterComp_KPW, CardPublickey, meter2.MeterInfo.MeterId, client.SubscriberId);
								WaterCompanyToMeterAndSubscriberCertificate waterCompanyToMeterAndSubscriberCertificate = new WaterCompanyToMeterAndSubscriberCertificate
								{
									//	(string signerPrivate_key, string entityPublicKey, uint meterId, uint subscriberCardId)
									Certificate = GeneralUtility.HexToByteArray(BLL_ClientInfo_Data.CertWaterCompToMeterSubscriber)
									//waterCompanyById.Certify(CardPublickey, meter2.MeterInfo.MeterId, client.SubscriberId));
								};

								byte[] payloadDataCertification = BERTLVPayloadBuilder.GeneratePayload(null, null, BitConverter.GetBytes(client.SubscriberId), BitConverter.GetBytes(meter2.MeterInfo.MeterId), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), new ModelBase[]
								  {
								mainCardInfo,
								hcwwtoWaterCompanyCertificate,
								waterCompanyToCardCertificate,

								  });
								byte[] response = APDUHELPER.ExecuteAPDU(214, 78, payloadDataCertification);
								bool flag5 = APDUHELPER.GetApduResponseEnum(response) == ApduResponseEnum.SuccessResponse;

								if (flag5)
								{



									byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(null, (CardNonce.Serialize()), BitConverter.GetBytes(client.SubscriberId), BitConverter.GetBytes(meter2.MeterInfo.MeterId), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLL_WaterComp_Data.WaterComp_KPW), new ModelBase[]
												{

													  waterCompanyToMeterAndSubscriberCertificate,
													  mainCardInfo,
													  clientCardInfo,
													  priceSchedule,
													  deductions,
													  offTimes,
													  creditInfo,
													  systemActions,
													  chargeDateInfo,
													  meter2.MeterInfo
												});
									byte[] responsex = APDUHELPER.ExecuteAPDU(214, 60, payloadData);
									bool flag4 = APDUHELPER.GetApduResponseEnum(responsex) == ApduResponseEnum.SuccessResponse;
									if (flag4)
									{
										client.MeterId = meter2.MeterInfo.MeterId;
										client.CardId = mainCardInfo.Id;
										BLLCardIssues_Data.LastTranscationDate = DateModel.GetSystemDate(mainCardInfo.LastTransactionDate);
										/*	waterCompanyById.RegisterCard(new CardPublicInfo
											{
												Id = mainCardInfo.Id,
												PublicKey = CardPublickey
											});*/
										Status = true;

									}
								}
							}

						}
						else
						{
							MessageBox.Show("Failed To Check Certification");
						}
					}
				}
				else
				{
					MessageBox.Show("Set Client Card Failed");
				}
			}
			catch (Exception ex)
			{
				//	this.Log(ex.ToString());
				MessageBox.Show("Set Client Card Failed");
			}




			return Status;

		}


		public bool ReadClientCard()
        {
			bool Status = false;

			try
			{
				byte[] array = APDUHelper.ExecuteAPDU(176, 15, null);
				bool flag = APDUHelper.GetApduResponseEnum(array) != ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					MessageBox.Show("Fail To Read Card");
				}
				else  
				{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());
					Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					bool flag2 = mainCardInfo.CardFun != 0 || (mainCardInfo.Mode != 1 && mainCardInfo.Mode != 2 && mainCardInfo.Mode != 3);
					if (flag2)
					{
						MessageBox.Show("Card Type Not Customer Card");
					}
					else
					{
						bool flag3 = mainCardInfo.CardFun == Convert.ToByte(CardFunctionEnum.CustomerCard);
						if (flag3)
						{
							//mostafa

							int Rannum = GetRandomNumber();

							this.System_Nounce[0] = (byte)(Rannum & 0xff);
							this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
							//mostafa
							byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);
							byte[] array0 = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);
							bool flag0 = APDUHELPER.GetApduResponseEnum(array0) == ApduResponseEnum.SuccessResponse;
							if (flag0)
							{

								APDUData apdudata3 = APDUData.ParseTLV(array0.Take(array0.Length - 2).ToArray<byte>());
								Tlv tlvn = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
								Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

								Tlv tlvsn = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
								SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);
								DAL_CardIssues Object = new DAL_CardIssues();
								BLLCardIssues_Data = Object.CardIssueDataSelection((int)mainCardInfo.Id);

								//Serialize();
								Tlv tlv2 = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
								CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);
								bool flag4 = Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLLCardIssues_Data.CardProd_KUCP);
								if (flag4)
								{
									string CardPublickey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
									//	byte[] signdata =Signer.Sign(waterCompanyById.KeyPair.PrivateKey, System_Nounce);
									//  bool flagSignouncT = Signer.ValidateSignature(System_Nounce, signdata, waterCompanyById.KeyPair.PublicKey);

									bool flagSignounc = Signer.ValidateSignature(System_Nounce, SignNonce.NonceSignature, CardPublickey);

									if (flagSignounc)
									{
										Rannum = GetRandomNumber();
										this.System_Nounce[0] = (byte)(Rannum & 0xff);
										this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
										//mostafa



										

										byte[] payloadDataseesion1 = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, CardNonce.Serialize(), BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), GeneralUtility.HexToByteArray(BLLCardIssues_Data.WaterComp_KPW), null);
										//  byte[] payloadDataseesion1 = BERTLV.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);

										byte[] array2 = APDUHELPER.ExecuteAPDU(176, 77, payloadDataseesion1);
										bool flag5 = APDUHELPER.GetApduResponseEnum(array2) == ApduResponseEnum.SuccessResponse;
										if (flag5)
										{

											APDUData apdudata2 = APDUData.ParseTLV(array2.Take(array2.Length - 2).ToArray<byte>());

											Tlv tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
											SignedNonce SignNonce2 = SignedNonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);



											bool flagSignounc2 = Signer.ValidateSignature(System_Nounce, SignNonce2.NonceSignature, CardPublickey);

											if (flagSignounc2)
											{

												Tlv tlv23 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
												ClientCardInfo clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);
												
												Tlv tlv3 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 200);
												PriceSchedule priceSchedule = PriceSchedule.Deserialize((tlv3 != null) ? tlv3.Value : null);

												Tlv tlv4 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
												Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);

												Tlv tlv5 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 202);
												OffTimes offTimes = OffTimes.Deserialize((tlv5 != null) ? tlv5.Value : null);

												Tlv tlv6 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
												CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

												Tlv tlv7 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
												MeterActions meterActions = MeterActions.Deserialize((tlv7 != null) ? tlv7.Value : null);

												Tlv tlv8 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
												SystemActions systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

												Tlv tlv9 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
												MeterInfo meterInfo = MeterInfo.Deserialize((tlv9 != null) ? tlv9.Value : null);

												Tlv tlv10 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
												CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

												Tlv tlv11 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
												Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

												Tlv tlv12 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
												MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);

												Tlv tlv13 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
												ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);

												Tlv tlv14 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 218);
												WaterCompanyToMeterAndSubscriberCertificate waterCompanyToMeterAndSubscriberCertificate = WaterCompanyToMeterAndSubscriberCertificate.Deserialize((tlv14 != null) ? tlv14.Value : null);






												BLLCardIssues_Data.Mode = mainCardInfo.Mode;
												BLLCardIssues_Data.Func= mainCardInfo.CardFun;
												BLLCardIssues_Data.ExpiratonDate= DateModel.GetSystemDate(mainCardInfo.ExpirationDate);
												BLLCardIssues_Data.CardNum= (int)mainCardInfo.Id;
												BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
												BLLCardIssues_Data.IssueDate= DateModel.GetSystemDate(mainCardInfo.IssueDate);
												BLLCardIssues_Data.LastTranscationDate= DateModel.GetSystemDate(mainCardInfo.LastTransactionDate);
												BLLCardIssues_Data.LastUploadBy =(byte) mainCardInfo.LastUpdatedBy;

												//Clinet info
												BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
												//BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
												BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
												BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
												BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

												BLL_ClientInfo_Data.ClientInfo_SubscriberID= (int)clientCardInfo.ClientId;
												//MeterInfo
												BLL_MeterIssues_Data.Meter_MeterNum =(int) meterInfo.MeterId;
												BLL_MeterIssues_Data.Meter_Origin= (int)(meterInfo.MeterOrigin + 2000);
												BLL_MeterIssues_Data.Meter_Man = meterInfo.MeterMan.ToString();
												BLL_MeterIssues_Data.Meter_Diameter = meterInfo.MeterDim;
												BLL_MeterIssues_Data.Meter_ChargeMode = meterInfo.ChargeMode;

												//charge basci info 
												BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
												BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit    = creditInfo.CutoffWarningLimit;
												BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;


												//Priceschduler
												/*
												this.labelCustomerPriceLevel1.Text = priceSchedule.Levels[0].Price.ToString();
												this.labelCustomerPriceLevel2.Text = priceSchedule.Levels[1].Price.ToString();
												this.labelCustomerPriceLevel3.Text = priceSchedule.Levels[2].Price.ToString();
												this.labelCustomerPriceLevel4.Text = priceSchedule.Levels[3].Price.ToString();
												this.labelCustomerPriceLevel5.Text = priceSchedule.Levels[4].Price.ToString();
												this.labelCustomerPriceLevel6.Text = priceSchedule.Levels[5].Price.ToString();
												this.labelCustomerPriceLevel7.Text = priceSchedule.Levels[6].Price.ToString();
												this.labelCustomerPriceLevel8.Text = priceSchedule.Levels[7].Price.ToString();
												this.labelCustomerPriceLevel9.Text = priceSchedule.Levels[8].Price.ToString();
												this.labelCustomerPriceLevel10.Text = priceSchedule.Levels[9].Price.ToString();
												this.labelCustomerPriceLevel11.Text = priceSchedule.Levels[10].Price.ToString();
												this.labelCustomerPriceLevel12.Text = priceSchedule.Levels[11].Price.ToString();
												this.labelCustomerPriceLevel13.Text = priceSchedule.Levels[12].Price.ToString();
												this.labelCustomerPriceLevel14.Text = priceSchedule.Levels[13].Price.ToString();
												this.labelCustomerPriceLevel15.Text = priceSchedule.Levels[14].Price.ToString();
												this.labelCustomerPriceLevel16.Text = priceSchedule.Levels[15].Price.ToString();
												this.labelCustomerToLevel1.Text = priceSchedule.Levels[0].StepMax.ToString();
												this.labelCustomerToLevel2.Text = priceSchedule.Levels[1].StepMax.ToString();
												this.labelCustomerToLevel3.Text = priceSchedule.Levels[2].StepMax.ToString();
												this.labelCustomerToLevel4.Text = priceSchedule.Levels[3].StepMax.ToString();
												this.labelCustomerToLevel5.Text = priceSchedule.Levels[4].StepMax.ToString();
												this.labelCustomerToLevel6.Text = priceSchedule.Levels[5].StepMax.ToString();
												this.labelCustomerToLevel7.Text = priceSchedule.Levels[6].StepMax.ToString();
												this.labelCustomerToLevel8.Text = priceSchedule.Levels[7].StepMax.ToString();
												this.labelCustomerToLevel9.Text = priceSchedule.Levels[8].StepMax.ToString();
												this.labelCustomerToLevel10.Text = priceSchedule.Levels[9].StepMax.ToString();
												this.labelCustomerToLevel11.Text = priceSchedule.Levels[10].StepMax.ToString();
												this.labelCustomerToLevel12.Text = priceSchedule.Levels[11].StepMax.ToString();
												this.labelCustomerToLevel13.Text = priceSchedule.Levels[12].StepMax.ToString();
												this.labelCustomerToLevel14.Text = priceSchedule.Levels[13].StepMax.ToString();
												this.labelCustomerToLevel15.Text = priceSchedule.Levels[14].StepMax.ToString();
												this.labelCustomerToLevel16.Text = priceSchedule.Levels[15].StepMax.ToString();
												string text = Convert.ToString((int)priceSchedule.Pricing, 2).PadLeft(16, '0');
												bool flag55 = text.Length > 0 && text[0] == '1';
												if (flag55)
												{
													this.checkBoxCustomerReadingCumulativeLevel1.Checked = true;
												}
												bool flag6 = text.Length > 1 && text[1] == '1';
												if (flag6)
												{
													this.checkBoxCustomerReadingCumulativeLevel2.Checked = true;
												}
												bool flag7 = text.Length > 2 && text[2] == '1';
												if (flag7)
												{
													this.checkBoxCustomerReadingCumulativeLevel3.Checked = true;
												}
												bool flag8 = text.Length > 3 && text[3] == '1';
												if (flag8)
												{
													this.checkBoxCustomerReadingCumulativeLevel4.Checked = true;
												}
												bool flag9 = text.Length > 4 && text[4] == '1';
												if (flag9)
												{
													this.checkBoxCustomerReadingCumulativeLevel5.Checked = true;
												}
												bool flag10 = text.Length > 5 && text[5] == '1';
												if (flag10)
												{
													this.checkBoxCustomerReadingCumulativeLevel6.Checked = true;
												}
												bool flag11 = text.Length > 6 && text[6] == '1';
												if (flag11)
												{
													this.checkBoxCustomerReadingCumulativeLevel7.Checked = true;
												}
												bool flag12 = text.Length > 7 && text[7] == '1';
												if (flag12)
												{
													this.checkBoxCustomerReadingCumulativeLevel8.Checked = true;
												}
												bool flag13 = text.Length > 8 && text[8] == '1';
												if (flag13)
												{
													this.checkBoxCustomerReadingCumulativeLevel9.Checked = true;
												}
												bool flag14 = text.Length > 9 && text[9] == '1';
												if (flag14)
												{
													this.checkBoxCustomerReadingCumulativeLevel10.Checked = true;
												}
												bool flag15 = text.Length > 10 && text[10] == '1';
												if (flag15)
												{
													this.checkBoxCustomerReadingCumulativeLevel11.Checked = true;
												}
												bool flag16 = text.Length > 11 && text[11] == '1';
												if (flag16)
												{
													this.checkBoxCustomerReadingCumulativeLevel12.Checked = true;
												}
												bool flag17 = text.Length > 12 && text[12] == '1';
												if (flag17)
												{
													this.checkBoxCustomerReadingCumulativeLevel13.Checked = true;
												}
												bool flag18 = text.Length > 13 && text[13] == '1';
												if (flag18)
												{
													this.checkBoxCustomerReadingCumulativeLevel14.Checked = true;
												}
												bool flag19 = text.Length > 14 && text[14] == '1';
												if (flag19)
												{
													this.checkBoxCustomerReadingCumulativeLevel15.Checked = true;
												}
												bool flag20 = text.Length > 15 && text[15] == '1';
												if (flag20)
												{
													this.checkBoxCustomerReadingCumulativeLevel16.Checked = true;
												}
												this.labelCustomerFeeLevel1.Text = priceSchedule.Levels[0].Fee.ToString();
												this.labelCustomerFeeLevel2.Text = priceSchedule.Levels[1].Fee.ToString();
												this.labelCustomerFeeLevel3.Text = priceSchedule.Levels[2].Fee.ToString();
												this.labelCustomerFeeLevel4.Text = priceSchedule.Levels[3].Fee.ToString();
												this.labelCustomerFeeLevel5.Text = priceSchedule.Levels[4].Fee.ToString();
												this.labelCustomerFeeLevel6.Text = priceSchedule.Levels[5].Fee.ToString();
												this.labelCustomerFeeLevel7.Text = priceSchedule.Levels[6].Fee.ToString();
												this.labelCustomerFeeLevel8.Text = priceSchedule.Levels[7].Fee.ToString();
												this.labelCustomerFeeLevel9.Text = priceSchedule.Levels[8].Fee.ToString();
												this.labelCustomerFeeLevel10.Text = priceSchedule.Levels[9].Fee.ToString();
												this.labelCustomerFeeLevel11.Text = priceSchedule.Levels[10].Fee.ToString();
												this.labelCustomerFeeLevel12.Text = priceSchedule.Levels[11].Fee.ToString();
												this.labelCustomerFeeLevel13.Text = priceSchedule.Levels[12].Fee.ToString();
												this.labelCustomerFeeLevel14.Text = priceSchedule.Levels[13].Fee.ToString();
												this.labelCustomerFeeLevel15.Text = priceSchedule.Levels[14].Fee.ToString();
												this.labelCustomerFeeLevel16.Text = priceSchedule.Levels[15].Fee.ToString();
												this.labelCustmerReadingMonthFees1.Text = priceSchedule.MonthFee1.ToString();
												this.labelCustmerReadingMonthFees2.Text = priceSchedule.MonthFee2.ToString();
												this.labelCustmerReadingPerMeterFees.Text = priceSchedule.PerMeterFees.ToString();
												this.checkBoxCustomerReadingFeesIncludeUnitsInCalc.Checked = Convert.ToBoolean(priceSchedule.NoOfUnitsInCalc);
												string text2 = Convert.ToString(priceSchedule.MonthFeesOptions, 2).PadLeft(8, '0');
												bool flag21 = text2.Length > 0 && text2[0] == '1';
												if (flag21)
												{
													this.checkBoxCustomerReadingFixedFees.Checked = true;
												}
												bool flag22 = text2.Length > 1 && text2[1] == '1';
												if (flag22)
												{
													this.checkBoxCustomerReadingPerUnitFees.Checked = true;
												}
												bool flag23 = text2.Length > 2 && text2[2] == '1';
												if (flag23)
												{
													this.checkBoxCustomerReadingStepFees.Checked = true;
												}
												bool flag24 = text2.Length > 3 && text2[3] == '1';
												if (flag24)
												{
													this.checkBoxCustomerReadingStepPerUnitFees.Checked = true;
												}
												this.labelCustomerReadingSwdPercent.Text = Convert.ToUInt32(priceSchedule.SewagePercentage).ToString();
												this.labelCustmerReadingSwgPrice.Text = priceSchedule.SewagePrice.ToString();
												*/
												BLL_PriceScheduler_Data.PriceSchedule_APPDate = DateModel.GetSystemDate(priceSchedule.AppDate);
												BLL_Deductions_Data.Deductions_Month = (deductions.Month);
												BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
												BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);

												/*string text3 = Convert.ToString(offTimes.WorkingDays, 2).PadLeft(8, '0');
												bool flag25 = text3.Length > 0 && text3[0] == '1';
												if (flag25)
												{
													this.checkBoxCustomerReadingSat.Checked = true;
												}
												bool flag26 = text3.Length > 1 && text3[1] == '1';
												if (flag26)
												{
													this.checkBoxCustomerReadingSun.Checked = true;
												}
												bool flag27 = text3.Length > 2 && text3[2] == '1';
												if (flag27)
												{
													this.checkBoxCustomerReadingMon.Checked = true;
												}
												bool flag28 = text3.Length > 3 && text3[3] == '1';
												if (flag28)
												{
													this.checkBoxCustomerReadingTue.Checked = true;
												}
												bool flag29 = text3.Length > 4 && text3[4] == '1';
												if (flag29)
												{
													this.checkBoxCustomerReadingWed.Checked = true;
												}
												bool flag30 = text3.Length > 5 && text3[5] == '1';
												if (flag30)
												{
													this.checkBoxCustomerReadingThu.Checked = true;
												}
												bool flag31 = text3.Length > 6 && text3[6] == '1';
												if (flag31)
												{
													this.checkBoxCustomerReadingFri.Checked = true;
												}
												this.labelCustomerReadingGracePeriod.Text = Convert.ToUInt32(offTimes.GracePeriod).ToString();
												this.labelCustomerReadingCutOffTime.Text = Convert.ToUInt32(offTimes.CutoffTime).ToString();
												this.lbl_CustomerGarceStartWH.Text = Convert.ToString(offTimes.WorkStart);
												this.lbl_CustomerGarceEndWH.Text = Convert.ToString(offTimes.WorkEnd);
												for (int i = 0; i < 25; i++)
												{
													bool flag32 = offTimes.Holidays[i].ToString() == "0";
													if (flag32)
													{
														break;
													}
													string text4 = Convert.ToUInt32(offTimes.Holidays[i].Day).ToString() + "-" + Convert.ToUInt32(offTimes.Holidays[i].Month).ToString();
													bool flag33 = text4 != "0-0";
													if (flag33)
													{
														this.listBoxCustomerReadingHolidayDay.Items.Add(text4);
													}
												
													*/
												BLL_ChargeBasicInf_Data.ChargeBasicInf_MaxOverdraftCredit = (creditInfo.MaxOverdraftCredit);
												BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = (int)systemActions.Actions;
												/*string text5 = Convert.ToString((int)systemActions.Actions, 2).PadLeft(8, '0');
												bool flag34 = text5.Length > 0 && text5[0] == '1';
												if (flag34)
												{
													this.checkBoxCustomerReadingSetCategoryType.Checked = true;
												}
												bool flag35 = text5.Length > 1 && text5[1] == '1';
												if (flag35)
												{
													this.checkBoxCustomerReadingSetPriceSched.Checked = true;
												}
												bool flag36 = text5.Length > 2 && text5[2] == '1';
												if (flag36)
												{
													this.checkBoxCustomerReadingSetOffTimes.Checked = true;
												}
												bool flag37 = text5.Length > 3 && text5[3] == '1';
												if (flag37)
												{
													this.checkBoxCustomerReadingSetCharge.Checked = true;
												}
												bool flag38 = text5.Length > 4 && text5[4] == '1';
												if (flag38)
												{
													this.checkBoxCustomerReadingSetDeduction.Checked = true;
												}
												bool flag39 = text5.Length > 5 && text5[5] == '1';
												if (flag39)
												{
													this.checkBoxCustomerReadingSetOverDardft.Checked = true;
												}
												bool flag40 = text5.Length > 6 && text5[6] == '1';
												if (flag40)
												{
													this.checkBoxCustomerReadingSetChargeDateInfo.Checked = true;
												*/
												BLL_Readings_Data.Readings_Reading = readings.Reading;
												BLL_Readings_Data.Readings_QuantityTotalNegative= (readings.QuantityTotalNegative);

												BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
												BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
												BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
												BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
												BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
												BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
												BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
												BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
												BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
												BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
												BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
												BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];


												BLL_CreditBalance_Data.ReturnMeterAction = meterActions.Actions;


												BLL_CreditBalance_Data.CreditBalance_RemainCredit=creditBalance.RemainCredit;
												BLL_CreditBalance_Data.CreditBalance_OverdraftCredit=creditBalance.OverdraftCredit;
												BLL_CreditBalance_Data.CreditBalance_ConsumedCredit=creditBalance.ConsumedCredit;
												BLL_CreditBalance_Data.CreditBalance_CumulativeCharges=creditBalance.CumulativeCharges;
												BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
												BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];





												BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
												BLL_MeterState_Data.MeterState_Malfun1_Day= meterState.MalFuns[0].Day;
												BLL_MeterState_Data.MeterState_Malfun1_Month= meterState.MalFuns[0].Month;

												BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
												BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
												BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

												BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
												BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
												BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

												BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
												BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
												BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

												BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
												BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
												BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

												//((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



												BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
												BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
												BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
												BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

												BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
												/*string text7 = Convert.ToString(meterState.MeterErrors, 2).PadLeft(8, '0');
												bool flag41 = text7[0] == '1';
												if (flag41)
												{
													this.lstMeterErrors.Items.Add("Good Battery");
												}
												else
												{
													this.lstMeterErrors.Items.Add("Battery Low");
												}
												bool flag42 = text7[1] == '1';
												if (flag42)
												{
													this.lstMeterErrors.Items.Add("Opened Valve");
												}
												else
												{
													this.lstMeterErrors.Items.Add("Closed Valve");
												}
												bool flag43 = text7[2] == '1';
												if (flag43)
												{
													this.lstMeterErrors.Items.Add("Valve Closing Error");
												}
												else
												{
													this.lstMeterErrors.Items.Add("Valve Closing Succeeded");
												}
												bool flag44 = text7[3] == '1';
												if (flag44)
												{
													this.lstMeterErrors.Items.Add("Valve Opening Error");
												}
												else
												{
													this.lstMeterErrors.Items.Add("Valve Opening Succeeded");
												}*/
												//MessageBox.Show("Read Client Card Successfully");
												Status = true;
											}
										}
									}
								}
							}
							else
							{
								MessageBox.Show("Read Client Card Failed");
							}
						}
						else
						{
							MessageBox.Show("Not Client Card");
						}
					}
				}
			}
			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("Fail To Read Client Card");
			}




			return Status;


        }

		public bool ConfigurationCard_ReadCard(ref List<BLL_CompanyCards_ReturnRead> MeterList)
        {


			bool Status = false;
			try
			{
				int Rannum = GetRandomNumber();

				this.System_Nounce[0] = (byte)(Rannum & 0xff);
				this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
				//mostafa
				byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);
				byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);

				bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());

					Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
					CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);

					Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
					Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

					Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
					SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);






					//CardPublicKey of card from number that received in data batabase
					DAL_CardIssues CardIssues_DAL = new DAL_CardIssues();
					BLLCardIssues_Data = CardIssues_DAL.CardIssueDataSelection((int)mainCardInfo.Id);

					bool flag3 = !Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLLCardIssues_Data.CardProd_KUCP);
					if (flag3)
					{
						MessageBox.Show("Fail To Verify CardProducerToCardCertificate Against Card Producer Public Key");

						return false;
					}
					flag3 = !(mainCardInfo.CardFun == CardFunctionEnum.ConfigureMeter.ToByte() && mainCardInfo.Mode == CardModeEnum.Active.ToByte());
					if (flag3)
					{
						MessageBox.Show("Card Error : This Card not Configuration Card ");
						return false;
					}
				
					string targetEntityPublicKey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
					string text = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);
					DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();
					BLL_WaterComp BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();
					BLLCardIssues_Data.Func = mainCardInfo.CardFun;
					BLLCardIssues_Data.Mode = mainCardInfo.Mode;
					BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
					BLLCardIssues_Data.LastUploadBy = (byte)mainCardInfo.LastUpdatedBy;
					BLLCardIssues_Data.LastTranscationDate = DateModel.GetSystemDate(mainCardInfo.LastTransactionDate);


					byte[] response2 = APDUHELPER.ExecuteAPDU(176, 65, null);
					bool flag7 = APDUHELPER.GetApduResponseEnum(response2) == ApduResponseEnum.SuccessResponse;
					if (flag7)
					{


						APDUData apdudata2 = APDUData.ParseTLV(response2.Take(response2.Length - 2).ToArray<byte>());
						
						bool flag0=Signer.ValidateSignature(apdudata2.payloadData, apdudata2.Signature,  BLL_WaterComp_Data.WaterComp_KUW );


                        if (flag0 == false)
                        {
							MessageBox.Show("Eroro Verify Payload");
							return false;

						}
					

						Tlv tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
						SignedNonce SignNonceRead = SignedNonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);  //not use in this scope 


						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 196);
						SignedNonce SignPayload = SignedNonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);


						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
						Nonce CardNounce = Nonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);

						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 205);
						ConfigurationCardInfo configurationCardInfo = ConfigurationCardInfo.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);
						//Mostafa
						BLL_Configuration_Data.ConfigCard_MeterAction = configurationCardInfo.Actions;
						BLL_Configuration_Data.ConfigCard_StartConsumerID = (int)configurationCardInfo.StartConsumerID;
						BLL_Configuration_Data.ConfigCard_EndConsumerID = (int)configurationCardInfo.EndConsumerID;
						BLL_Configuration_Data.ConfigCard_RestDate = DateModel.GetSystemDate(configurationCardInfo.ResetDate);
						BLL_Configuration_Data.ConfigCard_TimeEffective = configurationCardInfo.TimesEffective;

						//P1_Paramter_ReadCountList              = (byte)0x5E;
						//ReadCountList  DataCard.Nonce_Tag, DataCard.SignedNonce_Tag, 


						Rannum = GetRandomNumber();

						this.System_Nounce[0] = (byte)(Rannum & 0xff);
						this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);


						byte[] payloadDataseesion1 = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, CardNounce.Serialize(), BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLLCardIssues_Data.WaterComp_KPW), null);

						byte[] response3 = APDUHELPER.ExecuteAPDU(176, 94, payloadDataseesion1);
						flag7 = APDUHELPER.GetApduResponseEnum(response3) == ApduResponseEnum.SuccessResponse;
						if (flag7)
						{
							//get data of countlist from meter that setuped by meter 

							//

							Status = true;
							APDUData apdudata3 = APDUData.ParseTLV(response3.Take(response3.Length - 2).ToArray<byte>());

							Tlv tlvsn3 = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
							SignNonceRead = SignedNonce.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);

							tlvsn3 = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 122);
							CountList CoutnlistArr = CountList.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);


							tlvsn3 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
							CardNounce = Nonce.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);


							ushort CountListInt = ((ushort)(CoutnlistArr.CountListMeters[0] + (CoutnlistArr.CountListMeters[1] << 8)));

							BLL_Configuration_Data.CountList = CountListInt;
							for (ushort index = 0; index < CountListInt; index++)
							{
								CountList MeterListIndex = new CountList();
							

							  MeterListIndex.CountListMeters[0] = (byte)index;
						      MeterListIndex.CountListMeters[1] = (byte)(index >> 8);

								

								
						
								Rannum = GetRandomNumber();

								System_Nounce[0] = (byte)(Rannum & 0xff);
								System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);



								byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, (CardNonce.Serialize()), BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLLCardIssues_Data.WaterComp_KPW),false, new ModelBase[]
											{
												MeterListIndex

											});
							



								byte[] response4 = APDUHELPER.ExecuteAPDU(176, 67, payloadData);
								flag7 = APDUHELPER.GetApduResponseEnum(response4) == ApduResponseEnum.SuccessResponse;
								if (flag7)
								{
									APDUData apdudata4 = APDUData.ParseTLV(response4.Take(response4.Length - 2).ToArray<byte>());

									Tlv tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
									SignNonceRead = SignedNonce.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);

									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
									CardNounce = Nonce.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);
									



									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
									MeterActions MeterActionMeter = MeterActions.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);

									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
									MeterInfo MeterInoMeter = MeterInfo.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);
									//	MeterInfo
									//return meter action 
									BLL_CompanyCards_ReturnRead companyCard = new BLL_CompanyCards_ReturnRead();

									// Set its properties
									companyCard.BLL_MeterISsues_Data = new BLL_MeterIssues(); // Initialize BLL_MeterIssues object
																							  // Set properties of BLL_MeterIssues object
									companyCard.BLL_MeterISsues_Data.Meter_ChargeMode = MeterInoMeter.ChargeMode; // Set properties as needed
									companyCard.BLL_MeterISsues_Data.Meter_MeterNum = (int)MeterInoMeter.MeterId;
									companyCard.BLL_MeterISsues_Data.Meter_Origin = MeterInoMeter.MeterOrigin;
									companyCard.BLL_MeterISsues_Data.Meter_Diameter = MeterInoMeter.MeterDim;

									companyCard.Return_MeterAction = MeterActionMeter.Actions; // Set Return_MeterAction value
									MeterList.Add(companyCard);

									//send data of index and wait to read it and save it as string 

									//APDUData apdudata3 = APDUData.ParseTLV(response2.Take(response2.Length - 2).ToArray<byte>());

									//verify to insure that data is very OK or not verify any thing 
								}


							}
							Status = true;


							//CardIssues_DAL.Update(BLLCardIssues_Data);
							//DAL_ConfigCard Object = new DAL_ConfigCard();
							//Status = Object.Insert(ConfigData);
						}
						else
						{
							MessageBox.Show("Fail to Read Card");
						}



					}

				}
			}

			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("SYSTEM_ERROR");
			}
			return Status;

        }

		public bool MaintCardCard_ReadCard( ref List<BLL_CompanyCards_ReturnRead> MeterList)
		{


			bool Status = false;
			try
			{
				int Rannum = GetRandomNumber();

				this.System_Nounce[0] = (byte)(Rannum & 0xff);
				this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
				//mostafa
				byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);
				byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);

				bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());

					Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
					CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);

					Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
					Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

					Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
					SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);






					//CardPublicKey of card from number that received in data batabase
					DAL_CardIssues CardIssues_DAL = new DAL_CardIssues();
					BLLCardIssues_Data = CardIssues_DAL.CardIssueDataSelection((int)mainCardInfo.Id);

					bool flag3 = !Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLLCardIssues_Data.CardProd_KUCP);
					if (flag3)
					{
						MessageBox.Show("Fail To Verify CardProducerToCardCertificate Against Card Producer Public Key");

						return false;
					}
					flag3 = !(mainCardInfo.CardFun == CardFunctionEnum.ConfigureMeter.ToByte() && mainCardInfo.Mode == CardModeEnum.Active.ToByte());
					if (flag3)
					{
						MessageBox.Show("Card Error : This Card not Configuration Card ");
						return false;
					}

					string targetEntityPublicKey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
					string text = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);
					DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();
					BLL_WaterComp BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();

					BLLCardIssues_Data.Func = mainCardInfo.CardFun;
					BLLCardIssues_Data.Mode = mainCardInfo.Mode;
					BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
					BLLCardIssues_Data.LastUploadBy = (byte)mainCardInfo.LastUpdatedBy;
					BLLCardIssues_Data.LastTranscationDate = DateModel.GetSystemDate(mainCardInfo.LastTransactionDate);





					byte[] response2 = APDUHELPER.ExecuteAPDU(176, 69, null);
					bool flag7 = APDUHELPER.GetApduResponseEnum(response2) == ApduResponseEnum.SuccessResponse;
					if (flag7)
					{


						APDUData apdudata2 = APDUData.ParseTLV(response2.Take(response2.Length - 2).ToArray<byte>());

						bool flag0 = Signer.ValidateSignature(apdudata2.payloadData, apdudata2.Signature, BLL_WaterComp_Data.WaterComp_KUW);


						if (flag0 == false)
						{
							MessageBox.Show("Eroro Verify Payload");
							return false;

						}


						Tlv tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
						SignedNonce SignNonceRead = SignedNonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);  //not use in this scope 


						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 196);
						SignedNonce SignPayload = SignedNonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);


						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
						Nonce CardNounce = Nonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);

						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 206);
						MaintenanceCardInfo MaintCardInfo = MaintenanceCardInfo.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);
						BLL_MaintCard_Data.MaintCard_MeterAction = MaintCardInfo.Actions;
						BLL_MaintCard_Data.MaintCard_ClientCateg = MaintCardInfo.ClientCategory;
						BLL_MaintCard_Data.MaintCard_SwegToAPPly = MaintCardInfo.SewageToApply;
						BLL_MaintCard_Data.MaintCard_ActionDate = DateModel.GetSystemDate(MaintCardInfo.ActionDate);
						BLL_MaintCard_Data.MaintCard_CategToapply = MaintCardInfo.CategoryToApply;
						BLL_MaintCard_Data.MaintCard_StartConsumerID =(int) MaintCardInfo.StartConsumerID;
						BLL_MaintCard_Data.MaintCard_EndConsumerID =(int) MaintCardInfo.EndConsumerID;
						BLL_MaintCard_Data.MaintCard_TimeEffective = MaintCardInfo.TimesEffective;


						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 200);
						PriceSchedule priceSchedule = PriceSchedule.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);

						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
						Deductions deductions = Deductions.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);

						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 202);
						OffTimes offTimes = OffTimes.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);
						
						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
						ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);


						//P1_Paramter_ReadCountList              = (byte)0x5E;
						//ReadCountList  DataCard.Nonce_Tag, DataCard.SignedNonce_Tag, 
						BLL_PriceScheduler_Data.PriceSchedule_APPDate = DateModel.GetSystemDate(priceSchedule.AppDate);
						BLL_Deductions_Data.Deductions_Month = (deductions.Month);
						BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
						BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);


						Rannum = GetRandomNumber();

						this.System_Nounce[0] = (byte)(Rannum & 0xff);
						this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);


						byte[] payloadDataseesion1 = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, CardNounce.Serialize(), BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLLCardIssues_Data.WaterComp_KPW), null);

						byte[] response3 = APDUHELPER.ExecuteAPDU(176, 94, payloadDataseesion1);
						flag7 = APDUHELPER.GetApduResponseEnum(response3) == ApduResponseEnum.SuccessResponse;
						if (flag7)
						{
							//get data of countlist from meter that setuped by meter 

							//

							Status = true;
							APDUData apdudata3 = APDUData.ParseTLV(response3.Take(response3.Length - 2).ToArray<byte>());

							Tlv tlvsn3 = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
							SignNonceRead = SignedNonce.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);

							tlvsn3 = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 122);
							CountList CoutnlistArr = CountList.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);


							tlvsn3 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
							CardNounce = Nonce.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);


							ushort CountListInt = ((ushort)(CoutnlistArr.CountListMeters[0] + (CoutnlistArr.CountListMeters[1] << 8)));
							BLL_MaintCard_Data.CountList = CountListInt;

							for (ushort index = 0; index < CountListInt; index++)
							{
								CountList MeterListIndex = new CountList();


								MeterListIndex.CountListMeters[0] = (byte)index;
								MeterListIndex.CountListMeters[1] = (byte)(index >> 8);





								Rannum = GetRandomNumber();

								System_Nounce[0] = (byte)(Rannum & 0xff);
								System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);



								byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, (CardNonce.Serialize()), BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLLCardIssues_Data.WaterComp_KPW), false, new ModelBase[]
											{
												MeterListIndex

											});




								byte[] response4 = APDUHELPER.ExecuteAPDU(176, 70, payloadData);
								flag7 = APDUHELPER.GetApduResponseEnum(response4) == ApduResponseEnum.SuccessResponse;
								if (flag7)
								{
									APDUData apdudata4 = APDUData.ParseTLV(response4.Take(response4.Length - 2).ToArray<byte>());

									Tlv tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
									SignNonceRead = SignedNonce.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);

									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
									CardNounce = Nonce.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);




									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
									MeterActions MeterActionMeter = MeterActions.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);

									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
									MeterInfo MeterInoMeter = MeterInfo.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);
									//	MeterInfo
									//return meter action 

									BLL_CompanyCards_ReturnRead companyCard = new BLL_CompanyCards_ReturnRead();

									// Set its properties
									companyCard.BLL_MeterISsues_Data = new BLL_MeterIssues(); // Initialize BLL_MeterIssues object
																							  // Set properties of BLL_MeterIssues object
									companyCard.BLL_MeterISsues_Data.Meter_ChargeMode = MeterInoMeter.ChargeMode; // Set properties as needed
									companyCard.BLL_MeterISsues_Data.Meter_MeterNum = (int)MeterInoMeter.MeterId;
									companyCard.BLL_MeterISsues_Data.Meter_Origin = MeterInoMeter.MeterOrigin;
									companyCard.BLL_MeterISsues_Data.Meter_Diameter = MeterInoMeter.MeterDim;

									companyCard.Return_MeterAction = MeterActionMeter.Actions; // Set Return_MeterAction value
									MeterList.Add(companyCard);
									//send data of index and wait to read it and save it as string 

									//APDUData apdudata3 = APDUData.ParseTLV(response2.Take(response2.Length - 2).ToArray<byte>());

									//verify to insure that data is very OK or not verify any thing 
								}


							}


							//CardIssues_DAL.Update(BLLCardIssues_Data);
							//DAL_ConfigCard Object = new DAL_ConfigCard();
							//Status = Object.Insert(ConfigData);
						}
						else
						{
							MessageBox.Show("Fail to Read Card");
						}



					}

				}
			}

			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("SYSTEM_ERROR");
			}
			return Status;

		}


		public bool RetrivalCard_ReadCard(ref List<BLL_RetrivalCard_ReturnRead> retrivalList, ref List<BLL_CompanyCards_ReturnRead> MeterList)
		{


			bool Status = false;
			try
			{
				int Rannum = GetRandomNumber();

				this.System_Nounce[0] = (byte)(Rannum & 0xff);
				this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);
				//mostafa
				byte[] payloadDataseesion = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, null, BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(0), null, null);
				byte[] array = APDUHELPER.ExecuteAPDU(176, 59, payloadDataseesion);

				bool flag = APDUHELPER.GetApduResponseEnum(array) == ApduResponseEnum.SuccessResponse;
				if (flag)
				{
					APDUData apdudata = APDUData.ParseTLV(array.Take(array.Length - 2).ToArray<byte>());

					Tlv tlv = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 197);
					MainCardInfo mainCardInfo = MainCardInfo.Deserialize((tlv != null) ? tlv.Value : null);
					Tlv tlv2 = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 217);
					CardProducerToCardCertificate cardProducerToCardCertificate = CardProducerToCardCertificate.Deserialize((tlv2 != null) ? tlv2.Value : null);

					Tlv tlvn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 219);   //save as array short
					Nonce CardNonce = Nonce.Deserialize((tlvn != null) ? tlvn.Value : null);

					Tlv tlvsn = apdudata.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
					SignedNonce SignNonce = SignedNonce.Deserialize((tlvsn != null) ? tlvsn.Value : null);






					//CardPublicKey of card from number that received in data batabase
					DAL_CardIssues CardIssues_DAL = new DAL_CardIssues();
					BLLCardIssues_Data = CardIssues_DAL.CardIssueDataSelection((int)mainCardInfo.Id);

					bool flag3 = !Signer.VerifyX509Certificate(GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate), BLLCardIssues_Data.CardProd_KUCP);
					if (flag3)
					{
						MessageBox.Show("Fail To Verify CardProducerToCardCertificate Against Card Producer Public Key");

						return false;
					}
					flag3 = !(mainCardInfo.CardFun == CardFunctionEnum.RetrieveMeterInfo.ToByte() && mainCardInfo.Mode == CardModeEnum.Active.ToByte());
					if (flag3)
					{
						MessageBox.Show("Card Error : This Card not Configuration Card ");
						return false;
					}

					string targetEntityPublicKey = Signer.ExtractPublicKeyFromCertificate(cardProducerToCardCertificate.Certificate);
					string text = GeneralUtility.ByteArrayToHex(cardProducerToCardCertificate.Certificate);
					DAL_WaterComp DAL_WaterComp_Obj = new DAL_WaterComp();
					BLL_WaterComp BLL_WaterComp_Data = DAL_WaterComp_Obj.GetWaterCompData();

				
					BLLCardIssues_Data.Func = mainCardInfo.CardFun;
					BLLCardIssues_Data.Mode = mainCardInfo.Mode;
					BLLCardIssues_Data.WaterCompID = mainCardInfo.CompanyId;
					BLLCardIssues_Data.LastUploadBy = (byte)mainCardInfo.LastUpdatedBy;
					BLLCardIssues_Data.LastTranscationDate= DateModel.GetSystemDate(mainCardInfo.LastTransactionDate);




					byte[] response2 = APDUHELPER.ExecuteAPDU(176, 72, null);
					bool flag7 = APDUHELPER.GetApduResponseEnum(response2) == ApduResponseEnum.SuccessResponse;
					if (flag7)
					{


						APDUData apdudata2 = APDUData.ParseTLV(response2.Take(response2.Length - 2).ToArray<byte>());

						bool flag0 = Signer.ValidateSignature(apdudata2.payloadData, apdudata2.Signature, BLL_WaterComp_Data.WaterComp_KUW);


						if (flag0 == false)
						{
							MessageBox.Show("Eroro Verify Payload");
							return false;

						}


						Tlv tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
						SignedNonce SignNonceRead = SignedNonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);  //not use in this scope 


						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 196);
						SignedNonce SignPayload = SignedNonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);


						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
						Nonce CardNounce = Nonce.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);

						tlvsn2 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 207);
						RetrievalCardInfo retrievalCardInfo = RetrievalCardInfo.Deserialize((tlvsn2 != null) ? tlvsn2.Value : null);
						//Mostafa

						BLL_RetrivalCard_Data.RetrivalCard_RequiredData = retrievalCardInfo.Actions;
						BLL_RetrivalCard_Data.RetrivalCard_StartConsumerID = (int)retrievalCardInfo.StartConsumerID;
						BLL_RetrivalCard_Data.RetrivalCard_EndConsumerID = (int)retrievalCardInfo.EndConsumerID;
						BLL_RetrivalCard_Data.RetrivalCard_TimeEffective = retrievalCardInfo.TimesEffective;
						//P1_Paramter_ReadCountList              = (byte)0x5E;
						//ReadCountList  DataCard.Nonce_Tag, DataCard.SignedNonce_Tag, 
                       

						Rannum = GetRandomNumber();

						this.System_Nounce[0] = (byte)(Rannum & 0xff);
						this.System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);


						byte[] payloadDataseesion1 = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, CardNounce.Serialize(), BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLLCardIssues_Data.WaterComp_KPW), null);

						byte[] response3 = APDUHELPER.ExecuteAPDU(176, 94, payloadDataseesion1);
						flag7 = APDUHELPER.GetApduResponseEnum(response3) == ApduResponseEnum.SuccessResponse;
						if (flag7)
						{
							//get data of countlist from meter that setuped by meter 

							//

							Status = true;
							APDUData apdudata3 = APDUData.ParseTLV(response3.Take(response3.Length - 2).ToArray<byte>());

							Tlv tlvsn3 = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
							SignNonceRead = SignedNonce.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);

							tlvsn3 = apdudata3.Models.FirstOrDefault((Tlv m) => m.Tag == 122);
							CountList CoutnlistArr = CountList.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);


							tlvsn3 = apdudata2.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
							CardNounce = Nonce.Deserialize((tlvsn3 != null) ? tlvsn3.Value : null);


							ushort CountListInt = ((ushort)(CoutnlistArr.CountListMeters[0] + (CoutnlistArr.CountListMeters[1] << 8)));
							BLL_RetrivalCard_Data.CountList = CountListInt;

							for (ushort index = 0; index < CountListInt; index++)
							{
								CountList MeterListIndex = new CountList();


								MeterListIndex.CountListMeters[0] = (byte)index;
								MeterListIndex.CountListMeters[1] = (byte)(index >> 8);





								Rannum = GetRandomNumber();

								System_Nounce[0] = (byte)(Rannum & 0xff);
								System_Nounce[1] = (byte)(Rannum >> 8 & 0xff);



								byte[] payloadData = BERTLVPayloadBuilder.GeneratePayload(System_Nounce, (CardNonce.Serialize()), BitConverter.GetBytes(0), BitConverter.GetBytes(0), BitConverter.GetBytes(mainCardInfo.Id), GeneralUtility.HexToByteArray(BLLCardIssues_Data.WaterComp_KPW), false, new ModelBase[]
											{
												MeterListIndex

											});




								byte[] response4 = APDUHELPER.ExecuteAPDU(176, 80, payloadData);
								flag7 = APDUHELPER.GetApduResponseEnum(response4) == ApduResponseEnum.SuccessResponse;
								if (flag7)
								{
									APDUData apdudata4 = APDUData.ParseTLV(response4.Take(response4.Length - 2).ToArray<byte>());

									Tlv tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 220);
									SignNonceRead = SignedNonce.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);

									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 171);
									CardNounce = Nonce.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);

														

									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 204);
									MeterActions MeterActionMeter = MeterActions.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);

									tlvsn4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 199);
									MeterInfo MeterInoMeter = MeterInfo.Deserialize((tlvsn4 != null) ? tlvsn4.Value : null);



									// Create a new BLL_CompanyCards_ReturnRead object
									BLL_CompanyCards_ReturnRead companyCard = new BLL_CompanyCards_ReturnRead();

									// Set its properties
									companyCard.BLL_MeterISsues_Data = new BLL_MeterIssues(); // Initialize BLL_MeterIssues object
																							  // Set properties of BLL_MeterIssues object
									companyCard.BLL_MeterISsues_Data.Meter_ChargeMode= MeterInoMeter.ChargeMode; // Set properties as needed
									companyCard.BLL_MeterISsues_Data.Meter_MeterNum = (int)MeterInoMeter.MeterId;
									companyCard.BLL_MeterISsues_Data.Meter_Origin= MeterInoMeter.MeterOrigin;
									companyCard.BLL_MeterISsues_Data.Meter_Diameter = MeterInoMeter.MeterDim;

									companyCard.Return_MeterAction = MeterActionMeter.Actions; // Set Return_MeterAction value
									MeterList.Add(companyCard);


									Tlv tlv23 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 198);
									ClientCardInfo clientCardInfo = ClientCardInfo.Deserialize((tlv23 != null) ? tlv23.Value : null);

									Tlv tlv3 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 200);
									PriceSchedule priceSchedule = PriceSchedule.Deserialize((tlv3 != null) ? tlv3.Value : null);

									Tlv tlv4 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 201);
									Deductions deductions = Deductions.Deserialize((tlv4 != null) ? tlv4.Value : null);

									Tlv tlv5 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 202);
									OffTimes offTimes = OffTimes.Deserialize((tlv5 != null) ? tlv5.Value : null);

									Tlv tlv6 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 203);
									CreditInfo creditInfo = CreditInfo.Deserialize((tlv6 != null) ? tlv6.Value : null);

									Tlv tlv8 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 221);
									SystemActions systemActions = SystemActions.Deserialize((tlv8 != null) ? tlv8.Value : null);

									Tlv tlv10 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 208);
									CreditBalance creditBalance = CreditBalance.Deserialize((tlv10 != null) ? tlv10.Value : null);

									Tlv tlv11 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 209);
									Readings readings = Readings.Deserialize((tlv11 != null) ? tlv11.Value : null);

									Tlv tlv12 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 210);
									MeterState meterState = MeterState.Deserialize((tlv12 != null) ? tlv12.Value : null);
								
									Tlv tlv13 = apdudata4.Models.FirstOrDefault((Tlv m) => m.Tag == 195);
									ChargeDateInfo chargeDateInfo = ChargeDateInfo.Deserialize((tlv13 != null) ? tlv13.Value : null);


								

									//Clinet info
									BLL_ClientInfo_Data.ClientInfo_Activity = clientCardInfo.Activity;
									//BLL_Client_Data.Client_Number = (int)clientCardInfo.ClientId;
									BLL_ClientInfo_Data.ClientInfo_Category = clientCardInfo.Category;
									BLL_ClientInfo_Data.ClientInfo_SwGServices = clientCardInfo.SewageService;
									BLL_ClientInfo_Data.ClientInfo_NumOFUnit = clientCardInfo.NoOfUnits;

									BLL_ClientInfo_Data.ClientInfo_SubscriberID = (int)clientCardInfo.ClientId;
									
									//charge basci info 
									BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeAmount = creditInfo.ChargeAmount;
									BLL_ChargeBasicInf_Data.ChargeBasicInf_CutoffWarningLimit = creditInfo.CutoffWarningLimit;
									BLL_ChargeBasicInf_Data.ChargeBasicInf_ChargeNo = (int)creditInfo.ChargeNo;


									//Priceschduler
									/*
									this.labelCustomerPriceLevel1.Text = priceSchedule.Levels[0].Price.ToString();
									this.labelCustomerPriceLevel2.Text = priceSchedule.Levels[1].Price.ToString();
									this.labelCustomerPriceLevel3.Text = priceSchedule.Levels[2].Price.ToString();
									this.labelCustomerPriceLevel4.Text = priceSchedule.Levels[3].Price.ToString();
									this.labelCustomerPriceLevel5.Text = priceSchedule.Levels[4].Price.ToString();
									this.labelCustomerPriceLevel6.Text = priceSchedule.Levels[5].Price.ToString();
									this.labelCustomerPriceLevel7.Text = priceSchedule.Levels[6].Price.ToString();
									this.labelCustomerPriceLevel8.Text = priceSchedule.Levels[7].Price.ToString();
									this.labelCustomerPriceLevel9.Text = priceSchedule.Levels[8].Price.ToString();
									this.labelCustomerPriceLevel10.Text = priceSchedule.Levels[9].Price.ToString();
									this.labelCustomerPriceLevel11.Text = priceSchedule.Levels[10].Price.ToString();
									this.labelCustomerPriceLevel12.Text = priceSchedule.Levels[11].Price.ToString();
									this.labelCustomerPriceLevel13.Text = priceSchedule.Levels[12].Price.ToString();
									this.labelCustomerPriceLevel14.Text = priceSchedule.Levels[13].Price.ToString();
									this.labelCustomerPriceLevel15.Text = priceSchedule.Levels[14].Price.ToString();
									this.labelCustomerPriceLevel16.Text = priceSchedule.Levels[15].Price.ToString();
									this.labelCustomerToLevel1.Text = priceSchedule.Levels[0].StepMax.ToString();
									this.labelCustomerToLevel2.Text = priceSchedule.Levels[1].StepMax.ToString();
									this.labelCustomerToLevel3.Text = priceSchedule.Levels[2].StepMax.ToString();
									this.labelCustomerToLevel4.Text = priceSchedule.Levels[3].StepMax.ToString();
									this.labelCustomerToLevel5.Text = priceSchedule.Levels[4].StepMax.ToString();
									this.labelCustomerToLevel6.Text = priceSchedule.Levels[5].StepMax.ToString();
									this.labelCustomerToLevel7.Text = priceSchedule.Levels[6].StepMax.ToString();
									this.labelCustomerToLevel8.Text = priceSchedule.Levels[7].StepMax.ToString();
									this.labelCustomerToLevel9.Text = priceSchedule.Levels[8].StepMax.ToString();
									this.labelCustomerToLevel10.Text = priceSchedule.Levels[9].StepMax.ToString();
									this.labelCustomerToLevel11.Text = priceSchedule.Levels[10].StepMax.ToString();
									this.labelCustomerToLevel12.Text = priceSchedule.Levels[11].StepMax.ToString();
									this.labelCustomerToLevel13.Text = priceSchedule.Levels[12].StepMax.ToString();
									this.labelCustomerToLevel14.Text = priceSchedule.Levels[13].StepMax.ToString();
									this.labelCustomerToLevel15.Text = priceSchedule.Levels[14].StepMax.ToString();
									this.labelCustomerToLevel16.Text = priceSchedule.Levels[15].StepMax.ToString();
									string text = Convert.ToString((int)priceSchedule.Pricing, 2).PadLeft(16, '0');
									bool flag55 = text.Length > 0 && text[0] == '1';
									if (flag55)
									{
										this.checkBoxCustomerReadingCumulativeLevel1.Checked = true;
									}
									bool flag6 = text.Length > 1 && text[1] == '1';
									if (flag6)
									{
										this.checkBoxCustomerReadingCumulativeLevel2.Checked = true;
									}
									bool flag7 = text.Length > 2 && text[2] == '1';
									if (flag7)
									{
										this.checkBoxCustomerReadingCumulativeLevel3.Checked = true;
									}
									bool flag8 = text.Length > 3 && text[3] == '1';
									if (flag8)
									{
										this.checkBoxCustomerReadingCumulativeLevel4.Checked = true;
									}
									bool flag9 = text.Length > 4 && text[4] == '1';
									if (flag9)
									{
										this.checkBoxCustomerReadingCumulativeLevel5.Checked = true;
									}
									bool flag10 = text.Length > 5 && text[5] == '1';
									if (flag10)
									{
										this.checkBoxCustomerReadingCumulativeLevel6.Checked = true;
									}
									bool flag11 = text.Length > 6 && text[6] == '1';
									if (flag11)
									{
										this.checkBoxCustomerReadingCumulativeLevel7.Checked = true;
									}
									bool flag12 = text.Length > 7 && text[7] == '1';
									if (flag12)
									{
										this.checkBoxCustomerReadingCumulativeLevel8.Checked = true;
									}
									bool flag13 = text.Length > 8 && text[8] == '1';
									if (flag13)
									{
										this.checkBoxCustomerReadingCumulativeLevel9.Checked = true;
									}
									bool flag14 = text.Length > 9 && text[9] == '1';
									if (flag14)
									{
										this.checkBoxCustomerReadingCumulativeLevel10.Checked = true;
									}
									bool flag15 = text.Length > 10 && text[10] == '1';
									if (flag15)
									{
										this.checkBoxCustomerReadingCumulativeLevel11.Checked = true;
									}
									bool flag16 = text.Length > 11 && text[11] == '1';
									if (flag16)
									{
										this.checkBoxCustomerReadingCumulativeLevel12.Checked = true;
									}
									bool flag17 = text.Length > 12 && text[12] == '1';
									if (flag17)
									{
										this.checkBoxCustomerReadingCumulativeLevel13.Checked = true;
									}
									bool flag18 = text.Length > 13 && text[13] == '1';
									if (flag18)
									{
										this.checkBoxCustomerReadingCumulativeLevel14.Checked = true;
									}
									bool flag19 = text.Length > 14 && text[14] == '1';
									if (flag19)
									{
										this.checkBoxCustomerReadingCumulativeLevel15.Checked = true;
									}
									bool flag20 = text.Length > 15 && text[15] == '1';
									if (flag20)
									{
										this.checkBoxCustomerReadingCumulativeLevel16.Checked = true;
									}
									this.labelCustomerFeeLevel1.Text = priceSchedule.Levels[0].Fee.ToString();
									this.labelCustomerFeeLevel2.Text = priceSchedule.Levels[1].Fee.ToString();
									this.labelCustomerFeeLevel3.Text = priceSchedule.Levels[2].Fee.ToString();
									this.labelCustomerFeeLevel4.Text = priceSchedule.Levels[3].Fee.ToString();
									this.labelCustomerFeeLevel5.Text = priceSchedule.Levels[4].Fee.ToString();
									this.labelCustomerFeeLevel6.Text = priceSchedule.Levels[5].Fee.ToString();
									this.labelCustomerFeeLevel7.Text = priceSchedule.Levels[6].Fee.ToString();
									this.labelCustomerFeeLevel8.Text = priceSchedule.Levels[7].Fee.ToString();
									this.labelCustomerFeeLevel9.Text = priceSchedule.Levels[8].Fee.ToString();
									this.labelCustomerFeeLevel10.Text = priceSchedule.Levels[9].Fee.ToString();
									this.labelCustomerFeeLevel11.Text = priceSchedule.Levels[10].Fee.ToString();
									this.labelCustomerFeeLevel12.Text = priceSchedule.Levels[11].Fee.ToString();
									this.labelCustomerFeeLevel13.Text = priceSchedule.Levels[12].Fee.ToString();
									this.labelCustomerFeeLevel14.Text = priceSchedule.Levels[13].Fee.ToString();
									this.labelCustomerFeeLevel15.Text = priceSchedule.Levels[14].Fee.ToString();
									this.labelCustomerFeeLevel16.Text = priceSchedule.Levels[15].Fee.ToString();
									this.labelCustmerReadingMonthFees1.Text = priceSchedule.MonthFee1.ToString();
									this.labelCustmerReadingMonthFees2.Text = priceSchedule.MonthFee2.ToString();
									this.labelCustmerReadingPerMeterFees.Text = priceSchedule.PerMeterFees.ToString();
									this.checkBoxCustomerReadingFeesIncludeUnitsInCalc.Checked = Convert.ToBoolean(priceSchedule.NoOfUnitsInCalc);
									string text2 = Convert.ToString(priceSchedule.MonthFeesOptions, 2).PadLeft(8, '0');
									bool flag21 = text2.Length > 0 && text2[0] == '1';
									if (flag21)
									{
										this.checkBoxCustomerReadingFixedFees.Checked = true;
									}
									bool flag22 = text2.Length > 1 && text2[1] == '1';
									if (flag22)
									{
										this.checkBoxCustomerReadingPerUnitFees.Checked = true;
									}
									bool flag23 = text2.Length > 2 && text2[2] == '1';
									if (flag23)
									{
										this.checkBoxCustomerReadingStepFees.Checked = true;
									}
									bool flag24 = text2.Length > 3 && text2[3] == '1';
									if (flag24)
									{
										this.checkBoxCustomerReadingStepPerUnitFees.Checked = true;
									}
									this.labelCustomerReadingSwdPercent.Text = Convert.ToUInt32(priceSchedule.SewagePercentage).ToString();
									this.labelCustmerReadingSwgPrice.Text = priceSchedule.SewagePrice.ToString();
									*/
									BLL_PriceScheduler_Data.PriceSchedule_APPDate = DateModel.GetSystemDate(priceSchedule.AppDate);
									BLL_Deductions_Data.Deductions_Month = (deductions.Month);
									BLL_Deductions_Data.Deductions_MonthFees = (deductions.MonthFees);
									BLL_Deductions_Data.Deductions_AppDate = DateModel.GetSystemDate(deductions.AppDate);



									/*string text3 = Convert.ToString(offTimes.WorkingDays, 2).PadLeft(8, '0');
									bool flag25 = text3.Length > 0 && text3[0] == '1';
									if (flag25)
									{
										this.checkBoxCustomerReadingSat.Checked = true;
									}
									bool flag26 = text3.Length > 1 && text3[1] == '1';
									if (flag26)
									{
										this.checkBoxCustomerReadingSun.Checked = true;
									}
									bool flag27 = text3.Length > 2 && text3[2] == '1';
									if (flag27)
									{
										this.checkBoxCustomerReadingMon.Checked = true;
									}
									bool flag28 = text3.Length > 3 && text3[3] == '1';
									if (flag28)
									{
										this.checkBoxCustomerReadingTue.Checked = true;
									}
									bool flag29 = text3.Length > 4 && text3[4] == '1';
									if (flag29)
									{
										this.checkBoxCustomerReadingWed.Checked = true;
									}
									bool flag30 = text3.Length > 5 && text3[5] == '1';
									if (flag30)
									{
										this.checkBoxCustomerReadingThu.Checked = true;
									}
									bool flag31 = text3.Length > 6 && text3[6] == '1';
									if (flag31)
									{
										this.checkBoxCustomerReadingFri.Checked = true;
									}
									this.labelCustomerReadingGracePeriod.Text = Convert.ToUInt32(offTimes.GracePeriod).ToString();
									this.labelCustomerReadingCutOffTime.Text = Convert.ToUInt32(offTimes.CutoffTime).ToString();
									this.lbl_CustomerGarceStartWH.Text = Convert.ToString(offTimes.WorkStart);
									this.lbl_CustomerGarceEndWH.Text = Convert.ToString(offTimes.WorkEnd);
									for (int i = 0; i < 25; i++)
									{
										bool flag32 = offTimes.Holidays[i].ToString() == "0";
										if (flag32)
										{
											break;
										}
										string text4 = Convert.ToUInt32(offTimes.Holidays[i].Day).ToString() + "-" + Convert.ToUInt32(offTimes.Holidays[i].Month).ToString();
										bool flag33 = text4 != "0-0";
										if (flag33)
										{
											this.listBoxCustomerReadingHolidayDay.Items.Add(text4);
										}

										*/
									BLL_ChargeBasicInf_Data.ChargeBasicInf_MaxOverdraftCredit = (creditInfo.MaxOverdraftCredit);
									BLL_ChargeBasicInf_Data.ChargeBasicInf_MeterAction = (int)systemActions.Actions;
									/*string text5 = Convert.ToString((int)systemActions.Actions, 2).PadLeft(8, '0');
									bool flag34 = text5.Length > 0 && text5[0] == '1';
									if (flag34)
									{
										this.checkBoxCustomerReadingSetCategoryType.Checked = true;
									}
									bool flag35 = text5.Length > 1 && text5[1] == '1';
									if (flag35)
									{
										this.checkBoxCustomerReadingSetPriceSched.Checked = true;
									}
									bool flag36 = text5.Length > 2 && text5[2] == '1';
									if (flag36)
									{
										this.checkBoxCustomerReadingSetOffTimes.Checked = true;
									}
									bool flag37 = text5.Length > 3 && text5[3] == '1';
									if (flag37)
									{
										this.checkBoxCustomerReadingSetCharge.Checked = true;
									}
									bool flag38 = text5.Length > 4 && text5[4] == '1';
									if (flag38)
									{
										this.checkBoxCustomerReadingSetDeduction.Checked = true;
									}
									bool flag39 = text5.Length > 5 && text5[5] == '1';
									if (flag39)
									{
										this.checkBoxCustomerReadingSetOverDardft.Checked = true;
									}
									bool flag40 = text5.Length > 6 && text5[6] == '1';
									if (flag40)
									{
										this.checkBoxCustomerReadingSetChargeDateInfo.Checked = true;
									*/
									BLL_Readings_Data.Readings_Reading = readings.Reading;
									BLL_Readings_Data.Readings_QuantityTotalNegative = (readings.QuantityTotalNegative);

									BLL_Readings_Data.Readings_MonthConsumption1 = readings.MonthlyConsumption[0];
									BLL_Readings_Data.Readings_MonthConsumption2 = readings.MonthlyConsumption[1];
									BLL_Readings_Data.Readings_MonthConsumption3 = readings.MonthlyConsumption[2];
									BLL_Readings_Data.Readings_MonthConsumption4 = readings.MonthlyConsumption[3];
									BLL_Readings_Data.Readings_MonthConsumption5 = readings.MonthlyConsumption[4];
									BLL_Readings_Data.Readings_MonthConsumption6 = readings.MonthlyConsumption[5];
									BLL_Readings_Data.Readings_MonthConsumption7 = readings.MonthlyConsumption[6];
									BLL_Readings_Data.Readings_MonthConsumption8 = readings.MonthlyConsumption[7];
									BLL_Readings_Data.Readings_MonthConsumption9 = readings.MonthlyConsumption[8];
									BLL_Readings_Data.Readings_MonthConsumption10 = readings.MonthlyConsumption[9];
									BLL_Readings_Data.Readings_MonthConsumption11 = readings.MonthlyConsumption[10];
									BLL_Readings_Data.Readings_MonthConsumption12 = readings.MonthlyConsumption[11];





									BLL_CreditBalance_Data.CreditBalance_RemainCredit = creditBalance.RemainCredit;
									BLL_CreditBalance_Data.CreditBalance_OverdraftCredit = creditBalance.OverdraftCredit;
									BLL_CreditBalance_Data.CreditBalance_ConsumedCredit = creditBalance.ConsumedCredit;
									BLL_CreditBalance_Data.CreditBalance_CumulativeCharges = creditBalance.CumulativeCharges;
									BLL_CreditBalance_Data.CreditBalance_AppDate = DateModel.GetSystemDate(creditBalance.AppDate);
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly1 = creditBalance.UsedMonthly[0];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly2 = creditBalance.UsedMonthly[1];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly3 = creditBalance.UsedMonthly[2];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly4 = creditBalance.UsedMonthly[3];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly5 = creditBalance.UsedMonthly[4];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly6 = creditBalance.UsedMonthly[5];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly7 = creditBalance.UsedMonthly[6];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly8 = creditBalance.UsedMonthly[7];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly9 = creditBalance.UsedMonthly[8];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly10 = creditBalance.UsedMonthly[9];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly11 = creditBalance.UsedMonthly[10];
									BLL_CreditBalance_Data.CreditBalance_UsedMonthly12 = creditBalance.UsedMonthly[11];



									

									BLL_MeterState_Data.MeterState_Malfun1_Count = meterState.MalFuns[0].Count;
									BLL_MeterState_Data.MeterState_Malfun1_Day = meterState.MalFuns[0].Day;
									BLL_MeterState_Data.MeterState_Malfun1_Month = meterState.MalFuns[0].Month;

									BLL_MeterState_Data.MeterState_Malfun2_Count = meterState.MalFuns[1].Count;
									BLL_MeterState_Data.MeterState_Malfun2_Day = meterState.MalFuns[1].Day;
									BLL_MeterState_Data.MeterState_Malfun2_Month = meterState.MalFuns[1].Month;

									BLL_MeterState_Data.MeterState_Malfun3_Count = meterState.MalFuns[2].Count;
									BLL_MeterState_Data.MeterState_Malfun3_Day = meterState.MalFuns[2].Day;
									BLL_MeterState_Data.MeterState_Malfun3_Month = meterState.MalFuns[2].Month;

									BLL_MeterState_Data.MeterState_Malfun4_Count = meterState.MalFuns[3].Count;
									BLL_MeterState_Data.MeterState_Malfun4_Day = meterState.MalFuns[3].Day;
									BLL_MeterState_Data.MeterState_Malfun4_Month = meterState.MalFuns[3].Month;

									BLL_MeterState_Data.MeterState_Malfun5_Count = meterState.MalFuns[4].Count;
									BLL_MeterState_Data.MeterState_Malfun5_Day = meterState.MalFuns[4].Day;
									BLL_MeterState_Data.MeterState_Malfun5_Month = meterState.MalFuns[4].Month;

									//((int)((malFun == null) ? 0 : malFun.Count)).ToString(),



									BLL_MeterState_Data.MeterState_MeterStateDate1 = DateModel.GetSystemDate(meterState.StateDates[0]);
									BLL_MeterState_Data.MeterState_MeterStateDate2 = DateModel.GetSystemDate(meterState.StateDates[1]);
									BLL_MeterState_Data.MeterState_MeterStateDate3 = DateModel.GetSystemDate(meterState.StateDates[2]);
									BLL_MeterState_Data.MeterState_MeterStateDate4 = DateModel.GetSystemDate(meterState.StateDates[3]);

									BLL_MeterState_Data.MeterState_MeterErrors = meterState.MeterErrors;
									/*string text7 = Convert.ToString(meterState.MeterErrors, 2).PadLeft(8, '0');
									bool flag41 = text7[0] == '1';
									if (flag41)
									{
										this.lstMeterErrors.Items.Add("Good Battery");
									}
									else
									{
										this.lstMeterErrors.Items.Add("Battery Low");
									}
									bool flag42 = text7[1] == '1';
									if (flag42)
									{
										this.lstMeterErrors.Items.Add("Opened Valve");
									}
									else
									{
										this.lstMeterErrors.Items.Add("Closed Valve");
									}
									bool flag43 = text7[2] == '1';
									if (flag43)
									{
										this.lstMeterErrors.Items.Add("Valve Closing Error");
									}
									else
									{
										this.lstMeterErrors.Items.Add("Valve Closing Succeeded");
									}
									bool flag44 = text7[3] == '1';
									if (flag44)
									{
										this.lstMeterErrors.Items.Add("Valve Opening Error");
									}
									else
									{
										this.lstMeterErrors.Items.Add("Valve Opening Succeeded");
									}*/
									//MessageBox.Show("Read Client Card Successfully");



									BLL_RetrivalCard_ReturnRead ReturnData = new BLL_RetrivalCard_ReturnRead();

									ReturnData.BLL_ClientInfo_Data = new BLL_ClientInfo();
									ReturnData.BLL_ChargeBasicInf_Data = new BLL_ChargeBasicInf();
									ReturnData.BLL_CreditBalance_Data = new BLL_CreditBalance();
									ReturnData.BLL_Offtimes_Data = new BLL_Offtimes();
									ReturnData.BLL_Deductions_Data = new BLL_Deductions();
									ReturnData.BLL_PriceScheduler_Data = new BLL_PriceScheduler();
									ReturnData.BLL_MeterState_Data = new BLL_MeterState();
									ReturnData.BLL_Readings_Data = new BLL_Readings();

									ReturnData.BLL_ClientInfo_Data = BLL_ClientInfo_Data;
							     	ReturnData.BLL_ChargeBasicInf_Data = BLL_ChargeBasicInf_Data;
									ReturnData.BLL_PriceScheduler_Data = BLL_PriceScheduler_Data;
									ReturnData.BLL_Deductions_Data = BLL_Deductions_Data;
									ReturnData.BLL_CreditBalance_Data = BLL_CreditBalance_Data;
									ReturnData.BLL_Readings_Data = BLL_Readings_Data;
									ReturnData.BLL_MeterState_Data = BLL_MeterState_Data;



									retrivalList.Add(ReturnData);



								


									Status = true;



								}


							}


							//CardIssues_DAL.Update(BLLCardIssues_Data);
							//DAL_ConfigCard Object = new DAL_ConfigCard();
							//Status = Object.Insert(ConfigData);
						}
						else
						{
							MessageBox.Show("Fail to Read Card");
						}



					}

				}
			}

			catch (Exception ex)
			{
				//	this.Log(ex);
				MessageBox.Show("SYSTEM_ERROR");
			}
			return Status;

		}


	}
}
