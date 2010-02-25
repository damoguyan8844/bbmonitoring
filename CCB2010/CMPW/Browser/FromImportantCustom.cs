using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Browser
{
    public partial class FromImportantCustom : Form
    {
        const string URL_BASE = "http://38.0.192.17:91";
        const string URL_CONTENT = "http://38.0.192.17:91/asp/Qrymnitr/SystemShowOk.asp";
        const int INTERVAL = 30;//seconds

        Dictionary<string, DateTime> _handling = new Dictionary<string, DateTime>();

        public FromImportantCustom()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = false;
            webBrowser1.NewWindow += new CancelEventHandler( webBrowser1_NewWindow );
            webBrowser1.Navigate( URL_BASE );
        }

        void webBrowser1_NewWindow( object sender, CancelEventArgs e )
        {
            //webBrowser1.Navigate()
            //MessageBox.Show( sender.GetType().FullName );
            var browser = sender as WebBrowser;
            //MessageBox.Show( browser.Url.AbsoluteUri );
            //webBrowser1.Navigate( browser.Url.AbsoluteUri );
            webBrowser1.Navigate( URL_CONTENT );
            e.Cancel = true;
        }

        private void webBrowser1_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e )
        {
            string url = webBrowser1.Url.AbsoluteUri;
            if ( url == URL_CONTENT )
            {
                GetContent();
            }
            //else
            //    MessageBox.Show( "error" );
        }

        private void GetContent()
        {
            HtmlElementCollection list = webBrowser1.Document.GetElementsByTagName( "td" );
            if ( list.Count == 0 ) return;
            HtmlElement element = list[ 0 ];
            Dictionary<string, DateTime> handling = new Dictionary<string, DateTime>();
            string[] lines = element.InnerText.Split( '\n' );
            foreach( string line in lines )
            {
                if( line.Contains( "异常交易总笔数" ) )
                {
                    HandleExTrade( line );
                }
                if( line.Contains( "异常节点个数" ) )
                {
                    HandleExNode( line );
                }
                if( line.Contains( "中心状态" ) )
                {
                    HandleStatus( line );
                }
                lblTotalAlert.Text = "0";
                if( line.Contains( "处理中" ) )
                {
                    HandleSuspends( handling, line );
                }
            }
            _handling = handling;
        }

        private void HandleSuspends( Dictionary<string, DateTime> handling, string line )
        {
            if ( _handling.ContainsKey( line ) )
            {
                DateTime dt = _handling[ line ];
                handling.Add( line, dt );
                if ( dt + TimeSpan.FromSeconds( INTERVAL ) < DateTime.Now )
                {
                    lblTotalAlert.Text = "1";
                    lblDetail.Text = "交易处理超时 [" + dt.ToString( "HHmmss" ) +
                        "]: " + line;
                }
            }
            else
                handling.Add( line, DateTime.Now );
        }

        private void HandleStatus( string line )
        {
            string[] arr = line.Split();
            if ( arr.Length > 2 )
            {
                string s = arr[ arr.Length - 1 ];
                //MessageBox.Show( "中心状态 : " + s );
                if ( s == "正常工作" )
                    lblStatus.Text = "1";
                else if ( s == "暂停对外服务" )
                    lblStatus.Text = "2";
                else
                    lblStatus.Text = "-1";
            }
        }

        private void HandleExNode( string line )
        {
            string[] arr = line.Split();
            if ( arr.Length > 5 )
            {
                int num = 0;
                if ( int.TryParse( arr[ 5 ], out num ) )
                {
                    //MessageBox.Show( "异常节点" + num.ToString() );
                    lblExNode.Text = num.ToString();
                }
            }
        }

        private void HandleExTrade( string line )
        {
            string[] arr = line.Split();
            if ( arr.Length > 1 )
            {
                int num = 0;
                string s = arr[ 1 ].Substring( 0, arr[ 1 ].Length - 7 );
                if ( int.TryParse( s, out num ) )
                {
                    //MessageBox.Show( "异常交易" + num.ToString() );
                    lblExTrade.Text = num.ToString();
                }
            }
        }

    }
}
