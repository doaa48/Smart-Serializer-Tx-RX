#region Assembly UnifyWaterCard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// F:\DOAA\smart_wmm\Code\SmartCode\WaterMeter_id\WaterMeter_id\DLLs\UnifyWaterCard.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Linq;

namespace UnifyWaterCard.DataModels
{
    [TLVTag(209)]
    public class ReadingSmart : ModelBase
    {
        


        public double QuantityTotalNegative { get; set; }

        public double CurrentMonthConsumption { get; set; }

        public double Reading { get; set; }

        public override byte[] Serialize()
        {
            return SerializeAttributes( QuantityTotalNegative, CurrentMonthConsumption, Reading);
        }

        public static ReadingSmart Deserialize(byte[] data)
        {
            if (data != null && data.Length != 0)
            {
                return new ReadingSmart
                {
                    
                    QuantityTotalNegative = BitConverter.ToDouble(data, 0),
                    CurrentMonthConsumption = BitConverter.ToDouble(data, 8),
                    Reading = BitConverter.ToDouble(data, 16)
                };
            }

            return new ReadingSmart();
        }

       
    }
}
#if false // Decompilation log
'22' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
------------------
Resolve: 'BouncyCastle.Crypto, Version=1.9.0.0, Culture=neutral, PublicKeyToken=0e99375e54769942'
Found single assembly: 'BouncyCastle.Crypto, Version=1.9.0.0, Culture=neutral, PublicKeyToken=0e99375e54769942'
Load from: 'F:\DOAA\smart_wmm\Code\SmartCode\WaterMeter_id\WaterMeter_id\DLLs\BouncyCastle.Crypto.dll'
------------------
Resolve: 'log4net, Version=2.0.15.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a'
Found single assembly: 'log4net, Version=2.0.15.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a'
Load from: 'F:\DOAA\smart_wmm\Code\SmartCode\WaterMeter_id\WaterMeter_id\DLLs\log4net.dll'
------------------
Resolve: 'PCSC, Version=5.1.0.0, Culture=neutral, PublicKeyToken=13b76e54a2ee80a7'
Could not find by name: 'PCSC, Version=5.1.0.0, Culture=neutral, PublicKeyToken=13b76e54a2ee80a7'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll'
------------------
Resolve: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Found single assembly: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Load from: 'F:\DOAA\smart_wmm\Code\SmartCode\WaterMeter_id\packages\Newtonsoft.Json.13.0.3\lib\net45\Newtonsoft.Json.dll'
#endif
