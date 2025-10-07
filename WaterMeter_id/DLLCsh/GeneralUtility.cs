using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnifyWaterCard.Applets;
using UnifyWaterCard.Security;

namespace WaterMeter_id
{
    public class GeneralUtility
    {
        public static int GenerateRandomInteger()
        {
            return new Random().Next(1, int.MaxValue);
        }

        public static ushort GenerateRandomShort()
        {
            return (ushort)new Random().Next(1, 65535);
        }

        public static string ByteArrayToHex(byte[] array)
        {
            return BitConverter.ToString(array).Replace("-", "");
        }

        public static byte[] HexToByteArray(string hex)
        {
            try
            {
                if (hex == null)
                {
                    throw new ArgumentNullException("hex");
                }

                if (hex.Length % 2 == 1)
                {
                    hex += "0";
                }

                byte[] array = new byte[hex.Length / 2];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }

                return array;
            }
            catch
            {
                return null;
            }
        }

        public static bool ValidatePayload(byte[] data, string publicKey, out APDUData aPDUData)
        {
            aPDUData = APDUData.ParseTLV(data);
            byte[] data2 = data.Skip(aPDUData.HeaderLength).ToArray();
            return Signer.ValidateSignature(data2, aPDUData.Signature, publicKey);
        }
    }
}
