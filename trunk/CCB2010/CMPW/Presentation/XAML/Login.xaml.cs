using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.DAL;

namespace JOYFULL.CMPW.Presentation
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Operator CurrentOperator { get; set; }

        public Login( )
        {
            InitializeComponent();
            Operator[] ops = new OperatorDal().GetAll();
            foreach( Operator op in ops )
            {
                cbxUser.Items.Add( op.Name );
            }
            cbxUser.SelectedIndex = 0;
            EventManager.RegisterClassHandler( typeof( Window ),
                Keyboard.KeyUpEvent, new KeyEventHandler( Page_KeyUp ), true );
            EventManager.RegisterClassHandler( typeof( Window ),
                Keyboard.KeyDownEvent, new KeyEventHandler( Page_KeyDown ), true );
            lblWarning.Visibility = Visibility.Hidden;
            txtPswd.Focus();
            _fwWarning = new FadeWrapper( this, lblWarning );
        }

        private bool _enterPressed = false;
        private FadeWrapper _fwWarning;


        private void btnLogin_Click( object sender, RoutedEventArgs e )
        {
            CheckLogin();
        }
        private void CheckLogin()
        {
            string user = cbxUser.SelectedValue.ToString();
            string pswd = txtPswd.Password;
            Operator oper = new OperatorDal().Validate( user, pswd );
            if( oper == null )
            {
                txtPswd.SelectAll();
                _fwWarning.Fadeout( 1000, 3000 );          
            }
            else
            {
                CurrentOperator = oper;
                DoLogin( oper );
            }
        }
        private void DoLogin( Operator oper )
        {
            if (oper.IsAdmin)
            {
                App.SwitchToWindow("Config");
            }
            else
            {
                var mj = new Model.MonitorJob();
                mj.OperatorID = oper.ID;
                mj.TaskDate = (int)Math.Floor(DateTime.Today.ToOADate());
                mj.StartTime = DateTime.Now;

                DAL.MonitorJobDal motdal = new DAL.MonitorJobDal();
                motdal.AddMonitorJob(mj);

                App.MtJob = mj;
                App.SwitchToWindow("Monitor");
            }
            txtPswd.Password = string.Empty;

        }

        private void Page_KeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter )
            {
                _enterPressed = true;
            }
        }

        private void Page_KeyUp( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter && _enterPressed )
            {
                CheckLogin();
            }
            _enterPressed = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.LogOutOperator();
            App.KillProcess();
        }
    }
}
