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
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.DAL;
using JOYFULL.CMPW.Presentation.SystemsSetting;
using JOYFUL.CMPW.Presentation;

namespace JOYFULL.CMPW.Presentation
{
    /// <summary>
    /// Config.xaml 的交互逻辑
    /// </summary>
    public partial class Config : Window
    {
        public Config( )
        {
            InitializeComponent();
        }

        XmlDocument _doc = new XmlDocument();
        XmlNode _nodeSystem;
        List<UIElement> _ctr = new List<UIElement>();
        List<Label> _lbl = new List<Label>();
        

        readonly string FILE_NAME = 
            ConfigurationManager.AppSettings[ "BizConfigFile" ];
        static readonly string TITLE_TIME_REMINDER = "重要时间点";
        Operator _operator = null;

        TimeReminder _reminder = null;
        List<ColumnDefinition> _colDefList = new List<ColumnDefinition>();
        List<TextBox> _txt = new List<TextBox>();

        private void comboBox1_Initialized( object sender, EventArgs e )
        {
            _doc.Load( FILE_NAME );
            XmlNodeList sysArray = _doc.SelectSingleNode( "Systems" ).SelectNodes( "System" );
            foreach( XmlNode sys in sysArray )
            {
                comboBox1.Items.Add( sys.Attributes[ "name" ] );
            }
            comboBox1.Items.Add( TITLE_TIME_REMINDER );
        }
        private void comboBox1_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            string title = comboBox1.SelectedValue.ToString();
            if ( title == TITLE_TIME_REMINDER )
            {
                DrawUIWithReminder();
            }
            else
            {
                foreach ( XmlNode node in _doc.SelectSingleNode( "Systems" ).SelectNodes( "System" ) )
                {
                    if ( node.Attributes[ "name" ].Value == title )
                    {
                        _nodeSystem = node;
                        DrawUI( node );
                        break;
                    }
                }
            }
            btnOK.IsEnabled = btnCancel.IsEnabled = 
                btnApply.IsEnabled = true;
        }

        private void DrawUIWithReminder()
        {
            RowDefinitionCollection rowDef = grdSetting.RowDefinitions;
            rowDef.Clear();
            var rowTitle = new RowDefinition();
            rowTitle.Height = new GridLength( 50 );
            rowDef.Add( rowTitle );
            rowTitle = new RowDefinition();
            rowTitle.Height = new GridLength( 50 );
            rowDef.Add( rowTitle );

            var colDefs = grdSetting.ColumnDefinitions;
            if( _colDefList.Count == 0 )
            {
	            foreach( ColumnDefinition item in colDefs )
	            {
	                _colDefList.Add( item );
	            }
            }
            colDefs.Clear();

            var col0 = new ColumnDefinition();
            col0.Width = new GridLength( grdSetting.Width / 5 );
            colDefs.Add( col0 );
            var col1 = new ColumnDefinition();
            col1.Width = new GridLength( grdSetting.Width * 4 / 5 );
            colDefs.Add( col1 );

            _reminder = new TimeReminder();
            List<string> timeList, descList;
            _reminder.Get( out timeList, out descList );
            grdSetting.Children.Clear();
            _txt.Clear();

            TextBlock block = new TextBlock();
            block.FontSize = 20;
            block.Text = "时间点";
            Grid.SetColumn( block, 0 );
            Grid.SetRow( block, 1 );
            grdSetting.Children.Add( block );
            block = new TextBlock();
            block.FontSize = 20;
            block.Text = "提示文字";
            Grid.SetRow( block, 1 );
            Grid.SetColumn( block, 1 );
            grdSetting.Children.Add( block );

            for( int i = 0; i < timeList.Count; ++i )
            {
                var row = new RowDefinition();
                row.Height = new GridLength( 50 );
                rowDef.Add( row );

                TextBox txt = new TextBox();
                txt.Width = 55;
                txt.Height = 23;
                txt.Text = timeList[ i ];
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow( txt, i + 2 );
                Grid.SetColumn( txt, 0 );
                _txt.Add( txt );
                grdSetting.Children.Add( txt );

                txt = new TextBox();
                txt.Width = 400;
                txt.Height = 23;
                txt.Text = descList[ i ];
                txt.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow( txt, i + 2 );
                Grid.SetColumn( txt, 1 );
                _txt.Add( txt );
                grdSetting.Children.Add( txt );
            }
        }

