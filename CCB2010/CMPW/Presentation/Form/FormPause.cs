using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JOYFULL.CMPW.Presentation
{
    public partial class FormPause : Form
    {
        public enum ResumeTypeEnum
        {
            Auto = 0,
            Manual = 1,
            Exit = 2
        }

        public ResumeTypeEnum ResumeType
        {
            get;
            set;
        }
        /// <summary>in minute</summary>
        public int Interval { get; set; }

        public FormPause()
        {
            InitializeComponent();
        }

        private void rdbAuto_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = textBox1.Visible = rdbAuto.Checked;
            if( label1.Visible )
            {
                textBox1.Text = "5";
                textBox1.DeselectAll();
                textBox1.Focus();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Interval = -1;
            if( rdbAuto.Checked )
            {
                string info = string.Empty;
                int num = 0;
                if( int.TryParse( textBox1.Text, out num ) )
                {
                    if (num < 1)
                        info = "自动恢复监控的时间间隔不能小于1分钟";
                    else if( num > 1440 )
                        info = "自动恢复监控的时间间隔不能大于24小时";
                }
                else 
                {
                    info = "请输入1~1440之间的整数";
                }
                if( !string.IsNullOrEmpty( info ) )
                {
                    MessageBox.Show(info);
                    return;
                }
                Interval = num;
                this.ResumeType = ResumeTypeEnum.Auto;
            }
            else if( rdbManual.Checked )
            {
                this.ResumeType = ResumeTypeEnum.Manual;
            }
            else if( rdbStop.Checked )
            {
                this.ResumeType = ResumeTypeEnum.Exit;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
