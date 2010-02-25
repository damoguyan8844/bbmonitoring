using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Browser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
            webBrowser1.Navigate( "www.google.com" );
        }

        private void webBrowser1_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e )
        {
            button1.Enabled = true;
        }

        private void button1_Click( object sender, EventArgs e )
        {
            HtmlElementCollection coll = 
                webBrowser1.Document.GetElementsByTagName( "input" );
            foreach( HtmlElement element in coll )
            {
                if( element.GetAttribute( "name" ) == "q" )
                {
                    element.SetAttribute( "value", "张宏伟" );
                    break;
                }
            }
            foreach ( HtmlElement element in coll )
            {
                if ( element.GetAttribute( "value" ) == "Google Search" )
                {
                    element.InvokeMember( "Click" );
                    break;
                }
            }
        }
    }
}
