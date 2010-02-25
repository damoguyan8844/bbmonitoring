namespace JOYFULL.CMPW.Refresher
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
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent( )
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOverTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSpan = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxApps = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point( 216, 196 );
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size( 75, 23 );
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler( this.btnClose_Click );
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point( 259, 114 );
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size( 75, 23 );
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "应用";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler( this.btnApply_Click );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.label5 );
            this.groupBox1.Controls.Add( this.txtOverTime );
            this.groupBox1.Controls.Add( this.label2 );
            this.groupBox1.Controls.Add( this.label4 );
            this.groupBox1.Controls.Add( this.txtSpan );
            this.groupBox1.Controls.Add( this.label3 );
            this.groupBox1.Controls.Add( this.cbxApps );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Controls.Add( this.btnApply );
            this.groupBox1.Location = new System.Drawing.Point( 12, 12 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 352, 152 );
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 137, 118 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 77, 12 );
            this.label5.TabIndex = 13;
            this.label5.Text = "(格式hhmmss)";
            // 
            // txtOverTime
            // 
            this.txtOverTime.Location = new System.Drawing.Point( 75, 114 );
            this.txtOverTime.MaxLength = 6;
            this.txtOverTime.Name = "txtOverTime";
            this.txtOverTime.Size = new System.Drawing.Size( 47, 21 );
            this.txtOverTime.TabIndex = 12;
            this.txtOverTime.Text = "200000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font( "宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label2.Location = new System.Drawing.Point( 7, 118 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 57, 12 );
            this.label2.TabIndex = 11;
            this.label2.Text = "日终时刻";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 137, 71 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 17, 12 );
            this.label4.TabIndex = 10;
            this.label4.Text = "分";
            // 
            // txtSpan
            // 
            this.txtSpan.Location = new System.Drawing.Point( 75, 67 );
            this.txtSpan.Name = "txtSpan";
            this.txtSpan.Size = new System.Drawing.Size( 47, 21 );
            this.txtSpan.TabIndex = 9;
            this.txtSpan.Text = "5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font( "宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label3.Location = new System.Drawing.Point( 7, 71 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 57, 12 );
            this.label3.TabIndex = 8;
            this.label3.Text = "刷新周期";
            // 
            // cbxApps
            // 
            this.cbxApps.FormattingEnabled = true;
            this.cbxApps.Location = new System.Drawing.Point( 75, 21 );
            this.cbxApps.Name = "cbxApps";
            this.cbxApps.Size = new System.Drawing.Size( 259, 20 );
            this.cbxApps.TabIndex = 4;
            this.cbxApps.DropDown += new System.EventHandler( this.cbxApps_DropDown );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label1.Location = new System.Drawing.Point( 7, 21 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 57, 12 );
            this.label1.TabIndex = 3;
            this.label1.Text = "选择程序";
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point( 85, 196 );
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size( 75, 23 );
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler( this.btnStart_Click );
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 376, 241 );
            this.ControlBox = false;
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.btnClose );
            this.Controls.Add( this.btnStart );
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "CCBS系统自动刷新程序";
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSpan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxApps;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOverTime;
    }
}

