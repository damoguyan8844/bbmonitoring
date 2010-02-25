namespace JOYFUL.CMPW.Capturer
{
    partial class FormCCBS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormCCBS ) );
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxApps = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.btnBrowse );
            this.groupBox1.Controls.Add( this.txtPath );
            this.groupBox1.Controls.Add( this.label2 );
            this.groupBox1.Controls.Add( this.label4 );
            this.groupBox1.Controls.Add( this.txtNumber );
            this.groupBox1.Controls.Add( this.label3 );
            this.groupBox1.Controls.Add( this.cbxApps );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Location = new System.Drawing.Point( 11, 12 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 449, 162 );
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point( 368, 71 );
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size( 75, 23 );
            this.btnBrowse.TabIndex = 13;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler( this.btnBrowse_Click );
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point( 75, 72 );
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size( 282, 21 );
            this.txtPath.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font( "宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label2.Location = new System.Drawing.Point( 7, 76 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 57, 12 );
            this.label2.TabIndex = 11;
            this.label2.Text = "保存路径";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 155, 130 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 17, 12 );
            this.label4.TabIndex = 10;
            this.label4.Text = "张";
            // 
            // txtNumber
            // 
            this.txtNumber.Font = new System.Drawing.Font( "宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.txtNumber.Location = new System.Drawing.Point( 75, 126 );
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size( 62, 21 );
            this.txtNumber.TabIndex = 9;
            this.txtNumber.Text = "50";
            this.txtNumber.TextChanged += new System.EventHandler( this.txtNumber_TextChanged );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font( "宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label3.Location = new System.Drawing.Point( 7, 130 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 57, 12 );
            this.label3.TabIndex = 8;
            this.label3.Text = "截屏数目";
            // 
            // cbxApps
            // 
            this.cbxApps.FormattingEnabled = true;
            this.cbxApps.Location = new System.Drawing.Point( 75, 22 );
            this.cbxApps.Name = "cbxApps";
            this.cbxApps.Size = new System.Drawing.Size( 282, 20 );
            this.cbxApps.TabIndex = 4;
            this.cbxApps.DropDown += new System.EventHandler( this.cbxApps_DropDown );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label1.Location = new System.Drawing.Point( 7, 22 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 57, 12 );
            this.label1.TabIndex = 3;
            this.label1.Text = "选择程序";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point( 73, 197 );
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size( 75, 23 );
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler( this.btnStart_Click );
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point( 291, 196 );
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size( 75, 23 );
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler( this.btnClose_Click );
            // 
            // FormCCBS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 472, 232 );
            this.ControlBox = false;
            this.Controls.Add( this.btnClose );
            this.Controls.Add( this.groupBox1 );
            this.Controls.Add( this.btnStart );
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.Name = "FormCCBS";
            this.Text = "CCBS系统自动截屏程序";
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxApps;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnClose;
    }
}

