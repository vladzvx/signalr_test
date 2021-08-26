using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace IASK.Common
{
    public static class Secrets
    {

        public static string Semantic = string.Empty;
        public static string Descriptions = string.Empty;
        public static string VarcharList = string.Empty;
        public static string SphinxSearch = string.Empty;
        public static string BoolSattelite = string.Empty;
        public static string GetNames = string.Empty;
        public static string ConnectionString1 = string.Empty;

        static Secrets()
        {
            TryRead();
        }
        private static string pwd = "interviewer12key";

        private static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                byte[] IV = new byte[15];
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pwd, IV);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(IV) + Convert.ToBase64String(ms.ToArray());
            }
            return clearText;
        }

        private static string Decrypt(string cipherText)
        {
            byte[] IV = Convert.FromBase64String(cipherText.Substring(0, 20));
            cipherText = cipherText.Substring(20).Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(pwd, IV);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        internal static void TryRead(string fileName = "keys.pwd")
        {
            try
            {
                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(Path.Combine(folder, fileName));
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    if (xnode.Name.ToLower().Equals("semantic"))
                    {
                        Semantic = Decrypt(xnode.InnerText);
                    }
                    if (xnode.Name.ToLower().Equals("descriptions"))
                    {
                        Descriptions = Decrypt(xnode.InnerText);
                    }
                    if (xnode.Name.ToLower().Equals("varcharlist"))
                    {
                        VarcharList = Decrypt(xnode.InnerText);
                    }
                    if (xnode.Name.ToLower().Equals("sphinxsearch"))
                    {
                        SphinxSearch = Decrypt(xnode.InnerText);
                    }
                    if (xnode.Name.ToLower().Equals("boolsattelite"))
                    {
                        BoolSattelite = Decrypt(xnode.InnerText);
                    }
                    if (xnode.Name.ToLower().Equals("names"))
                    {
                        GetNames = Decrypt(xnode.InnerText);
                    }
                    if (xnode.Name.ToLower().Equals("connectionstring1"))
                    {
                        ConnectionString1 = Decrypt(xnode.InnerText);
                    }
                }
            }
            catch
            {
            }
        }

        public static void Write(string fileName = "keys.pwd")
        {
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            XmlDocument xDoc = new XmlDocument();
            XmlDeclaration XmlDec = xDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xDoc.AppendChild(XmlDec);


            XmlElement keys = xDoc.CreateElement("keys");
            xDoc.AppendChild(keys);

            XmlElement sem = xDoc.CreateElement("semantic");
            XmlElement desc = xDoc.CreateElement("descriptions");
            XmlElement varc = xDoc.CreateElement("varcharlist");
            XmlElement sph = xDoc.CreateElement("sphinxsearch");
            XmlElement _bool = xDoc.CreateElement("boolsattelite");
            XmlElement _names = xDoc.CreateElement("names");
            XmlElement ConnectionStr1 = xDoc.CreateElement("connectionstring1");
            sem.InnerText = Encrypt(Semantic);
            desc.InnerText = Encrypt(Descriptions);
            varc.InnerText = Encrypt(VarcharList);
            sph.InnerText = Encrypt(SphinxSearch);
            _bool.InnerText = Encrypt(BoolSattelite);
            _names.InnerText = Encrypt(GetNames);
            ConnectionStr1.InnerText = Encrypt(ConnectionString1);
            keys.AppendChild(sem);
            keys.AppendChild(desc);
            keys.AppendChild(varc);
            keys.AppendChild(sph);
            keys.AppendChild(_bool);
            keys.AppendChild(_names);
            keys.AppendChild(ConnectionStr1);
            xDoc.Save(Path.Combine(folder, fileName));
        }
    }
}
