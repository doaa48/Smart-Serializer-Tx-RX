using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class Tlv
    {
        private readonly int _valueOffset;

        public byte[] Data
        {
            get;
            private set;
        }

        public string HexData => GetHexString(Data);

        public int Tag
        {
            get;
            private set;
        }

        public string HexTag => Tag.ToString("X");

        public int Length
        {
            get;
            private set;
        }

        public string HexLength => Length.ToString("X");

        public byte[] Value
        {
            get
            {
                byte[] array = new byte[Length];
                Array.Copy(Data, _valueOffset, array, 0, Length);
                return array;
            }
        }

        public string HexValue => GetHexString(Value);

        public ICollection<Tlv> Children
        {
            get;
            set;
        }

        private Tlv(int tag, int length, int valueOffset, byte[] data)
        {
            Tag = tag;
            Length = length;
            Data = data;
            Children = new List<Tlv>();
            _valueOffset = valueOffset;
        }

        public static ICollection<Tlv> Parse(string tlv)
        {
            return ParseTlv(tlv);
        }

        public static ICollection<Tlv> Parse(byte[] rawTlv)
        {
            return ParseTlv(rawTlv);
        }

        public static ICollection<Tlv> ParseTlv(string tlv)
        {
            if (string.IsNullOrWhiteSpace(tlv))
            {
                throw new ArgumentException("tlv");
            }

            return ParseTlv(GetBytes(tlv));
        }

        public static ICollection<Tlv> ParseTlv(byte[] tlv)
        {
            if (tlv == null || tlv.Length == 0)
            {
                throw new ArgumentException("tlv");
            }

            List<Tlv> result = new List<Tlv>();
            ParseTlv(tlv, result);
            return result;
        }

        private static void ParseTlv(byte[] rawTlv, ICollection<Tlv> result)
        {
            int num = 0;
            int num2 = 0;
            while (num < rawTlv.Length)
            {
                if (rawTlv[num] == 0)
                {
                    num++;
                }
                else
                {
                    bool flag = (rawTlv[num] & 0x20) != 0;
                    bool flag2 = (rawTlv[num] & 0x1F) == 31;
                    while (flag2 && (rawTlv[++num] & 0x80) != 0)
                    {
                    }

                    num++;
                    int @int = GetInt(rawTlv, num2, num - num2);
                    bool flag3 = (rawTlv[num] & 0x80) != 0;
                    int num3 = flag3 ? GetInt(rawTlv, num + 1, rawTlv[num] & 0x1F) : rawTlv[num];
                    num = (flag3 ? (num + (rawTlv[num] & 0x1F) + 1) : (num + 1));
                    num += num3;
                    byte[] array = new byte[num - num2];
                    Array.Copy(rawTlv, num2, array, 0, num - num2);
                    Tlv tlv = new Tlv(@int, num3, array.Length - num3, array);
                    result.Add(tlv);
                    if (flag)
                    {
                        ParseTlv(tlv.Value, tlv.Children);
                    }
                }

                num2 = num;
            }
        }

        private static string GetHexString(byte[] arr)
        {
            StringBuilder stringBuilder = new StringBuilder(arr.Length * 2);
            foreach (byte b in arr)
            {
                stringBuilder.AppendFormat("{0:X2}", b);
            }

            return stringBuilder.ToString();
        }

        private static byte[] GetBytes(string hexString)
        {
            return (from x in Enumerable.Range(0, hexString.Length)
                    where x % 2 == 0
                    select Convert.ToByte(hexString.Substring(x, 2), 16)).ToArray();
        }

        private static int GetInt(byte[] data, int offset, int length)
        {
            int num = 0;
            for (int i = 0; i < length; i++)
            {
                num = ((num << 8) | data[offset + i]);
            }

            return num;
        }
    }
}
