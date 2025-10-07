using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

using System.Runtime.InteropServices;
using UnifyWaterCard.DataModels;
using UnifyWaterCard.Helpers;

namespace WaterMeter_id
{
    public class APDUHELPER    
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(APDUHELPER));

        private static int icdev;

        private const byte Class = 0;

        private const byte DataLength = 120;

        [DllImport("DCRF32")]
        public static extern int dc_init(int prot, uint baud);

        [DllImport("DCRF32")]
        public static extern short dc_exit(int icdev);

        [DllImport("DCRF32")]
        public static extern uint dc_card(int icdev, int mode, ref long snr);

        [DllImport("DCRF32")]
        public static extern int dc_load_key(int icdev, int mode, int SecNr, ref byte[] NKey);

        [DllImport("DCRF32")]
        public static extern int dc_authentication(int icdev, int mode, int SecNr);

        [DllImport("DCRF32")]
        public static extern int dc_authentication_passaddr(int icdev, int mode, int _Addr, ref byte[] passbuff);

        [DllImport("DCRF32")]
        public static extern uint dc_request(int icdev, int mode, ref int TagType);

        [DllImport("DCRF32")]
        public static extern uint dc_select(int icdev, long Snr, ref byte[] Size);

        [DllImport("DCRF32")]
        public static extern int dc_read(int icdev, int _Adr, ref byte[] Data);

        [DllImport("DCRF32")]
        public static extern int dc_pro_command(int icdev, byte slen, byte[] sendbuffer, ref byte rlen, ref byte[] databuffer, byte timeout);

        [DllImport("DCRF32")]
        public static extern short dc_pro_command(int icdev, byte slen, byte[] sendbuff, ref byte rlen, [Out] byte[] recvbuff, byte timeout);

        [DllImport("DCRF32")]
        public static extern short dc_beep(int icdev, ushort misc);

        [DllImport("DCRF32")]
        public static extern int dc_pro_command_hex(int icdev, int slen, byte[] sendbuffer, ref int rlen, ref byte[] databuffer, int timeout);

        [DllImport("DCRF32")]
        public static extern int dc_pro_commandsource(int icdev, int slen, byte[] sendbuffer, ref int rlen, ref byte[] databuffer, int timeout);

        [DllImport("DCRF32")]
        public static extern short dc_config_card(int icdev, byte cardType);

        [DllImport("DCRF32")]
        public static extern short dc_pro_reset(int icdev, ref byte rlen, [Out] byte[] recvbuff);

        [DllImport("DCRF32")]
        public static extern short dc_reset(int icdev, uint sec);

        [DllImport("DCRF32")]
        public static extern short dc_card_double_hex(int icdev, byte model, [Out] char[] snr);

        public APDUHELPER()
        {
            XmlConfigurator.Configure();
        }

        public static byte[] ExecuteAPDU(byte inst, byte operation, byte[] payloadData)
        {
            try
            {
                icdev = ConnectToReader();
                byte[] result;
                if (icdev >= 0)
                {
                    switch (inst)
                    {
                        case 214:
                            result = WriteInBatches(operation, payloadData);
                            break;
                        case 176:
                            result = ReadInBatches(operation, payloadData);
                            break;
                        case 224:
                            result = ExecuteWriteAPDU(0, 224, operation, 0, 0, 0, payloadData);
                            break;
                        default:
                            result = BitConverter.GetBytes((ushort)27904);
                            break;
                    }
                    CloseConnectionReader();
                }
                else
                {
                    result = SmartCardAPDUHelper.ExecuteAPDU(inst, operation, payloadData);
                }

                return result;
            }
            catch
            {
                CloseConnectionReader();
                return BitConverter.GetBytes((ushort)27904);
            }
        }

        private static byte[] ReadInBatches(byte operation, byte[] payloadData)
        {
            byte[] array = new byte[0];
            byte b = 0;
            byte[] array2 = ExecuteReadAPDU(0, 176, operation, 0, payloadData);
            if (GetApduResponseEnum(array2) == ApduResponseEnum.SuccessResponse)
            {
                return array2;
            }
            payloadData = null;
            if (array2 != null && array2.Skip(array2.Length - 2).ToArray()[0] == BitConverter.GetBytes((ushort)20736)[1])
            {
                array = new byte[0];
                byte b2 = array2.Skip(array2.Length - 2).ToArray()[1];
                while (b != b2)
                {
                    array2 = ExecuteReadAPDU(0, 192, operation, b, payloadData);
                    b = (byte)(b + 1);
                    if (b != b2)
                    {
                        array = array.Concat(array2.Take(array2.Length - 2)).ToArray();
                        continue;
                    }

                    array = array.Concat(array2).ToArray();
                    break;
                }

                return array;
            }

            return array2;
        }

        private static byte[] WriteInBatches(byte operation, byte[] payloadData)
        {
            byte[] array = BitConverter.GetBytes(ushort.MaxValue);
            if (payloadData == null)
            {
                return ExecuteWriteAPDU(0, 214, operation, 0, 0, 0, null);
            }

            byte b = (byte)Math.Ceiling((decimal)payloadData.Length / 120m);
            if (b == 1)
            {
                return ExecuteWriteAPDU(0, 214, operation, 0, (byte)payloadData.Length, 0, payloadData);
            }

            for (short num = 0; num < b; num = (short)(num + 1))
            {
                byte[] array2 = null;
                byte b2 = 0;
                if (num == b - 1)
                {
                    array2 = payloadData.Skip((b - 1) * 120).ToArray();
                    b2 = byte.MaxValue;
                }
                else
                {
                    array2 = payloadData.Skip(num * 120).Take(120).ToArray();
                    b2 = 1;
                }

                array = ExecuteWriteAPDU(0, 214, operation, b2, (byte)array2.Length, (byte)(num + 1), array2);
                if (GetApduResponseEnum(array) != ApduResponseEnum.SuccessResponse)
                {
                    return array;
                }
            }

            return array;
        }



        private static byte[] ExecuteWriteAPDU(byte cls, byte ins, byte p1, byte p2, byte le, byte batchesNo, byte[] payloadData)
        {
            try
            {
                if (icdev < 0)
                {
                    return BitConverter.GetBytes((ushort)65281).Reverse().ToArray();
                }

                le = (byte)(le + 1);
                byte[] array = new byte[5] { cls, ins, p1, p2, le };
                byte[] array2 = new byte[le + 5];
                array.CopyTo(array2, 0);
                new byte[1] { batchesNo }.CopyTo(array2, 5);
                payloadData?.CopyTo(array2, 6);
                byte[] array3 = new byte[122];
                byte slen = (byte)array2.Length;
                byte rlen = (byte)array3.Length;
                array2[2] = p1;

                short num = dc_pro_command(icdev, slen, array2, ref rlen, array3, 150);
                byte[] array4 = new byte[rlen];
                array4 = array3.Take(rlen).ToArray();
                ILog logger = _logger;
                string[] obj = new string[6] { " Operation : ", null, null, null, null, null };
                CardOperationModel cardOperationModel = (CardOperationModel)p1;
                obj[1] = cardOperationModel.ToString();
                obj[2] = " => Request : ";
                obj[3] = GeneralUtility.ByteArrayToHex(array2);
                obj[4] = " => Response : ";
                obj[5] = GeneralUtility.ByteArrayToHex(array4);
                logger.Info(string.Concat(obj));
                return array4;
            }
            catch
            {
                return BitConverter.GetBytes(ushort.MaxValue);
            }
        }

        private static byte[] ExecuteReadAPDU(byte cls, byte ins, byte p1, byte p2, byte[] payloadData)
        {
            try
            {
                if (icdev < 0)
                {
                    return BitConverter.GetBytes((ushort)65281).Reverse().ToArray();
                }

                byte b = 121;
                byte[] array = null;


                byte slen = 0;
                if (payloadData != null)
                {
                    byte[] arraydta = new byte[500];

                    arraydta[0] = cls;
                    arraydta[1] = ins;
                    arraydta[2] = p1;
                    arraydta[3] = p2;
                    arraydta[4] = (byte)payloadData.Length;
                    payloadData?.CopyTo(arraydta, 5);
                    arraydta[payloadData.Length + 5] = b;


                    slen = (byte)(payloadData.Length + 6);
                    array = new byte[slen];

                    for (int i = 1; i < slen; i++)
                    {
                        array[i] = arraydta[i];
                    }


                }
                else
                {

                    array = new byte[5] { cls, ins, p1, p2, b };
                    payloadData?.CopyTo(array, 4);
                    slen = (byte)array.Length;
                }
                byte rlen = 0;
                byte[] array2 = new byte[b + 2];
                int num = dc_pro_command(icdev, slen, array, ref rlen, array2, 150);
                byte[] array3 = array2.Take(rlen).ToArray();
                ILog logger = _logger;
                string[] obj = new string[6] { " Operation : ", null, null, null, null, null };
                CardOperationModel cardOperationModel = (CardOperationModel)p1;
                obj[1] = cardOperationModel.ToString();
                obj[2] = " => Request : ";
                obj[3] = GeneralUtility.ByteArrayToHex(array);
                obj[4] = " => Response : ";
                obj[5] = GeneralUtility.ByteArrayToHex(array3);
                logger.Info(string.Concat(obj));
                return array3;
            }
            catch
            {
                return BitConverter.GetBytes(ushort.MaxValue);
            }
        }

        private static int ConnectToReader()
        {
            icdev = 0;
            char[] snr = new char[128];
            byte rlen = 0;
            icdev = dc_init(100, 115200u);
            if (icdev < 0)
            {
                return -1;
            }

            if (dc_config_card(icdev, 65) != 0)
            {
                dc_exit(icdev);
                return -2;
            }

            if (dc_reset(icdev, 10u) != 0)
            {
                dc_exit(icdev);
                return -3;
            }

            if (dc_card_double_hex(icdev, 0, snr) != 0)
            {
                dc_exit(icdev);
                return -4;
            }

            byte[] recvbuff = new byte[128];
            if (dc_pro_reset(icdev, ref rlen, recvbuff) != 0)
            {
                dc_exit(icdev);
                return -5;
            }

            byte[] array = new byte[12]
            {
                0, 164, 4, 0, 7, 69, 71, 89, 72, 67,
                87, 87
            };
            byte[] array2 = new byte[128];
            byte slen = (byte)array.Length;
            if (dc_pro_command(icdev, slen, array, ref rlen, array2, 7) != 0 || array2[rlen - 1] != 0 || array2[rlen - 2] != 144)
            {
                dc_exit(icdev);
                return -6;
            }

            return icdev;
        }

        private static void CloseConnectionReader()
        {
            dc_beep(icdev, 10);
            dc_exit(icdev);
            icdev = -1;
        }

        public static ApduResponseEnum GetApduResponseEnum(byte[] response)
        {
            try
            {
                byte[] apduResponseStatus = GetApduResponseStatus(response);
                return (ApduResponseEnum)BitConverter.ToUInt16(apduResponseStatus, 0);
            }
            catch (Exception)
            {
                return ApduResponseEnum.GeneralError;
            }
        }

        public static byte[] GetApduResponseStatus(byte[] response)
        {
            try
            {
                byte[] array = response.Skip(response.Length - 2).ToArray();
                Array.Reverse(array);
                return array;
            }
            catch (Exception)
            {
                return BitConverter.GetBytes(65535);
            }
        }
    }

}
