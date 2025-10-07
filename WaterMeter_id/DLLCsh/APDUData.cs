using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterMeter_id
{
    public class APDUData
     
    {
        public byte[] SubscriberID { get; set; } = null;


        public byte[] MeterID { get; set; } = null;


        public byte[] CardID { get; set; } = null;


        public byte[] Signature { get; set; } = null;

        public byte[] payloadData { get; set; } = null;
        public ICollection<Tlv> Models { get; set; } = new List<Tlv>();


        public int HeaderLength { get; set; }

        public int PayloadLength { get; set; }

        public int SessionLength { get; set; }

        public int NonceLength { get; set; }
        public int AdditionalDataLength { get; set; }
        private APDUData()
        {
        }
        /*public static APDUMOS ParseTLV(byte[] data)
        {
            APDUMOS aPDUData = new APDUMOS();
            try
            {
                if (data == null)
                {
                    return new APDUMOS();
                }

                Tlv tlv = null;
                Tlv tlv2 = null;
                ICollection<Tlv> collection = null;
                Tlv tlv3 = null;
                Tlv tlv4 = null;
                ICollection<Tlv> collection2 = null;
                ICollection<Tlv> source = Tlv.ParseTlv(data);

                // Parse TLV with tag 224
                if (source.Any((Tlv t) => t.Tag == 224))
                {
                    tlv = source.First((Tlv t) => t.Tag == 224);
                    aPDUData.HeaderLength = tlv.Data.Length;

                    // Parse TLV with tag 240 inside TLV with tag 224
                    ICollection<Tlv> source2 = Tlv.ParseTlv(tlv.Value);
                    if (source2.Any((Tlv t) => t.Tag == 240))
                    {
                        tlv2 = source2.First((Tlv t) => t.Tag == 240);
                        if (tlv2.Value.Length != 0)
                        {
                            collection = Tlv.ParseTlv(tlv2.Value);
                            if (collection.Any((Tlv t) => t.Tag == 192))
                            {
                                aPDUData.SubscriberID = collection.First((Tlv t) => t.Tag == 192).Value;
                            }

                            if (collection.Any((Tlv t) => t.Tag == 193))
                            {
                                aPDUData.MeterID = collection.First((Tlv t) => t.Tag == 193).Value;
                            }

                            if (collection.Any((Tlv t) => t.Tag == 194))
                            {
                                aPDUData.CardID = collection.First((Tlv t) => t.Tag == 194).Value;
                            }
                        }
                    }

                    // Parse TLV with tag 196 inside TLV with tag 240
                    if (source2.Any((Tlv t) => t.Tag == 196))
                    {
                        tlv3 = source2.First((Tlv t) => t.Tag == 196);
                        aPDUData.Signature = tlv3.Value;
                    }
                }

                // Parse all TLVs with tag 225
                foreach (Tlv tlv5 in source.Where(t => t.Tag == 225))
                {
                    aPDUData.PayloadLength += tlv5.Length;
                    collection2 = Tlv.ParseTlv(tlv5.Value);
                    foreach (var model in collection2)
                    {
                        aPDUData.Models.Add(model);
                    }
                }

                // Parse all TLVs with tag 226
                foreach (Tlv tlv6 in source.Where(t => t.Tag == 226))
                {
                    aPDUData.AdditionalDataLength += tlv6.Length;
                    ICollection<Tlv> additionalData = Tlv.ParseTlv(tlv6.Value);
                    // Process additional data here...
                }

                return aPDUData;
            }
            catch (Exception)
            {
                return aPDUData;
            }
        }
        */
        public static APDUData ParseTLV(byte[] data)
        {
            APDUData aPDUData = new APDUData();
            try
            {
                if (data == null)
                {
                    return new APDUData();
                }

                Tlv tlv = null;
                Tlv tlv2 = null;
                ICollection<Tlv> collection = null;
                Tlv tlv3 = null;
                Tlv tlv4 = null;
                Tlv tlv5 = null;
                ICollection<Tlv> collection2 = null;
                ICollection<Tlv> collection3 = null;
                ICollection<Tlv> collection4 = null;
                ICollection<Tlv> source = Tlv.ParseTlv(data);
                if (source.Any((Tlv t) => t.Tag == 224))
                {
                    tlv = source.First((Tlv t) => t.Tag == 224);
                    aPDUData.HeaderLength = tlv.Data.Length;
                    ICollection<Tlv> source2 = Tlv.ParseTlv(tlv.Value);
                    if (source2.Any((Tlv t) => t.Tag == 240))
                    {
                        tlv2 = source2.First((Tlv t) => t.Tag == 240);
                        if (tlv2.Value.Length != 0)
                        {
                            collection = Tlv.ParseTlv(tlv2.Value);
                            if (collection.Any((Tlv t) => t.Tag == 192))
                            {
                                aPDUData.SubscriberID = collection.First((Tlv t) => t.Tag == 192).Value;
                            }

                            if (collection.Any((Tlv t) => t.Tag == 193))
                            {
                                aPDUData.MeterID = collection.First((Tlv t) => t.Tag == 193).Value;
                            }

                            if (collection.Any((Tlv t) => t.Tag == 194))
                            {
                                aPDUData.CardID = collection.First((Tlv t) => t.Tag == 194).Value;
                            }
                        }
                    }

                    if (source2.Any((Tlv t) => t.Tag == 196))
                    {
                        tlv3 = source2.First((Tlv t) => t.Tag == 196);
                        aPDUData.Signature = tlv3.Value;
                    }
                }
           
                tlv4 = source.First((Tlv t) => t.Tag == 225);
                aPDUData.PayloadLength = tlv4.Length;
                aPDUData. payloadData = tlv4.Value;
                collection2 = Tlv.ParseTlv(tlv4.Value);

                if (collection2.Any((Tlv t) => t.Tag == 161))
                {
                    tlv3 = collection2.First((Tlv t) => t.Tag == 161);
                    collection4 = Tlv.ParseTlv(tlv3.Value);
                    foreach (var item in collection4)
                    {
                        aPDUData.Models.Add(item); // add each item from collection2 to aPDUData.Models
                    }
                }
                else if (collection2.Any((Tlv t) => t.Tag == 162))
                {
                    tlv3 = collection2.First((Tlv t) => t.Tag == 162);
                    collection4 = Tlv.ParseTlv(tlv3.Value);
                    foreach (var item in collection4)
                    {
                        aPDUData.Models.Add(item); // add each item from collection2 to aPDUData.Models
                    }
                }
                else if (collection2.Any((Tlv t) => t.Tag == 163))
                {
                    tlv3 = collection2.First((Tlv t) => t.Tag == 163);
                    collection4 = Tlv.ParseTlv(tlv3.Value);
                    foreach (var item in collection4)
                    {
                        aPDUData.Models.Add(item); // add each item from collection2 to aPDUData.Models
                    }
                }

                else { }

                foreach (var item in collection2)
                {
                    aPDUData.Models.Add(item); // add each item from collection2 to aPDUData.Models
                }

                tlv5 = source.First((Tlv t) => t.Tag == 226);
                aPDUData.PayloadLength = tlv5.Length;
                collection3 = Tlv.ParseTlv(tlv5.Value);
                foreach (var item in collection3)
                {
                    aPDUData.Models.Add(item); // add each item from collection3 to aPDUData.Models
                }
                return aPDUData;
            }
            catch (Exception)
            {
                return aPDUData;
            }
        }
    }

}
