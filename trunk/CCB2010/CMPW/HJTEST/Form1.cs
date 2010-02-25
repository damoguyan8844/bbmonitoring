using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JOYFULL.CMPW.DAL;
using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.Report;
using JOYFULL.CMPW;

namespace HJTEST
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            ///ColorIdentifier.Interest();
            this.pictureBox1.Image = ColorIdentifier.Get();
        }

        private void button2_Click( object sender, EventArgs e )
        {
            //JOYFULL.CMPW.Report.Exportor exp = new JOYFULL.CMPW.Report.Exportor();
            string s = DalHelper.Encrypt("1");
            MessageBox.Show(s);
            s = DalHelper.Decrypt(s);
            MessageBox.Show(s);
        }

        private void button3_Click( object sender, EventArgs e )
        {

            //Exportor exp = new Exportor();
            //exp.NewBook();
            //exp.Write( 1, 1, "test" );
            //exp.Save( "c:/test.xls" );
            //exp.NewBook();
            //exp.Write( 1, 1, "test2" );
            //exp.Save( "c:/test2.xls" );
            //exp.Dispose();
            var player = new JOYFULL.CMPW.Alert.AlarmBuzzer();
            player.Play(500, "", "c:/音频.wav");
        }
    }
}
