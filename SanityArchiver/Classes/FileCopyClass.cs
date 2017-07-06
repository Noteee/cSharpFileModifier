using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace SanityArchiver
{
    public class FileCopyClass: ICopy

    {
        public void fileSenderWDialog(string source, string destination)
        {
            try
            {
                FileSystem.CopyFile(source, destination,
                    UIOption.AllDialogs);
                if (File.Exists(destination))
                    MessageBox.Show("Copy Success");
            }
            catch (Exception e)
            {
                MessageBox.Show("Copy Cancelled");
            }
            
        }
    }
}