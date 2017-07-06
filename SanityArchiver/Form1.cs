using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;


namespace SanityArchiver
{
    public partial class SanityArchiver : MetroFramework.Forms.MetroForm
    {
        private string selectedFileName,
            selectedFilePath,
            compressedDestinationPath,
            destinationPath,
            encryptDestination,
            decryptDestination,
            decompressDestinationPath;


        private void destinationPathBrowserButton_MouseClick(object sender, MouseEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string destinationPath = fbd.SelectedPath + "\\" + this.selectedFileName;
                this.destinationPath = destinationPath;
                this.encryptDestination = fbd.SelectedPath + "\\encrypted" + this.selectedFileName;
                this.decryptDestination = fbd.SelectedPath + "\\un" + this.selectedFileName;
                textBox2.Text = destinationPath;

                if (File.Exists(selectedFilePath))
                {
                    pictureBox14.Visible = true;
                }
            }
        }

        void directoryInfo()
        {
            try
            {
                DriveInfo[] myDrives = DriveInfo.GetDrives();
                Int64 freeSpaceC = Convert.ToInt64(myDrives[0].TotalFreeSpace) / 1024 / 1024 / 1024;
                Int64 freeSpaceF = Convert.ToInt64(myDrives[2].TotalFreeSpace) / 1024 / 1024 / 1024;

                textBox3.AppendText(myDrives[0].Name + "-->" + freeSpaceC.ToString() + "Gb FreeSpace remaining");
                textBox3.AppendText("\n");
                textBox3.AppendText(myDrives[2].Name + "-->" + freeSpaceF.ToString() + "Gb FreeSpace remaining");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void fileCopyButton_MouseClick(object sender, MouseEventArgs e)
        {
            FileCopyClass fileCopy = new FileCopyClass();
            if (textBox1.Text != null && textBox2.Text != null && textBox1.Text == selectedFileName &&
                textBox2.Text == destinationPath)
            {
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                    fileCopy.fileSenderWDialog(selectedFilePath, destinationPath);
                }

                else
                {
                    fileCopy.fileSenderWDialog(selectedFilePath, destinationPath);
                }
            }
        }

        private void deleteFileButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(selectedFilePath))
            {
                FileSystem.DeleteFile(selectedFilePath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                if (!File.Exists(selectedFilePath))
                    MessageBox.Show("Delete Success!");
            }
        }

