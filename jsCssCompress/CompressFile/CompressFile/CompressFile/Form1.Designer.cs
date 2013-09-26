namespace CompressFile
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chooseJsCompressorFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.jsCompressorFilePath = new System.Windows.Forms.TextBox();
            this.cssCompressFilePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chooseCssCompressorFile = new System.Windows.Forms.Button();
            this.choosePath = new System.Windows.Forms.Button();
            this.compress = new System.Windows.Forms.Button();
            this.compressFileListBox = new System.Windows.Forms.ListBox();
            this.outInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // chooseJsCompressorFile
            // 
            this.chooseJsCompressorFile.Location = new System.Drawing.Point(513, 12);
            this.chooseJsCompressorFile.Name = "chooseJsCompressorFile";
            this.chooseJsCompressorFile.Size = new System.Drawing.Size(75, 23);
            this.chooseJsCompressorFile.TabIndex = 0;
            this.chooseJsCompressorFile.Text = "选择";
            this.chooseJsCompressorFile.UseVisualStyleBackColor = true;
            this.chooseJsCompressorFile.Click += new System.EventHandler(this.chooseJsCompressorFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Js压缩工具目录：";
            // 
            // jsCompressorFilePath
            // 
            this.jsCompressorFilePath.Location = new System.Drawing.Point(119, 13);
            this.jsCompressorFilePath.Name = "jsCompressorFilePath";
            this.jsCompressorFilePath.ReadOnly = true;
            this.jsCompressorFilePath.Size = new System.Drawing.Size(372, 21);
            this.jsCompressorFilePath.TabIndex = 2;
            // 
            // cssCompressFilePath
            // 
            this.cssCompressFilePath.Location = new System.Drawing.Point(119, 47);
            this.cssCompressFilePath.Name = "cssCompressFilePath";
            this.cssCompressFilePath.ReadOnly = true;
            this.cssCompressFilePath.Size = new System.Drawing.Size(372, 21);
            this.cssCompressFilePath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Css压缩工具目录：";
            // 
            // chooseCssCompressorFile
            // 
            this.chooseCssCompressorFile.Location = new System.Drawing.Point(513, 46);
            this.chooseCssCompressorFile.Name = "chooseCssCompressorFile";
            this.chooseCssCompressorFile.Size = new System.Drawing.Size(75, 23);
            this.chooseCssCompressorFile.TabIndex = 5;
            this.chooseCssCompressorFile.Text = "选择";
            this.chooseCssCompressorFile.UseVisualStyleBackColor = true;
            this.chooseCssCompressorFile.Click += new System.EventHandler(this.chooseCssCompressorFile_Click);
            // 
            // choosePath
            // 
            this.choosePath.Location = new System.Drawing.Point(513, 178);
            this.choosePath.Name = "choosePath";
            this.choosePath.Size = new System.Drawing.Size(75, 23);
            this.choosePath.TabIndex = 7;
            this.choosePath.Text = "选择目录";
            this.choosePath.UseVisualStyleBackColor = true;
            this.choosePath.Click += new System.EventHandler(this.choosePath_Click);
            // 
            // compress
            // 
            this.compress.Location = new System.Drawing.Point(514, 228);
            this.compress.Name = "compress";
            this.compress.Size = new System.Drawing.Size(75, 23);
            this.compress.TabIndex = 8;
            this.compress.Text = "压缩";
            this.compress.UseVisualStyleBackColor = true;
            this.compress.Click += new System.EventHandler(this.compress_Click);
            // 
            // compressFileListBox
            // 
            this.compressFileListBox.FormattingEnabled = true;
            this.compressFileListBox.ItemHeight = 12;
            this.compressFileListBox.Location = new System.Drawing.Point(14, 91);
            this.compressFileListBox.Name = "compressFileListBox";
            this.compressFileListBox.Size = new System.Drawing.Size(477, 208);
            this.compressFileListBox.TabIndex = 9;
            // 
            // outInfo
            // 
            this.outInfo.Location = new System.Drawing.Point(14, 315);
            this.outInfo.Multiline = true;
            this.outInfo.Name = "outInfo";
            this.outInfo.Size = new System.Drawing.Size(575, 102);
            this.outInfo.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 429);
            this.Controls.Add(this.outInfo);
            this.Controls.Add(this.compressFileListBox);
            this.Controls.Add(this.compress);
            this.Controls.Add(this.choosePath);
            this.Controls.Add(this.chooseCssCompressorFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cssCompressFilePath);
            this.Controls.Add(this.jsCompressorFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chooseJsCompressorFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button chooseJsCompressorFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox jsCompressorFilePath;
        private System.Windows.Forms.TextBox cssCompressFilePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button chooseCssCompressorFile;
        private System.Windows.Forms.Button choosePath;
        private System.Windows.Forms.Button compress;
        private System.Windows.Forms.ListBox compressFileListBox;
        private System.Windows.Forms.TextBox outInfo;
    }
}

