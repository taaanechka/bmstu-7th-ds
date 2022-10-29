#nullable disable

using System.Security.Cryptography;

namespace Signature
{
    public class Signer
    {
        #region parameters
        public string PublicKeyFilename { get; set; } = @"data/keys/public_key.txt";
        public string PrivateKeyFilename { get; set; } = @"data/keys/private_key.txt";
        public string DataFilename { get; set; }
        public string SignatureFilename { get; set; } = @"data/out/signature";
        public string HashName { get; set; } = "SHA256";
        #endregion

        public Signer(string dataFilename)
        {
            DataFilename = dataFilename;
        }

        public void GenerateKeys()
        {
            using var rsa = new RSACryptoServiceProvider();

            var publikKey = rsa.ToXmlString(false); // false - запишет только публичный ключ.
            var privateKey = rsa.ToXmlString(true); // true - запишет и приватный, и публичный ключи.

            File.WriteAllText(PublicKeyFilename, publikKey);
            File.WriteAllText(PrivateKeyFilename, privateKey);
        }

        public int CreateSignature(out byte[] signatureHash)
        {
            byte[] hashValue;
            int err = getHashFromFile(DataFilename, out hashValue);
            if (err != Consts.OK)
            {
                signatureHash = null;
                return err;
            }
            
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(getPrivateKey());

            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm(HashName);

            signatureHash = rsaFormatter.CreateSignature(hashValue);

            return Consts.OK;
        }

        public bool IsSigned(out int err)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(getPublicKey());

            var signature = readDataFromFile(SignatureFilename);
            byte[] fileHash;

            err = getHashFromFile(DataFilename, out fileHash);
            if (err != Consts.OK)
            {
                return false;
            }

            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm(HashName);

            var result = rsaDeformatter.VerifySignature(fileHash, signature);
            
            return result;
        }
        private string getPrivateKey()
        {
            var key = File.ReadAllText(PrivateKeyFilename);
            return key;
        }

        private string getPublicKey()
        {
            var key = File.ReadAllText(PublicKeyFilename);
            return key;
        }

        private byte[] getHash(byte[] fileData)
        {
            var hashAlgorithm = HashAlgorithm.Create(HashName);
            var hashValue = hashAlgorithm.ComputeHash(fileData);

            return hashValue;
        }

        private int getHashFromFile(string filename, out byte[] hashValue)
        {
            if (!File.Exists(filename))
            {
                hashValue = null;
                return (int)Consts.Errors.ExistsErr;
            }

            var fileData = readDataFromFile(filename);
            hashValue = getHash(fileData);

            return Consts.OK;
        }

        private byte[] readDataFromFile(string filename)
        {
            using var fs = File.OpenRead(filename);

            var fileData = new byte[fs.Length];
            fs.Read(fileData, 0, (int)fs.Length);

            return fileData;
        }
    }
}