        private void DrawUI( XmlNode node )
        {
            _ctr.Clear();
            _lbl.Clear();
            
            // column definition has been modified within function DrawUIWithReminder
            if( _colDefList.Count != 0 ) 
            {
                grdSetting.ColumnDefinitions.Clear();
                foreach( ColumnDefinition item in _colDefList )
                {
                    grdSetting.ColumnDefinitions.Add( item );
                }
            }
            XmlNodeList list = node.SelectNodes( "Setting" );
            RowDefinitionCollection rowDef = grdSetting.RowDefinitions;
            rowDef.Clear();
            grdSetting.Children.Clear();
            bool doubleColumn = list.Count > 25;
            for (int i = 0; i < list.Count; ++i )
            {
                XmlNode n = list[i];
                RowDefinition def = new RowDefinition();
                if (doubleColumn)
                    def.Height = new GridLength(grdSetting.Height * 2 / (list.Count + 3));
                else
                    def.Height = new GridLength(grdSetting.Height / (list.Count + 1 ) );
                rowDef.Add(def);

                Label lbl = new Label();
                string content = n.Attributes["label"].Value;
                lbl.Content = content;
                lbl.Height = 30;
                if (content.Length > 15 || list.Count > 15 )
                    lbl.FontSize = 12;
                if (doubleColumn)
                {
                    Grid.SetRow(lbl, i % ((list.Count+1) / 2 ));
                    Grid.SetColumn(lbl, i >= ((list.Count + 1) / 2) ? 4 : 0);
                }
                else
                {
                    Grid.SetRow(lbl, i);
                    Grid.SetColumn(lbl, 0);
                }
                lbl.HorizontalAlignment = HorizontalAlignment.Right;
                lbl.VerticalAlignment = VerticalAlignment.Center;

                UIElement element;
                string type = n.Attributes["type"].Value;
                switch (type)
                {
                    case "TextBox":
                        TextBox txt = new TextBox();
                        txt.Text = n.InnerText;
                        txt.HorizontalAlignment = HorizontalAlignment.Left;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        txt.Height = doubleColumn ? 23 : 30;
                        txt.Width = 60;
                        element = txt;
                        break;
                    case "CheckBox":
                        CheckBox chk = new CheckBox();
                        chk.IsChecked =
                            n.InnerText == "enabled";
                        chk.HorizontalAlignment = HorizontalAlignment.Left;
                        chk.VerticalAlignment = VerticalAlignment.Center;
                        chk.Height = chk.Width = 13;
                        element = chk;
                        break;
                    case "ComboBox":
                        ComboBox cbx = new ComboBox();
                        foreach (XmlNode item in n.ChildNodes)
                        {
                            cbx.Items.Add(item.InnerText);
                        }
                        cbx.HorizontalAlignment = HorizontalAlignment.Left;
                        cbx.VerticalAlignment = VerticalAlignment.Center;
                        cbx.Height = 30; cbx.Width = 60;
                        element = cbx;
                        break;
                    default:
                        element = new Label();
                        (element as Label).Content = "Unknown type";
                        break;
                }
                if (doubleColumn)
                {
                    Grid.SetRow(element, i % ((list.Count + 1) / 2));
                    Grid.SetColumn(element, i >= ((list.Count + 1) / 2) ? 6 : 2);
                }
                else
                {
                    Grid.SetRow(element, i);
                    Grid.SetColumn(element, 2);
                }
                


                grdSetting.Children.Add(lbl);
                grdSetting.Children.Add(element);
                _lbl.Add( lbl );
                _ctr.Add(element);
            }
        }

        


        # region Click Event of Apply, OK, Cancel buttons
        private void btnOK_Click( object sender, RoutedEventArgs e )
        {
            if( Apply() )
                BackToMainPage();
        }

        private void btnApply_Click( object sender, RoutedEventArgs e )
        {
            if (Apply())
                btnApply.IsEnabled = false;
        }

        private void btnCancel_Click( object sender, RoutedEventArgs e )
        {
            BackToMainPage();
        }


