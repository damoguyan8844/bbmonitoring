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
    public partial class FormAddOperator : Form
    {
        public FormAddOperator()
        {
            InitializeComponent();
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        private void btnOK_Click( object sender, EventArgs e )
        {
            string s = string.Empty;
            if ( textBox1.Text.Length > 5 )
            {
                s = "操作员用户名过长，请重新输入。";
                textBox1.Text = string.Empty;
                textBox1.Focus();
            }
            else if( textBox2.Text.Length < 6 || textBox3.Text.Length < 6 )
            {
                s = "操作员登陆密码不能少于6位，请重新输入";
                textBox2.Text = textBox3.Text = string.Empty;
                textBox2.Focus();
            }
            else if( textBox2.Text != textBox3.Text )
            {
                s = "密码前后输入不一致，请重新输入";
                textBox2.Text = textBox3.Text = string.Empty;
                textBox2.Focus();
            }
            if( string.IsNullOrEmpty( s ) )
            {
                UserName = textBox1.Text;
                Password = textBox2.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show( s );
            }
        }
    }
}
