using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;

namespace CompressFile
{
    public partial class Form1 : Form
    {
        private string jsCompressFile;
        private string cssCompressFile;
        private List<string> compressFileList;
        private StringBuilder errorMsg;
        private string currentPath;
        private string combineFilePath;
        private List<CombineFile> combineFileList;
        public Form1()
        {
            InitializeComponent();
            currentPath = Directory.GetCurrentDirectory();
            checkBox1.Checked = true;
            if (File.Exists(currentPath + "\\compiler.jar"))
            {
                jsCompressFile = currentPath + "\\compiler.jar";
                jsCompressorFilePath.Text = jsCompressFile;
            }
            if (File.Exists(currentPath + "\\yuicompressor.jar"))
            {
                cssCompressFile = currentPath + "\\yuicompressor.jar";
                cssCompressFilePath.Text = cssCompressFile;
            }
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

        private string compressFile(string file, string outputFile, string compressApp)
        {
            Process cmd = new Process();
            
            
            cmd.StartInfo.FileName = "cmd.exe";

            //将cmd的标准输入和输出全部重定向到.NET的程序里

            cmd.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常

            cmd.StartInfo.RedirectStandardInput = true; //标准输入
            cmd.StartInfo.RedirectStandardOutput = true; //标准输出
            cmd.StartInfo.RedirectStandardError = true;

            //不显示命令行窗口界面
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            cmd.Start(); //启动进程
            /*
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
            }
             * */
            string cmdStr = "";
            if (compressApp.Equals(jsCompressFile))
            {
                cmdStr = "java -jar " + compressApp + " --js=" + file + " --js_output_file=" + outputFile;
            }
            else
            {
                cmdStr = "java -jar " + compressApp + " --type css --charset utf-8 -v " + file + " > " + outputFile;
            }
            cmd.StandardInput.WriteLine(cmdStr);
            cmd.StandardInput.WriteLine("exit");
            cmd.WaitForExit();
            string errMsg = cmd.StandardError.ReadToEnd();
            cmd.Close();
            

            
            /*
            cmd.StartInfo.FileName = "java";
            if (compressApp.Equals(jsCompressFile))
            {
                cmd.StartInfo.Arguments = " -jar " + compressApp + " --js=" + file + " --js_output_file=" + outputFile;
            }
            else
            {
                cmd.StartInfo.Arguments = " -jar " + compressApp + " --type css --charset utf-8 -v " + file + " > " + outputFile;
            }

            //将cmd的标准输入和输出全部重定向到.NET的程序里

            cmd.StartInfo.UseShellExecute = false; //此处必须为false否则引发异常

            cmd.StartInfo.RedirectStandardInput = true; //标准输入
            cmd.StartInfo.RedirectStandardOutput = true; //标准输出
            cmd.StartInfo.RedirectStandardError = true;

            //不显示命令行窗口界面
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            cmd.Start(); //启动进程

            cmd.WaitForExit();
            string errMsg = cmd.StandardError.ReadToEnd();
            cmd.Close();
            */



            return errMsg;
        }

        private void compress_Click(object sender, EventArgs e)
        {
            if (compressFileList != null)
            {
                errorMsg = new StringBuilder();
                foreach (string file in compressFileList)
                {
                    if (file.IndexOf(".js") != -1)
                    {
                        //string cmdStr = "java -jar " + jsCompressFile + " --js=" + file + " --js_output_file=" + file.Replace(".js", ".min.js");
                        //cmd.StandardInput.WriteLine(cmdStr);
                        errorMsg.Append(compressFile(file, file.Replace(".js", ".min.js"), jsCompressFile));
                    }
                    else
                    {
                        //string cmdStr = "java -jar " + cssCompressFile + " --type css --charset utf-8 -v " + file + " > " + file.Replace(".css", ".min.css");
                        //cmd.StandardInput.WriteLine(cmdStr);
                        errorMsg.Append(compressFile(file, file.Replace(".css", ".min.css"), cssCompressFile));
                    }
                }
                //compressFile(compressFileList);
                outInfo.Text = errorMsg.ToString();
                MessageBox.Show("完成");
            }
            else
            {
                MessageBox.Show("请先选择文件目录");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = dlg.FileName;
                if (File.Exists(fileName))
                {
                    string outputInfo =  "";
                    combineFileList = getCombineFileList(fileName);
                    foreach (CombineFile combineFile in combineFileList)
                    {
                        string combineFileStr = combineFile.name + " : ";
                        foreach(string name  in combineFile.fileList)
                        {
                            combineFileStr += (name + ", ");
                        }
                        outputInfo += (combineFileStr.Substring(0, combineFileStr.Length - 2) + "\r\n\r\n");
                    }
                    combineFileTextBox.Text = outputInfo;
                }
            }
        }

        private List<CombineFile> getCombineFileList(string fileName)
        {
            List<CombineFile> fileList = new List<CombineFile>();
            StreamReader reader = new StreamReader(fileName);
            string data = reader.ReadToEnd();
            reader.Close();
            FileInfo fileInfo = new FileInfo(fileName);
            combineFilePath = fileInfo.DirectoryName + "\\";
            Regex r = new Regex(@"\/\*\*\*\*COMBINE_START[\s\S]*?\/\*\*\*\*COMBINE_END\*\/");
            Match m = r.Match(data);
            while(m.Success)
            {
                CombineFile combineFile = new CombineFile();
                int index = m.Value.IndexOf(@"*/");
                combineFile.name = m.Value.Substring(18, index - 18).Replace('"', ' ').Replace("/", "\\").Trim();
                string fileArrayStr = m.Value.Substring(index + 2);
                fileArrayStr = fileArrayStr.Substring(0, fileArrayStr.Length - 18).Trim();
                int start = fileArrayStr.IndexOf('(');
                int end = fileArrayStr.IndexOf(')');
                string[] fileNameArr = fileArrayStr.Substring(start + 1, end - start - 1).Split(',');
                for (int i = 0, len = fileNameArr.Length; i < len; i++)
                {
                    combineFile.addFile(fileNameArr[i].Replace('"', ' ').Replace("/", "\\").Trim());
                }
                m = m.NextMatch();
                if (checkCombine(combineFile.fileList, combineFile.name.Trim(), combineFilePath))
                {
                    fileList.Add(combineFile);
                }
            }
            return fileList;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (combineFileList == null)
            {
                MessageBox.Show("请先选择文件");
                return;
            }
            errorMsg = new StringBuilder();
            foreach (CombineFile combineFile in combineFileList)
            {
                StringBuilder buffer = new StringBuilder();
                bool appendDivide = false;
                if (combineFile.name.IndexOf(".css") == -1)
                {
                    appendDivide = true;
                }
                foreach (string file in combineFile.fileList)
                {
                    string allContent = File.ReadAllText(combineFilePath + file.Trim());
                    if (!appendDivide)
                    {
                        allContent = allContent.Replace("../../images", "../images").Replace("../images", "../../images").Replace("../../images", "./images");
                    }
                    buffer.Append(allContent);
                    if (appendDivide)
                    {
                        buffer.Append(";");
                    }
                }
                string tmpFile, compressApp, saveFile = combineFilePath + combineFile.name.Trim();
                if (combineFile.name.IndexOf(".css") != -1)
                {
                    tmpFile = saveFile.Replace(".min.css", ".tmp.css");
                    compressApp = cssCompressFile;
                }
                else
                {
                    tmpFile = saveFile.Replace(".min.js", ".tmp.js");
                    compressApp = jsCompressFile;
                }
                File.WriteAllText(tmpFile, buffer.ToString());
                errorMsg.Append(saveFile + "\r\n");
                errorMsg.Append(compressFile(tmpFile, saveFile, compressApp));
                errorMsg.Append("\r\n\r\n");
                if (checkBox1.Checked)
                {
                    File.Delete(tmpFile);
                }
            }
            MessageBox.Show("合并完成");
            outInfo.Text = errorMsg.ToString();
        }

        private bool checkCombine(List<string> fileList, string combineFile, string combineFilePath)
        {
            bool needCombine = true;
            DateTime lastModify = DateTime.Today;
            DateTime combineFileModify;
            if (!File.Exists(combineFilePath + combineFile))
            {
                return needCombine;
            }
            combineFileModify = new FileInfo(combineFilePath + combineFile).LastWriteTime;
            foreach (string file in fileList)
            {
                FileInfo fileInfo = new FileInfo(combineFilePath + file.Trim());
                if (fileInfo.LastWriteTime > lastModify)
                {
                    lastModify = fileInfo.LastWriteTime;
                }
            }
            if (combineFileModify > lastModify)
            {
                needCombine = false;
            }
            return needCombine;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                compressFileList = new List<string>();
                foreach (string fileName in dlg.FileNames)
                {
                    if ((fileName.IndexOf(".min.js") == -1 && fileName.IndexOf(".js") != -1) || (fileName.IndexOf(".min.css") == -1 && fileName.IndexOf(".css") != -1))
                    {
                        compressFileList.Add(fileName);
                    }
                }
            }
            compressFileListBox.DataSource = compressFileList;
        }

        private void compressFileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class CombineFile
    {
        public List<string> fileList;
        public string name;
        public CombineFile()
        {
            fileList = new List<string>();
        }
        public void addFile(string file)
        {
            fileList.Add(file);
        }
        /*
        public string[] commonFileArr;
        public List<OutPutFile> outputFileList;
        public CombineFile()
        {
            outputFileList = new List<OutPutFile>();
        }   
        */

    }

    public class OutPutFile
    {
        public string name;
        public string[] privateFileArr;
    }
}
