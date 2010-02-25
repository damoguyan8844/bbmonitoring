namespace Browser
{
    partial class FromImportantCustom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FromImportantCustom ) );
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblExNode = new System.Windows.Forms.Label();
            this.lblExTrade = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblResultAlert = new System.Windows.Forms.Label();
            this.lblTotalAlert = new System.Windows.Forms.Label();
            this.lblDetail = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point( 12, 144 );
            this.webBrowser1.MinimumSize = new System.Drawing.Size( 20, 20 );
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size( 992, 550 );
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler( this.webBrowser1_DocumentCompleted );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label1.Location = new System.Drawing.Point( 185, 27 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 171, 22 );
            this.label1.TabIndex = 1;
            this.label1.Text = "异常交易总笔数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label2.Location = new System.Drawing.Point( 185, 61 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 148, 22 );
            this.label2.TabIndex = 2;
            this.label2.Text = "异常节点个数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label3.Location = new System.Drawing.Point( 450, 27 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 217, 22 );
            this.label3.TabIndex = 3;
            this.label3.Text = "中心状态为正常工作";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.lblStatus.Location = new System.Drawing.Point( 727, 27 );
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size( 22, 22 );
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "1";
            // 
            // lblExNode
            // 
            this.lblExNode.AutoSize = true;
            this.lblExNode.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.lblExNode.Location = new System.Drawing.Point( 395, 61 );
            this.lblExNode.Name = "lblExNode";
            this.lblExNode.Size = new System.Drawing.Size( 22, 22 );
            this.lblExNode.TabIndex = 5;
            this.lblExNode.Text = "0";
            // 
            // lblExTrade
            // 
            this.lblExTrade.AutoSize = true;
            this.lblExTrade.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.lblExTrade.Location = new System.Drawing.Point( 395, 27 );
            this.lblExTrade.Name = "lblExTrade";
            this.lblExTrade.Size = new System.Drawing.Size( 22, 22 );
            this.lblExTrade.TabIndex = 4;
            this.lblExTrade.Text = "0";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.Label8.Location = new System.Drawing.Point( 798, 27 );
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size( 148, 22 );
            this.Label8.TabIndex = 8;
            this.Label8.Text = "交易结果报警";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label9.Location = new System.Drawing.Point( 450, 61 );
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size( 194, 22 );
            this.label9.TabIndex = 7;
            this.label9.Text = "交易全程状态报警";
            // 
            // lblResultAlert
            // 
            this.lblResultAlert.AutoSize = true;
            this.lblResultAlert.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.lblResultAlert.Location = new System.Drawing.Point( 982, 27 );
            this.lblResultAlert.Name = "lblResultAlert";
            this.lblResultAlert.Size = new System.Drawing.Size( 22, 22 );
            this.lblResultAlert.TabIndex = 11;
            this.lblResultAlert.Text = "0";
            // 
            // lblTotalAlert
            // 
            this.lblTotalAlert.AutoSize = true;
            this.lblTotalAlert.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.lblTotalAlert.Location = new System.Drawing.Point( 727, 61 );
            this.lblTotalAlert.Name = "lblTotalAlert";
            this.lblTotalAlert.Size = new System.Drawing.Size( 22, 22 );
            this.lblTotalAlert.TabIndex = 10;
            this.lblTotalAlert.Text = "0";
            // 
            // lblDetail
            // 
            this.lblDetail.AutoSize = true;
            this.lblDetail.ForeColor = System.Drawing.Color.Red;
            this.lblDetail.Location = new System.Drawing.Point( 185, 98 );
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new System.Drawing.Size( 0, 12 );
            this.lblDetail.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font( "宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 134 ) ) );
            this.label4.Location = new System.Drawing.Point( 80, 60 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 58, 22 );
            this.label4.TabIndex = 13;
            this.label4.Text = "0009";
            // 
            // FromImportantCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 1016, 706 );
            this.Controls.Add( this.label4 );
            this.Controls.Add( this.lblDetail );
            this.Controls.Add( this.lblResultAlert );
            this.Controls.Add( this.lblTotalAlert );
            this.Controls.Add( this.Label8 );
            this.Controls.Add( this.label9 );
            this.Controls.Add( this.lblStatus );
            this.Controls.Add( this.lblExNode );
            this.Controls.Add( this.lblExTrade );
            this.Controls.Add( this.label3 );
            this.Controls.Add( this.label2 );
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.webBrowser1 );
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.Name = "FromImportantCustom";
            this.Text = "重客系统监控辅助程序";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblExNode;
        private System.Windows.Forms.Label lblExTrade;
        private System.Windows.Forms.Label Label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblResultAlert;
        private System.Windows.Forms.Label lblTotalAlert;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.Label label4;
    }
}