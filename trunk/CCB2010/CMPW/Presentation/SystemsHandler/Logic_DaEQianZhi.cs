using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using JOYFULL.CMPW.DAL;
using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.Digit;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished
    /// </summary>
    class Logic_DaEQianZhi:SubSystemLogic
    {
        static public string _SystemName = "大额前置系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 2;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        static public string _SystemSignContent = "收到时间信息内容";
        static public Rect _SystemSignRange = new Rect(340,120,782,150);
        static public int _SystemSignThreshHold = 128;

        //public override bool EnsureSystem()
        //{
        //    string txt = string.Empty;
        //    IntPtr hBmp = _SystemBitMap.Clone(
        //        new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
        //        _SystemBitMap.PixelFormat).GetHbitmap();
        //    string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
        //    bool recSuccessed = Digit.Digit.RecognitionBlackText(hBmp,
        //        400, 650, 838, 755, _SystemSignThreshHold,
        //        fileName, out txt);
        //    SystemsHandler.DeleteObject(hBmp);
        //    hBmp = IntPtr.Zero;
        //    //系统状态：日间处理
        //    return recSuccessed &&
        //        (txt.Contains("系统") || txt.Contains("状态"));
        //}

        static public bool CheckSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap)
        {
            //log.Info("Check System: " + _SystemID.ToString());
            string titleName = SystemsHandler.GetSystemFolder(_SystemID) + "title.tif";
            string strContent;
            if (Digit.Digit.RecognitionBlackText(hBitMap,
                _SystemSignRange.Left,
                _SystemSignRange.Top,
                _SystemSignRange.Right,
                _SystemSignRange.Bottom,
                _SystemSignThreshHold,
                titleName, out strContent))
            {
                //if (strContent.Contains(_SystemSignContent))
                if (CheckSystemSign(strContent, _SystemSignContent))
                {
                    return true;
                }
            }
            return false;
        }

     
        public Logic_DaEQianZhi()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
            m_DigitParser.IsBlackText = true;
        }

        static public int _SystemDigitThreshHold = 160;
        static public int[] grid_lines_middle = new int[] { 238, 265, 291, 320, 347, 372, 404, 433, 460, 488, 517, 544, 572 };
       
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            if (condition == null) return;
            
            string strContent;
            string labelTitleName = SystemsHandler.GetSystemFolder(_SystemID) + condition.Label + ".bmp";
            
            if (condition.Label == "登录状态退出")
            {
                if (Digit.Digit.ComplexTextParse_2(labelTitleName, 128, out strContent))
                {
                    base.CheckConditionWithValue(_SystemID, condition,strContent,true);
                }
            }
            else if (condition.Label == "后台进程异常通知")
            {
                if (Digit.Digit.ComplexTextParse_2(labelTitleName, 160, out strContent))
                {
                    base.CheckConditionWithValue(_SystemID, condition,strContent,true);
                }
            }
            else if(condition.Label == "头寸预警通知")
            {
                System.Drawing.Bitmap bmpFile = SystemsHandler.GetSystemScreen(_SystemID);
                if (bmpFile == null) return;

                IntPtr hBitMap = IntPtr.Zero;
                try
                {
                    hBitMap= bmpFile.GetHbitmap();
                    Rect range = condition.Range;
                    
                    for (int index = grid_lines_middle.Length - 1; index >= 0; --index)
                    {
                        range.Top = grid_lines_middle[index] - 15;
                        range.Bottom = grid_lines_middle[index] + 15;

                        string titleName = SystemsHandler.GetSystemFolder(_SystemID) +
                            "_" + range.Top.ToString() + "_" + range.Bottom.ToString() + ".tif";
                        string ret;
                        if( Digit.Digit.RecognitionBlackText( hBitMap, 
                            range.Left, range.Top, range.Right, range.Bottom,
                            128, titleName, out ret))//只看最后一行文字是否包含有“头寸预警通知”
                        {
                            base.CheckConditionWithValue(_SystemID,condition, ret,true);
                            break;
                        }

                    }
                    SystemsHandler.DeleteObject(hBitMap);
                    bmpFile.Dispose();
                    bmpFile = null;
                }
                catch (System.Exception e)
                {
                    if(hBitMap!=IntPtr.Zero)
                        SystemsHandler.DeleteObject(hBitMap);
                    log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                }
            }
        }
    }
}
