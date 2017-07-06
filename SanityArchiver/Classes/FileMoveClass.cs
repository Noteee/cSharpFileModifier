using System;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace SanityArchiver
{
    public class FileMoveClass:ICopy
    {
        public void fileSenderWDialog(string source, string destination)
        {
            try
            {
                FileSystem.MoveFile(source, destination, UIOption.AllDialogs);
                MessageBox.Show("Move Success");
            }
            catch (Exception e)
            {
                MessageBox.Show("Move Canceled");
            }
            
        }
    }
}