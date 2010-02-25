using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Browser
{
    public partial class FormCTS : Form
    {
        const string URL_LOGIN_NEW = "http://login.management.ccb:8100/siteminderagent/forms/login.fcc?TYPE=33554432&REALMOID=06-0004216f-29f7-1b4c-87f7-d4285baa77b7&GUID=&SMAUTHREASON=0&METHOD=GET&SMAGENTNAME=-SM-jzmvI6dWEsqUxjZQWTp8jPmaFVxA7KUn1vMThL%2fvgJ0FMtdZCGtt%2fiW6K4ziKUx7&TARGET=-SM-http%3a%2f%2fcts%2emanagement%2eccb%3a8011%2f";
        const string URL_HOST = "http://11.156.32.24:8011";
        //const string URL_SELECT = "http://11.156.32.24:8011/aspshare/cts08/CTS7808000_00.asp?";
        const string URL_SELECT = "http://cts.management.ccb:8011/aspshare/cts08/CTS7808000_00.asp?";
        const string URL_BANK =        "http://cts.management.ccb:8011/aspshare/cts08/CTS7808000_00ok.asp";
        const string URL_STOCKJOBBER = "http://cts.management.ccb:8011/aspshare/cts08/CTS7808000_00ok.asp";
        const string URL_FRAMESET = "http://cts.management.ccb:8011/frameset1.asp";

        const string URL_CONTENT = "http://11.156.32.24:8011/aspshare/cts08/CTS7808000_00ok.asp";
        const string URL_LOGIN = "http://11.156.32.24:8011/login.asp";
        //const string URL_FRAMESET = "http://11.156.32.24:8011/frameset1.asp";

        static readonly int INTERVAL = 5 * 60 * 1000;

        System.Threading.Timer _timer = null; 

        private bool _bBank = true;

        public FormCTS()
        {
            InitializeComponent();
            //webBrowser1.Navigate( URL_HOST );
            webBrowser1.Navigate( URL_LOGIN_NEW );
        }

        private void webBrowser1_DocumentCompleted( object sender, WebBrowserDocumentCompletedEventArgs e )
        {
            string url = webBrowser1.Url.AbsoluteUri;
            switch ( url )
            {
                case URL_LOGIN:
                    DoLogin();
                    break;
                case URL_LOGIN_NEW:
                    DoLoginNew();
                    break;
                case URL_FRAMESET:
                    StartGetContent();
                    break;
                case URL_SELECT:
                    DoSelect();
                    break;
                //case URL_CONTENT:
                default:
                    GetContent();
                    break;
            }
        }

        private void DoLoginNew()
        {
            //HtmlElementCollection list =
            //    webBrowser1.Document.GetElementsByTagName( "input" );
            //foreach ( HtmlElement element in list )
            //{
            //    string name = element.GetAttribute( "name" );
            //    if ( name == "userid" )
            //        element.SetAttribute( "value", "0340000000000000" );
            //    else if ( name == "tellerid" )
            //        element.SetAttribute( "value", "340003" );
            //    //else if ( name == "Password" )
            //    //    element.SetAttribute( "value", "123456" );
            //    //else if ( name == "btnOK" )
            //    //    element.InvokeMember( "Click" );
            //}

            HtmlElementCollection list =
                webBrowser1.Document.GetElementsByTagName( "select" );
            foreach ( HtmlElement element in list )
            {
                string name = element.GetAttribute( "name" );
                if ( name == "auth" )
                {
                    element.SetAttribute( "selectedIndex", "11010001" );
                    break;
                }
            }
        }

        private void StartGetContent()
        {
            _timer = new System.Threading.Timer(
                new TimerCallback( NavigateSelect ), null, 0, INTERVAL );
            
        }

        private void NavigateSelect( object o )
        {
            webBrowser1.Navigate( URL_SELECT );
        }

        private void DoLogin()
        {
            HtmlElementCollection list =
                webBrowser1.Document.GetElementsByTagName( "input" );
            foreach ( HtmlElement element in list )
            {
                string name = element.GetAttribute( "name" );
                if ( name == "userid" )
                    element.SetAttribute( "value", "0340000000000000" );
                else if ( name == "tellerid" )
                    element.SetAttribute( "value", "340003" );
                //else if ( name == "Password" )
                //    element.SetAttribute( "value", "123456" );
                //else if ( name == "btnOK" )
                //    element.InvokeMember( "Click" );
            }

        }
        private void DoSelect()
        {
            HtmlElementCollection coll =
                webBrowser1.Document.GetElementsByTagName( "input" );
            bool begin = false;
            bool end = false;
            foreach ( HtmlElement element in coll )
            {
                string name = element.GetAttribute( "name" );
                if ( name == "BeginTime" )
                {
                    DateTime dt0 = DateTime.Now.AddMinutes( -30 );
                    string s = dt0.ToString( "HHmmss" );
                    element.SetAttribute( "value", s );
                    begin = true;
                }
                else if ( name == "EndTime" )
                {
                    string s = DateTime.Now.ToString( "HHmmss" );
                    element.SetAttribute( "value", s );
                    end = true;
                }
                if ( begin && end ) break;
            }

            if ( !_bBank )
            {
                coll = webBrowser1.Document.GetElementsByTagName( "select" );
                foreach ( HtmlElement element in coll )
                {
                    string name = element.GetAttribute( "name" );
                    if ( name == "StatisticFlag" )
                    {
                        //string index = _bBank ? "0" : "1";
                        element.SetAttribute( "selectedIndex", "1" );
                        //MessageBox.Show( "stock" );
                        break;
                    }
                }
            }
            //else
            //    MessageBox.Show( "bank" );
            HtmlElement btn = webBrowser1.Document.GetElementById( "button1" );
            btn.InvokeMember( "Click" );

            _bBank = !_bBank;
        }

        void GetContent()
        {
            int handling = 0;
            int failed = 0;
            int successed = 0;

            int haHandling = 0;
            int haFailed = 0;
            int haSuccessed = 0;

            int gyHandling = 0;
            int gyFailed = 0;
            int gySuccessed = 0;

            bool bStock = false;
            bool bBank = false;

            HtmlElementCollection list = webBrowser1.Document.GetElementsByTagName( "td" );
            if ( list.Count == 0 ) return;
            //System.IO.StreamWriter sw = new System.IO.StreamWriter( "C:/content.txt", true );
            //sw.Write( list[ 0 ].InnerText );
            //sw.Close();
            //return;

            string content = list[ 0 ].InnerText;
            string[] lines = content.Split( '\n' );
            //MessageBox.Show( lines.Length.ToString() );
            foreach( string line in lines )
            {
                //if( line.Trim().StartsWith( "065" ) ) //华安证券
                if ( line.Contains( "华安证券" ) ) //华安证券
                {
                    bStock = true;
                    string[] arr = line.Split();
                    
                    if ( arr.Length < 5 ) continue;
                    //MessageBox.Show( arr[ 4 ] );
                    int num = 0;
                    int.TryParse( arr[ 4 ], out num );
                    if ( line.Contains( "正常处理中" ) )
                        haHandling = num;
                    else if ( line.Contains( "失败结束" ) )
                        haFailed = num;
                    else if ( line.Contains( "成功结束" ) )
                        haSuccessed = num;
                }
                //else if ( line.Trim().StartsWith( "083" ) ) //国元证券
                else if ( line.Contains( "国元证券" ) ) //国元证券
                {
                    bStock = true;
                    string[] arr = line.Split();
                    if ( arr.Length < 5 ) continue;
                    //MessageBox.Show( arr[ 4 ] );
                    int num = 0;
                    int.TryParse( arr[ 4 ], out num );
                    if ( line.Contains( "正常处理中" ) )
                        gyHandling = num;
                    else if ( line.Contains( "失败结束" ) )
                        gyFailed = num;
                    else if ( line.Contains( "成功结束" ) )
                        gySuccessed = num;
                }

                //if( line.Trim().StartsWith( "0340" ) ) //安徽
                if ( line.Contains( "安徽" ) ) //安徽
                {
                    bBank = true;
                    string[] arr = line.Split( );
                    if ( arr.Length < 5 ) continue;
                    //MessageBox.Show( arr[ 4 ] );
                    int num = 0;
                    int.TryParse( arr[ 4 ], out num );
                    if ( line.Contains( "正常处理中" ) )
                        handling = num;
                    else if ( line.Contains( "失败结束" ) )
                        failed = num;
                    else if ( line.Contains( "成功结束" ) )
                        successed = num;
                }
            }

            if( bStock )
            {
                lblHAHandling.Text = haHandling.ToString();
                lblHAFailed.Text = haFailed.ToString();
                lblHASuccessed.Text = haSuccessed.ToString();

                lblGYHandling.Text = gyHandling.ToString();
                lblGYFailed.Text = gyFailed.ToString();
                lblGYSuccessed.Text = gySuccessed.ToString();

                lblTime.Text = DateTime.Now.ToString( "HH:mm:ss" );
                lblNextTime.Text =
                    DateTime.Now.AddMilliseconds( INTERVAL ).ToString( "HH:mm:ss" );
            }
            else if( bBank )
            {
                lblHandling.Text = handling.ToString();
                lblFailed.Text = failed.ToString();
                lblSuccessed.Text = successed.ToString();

                lblTime.Text = DateTime.Now.ToString( "HH:mm:ss" );
                lblNextTime.Text =
                    DateTime.Now.AddMilliseconds( INTERVAL ).ToString( "HH:mm:ss" );
                webBrowser1.Navigate( URL_SELECT );
            }
            
        }
    }
}
