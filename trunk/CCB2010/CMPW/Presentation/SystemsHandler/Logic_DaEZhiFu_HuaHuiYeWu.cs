using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JOYFULL.CMPW.Digit;
using System.Threading;
using JOYFULL.CMPW.Presentation.SystemsSetting;
using JOYFULL.CMPW.Presentation.BGCapture;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished 
    /// </summary>
    class Logic_DaEZhiFu_HuaHuiYeWu:Logic_BWSystemLogic
    {
        static public string _SystemName = "大额支付系统(汇划业务)";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 3;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        public override bool EnsureSystem()
        {
            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
            //    _SystemBitMap.PixelFormat).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
            //bool recSuccessed = Digit.Digit.RecognitionWhiteText(hBmp,
            //    220, 625, 600, 660, _SystemDigitThreshHold,
            //    fileName, out txt);
            //SystemsHandler.DeleteObject(hBmp);
            //hBmp = IntPtr.Zero;
            ////ESC -- 退出：每隔 40 秒自动刷新
            //return recSuccessed && 
            //    (txt.Contains("退出") || txt.Contains("自动") || txt.Contains("刷新"));

            string txt = string.Empty;
            IntPtr hBmp = _SystemBitMap.Clone(
                new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
                _SystemBitMap.PixelFormat ).GetHbitmap();
            string fileName = SystemsHandler.GetSystemFolder( _SystemID ) + "ensure.bmp";
            // ESC -- 退出：每隔 40 秒自动刷新
            bool recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 425, 630, 453, 655, fileName,
                _SystemDigitThreshHold, true );
            recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
                JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );

            SystemsHandler.DeleteObject( hBmp );
            hBmp = IntPtr.Zero;
            return recSuccessed && ( txt == "30" || txt == "40" || txt == "42" );
        }

        public Logic_DaEZhiFu_HuaHuiYeWu()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }

        static public int _SystemDigitThreshHold = 180;
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            if(condition.Label.Contains("无变化"))
            {
                string value = GetConditionValue(_SystemID,condition);
                if (value == "") return;

                if(condition.lastChangeValue==value)
                {
                    TimeSpan time = DateTime.Now - condition.lastChangeTime;
                    long seconds = (long)time.TotalSeconds;
                    base.CheckConditionWithValue(_SystemID,condition, seconds.ToString(),true);
                }
                else
                {
                    condition.lastChangeValue = value;
                    condition.lastChangeTime = DateTime.Now;

                    log.Info(_SystemID.ToString() + "  Condition:" + condition.Label + "NewValue:" + value.ToString());
                }
            }
            else
            {
                base.CheckCondition(_SystemID,condition);
            }
        }

        SequenceTimeSpanCheck _timieRecordToDb = new SequenceTimeSpanCheck(new TimeSpan(8, 40, 0), new TimeSpan(17, 35, 0),30*60);

        public override void RecordToDb(SystemsSetting.System sys)
        {
            long checkMark = 0;
            if (!_timieRecordToDb.IsTimePointOn(DateTime.Now.TimeOfDay, out checkMark))
                return;
            DoRecord( sys );            
        }

        public override void RecordLast( SystemsSetting.System sys )
        {
            DoRecord( sys );
        }

        private void DoRecord( SystemsSetting.System sys )
        {
            Model.LargeValuePaymentSystemBiz hvps =
                new JOYFULL.CMPW.Model.LargeValuePaymentSystemBiz();
            hvps.BizDate = (int)DateTime.Today.ToOADate();
            hvps.Time = DateTime.Now;
            int tmp = 0;
            foreach ( KeyValuePair<int, Condition> pair in sys.alertConditions )
            {
                Condition c = pair.Value;
                string label = c.Label;
                if ( label.Contains( "往帐接收" ) &&
                    Int32.TryParse( c.lastCheckValue, out tmp ) )
                    hvps.ExpenditureAccept = tmp;
                else if ( label.Contains( "往帐被拒绝" ) &&
                    Int32.TryParse( c.lastCheckValue, out tmp ) )
                    hvps.ExpenditureReject = tmp;
                else if ( label.Contains( "往帐发送失败" ) &&
                    Int32.TryParse( c.lastCheckValue, out tmp ) )
                    hvps.ExpenditureFailure = tmp;
                else if ( label.Contains( "排队" ) &&
                    Int32.TryParse( c.lastCheckValue, out tmp ) )
                    hvps.Queue = tmp;
                else if ( label.Contains( "来帐接收" ) &&
                    Int32.TryParse( c.lastCheckValue, out tmp ) )
                    hvps.ReceiptAccept = tmp;
                else if ( label.Contains( "来帐被拒绝" ) &&
                    Int32.TryParse( c.lastCheckValue, out tmp ) )
                    hvps.ReceiptReject = tmp;
                else if ( label.Contains( "来帐未响应" ) &&
                    Int32.TryParse( c.lastCheckValue, out tmp ) )
                    hvps.ReceiptFailure = tmp;
            }
            DAL.LargeValuePaymentSystemBizDal dal =
                new JOYFULL.CMPW.DAL.LargeValuePaymentSystemBizDal();
            dal.AddLargeValuePaymentSystemBiz( hvps );
        }
    }
}