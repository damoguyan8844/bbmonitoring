using JOYFULL.CMPW.Monitor;
namespace MonitorTest
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxCapture = new System.Windows.Forms.PictureBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textIdle = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textCapture = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textSettingFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboSystems = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxRect = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnWhiteText = new System.Windows.Forms.Button();
            this.btnBlackText = new System.Windows.Forms.Button();
            this.btnBlackDigit = new System.Windows.Forms.Button();
            this.btnWhiteDigit = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBlackWhite = new System.Windows.Forms.TextBox();
            this.textOCRContent = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCapture)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCapture
            // 
            this.pictureBoxCapture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxCapture.Location = new System.Drawing.Point(12, 29);
            this.pictureBoxCapture.Name = "pictureBoxCapture";
            this.pictureBoxCapture.Size = new System.Drawing.Size(882, 520);
            this.pictureBoxCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCapture.TabIndex = 0;
            this.pictureBoxCapture.TabStop = false;
            this.pictureBoxCapture.Click += new System.EventHandler(this.pictureBoxCapture_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.Location = new System.Drawing.Point(135, 629);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(91, 27);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "结束截屏";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 560);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "刷新间隔(毫秒)：";
            // 
            // textIdle
            // 
            this.textIdle.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textIdle.Location = new System.Drawing.Point(135, 558);
            this.textIdle.Name = "textIdle";
            this.textIdle.Size = new System.Drawing.Size(91, 20);
            this.textIdle.TabIndex = 9;
            this.textIdle.Text = "100";
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(11, 629);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(95, 27);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "开始截屏";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "屏幕文件名：";
            // 
            // textCapture
            // 
            this.textCapture.BackColor = System.Drawing.SystemColors.Control;
            this.textCapture.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textCapture.Location = new System.Drawing.Point(95, 3);
            this.textCapture.Name = "textCapture";
            this.textCapture.Size = new System.Drawing.Size(799, 20);
            this.textCapture.TabIndex = 12;
            this.textCapture.TextChanged += new System.EventHandler(this.textCapture_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 600);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "保存至文件夹  ：";
            // 
            // textFolder
            // 
            this.textFolder.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textFolder.Location = new System.Drawing.Point(135, 590);
            this.textFolder.Name = "textFolder";
            this.textFolder.Size = new System.Drawing.Size(91, 20);
            this.textFolder.TabIndex = 14;
            this.textFolder.Text = "CaptureTemp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(646, 560);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "配置文件：";
            // 
            // textSettingFile
            // 
            this.textSettingFile.BackColor = System.Drawing.SystemColors.Control;
            this.textSettingFile.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textSettingFile.Location = new System.Drawing.Point(738, 557);
            this.textSettingFile.Name = "textSettingFile";
            this.textSettingFile.Size = new System.Drawing.Size(156, 20);
            this.textSettingFile.TabIndex = 16;
            this.textSettingFile.Text = "SystemsSetting.xml";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(314, 566);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "当前系统：";
            // 
            // cboSystems
            // 
            this.cboSystems.DisplayMember = "1";
            this.cboSystems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboSystems.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboSystems.FormattingEnabled = true;
            this.cboSystems.Items.AddRange(new object[] {
            "CCBS分行终端系统",
            "大额前置系统",
            "大额支付系统(汇划业务)",
            "小额前置系统",
            "小额直联系统(非实时业务)",
            "大额支付系统(事务信息)",
            "小额直联系统(非实时事务)",
            "支票影像系统",
            "重客系统",
            "清算系统",
            "清算直联系统",
            "CTS系统",
            "证券系统",
            "银保通系统"});
            this.cboSystems.Location = new System.Drawing.Point(404, 557);
            this.cboSystems.Name = "cboSystems";
            this.cboSystems.Size = new System.Drawing.Size(160, 20);
            this.cboSystems.TabIndex = 18;
            this.cboSystems.ValueMember = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(314, 600);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "区域位置：";
            // 
            // textBoxRect
            // 
            this.textBoxRect.Location = new System.Drawing.Point(404, 597);
            this.textBoxRect.Name = "textBoxRect";
            this.textBoxRect.Size = new System.Drawing.Size(160, 20);
            this.textBoxRect.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(402, 582);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(162, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "( Left;Top;Right;Bottom )";
            // 
            // btnWhiteText
            // 
            this.btnWhiteText.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWhiteText.Location = new System.Drawing.Point(282, 629);
            this.btnWhiteText.Name = "btnWhiteText";
            this.btnWhiteText.Size = new System.Drawing.Size(74, 27);
            this.btnWhiteText.TabIndex = 22;
            this.btnWhiteText.Text = "白色文本";
            this.btnWhiteText.UseVisualStyleBackColor = true;
            this.btnWhiteText.Click += new System.EventHandler(this.btnWhiteText_Click);
            // 
            // btnBlackText
            // 
            this.btnBlackText.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBlackText.Location = new System.Drawing.Point(362, 629);
            this.btnBlackText.Name = "btnBlackText";
            this.btnBlackText.Size = new System.Drawing.Size(74, 27);
            this.btnBlackText.TabIndex = 23;
            this.btnBlackText.Text = "黑色文本";
            this.btnBlackText.UseVisualStyleBackColor = true;
            this.btnBlackText.Click += new System.EventHandler(this.btnBlackText_Click);
            // 
            // btnBlackDigit
            // 
            this.btnBlackDigit.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBlackDigit.Location = new System.Drawing.Point(522, 629);
            this.btnBlackDigit.Name = "btnBlackDigit";
            this.btnBlackDigit.Size = new System.Drawing.Size(74, 27);
            this.btnBlackDigit.TabIndex = 24;
            this.btnBlackDigit.Text = "黑色数字";
            this.btnBlackDigit.UseVisualStyleBackColor = true;
            this.btnBlackDigit.Click += new System.EventHandler(this.btnBlackDigit_Click);
            // 
            // btnWhiteDigit
            // 
            this.btnWhiteDigit.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWhiteDigit.Location = new System.Drawing.Point(442, 629);
            this.btnWhiteDigit.Name = "btnWhiteDigit";
            this.btnWhiteDigit.Size = new System.Drawing.Size(74, 27);
            this.btnWhiteDigit.TabIndex = 25;
            this.btnWhiteDigit.Text = "白色数字";
            this.btnWhiteDigit.UseVisualStyleBackColor = true;
            this.btnWhiteDigit.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(646, 600);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 12);
            this.label8.TabIndex = 26;
            this.label8.Text = "黑白化值：";
            // 
            // textBlackWhite
            // 
            this.textBlackWhite.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBlackWhite.Location = new System.Drawing.Point(738, 598);
            this.textBlackWhite.Name = "textBlackWhite";
            this.textBlackWhite.Size = new System.Drawing.Size(156, 20);
            this.textBlackWhite.TabIndex = 27;
            this.textBlackWhite.Text = "128";
            // 
            // textOCRContent
            // 
            this.textOCRContent.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOCRContent.Location = new System.Drawing.Point(738, 633);
            this.textOCRContent.Name = "textOCRContent";
            this.textOCRContent.Size = new System.Drawing.Size(156, 20);
            this.textOCRContent.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(646, 635);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 12);
            this.label9.TabIndex = 28;
            this.label9.Text = "识别内容：";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 670);
            this.Controls.Add(this.textOCRContent);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBlackWhite);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnWhiteDigit);
            this.Controls.Add(this.btnBlackDigit);
            this.Controls.Add(this.btnBlackText);
            this.Controls.Add(this.btnWhiteText);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxRect);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboSystems);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textSettingFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textCapture);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.textIdle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.pictureBoxCapture);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCapture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxCapture;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textIdle;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textCapture;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textSettingFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboSystems;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxRect;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnWhiteText;
        private System.Windows.Forms.Button btnBlackText;
        private System.Windows.Forms.Button btnBlackDigit;
        private System.Windows.Forms.Button btnWhiteDigit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBlackWhite;
        private System.Windows.Forms.TextBox textOCRContent;
        private System.Windows.Forms.Label label9;
    }
}