        private void fileMoveButton_MouseClick(object sender, MouseEventArgs e)
        {
            FileMoveClass fileMove = new FileMoveClass();
            if (textBox1.Text != null && textBox2.Text != null && textBox1.Text == selectedFileName &&
                textBox2.Text == destinationPath)
            {
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                    fileMove.fileSenderWDialog(selectedFilePath, destinationPath);
                }

                else
                {
                    fileMove.fileSenderWDialog(selectedFilePath, destinationPath);
                }
            }
        }

        private void openFileButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox1.Text != null && textBox1.Text == selectedFileName)
                Process.Start(selectedFilePath);
        }

        private void fileCompressButton_MouseClick(object sender, MouseEventArgs e)
        {
            CompressClass compress = new CompressClass();
            if (!File.Exists(compressedDestinationPath) && textBox2.Text != null && textBox2.Text == destinationPath &&
                textBox1.Text != null && textBox1.Text == selectedFileName)
            {
                compress.doCompression(selectedFilePath, destinationPath);
            }
            else
            {
                if (File.Exists(compressedDestinationPath))
                {
                    File.Delete(compressedDestinationPath);
                    compress.doCompression(selectedFilePath, destinationPath);
                }
            }
        }

        private void encryptFileButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(selectedFilePath))
            {
                EncryptClass encrypt = new EncryptClass();
                encrypt.doCrypt(selectedFilePath, encryptDestination);
            }
        }

        private void decryptFileButton_MouseClick(object sender, MouseEventArgs e)
        {
            DecryptClass decrypt = new DecryptClass();
            if (File.Exists(selectedFilePath))
            {
                if (!File.Exists(destinationPath))
                    decrypt.doCrypt(selectedFilePath, decryptDestination);
            }
        }

        private void makeFileReadOnlyButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(selectedFilePath))
            {
                File.SetAttributes(selectedFilePath, FileAttributes.ReadOnly);
                MessageBox.Show("File is read only!");
            }
        }

        private void removeReadOnlyButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(selectedFilePath))
            {
                string file = selectedFilePath;
                FileAttributes attrs = File.GetAttributes(file);
                if (attrs.HasFlag(FileAttributes.ReadOnly))
                {
                    File.SetAttributes(file, attrs & ~FileAttributes.ReadOnly);
                    MessageBox.Show("ReadOnly removed!");
                }
            }
        }

        private void fileCopyButton_MouseEnter(object sender, EventArgs e)
        {
            label1.Visible = true;
        }

        private void fileCopyButton_MouseLeave(object sender, EventArgs e)
        {
            label1.Visible = false;
        }

        private void fileCompressButton_MouseEnter(object sender, EventArgs e)
        {
            label2.Visible = true;
        }

        private void fileCompressButton_MouseLeave(object sender, EventArgs e)
        {
            label2.Visible = false;
        }

        private void FileMoveButton_MouseEnter(object sender, EventArgs e)
        {
            label3.Visible = true;
        }

        private void fileMoveButton_MouseLeave(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        private void decompressFileButton_MouseEnter(object sender, EventArgs e)
        {
            label4.Visible = true;
        }

        private void decompressFileButton_MouseLeave(object sender, EventArgs e)
        {
            label4.Visible = false;
        }

        private void encryptFileButton_MouseEnter(object sender, EventArgs e)
        {
            label5.Visible = true;
        }

        private void encryptFileButton_MouseLeave(object sender, EventArgs e)
        {
            label5.Visible = false;
        }

        private void decryptFileButton_MouseEnter(object sender, EventArgs e)
        {
            label6.Visible = true;
        }

        private void decryptFileButton_MouseLeave(object sender, EventArgs e)
        {
            label6.Visible = false;
        }

        private void openFileButton_MouseEnter(object sender, EventArgs e)
        {
            label9.Visible = true;
        }

        private void openFileButton_MouseLeave(object sender, EventArgs e)
        {
            label9.Visible = false;
        }

        private void makeFileReadOnlyButton_MouseEnter(object sender, EventArgs e)
        {
            label7.Visible = true;
        }

        private void makeFileReadOnlyButton_MouseLeave(object sender, EventArgs e)
        {
            label7.Visible = false;
        }

        private void removeReadOnlyButton_MouseEnter(object sender, EventArgs e)
        {
            label8.Visible = true;
        }

        private void removeReadOnlyButton_MouseLeave(object sender, EventArgs e)
        {
            label8.Visible = false;
        }

        private void deleteFileButton_MouseEnter(object sender, EventArgs e)
        {
            label10.Visible = true;
        }

        private void deleteFileButton_MouseLeave(object sender, EventArgs e)
        {
            label10.Visible = false;
        }

        private void decompressFileButton_MouseClick(object sender, MouseEventArgs e)
        {
            DecompressClass decompress = new DecompressClass();
            if (destinationPath != null)
            {
                var token = destinationPath.ToList();
                string decompressedDestination = "";
                for (int i = 0; i < token.Count - 3; i++)
                {
                    decompressedDestination += token[i];
                }
                if (!File.Exists(decompressedDestination))
                    if (textBox1.Text != "Plase select the file u'd like to work with !" &&
                        textBox2.Text != "Please select the destination folder if needed !")
                        decompress.doCompression(selectedFilePath, decompressedDestination);
                    else
                    {
                        File.Delete(decompressedDestination);
                        decompress.doCompression(selectedFilePath, decompressedDestination);
                    }
            }
        }

        public SanityArchiver()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Plase select the file u'd like to work with !";
            textBox2.Text = "Please select the destination folder if needed !";
            directoryInfo();
        }

        private void sourceFileBrowserButton_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string selectedFilePath = ofd.FileName;
                string selectedFileName = ofd.SafeFileName;
                this.selectedFilePath = selectedFilePath;
                this.selectedFileName = selectedFileName;
                textBox1.Text = selectedFileName;
                long lengthOfFile = new System.IO.FileInfo(selectedFilePath).Length;
                textBox4.Text = (lengthOfFile / 1024 / 1024).ToString() + "MB (" + lengthOfFile / 1204 / 1024 / 1024 +
                                "Gb) " + "Free space needed";
                if (File.Exists(selectedFilePath))
                {
                    pictureBox13.Visible = true;
                }
            }
        }
    }
}