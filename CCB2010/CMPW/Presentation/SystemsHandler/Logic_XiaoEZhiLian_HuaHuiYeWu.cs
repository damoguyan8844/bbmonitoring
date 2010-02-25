using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JOYFULL.CMPW.Digit;
using JOYFULL.CMPW.Presentation.SystemsSetting;
using JOYFULL.CMPW.Presentation.BGCapture;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished
    /// </summary>
    class Logic_XiaoEZhiLian_HuaHuiYeWu : Logic_BWSystemLogic
    {
        static public string _SystemName = "小额直联系统(非实时业务)";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 5;
        static public System.Drawing.Bitmap _SystemBitMap = null;
        static public int _SystemDigitThreshHold = 180;

        static private readonly Rect RangeEnsure =
            new Rect(220, 625, 620, 670);

        public override bool EnsureSystem()
        {
            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone( 
            //    new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
            //    _SystemBitMap.PixelFormat).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
            //bool recSuccessed = Digit.Digit.RecognitionWhiteText(hBmp,
            //    220, 625, 620, 670, _SystemDigitThreshHold,
            //    fileName, out txt);
            //SystemsHandler.DeleteObject(hBmp);
            //hBmp = IntPtr.Zero;
            ////ESC -- 退出：每隔 30 秒自动刷新
            //return recSuccessed && 
            //    (txt.Contains("退出") || txt.Contains("自动") || txt.Contains("刷新"));

            string txt = string.Empty;
            IntPtr hBmp = _SystemBitMap.Clone(
                new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
                _SystemBitMap.PixelFormat ).GetHbitmap();
            string fileName = SystemsHandler.GetSystemFolder( _SystemID ) + "ensure.bmp";
            // ESC -- 退出：每隔 30 秒自动刷新
            bool recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 425, 630, 453, 660, fileName,
                _SystemDigitThreshHold, true );
            recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
                JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );

            SystemsHandler.DeleteObject( hBmp );
            hBmp = IntPtr.Zero;
            return recSuccessed && ( txt == "30" || txt == "40");
        }

        public Logic_XiaoEZhiLian_HuaHuiYeWu()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }


        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            if (condition.Label.Contains("无变化"))
            {
                string value = GetConditionValue(_SystemID, condition);
                if (value == "") return;

                if (condition.lastChangeValue == value)
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
                base.CheckCondition(_SystemID, condition);
        }


        SequenceTimeSpanCheck _timieRecordToDb = new SequenceTimeSpanCheck(new TimeSpan(8, 0, 0), new TimeSpan(18, 05, 0), 30 * 60);

        public override void RecordToDb(SystemsSetting.System sys)
        {
            long checkMark = 0;
            if (!_timieRecordToDb.IsTimePointOn(DateTime.Now.TimeOfDay, out checkMark))
                return;

            Model.SmallValuePaymentSystemBiz svps =
                new JOYFULL.CMPW.Model.SmallValuePaymentSystemBiz();
            svps.BizDate = (int)DateTime.Today.ToOADate();
            svps.Time = DateTime.Now;

            int tmp = 0;
            foreach (KeyValuePair<int, Condition> pair in sys.alertConditions)
            {
                Condition c = pair.Value;
                string label = c.Label;
                if (label.Contains("往帐接收") &&
                    Int32.TryParse(c.lastCheckValue, out tmp))
                    svps.ExpenditureAccept = tmp;
                else if (label.Contains("往账异常") &&
                    Int32.TryParse(c.lastCheckValue, out tmp))
                    svps.ExpenditureException = tmp;
                else if (label.Contains("已发包数") &&
                    Int32.TryParse(c.lastCheckValue, out tmp))
                    svps.SentPackageCount = tmp;
                else if (label.Contains("来账接收") &&
                    Int32.TryParse(c.lastCheckValue, out tmp))
                    svps.ReceiptAccept = tmp;
                else if (label.Contains("来账异常") &&
                    Int32.TryParse(c.lastCheckValue, out tmp))
                    svps.ReceiptException = tmp;
                else if (label.Contains("来账已发") &&
                    Int32.TryParse(c.lastCheckValue, out tmp))
                    svps.ReceiptPackageCount = tmp;
                tmp = 0;
            }
            DAL.SmallValuePaymentSystemBizDal dal =
                new JOYFULL.CMPW.DAL.SmallValuePaymentSystemBizDal();
            dal.AddSmallValuePaymentSystemBiz(svps);
        }
    }
}
