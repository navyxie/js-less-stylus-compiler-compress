using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CompressFile
{
    public partial class Form1 : Form
    {
        private string jsCompressFile;
        private string cssCompressFile;
        private List<string> compressFileList;
        public Form1()
        {
            InitializeComponent();
        }

        private void chooseJsCompressorFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                jsCompressFile = dlg.FileName;
                jsCompressorFilePath.Text = jsCompressFile;
            }
        }

        private void chooseCssCompressorFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                cssCompressFile = dlg.FileName;
                cssCompressFilePath.Text = cssCompressFile;
            }
        }

        private void choosePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string path = dlg.SelectedPath;
                compressFileList = getAllCompressFile(path);
                /*
                foreach (string file in compressFileList)
                {
                    ListViewItem item = new ListViewItem(file);
                    compressFileListView.Items.Add(item);
                }
                 */
                compressFileListBox.DataSource = compressFileList;
            }
        }

        private List<string> getAllCompressFile(string path)
        {
            string[] jsFileNames = Directory.GetFiles(path, "*.js", SearchOption.AllDirectories);
            string[] cssFileNames = Directory.GetFiles(path, "*.css", SearchOption.AllDirectories);
            List<string> fileList = new List<string>();
            foreach (string fileName in jsFileNames)
            {
                if (fileName.IndexOf(".min.js") == -1)
                {
                    if(!hasCompress(fileName))
                    {
                        fileList.Add(fileName);
                    }
                }
            }
            foreach (string fileName in cssFileNames)
            {
                if (fileName.IndexOf(".min.css") == -1)
                {
                    if (!hasCompress(fileName))
                    {
                        fileList.Add(fileName);
                    }
                }
            }
            return fileList;
        }

        private bool hasCompress(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            FileInfo compressFileInfo = new FileInfo(file.Replace(".js", ".min.js").Replace(".css", ".min.css"));
            if (!compressFileInfo.Exists)
            {
                return false;
            }
            if(fileInfo.LastWriteTime.Ticks > compressFileInfo.LastWriteTime.Ticks)
            {
                return false;
            }
            return true;
        }

        private string compressFile(List<string> fileList)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";

            //将cmd的标准输入和输出全部重定向到.NET的程序里

            cmd.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常

            cmd.StartInfo.RedirectStandardInput = true; //标准输入
            cmd.StartInfo.RedirectStandardOutput = true; //标准输出

            //不显示命令行窗口界面
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            cmd.Start(); //启动进程
            foreach (string file in fileList)
            {
                if (file.IndexOf(".js") != -1)
                {
                    string cmdStr = "java -jar " + jsCompressFile + " --js=" + file + " --js_output_file=" + file.Replace(".js", ".min.js");
                    cmd.StandardInput.WriteLine(cmdStr);
                }
                else
                {
                    string cmdStr = "java -jar " + cssCompressFile + " --type css --charset utf-8 -v " + file + " > " + file.Replace(".css", ".min.css");
                    cmd.StandardInput.WriteLine(cmdStr);
                }
                //outInfo.Text = cmd.StandardOutput.ReadToEnd();
            }

            cmd.StandardInput.WriteLine("exit");
            return "";//cmd.StandardOutput.ReadToEnd();
        }

        private void compress_Click(object sender, EventArgs e)
        {
            if (compressFileList != null)
            {
                compressFile(compressFileList);
                MessageBox.Show("完成");
            }
            else
            {
                MessageBox.Show("请先选择文件目录");
            }

        }
    }
}