        private bool ApplyRemindingSetting()
        {
            List<string> timeList = new List<string>();
            List<string> descList = new List<string>();
            for( int i = 0; i < _txt.Count; i += 2 )
            {
                timeList.Add( _txt[ i ].Text );
                descList.Add( _txt[ i + 1 ].Text );
            }
            _reminder.Save( timeList, descList );
            return true;
        }
        private bool Apply()
        {
            try
            {
                string sysName = comboBox1.SelectedValue.ToString();
                if( sysName == TITLE_TIME_REMINDER )
                    return ApplyRemindingSetting(); 

                SystemsSetting.SystemsSetting setting = new SystemsSetting.SystemsSetting();
                for ( int index = 0; index < _ctr.Count; ++index )
                {
                    UIElement e = _ctr[ index ];
                    string value = string.Empty;
                    if ( e is TextBox )
                    {
                        value = ( e as TextBox ).Text;
                    }
                    else if ( e is CheckBox )
                    {
                        value = ( e as CheckBox ).IsChecked.Value ? "enabled" : "disabled";
                    }
                    else if ( e is ComboBox )
                    {
                        value = ( e as ComboBox ).SelectedValue.ToString();
                    }
                    XmlNode node = _nodeSystem.SelectNodes( "Setting" )[ index ] as XmlNode;
                    node.InnerText = value;
                    
                    string label = _lbl[ index ].Content.ToString();
                    if( label.Contains( "开始时间" ) ||
                        label.Contains("结束时间") ||
                        label.Contains("检查时间间隔(秒)") )
                    {
                        setting.UpdateConditionPara(sysName, label, value);
                    //    setting.UpdateConditionPara(
                    //        Parameter.ParameterEnum.BeginTimePara, sysName, label, value );
                    //}
                    //else if( label.Contains( "结束时间" ) )
                    //{
                    //    setting.UpdateConditionPara(
                    //        Parameter.ParameterEnum.EndTimePara, sysName, label, value );
                    //}
                    //else if( label.Contains( "检查时间间隔(秒)" ) )
                    //{
                    //    setting.UpdateConditionPara(
                    //        Parameter.ParameterEnum.FreqPara, sysName, label, value );
                    }
                    else
                    {
                        setting.UpdateConditionValue( sysName, label, value );
                    }


                }
                _doc.Save( FILE_NAME );
            }
            catch (System.Exception e)
            {
                MessageBox.Show( "设置值有误，请重新输入" );
                return false;
            }
            foreach( UIElement item in _ctr )
            {
                item.IsEnabled = false;
            }
            return true;
        }

        private void BackToMainPage()
        {
            //////NavigationService.Navigate(
            //////    new System.Uri( "XAML/Monitor.xaml", System.UriKind.Relative ) );
            App.SwitchToWindow( "Login" );
        }
        # endregion

        private void treeView1_Initialized( object sender, EventArgs e )
        {
            InitTreeView();
        }

        private void InitTreeView()
        {
            treeView1.Items.Clear();
            Operator[] ops = new OperatorDal().GetAll();
            foreach ( Operator op in ops )
            {
                //if ( !op.IsAdmin )
                    treeView1.Items.Add( op.Name );
            }
        }

        private void btnAddOperator_Click( object sender, RoutedEventArgs e )
        {
            var frm = new FormAddOperator();
            if( frm.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                Operator op = new Operator();
                op.Name = frm.UserName;
                op.Password = frm.Password;
                op.Priority = 2;
                new OperatorDal().AddOperator(op);
                InitTreeView();
            }
        }

        private void btnChagePassword_Click( object sender, RoutedEventArgs e )
        {
            var frm = new FormChangePassword( _operator.Name, _operator.Password );
            if( frm.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                OperatorDal dal = new OperatorDal();
                var op = dal.GetOperatorByName( _operator.Name );
                op.Password = frm.Password;
                dal.UpdateOperator( op );
            }
        }

        private void btnDeleteOperator_Click( object sender, RoutedEventArgs e )
        {
            string name = _operator.Name;
            string msg = "确认删除操作员账号：" + name + "?";
            if( MessageBox.Show( msg, "请确认", MessageBoxButton.YesNo ) == MessageBoxResult.Yes )
            {
                new OperatorDal().RemoveOperatorByName( name );
                InitTreeView();
            }
        }

        private void treeView1_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e )
        {
            if (treeView1.SelectedItem == null)
                return;
            string name = treeView1.SelectedItem.ToString();
            var dal = new OperatorDal();
            var op = dal.GetOperatorByName( name );
            _operator = op;
            btnDeleteOperator.IsEnabled = !op.IsAdmin;
            btnChagePassword.IsEnabled = true;
        }

        private void treeView1_GotFocus( object sender, RoutedEventArgs e )
        {
            btnAddOperator.IsEnabled = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.KillProcess();
        }

        private void lblLeft_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void lblRight_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
