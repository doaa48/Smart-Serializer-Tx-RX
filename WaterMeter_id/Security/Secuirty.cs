using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifyWaterCard.Security;

namespace WaterMeter_id
{

    public class Secuirty
    {


        public void Secuirty_GenerateKeyPair(ref string Public_Key, ref string Private_Key)
        {
            KeyPair KeyPair_strc = Signer.GenerateKeyPairs();
            Public_Key = KeyPair_strc.PublicKey;
            Private_Key = KeyPair_strc.PrivateKey;


        }
        public string CreateCertificate(string signerName, string signerPrivate_key, string targetName, string targetPublickey)
        {
            return Signer.GenerateX509Certificate(signerName, signerPrivate_key, targetName, targetPublickey);
        }
        public string CreateCertificate(string signerPrivate_key, string entityPublicKey, uint meterId, uint subscriberCardId)
        {
            return Signer.CertifyWithExtension(signerPrivate_key, entityPublicKey, meterId, subscriberCardId);


        }
    }
}
