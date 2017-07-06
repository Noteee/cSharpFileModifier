using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace SanityArchiver
{
    public class DecompressClass:ICompress
    {
        public void doCompression(string source, string destination)
        {
            try
            {
                using (FileStream fs = File.OpenRead(source))
                using (FileStream fw = File.Create(destination))

                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Decompress, false))
                {
                    byte[] buffer = new byte[1024];
                    int nRead;
                    while ((nRead = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fw.Write(buffer, 0, nRead);
                    }
                }
                MessageBox.Show("Decompress Success!");
            }
            catch (Exception e)
            {
                MessageBox.Show("Decompress Failed");
            }
            
        }
    }
}