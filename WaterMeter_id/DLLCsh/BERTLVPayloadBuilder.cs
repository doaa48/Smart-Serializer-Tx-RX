using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifyWaterCard.DataModels;
using UnifyWaterCard.Helpers;

namespace WaterMeter_id
{
    public class BERTLVPayloadBuilder
    {
        public static byte[] GeneratePayload(byte[] System_Nounce, byte[] CardNounToSign, byte[] subscriberID, byte[] meterID, byte[] cardID, byte[] keyToSign = null,bool signPayload=true, params ModelBase[] models)
        {
            List<Tuple<byte?, ModelBase>> payloadObjects = null;
            if (models != null)
            {
                payloadObjects = ConvertModelsToTuples(models);
            }

            return GenerateAPDUData(payloadObjects, subscriberID, meterID, cardID, keyToSign, System_Nounce, CardNounToSign, signPayload);

        }
        public static byte[] GeneratePayload(byte[] System_Nounce, byte[] CardNounToSign, byte[] subscriberID, byte[] meterID, byte[] cardID, byte[] keyToSign = null, params ModelBase[] models)
        {
            List<Tuple<byte?, ModelBase>> payloadObjects = null;
            if (models != null)
            {
                payloadObjects = ConvertModelsToTuples(models);
            }

            return GenerateAPDUData(payloadObjects, subscriberID, meterID, cardID, keyToSign, System_Nounce, CardNounToSign);

        }
        public static byte[] GeneratePayload(byte[] subscriberID, byte[] meterID, byte[] cardID, byte[] keyToSign = null, params ModelBase[] models)
        {
            List<Tuple<byte?, ModelBase>> payloadObjects = ConvertModelsToTuples(models);
            return GenerateAPDUData(payloadObjects, subscriberID, meterID, cardID, keyToSign);
        }

        public static byte[] GeneratePayload(byte[] cardID, byte[] keyToSign = null, params ModelBase[] models)
        {
            List<Tuple<byte?, ModelBase>> payloadObjects = ConvertModelsToTuples(models);
            return GenerateAPDUData(payloadObjects, null, null, cardID, keyToSign);
        }

        private static List<Tuple<byte?, ModelBase>> ConvertModelsToTuples(ModelBase[] models)
        {
            List<Tuple<byte?, ModelBase>> modelList = new List<Tuple<byte?, ModelBase>>();
            models.ToList().ForEach(delegate (ModelBase model)
            {
                modelList.Add(new Tuple<byte?, ModelBase>(model?.Tag, model));
            });
            return modelList;
        }
        private static byte[] GenerateAPDUData(List<Tuple<byte?, ModelBase>> payloadObjects, byte[] subscriberID = null, byte[] meterID = null, byte[] cardID = null, byte[] keyToSign = null, byte[] System_Nounce = null, byte[] CardNounToSign = null,bool payloadSign=true)
        {
            byte[] arraysign = null;
            byte[] array2 = new byte[0];
            byte[] array3 = new byte[0];

            if (CardNounToSign != null && CardNounToSign.Length != 0)
            {
                arraysign = ((!IsDefaultKey(keyToSign)) ? Signer.Sign(GeneralUtility.ByteArrayToHex(keyToSign), CardNounToSign) : new byte[0]);
            }
            array3 = GeneratePyloadSeesion(System_Nounce, arraysign);  //data of E3

            array2 = GeneratePayload(payloadObjects);


            byte[] array = null;
            byte[] first = null;
            if (payloadObjects != null)
            {//4 

                bool flage = (array2[1] & 0x80)==0x00;
        
                    if (flage)
                    {

                 
                    array = (payloadSign) ? Signer.Sign(GeneralUtility.ByteArrayToHex(keyToSign), array2.Skip(2).ToArray()) : new byte[0];
                    }
                else
                    {
                        int temp = array2[1] & 0x7f;
                     array = (payloadSign) ? Signer.Sign(GeneralUtility.ByteArrayToHex(keyToSign), array2.Skip(2+temp).ToArray()) : new byte[0];
                }
               
            }
            else
            {
                array = new byte[1];
                array[0] = 0;
            }
            first = GeneratePayloadHeader(subscriberID, meterID, cardID, array);

            return (first.Concat(array2).Concat(array3).ToArray());
        }



