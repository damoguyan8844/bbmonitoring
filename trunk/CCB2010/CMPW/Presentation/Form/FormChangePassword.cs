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
    public partial class FormChangePassword : Form
    {
        string _ps = string.Empty;
        public FormChangePassword( string name, string ps )
        {
            InitializeComponent();
            this.Text = "修改用户密码 - " + name;
            _ps = ps;
            var op = App.Oper;
            if ( op != null && op.IsAdmin ) //主管，则自动填写密码
                textBox1.Text = ps;
        }

        private void btnOK_Click( object sender, EventArgs e )
        {
            string info = string.Empty;
            var op = App.Oper;
            if( ( op == null || !op.IsAdmin ) && textBox1.Text != _ps )
            {
                info = "原密码输入错误，请检查后重输";
                textBox1.Text = string.Empty;
                textBox1.Focus();
            }
            else if( textBox2.Text.Length < 6 || textBox3.Text.Length < 6 )
            {
                info = "操作员登陆密码不能少于6位，请重新输入";
                textBox2.Text = textBox3.Text = string.Empty;
                textBox2.Focus();
            }
            else if( textBox2.Text != textBox3.Text )
            {
                info = "新密码前后输入不一致，请重新输入";
                textBox2.Text = textBox3.Text = string.Empty;
                textBox2.Focus();
            }
            if( string.IsNullOrEmpty( info ) )
            {
                Password = textBox2.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show( info );
            }
        }
        public string Password { get; set; }
    }
}
