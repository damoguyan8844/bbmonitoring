using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JOYFULL.CMPW.Digit;
using System.Threading;
using JOYFULL.CMPW.Presentation.BGCapture;
using JOYFULL.CMPW.Presentation.SystemsSetting;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished
    /// </summary>
    
    class Logic_DaEZhiFu_ShiWuXinXi:SubSystemLogic
    {
        static public string _SystemName = "大额支付系统(事务信息)";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 6;
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

        public Logic_DaEZhiFu_ShiWuXinXi()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }

        static public int _SystemDigitThreshHold = 180;
        public override void CheckCondition( SystemsSetting.Condition condition)
        {
            base.CheckCondition(_SystemID,condition);
        }

        SequenceTimeSpanCheck _timieRecordToDb = new SequenceTimeSpanCheck( 
            new TimeSpan( 8, 40, 0 ), new TimeSpan( 17, 35, 0 ), 30 * 60 );

        public override void RecordToDb( JOYFULL.CMPW.Presentation.SystemsSetting.System sys )
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
            int tmp = 0;
            foreach ( KeyValuePair<int, Condition> pair in sys.alertConditions )
            {
                if ( pair.Value.Label.Contains( "未回复查询" ) &&
                    Int32.TryParse( pair.Value.lastCheckValue, out tmp ) )
                {
                    Model.UnansweredQuery uq = new JOYFULL.CMPW.Model.UnansweredQuery();
                    uq.Date = (int)DateTime.Today.ToOADate();
                    uq.Time = DateTime.Now;
                    uq.Value = tmp;

                    DAL.UnansweredQueryDAL dal = new JOYFULL.CMPW.DAL.UnansweredQueryDAL();
                    dal.Add( uq );
                    break;
                }
            }
        }
    }
}