        private static byte[] GenerateAPDUData(List<Tuple<byte?, ModelBase>> payloadObjects, byte[] subscriberID = null, byte[] meterID = null, byte[] cardID = null, byte[] keyToSign = null)
        {
            byte[] array = null;
            byte[] array2 = GeneratePayload(payloadObjects);
            array = ((!IsDefaultKey(keyToSign)) ? Signer.Sign(GeneralUtility.ByteArrayToHex(keyToSign), array2) : new byte[0]);
            byte[] first = GeneratePayloadHeader(subscriberID, meterID, cardID, array);
            return first.Concat(array2).ToArray();
        }

        private static byte[] GeneratePayload(List<Tuple<byte?, ModelBase>> payloadObjects)
        {
            List<byte> payloadTLV = new List<byte>();

            if (payloadObjects != null)
            {
                payloadObjects.ForEach(delegate (Tuple<byte?, ModelBase> payloadObject)
                {
                    byte[] collection = GenerateTLVTag(payloadObject.Item1.Value, payloadObject.Item2.Serialize());
                    payloadTLV.AddRange(collection);
                });
                return GenerateTLVTag(225, payloadTLV.ToArray());
            }
            byte[] array = new byte[0];

            return array;// GenerateTLVTag(225, array);
        }
        private static byte[] GeneratePyloadSeesion(byte[] sysNounce = null, byte[] SigntureNounce = null)
        {
            byte[] first = new byte[0];
            byte[] second = new byte[0];
            byte[] second2 = new byte[0];
            byte[] array = new byte[0];
            if ((SigntureNounce == null) && (sysNounce == null))
            {

                return array;
            }
            if (sysNounce != null && sysNounce.Length != 0)
            {
                first = GenerateTLVTag(219, sysNounce);
            }

            if (SigntureNounce != null && SigntureNounce.Length != 0)
            {
                second = GenerateTLVTag(220, SigntureNounce);
            }


            return GenerateTLVTag(226, first.Concat(second).Concat(second2).ToArray());
        }
        private static byte[] GeneratePayloadHeader(byte[] subscriberID, byte[] meterID, byte[] cardID, byte[] payloadSignature)
        {
            byte[] first = new byte[0];
            byte[] second = new byte[0];
            byte[] second2 = new byte[0];
            byte[] array = new byte[0];
            if (subscriberID != null && subscriberID.Length != 0)
            {
                first = GenerateTLVTag(192, subscriberID);
            }

            if (meterID != null && meterID.Length != 0)
            {
                second = GenerateTLVTag(193, meterID);
            }

            if (cardID != null && cardID.Length != 0)
            {
                second2 = GenerateTLVTag(194, cardID);
            }

            array = GenerateTLVTag(196, payloadSignature);
            byte[] first2 = GenerateTLVTag(240, first.Concat(second).Concat(second2).ToArray());
            return GenerateTLVTag(224, first2.Concat(array).ToArray());
        }

        public static byte[] GenerateTLVTag(byte tag, byte[] data)
        {
            byte[] dataTag = BERTLVBuilder.GetDataTag(tag);
            if (data.Length == 0)
            {
                data = new byte[1];
            }

            byte[] dataLengthInBerTLVFormat = BERTLVBuilder.GetDataLengthInBerTLVFormat(data.Length);
            return dataTag.Concat(dataLengthInBerTLVFormat).Concat(data).ToArray();
        }

        private static bool IsDefaultKey(byte[] key)
        {
            return key?.ToList().All((byte k) => k == 0) ?? true;
        }
    }
}
