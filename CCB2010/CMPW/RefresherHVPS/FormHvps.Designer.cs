namespace JOYFULL.CMPW.Refresher
{
    partial class FormHvps
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( FormHvps ) );
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point( 60, 59 );
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size( 75, 23 );
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler( this.btnStart_Click );
            this.btnStart.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.btnStart_KeyPress );
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point( 194, 59 );
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size( 75, 23 );
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler( this.btnClose_Click );
            this.btnClose.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.btnClose_KeyPress );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 123, 122 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 83, 12 );
            this.label1.TabIndex = 2;
            this.label1.Text = "点击 P 键暂停";
            // 
            // FormHvps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 328, 173 );
            this.ControlBox = false;
            this.Controls.Add( this.label1 );
            this.Controls.Add( this.btnClose );
            this.Controls.Add( this.btnStart );
            this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
            this.Name = "FormHvps";
            this.Text = "大额前置系统监控辅助程序";
            this.Activated += new System.EventHandler( this.FormHvps_Activated );
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.FormHvps_KeyPress );
            this.ResumeLayout( false );
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
    }
}