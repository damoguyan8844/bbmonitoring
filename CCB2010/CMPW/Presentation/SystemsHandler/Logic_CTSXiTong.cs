using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JOYFULL.CMPW.Digit;
using System.Threading;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished
    /// </summary>
    class Logic_CTSXiTong : SubSystemLogic
    {
        static public string _SystemName = "CTS系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 12;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        static public string _SystemSignContent = "0012";
        //static public Rect _SystemSignRange = new Rect(60, 80, 115, 110);
        static public Rect _SystemSignRange = new Rect(60, 145, 150, 175);
        static public int _SystemSignThreshHold = 128;
        static public Digit.Digit _DigitParser = new Digit.Digit();

        public override bool EnsureSystem()
        {
            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
            //    _SystemBitMap.PixelFormat).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
            //bool recSuccessed = Digit.Digit.RecognitionBlackText(hBmp,
            //    20, 70, 270, 175, _SystemDigitThreshHold,
            //    fileName, out txt);
            //SystemsHandler.DeleteObject(hBmp);
            //hBmp = IntPtr.Zero;
            ////安徽分行华安证券国元证券
            //return recSuccessed &&
            //    (txt.Contains("国元") || txt.Contains("证券"));

            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
            //    _SystemBitMap.PixelFormat ).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder( _SystemID ) + "ensure.bmp";
            //// 0009
            //bool recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp,
            //    _SystemSignRange.Left,
            //    _SystemSignRange.Top,
            //    _SystemSignRange.Right,
            //    _SystemSignRange.Bottom, fileName,
            //    _SystemDigitThreshHold, false );
            //recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
            //    JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );

            //SystemsHandler.DeleteObject( hBmp );
            //hBmp = IntPtr.Zero;
            //return recSuccessed && ( txt == "30" || txt == "40" );

            return true;
        }

        static public bool CheckSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap)
        {
            //log.Info("Check System: " + _SystemID.ToString());
            string titleName = SystemsHandler.GetSystemFolder(_SystemID) + "title.bmp";
            if (ANNWrapper.SaveBlockToBMP3(hBitMap,
                _SystemSignRange.Left,
                _SystemSignRange.Top,
                _SystemSignRange.Right,
                _SystemSignRange.Bottom,
                titleName))
            {
                _DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
                _DigitParser.TextThreshHold = _SystemSignThreshHold;

                //log.Info("Check System: 2 " + _SystemID.ToString());

                string strContent = "";
                ANNWrapper.BlackWhiteBMP(titleName, _SystemSignThreshHold);
                if (_DigitParser.TryParse(titleName, Digit.Digit.DigitType.DIGIT_INT, out strContent))
                {
                    if (strContent == _SystemSignContent)
                    {
                        return true;
                    }
                }
            }
            //log.Info("Check System: 3 " + _SystemID.ToString());
            return false;
        }

        public Logic_CTSXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }

        static public int _SystemDigitThreshHold = 128;

        public override void CheckCondition( SystemsSetting.Condition condition)
        {
            if (condition.Enable == false) return;
            _DigitParser.TextThreshHold = _SystemDigitThreshHold;
            
            if(condition.Label.Contains("/"))
            {
                int oldValue = condition.IntValue;
                try
                {
                    /*
                     * <9:00 , 50%
                     * 12:00-13:00 50%
                     * >15:00, 50%
                     */
                    int hour = DateTime.Now.Hour;
                    int newValue = oldValue;

                    if ((hour < 9) || (hour == 12) || (hour >= 15))
                    {
                        newValue = 50;
                    }

                    condition.IntValue = newValue;

                    if (condition.Label.Contains("安徽分行"))
                    {
                        string left = base.GetConditionValue(_SystemID, Digit.Digit.DigitType.DIGIT_INT, "安徽分行 - 失败结束笔数");
                        string right = base.GetConditionValue(_SystemID, Digit.Digit.DigitType.DIGIT_INT, "安徽分行 - 成功结束笔数");

                        int lValue = 0; int rValue = 1;
                        if (Int32.TryParse(left, out lValue) && Int32.TryParse(right, out rValue))
                        {
                            //if (lValue != 0 )
                            {
                                if (rValue == 0)
                                {
                                    lValue = 0;
                                    rValue = 1;
                                }
                                int rank = (int)(lValue * 100 / rValue);
                                base.CheckConditionWithValue(_SystemID, condition, rank.ToString(), true);
                            }
                        }
                    }
                    else if (condition.Label.Contains("华安证券"))
                    {
                        string left = base.GetConditionValue(_SystemID, Digit.Digit.DigitType.DIGIT_INT, "华安证券 - 失败结束笔数");
                        string right = base.GetConditionValue(_SystemID, Digit.Digit.DigitType.DIGIT_INT, "华安证券 - 成功结束笔数");

                        int lValue = 0; int rValue = 1;
                        if (Int32.TryParse(left, out lValue) && Int32.TryParse(right, out rValue))
                        {
                            //if (lValue != 0)
                            {
                                if (rValue == 0)
                                {
                                    lValue = 0;
                                    rValue = 1;
                                }

                                int rank = (int)(lValue * 100 / rValue);
                                base.CheckConditionWithValue(_SystemID, condition, rank.ToString(), true);
                            }
                        }
                    }
                    else if (condition.Label.Contains("国元证券"))
                    {
                        string left = base.GetConditionValue(_SystemID, Digit.Digit.DigitType.DIGIT_INT, "国元证券 - 失败结束笔数");
                        string right = base.GetConditionValue(_SystemID, Digit.Digit.DigitType.DIGIT_INT, "国元证券 - 成功结束笔数");

                        int lValue = 0; int rValue = 1;
                        if (Int32.TryParse(left, out lValue) && Int32.TryParse(right, out rValue))
                        {
                            //if (lValue != 0)
                            {
                                if (rValue == 0)
                                {
                                    lValue = 0;
                                    rValue = 1;
                                }
                                int rank = (int)(lValue * 100 / rValue);
                                base.CheckConditionWithValue(_SystemID, condition, rank.ToString(), true);
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                }
                condition.IntValue = oldValue;
            }
            else
            {
                base.CheckCondition(_SystemID, condition);
            }
        }
    }
}
