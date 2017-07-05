﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Security;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.Win32.SafeHandles;

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


        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
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

        private void fileCopyWithDialog(string sourcePath, string destinationPath, string message)
        {
            sourcePath = selectedFilePath;
            destinationPath = this.destinationPath;
            FileSystem.CopyFile(sourcePath, destinationPath,
                UIOption.AllDialogs);
            if (File.Exists(destinationPath))
                MessageBox.Show(message);
        }

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox1.Text == selectedFileName &&
                textBox2.Text == destinationPath)
            {
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                    fileCopyWithDialog(selectedFilePath, destinationPath, "Copy Success!");
                }

                else
                {
                    fileCopyWithDialog(selectedFilePath, destinationPath, "Copy Success!");
                }
            }
        }


        private void pictureBox9_MouseClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(selectedFilePath))
            {
                FileSystem.DeleteFile(selectedFilePath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                if (!File.Exists(selectedFilePath))
                    MessageBox.Show("Delete Success!");
            }
        }

        private void pictureBox5_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox1.Text != null && textBox2.Text != null && textBox1.Text == selectedFileName &&
                textBox2.Text == destinationPath)
            {
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                    fileCopyWithDialog(selectedFilePath, destinationPath, "Move Success!");
                    FileSystem.DeleteFile(selectedFilePath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                }

                else
                {
                    fileCopyWithDialog(selectedFilePath, destinationPath, "Move Success!");
                    FileSystem.DeleteFile(selectedFilePath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                }
            }
        }

        private void pictureBox12_MouseClick(object sender, MouseEventArgs e)
        {
            if (textBox1.Text != null && textBox1.Text == selectedFileName)
                Process.Start(selectedFilePath);
        }

        private void pictureBox4_MouseClick(object sender, MouseEventArgs e)
        {
            if (!File.Exists(compressedDestinationPath) && textBox2.Text != null && textBox2.Text == destinationPath &&
                textBox1.Text != null && textBox1.Text == selectedFileName)
            {
                doCompress();
            }
            else
            {
                if (File.Exists(compressedDestinationPath))
                {
                    File.Delete(compressedDestinationPath);
                    doCompress();
                }
            }
        }

        void doDecompress()
        {
            var token = destinationPath.ToList();
            string decompressedDestination = "";
            for (int i = 0; i < token.Count - 3; i++)
            {
                decompressedDestination += token[i];
            }

            using (FileStream fs = File.OpenRead(selectedFilePath))
            using (FileStream fw = File.Create(decompressedDestination))

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

        void doCompress()
        {
            compressedDestinationPath = destinationPath + ".gz";
            var bytes = File.ReadAllBytes(selectedFilePath);
            using (FileStream fs = new FileStream(compressedDestinationPath, FileMode.CreateNew))
            using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false))
            {
                zipStream.Write(bytes, 0, bytes.Length);
            }
            MessageBox.Show("Compress Success!");
        }

        private void pictureBox7_MouseClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(selectedFilePath))
            {
                //File.Encrypt(selectedFilePath);
                Encrypt(selectedFilePath, encryptDestination);
            }
        }

        private void pictureBox8_MouseClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(selectedFilePath))
            {
                if (!File.Exists(destinationPath))
                    Decrypt(selectedFilePath, decryptDestination);
                MessageBox.Show("Decrypt complete!");
            }
        }

        private void pictureBox10_MouseClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(selectedFilePath))
            {
                File.SetAttributes(selectedFilePath, FileAttributes.ReadOnly);
                MessageBox.Show("File is read only!");
            }
        }

        private void pictureBox11_MouseClick(object sender, MouseEventArgs e)
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

        private void Decrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                {
                    using (CryptoStream cs =
                        new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                        {
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                fsOutput.WriteByte((byte) data);
                            }
                        }
                    }
                }
            }
        }

        private void Encrypt(string inputFilePath, string outputfilePath)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                {
                    using (CryptoStream cs =
                        new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte) data);
                            }
                        }
                    }
                }
                MessageBox.Show("Encrypt complete");
            }
        }


        private void pictureBox6_MouseClick(object sender, MouseEventArgs e)
        {
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
                        doDecompress();
                    else
                    {
                        File.Delete(decompressedDestination);
                        doDecompress();
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
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
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
                if (File.Exists(selectedFilePath))
                {
                    pictureBox13.Visible = true;
                }
            }
        }
    }
}