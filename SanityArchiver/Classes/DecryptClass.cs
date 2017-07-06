using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace SanityArchiver
{
    public class DecryptClass:ICrypt
    {
        public void doCrypt(string source, string destination)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (FileStream fsInput = new FileStream(source, FileMode.Open))
                    {
                        using (CryptoStream cs =
                            new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (FileStream fsOutput = new FileStream(destination, FileMode.Create))
                            {
                                int data;
                                while ((data = cs.ReadByte()) != -1)
                                {
                                    fsOutput.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                    MessageBox.Show("DeCrpyt Success");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("DeCrpyt Failed");
            }
        }
    }
}