
using System;
using System.Linq;

namespace UnifyWaterCard.DataModels
{
	[TLVTag(122)]
	public class CountList : ModelBase
	{

		public byte[] CountListMeters
		{
			get;
			set;
		} = new byte[2];
		public override byte[] Serialize()
		{
			return CountListMeters;
		}

		public static CountList Deserialize(byte[] data)
		{
			if (data != null && data.Length != 0)
			{
				return new CountList
				{
					CountListMeters = data
				};
			}

			return new CountList();
		}


	}
}
#if false // Decompilation log
'244' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
------------------
Resolve: 'BouncyCastle.Crypto, Version=1.9.0.0, Culture=neutral, PublicKeyToken=0e99375e54769942'
Found single assembly: 'BouncyCastle.Crypto, Version=1.9.0.0, Culture=neutral, PublicKeyToken=0e99375e54769942'
Load from: 'F:\chargeCenter\Center\wmm-recharge-center\Code\WaterMeter_id\DLLs\BouncyCastle.Crypto.dll'
------------------
Resolve: 'log4net, Version=2.0.15.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a'
Found single assembly: 'log4net, Version=2.0.15.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a'
Load from: 'F:\chargeCenter\Center\wmm-recharge-center\Code\WaterMeter_id\DLLs\log4net.dll'
------------------
Resolve: 'PCSC, Version=5.1.0.0, Culture=neutral, PublicKeyToken=13b76e54a2ee80a7'
Could not find by name: 'PCSC, Version=5.1.0.0, Culture=neutral, PublicKeyToken=13b76e54a2ee80a7'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll'
------------------
Resolve: 'Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
Found single assembly: 'Newtonsoft.Json, Version=11.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed'
WARN: Version mismatch. Expected: '13.0.0.0', Got: '11.0.0.0'
Load from: 'F:\chargeCenter\Center\wmm-recharge-center\Code\WaterMeter_id\packages\Newtonsoft.Json.11.0.2\lib\net45\Newtonsoft.Json.dll'
#endif
