using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace SanityArchiver
{
    public class CompressClass:ICompress
    {
        public void doCompression(string source, string destination)
        {
            try
            {
                string compressedDestinationPath = destination + ".gz";
                var bytes = File.ReadAllBytes(source);
                using (FileStream fs = new FileStream(compressedDestinationPath, FileMode.CreateNew))
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false))
                {
                    zipStream.Write(bytes, 0, bytes.Length);
                }
                MessageBox.Show("Compress Success!");
            }
            catch (Exception e)
            {
                MessageBox.Show("Compression Failed");
            }
            
        }
    }
}