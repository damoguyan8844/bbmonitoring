using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JOYFULL.CMPW.Refresher
{
    internal partial class FrmKey : Form
    {
        public FrmKey( )
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyUp += new KeyEventHandler( FrmKey_KeyUp );
            this.Focus();
        }

        void FrmKey_KeyUp( object sender, KeyEventArgs e )
        {
            KeyData = e.KeyData;
            Description = ( e.Control ? "Ctrl + " : "" ) +
                ( e.Shift ? "Shift + " : "" ) +
                ( e.Alt ? "Alt + " : "" ) + e.KeyCode.ToString();
            this.DialogResult = DialogResult.OK;
        }

        public Keys KeyData { get; set; }
        public string Description { get; set; }
    }
}
