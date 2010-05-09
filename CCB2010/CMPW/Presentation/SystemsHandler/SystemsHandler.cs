using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Threading;
using System.IO;

using JOYFULL.CMPW.Model;
using System.Windows.Forms;
using JOYFULL.CMPW.Digit;
using log4net;
using System.Drawing;
using System.Runtime.InteropServices;
using JOYFULL.CMPW.Monitor;
using JOYFULL.CMPW.Alert;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    public class SystemsHandler
    {
        public static object _syncBitmap_Lock = new object();

        private static LogCallbackDelegate _logHandler = null;
        private static TCApp.TCSDK_LogDelegate _tcsdk_LogHandler = null;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        private static readonly ILog log = LogManager.GetLogger(typeof(SystemsHandler));
        private static readonly ILog logANN = LogManager.GetLogger(typeof(ANNWrapper));
        private static readonly ILog logTCSDK = LogManager.GetLogger(typeof(TCApp.TCSDKWrapper));

        public readonly static string IMAGE_FOLDER = ConfigurationManager.AppSettings["ImageFolder"];
        public readonly static string IMAGE_SHARED = ConfigurationManager.AppSettings["ImageShared"];
        public readonly static string PARAs_FOLDER = ConfigurationManager.AppSettings["ParasFolder"];
        public readonly static string ERRORREC_FOLDER = ConfigurationManager.AppSettings["RecErrorFolder"];
        public readonly static string ALERT_FOLDER = ConfigurationManager.AppSettings["AlertFolder"];
        
        public readonly static int m_channelID = Int32.Parse(ConfigurationManager.AppSettings["ChannelID"]);
        
        static public int INVALID_SYSTEMID = -1;
        static public SystemsSetting.SystemsSetting m_settings = new SystemsSetting.SystemsSetting();
        static public Dictionary<int, string> g_subsystemFolders = new Dictionary<int, string>();
        static public Dictionary<int, SubSystemMonitorJob> g_subsystemMonitorJobs = new Dictionary<int, SubSystemMonitorJob>();
        static public Dictionary<int, DateTime> g_subsystemSignalTime= new Dictionary<int,DateTime>();
        static public Dictionary<int, DateTime> g_subSystemCheckTime = new Dictionary<int, DateTime>();

        public Dictionary<int, SubSystemHandler> m_handlers = new Dictionary<int,SubSystemHandler>();
        public DAL.SubSystemDal m_dal = new DAL.SubSystemDal();
        public BGCapture.Capturer m_capture = new BGCapture.Capturer(IMAGE_SHARED, m_channelID);

        public Model.Operator m_Operator = null;
        public Model.MonitorJob m_MonitorJob = null;

        private Thread m_handlerThread = null;

        private static List<Bitmap> s_bmpList = new List<Bitmap>(15);
        private static List<string> s_sysInfo = new List<string>(15);

        private AlarmBuzzer altRemindingBuzzer = new AlarmBuzzer();
      
        public SystemsHandler(List<int> sysIDs)
        {
            if( s_bmpList.Count == 0 )
            {
                for (int i = 0; i < 15; ++i)
                {
                    s_bmpList.Add(null);
                    s_sysInfo.Add("");
                }
            }
            if (_logHandler == null)
            {
                _logHandler = new LogCallbackDelegate(ANNRecognitionLog);
                ANNWrapper.SetLogHandler(_logHandler);
            }

            if (_tcsdk_LogHandler == null)
            {
                _tcsdk_LogHandler = new TCApp.TCSDK_LogDelegate(TCSDKLog);
                TCApp.TCSDKWrapper.SetLogHandler(_tcsdk_LogHandler);
            } 

            m_handlers.Clear();
            
            DAL.SubSystemMonitorJobDal dal = new DAL.SubSystemMonitorJobDal();
                    
            foreach (int sysID in sysIDs)
            {
                SystemsSetting.System setting = m_settings.systems[sysID];
                Model.SubSystem dbSubSystem = m_dal.GetSubSystemById(sysID);

                SubSystemLogic logic = null;
                string logicName;
                if (sysID == Logic_ZhongKeXiTong._SystemID)
                {
                    logic = new Logic_ZhongKeXiTong();
                    logicName = Logic_ZhongKeXiTong._SystemName;

                    Logic_ZhongKeXiTong._SystemSignContent = setting.Sign;
                }
                else if (sysID == Logic_ZhengQuanXiTong._SystemID)
                {
                    logic = new Logic_ZhengQuanXiTong();
                    logicName = Logic_ZhengQuanXiTong._SystemName;

                    Logic_ZhengQuanXiTong._SystemSignContent = setting.Sign;
                }
                else if (sysID == Logic_QingSuanXiTong._SystemID)
                {
                    logic = new Logic_QingSuanXiTong();
                    logicName = Logic_QingSuanXiTong._SystemName;

                    Logic_BWSystemLogic._SystemSignContent_10 = setting.Sign;

                    if (dal.GetByDateAndSystemId(DateTime.Today, sysID) != null)
                    {
                        Logic_QingSuanXiTong._IsSignedIn = true;
                    }
                }
                else if (sysID == Logic_QingSuanZhiLianXiTong._SystemID)
                {
                    logic = new Logic_QingSuanZhiLianXiTong();
                    logicName = Logic_QingSuanZhiLianXiTong._SystemName;
                    
                    Logic_BWSystemLogic._SystemSignContent_11 = setting.Sign;
                    if (dal.GetByDateAndSystemId(DateTime.Today, sysID) != null)
                    {
                        Logic_QingSuanXiTong._IsSignedIn = true;
                    }
                }
                else if (sysID == Logic_DaEZhiFu_HuaHuiYeWu._SystemID)
                {
                    logic = new Logic_DaEZhiFu_HuaHuiYeWu();
                    logicName = Logic_DaEZhiFu_HuaHuiYeWu._SystemName;

                    Logic_BWSystemLogic._SystemSignContent_3 = setting.Sign;
                }
                else if (sysID == Logic_DaEZhiFu_ShiWuXinXi._SystemID)
                {
                    logic = new Logic_DaEZhiFu_ShiWuXinXi();
                    logicName = Logic_DaEZhiFu_ShiWuXinXi._SystemName;

                    Logic_BWSystemLogic._SystemSignContent_6 = setting.Sign;
                }
                else if (sysID == Logic_DaEQianZhi._SystemID)
                {
                    logic = new Logic_DaEQianZhi();
                    logicName = Logic_DaEQianZhi._SystemName;

                    Logic_DaEQianZhi._SystemSignContent = setting.Sign;
                }
                else if (sysID == Logic_XiaoEZhiLian_HuaHuiYeWu._SystemID)
                {
                    logic = new Logic_XiaoEZhiLian_HuaHuiYeWu();
                    logicName = Logic_XiaoEZhiLian_HuaHuiYeWu._SystemName;

                    Logic_BWSystemLogic._SystemSignContent_5 = setting.Sign;
                }
                else if (sysID == Logic_XiaoEZhiLian_ShiWuXinXi._SystemID)
                {
                    logic = new Logic_XiaoEZhiLian_ShiWuXinXi();
                    logicName = Logic_XiaoEZhiLian_ShiWuXinXi._SystemName;

                    Logic_BWSystemLogic._SystemSignContent_7 = setting.Sign;
                }
                else if (sysID == Logic_XiaoEQianZhiXiTong._SystemID)
                {
                    logic = new Logic_XiaoEQianZhiXiTong();
                    logicName = Logic_XiaoEQianZhiXiTong._SystemName;

                    Logic_XiaoEQianZhiXiTong._SystemSignContent = setting.Sign;
                }
                else if (sysID == Logic_ZhiPiaoYingXiangXiTong._SystemID)
                {
                    logic = new Logic_ZhiPiaoYingXiangXiTong();
                    logicName = Logic_ZhiPiaoYingXiangXiTong._SystemName;

                    Logic_ZhiPiaoYingXiangXiTong._SystemSignContent = setting.Sign;
                }
                else if (sysID == Logic_YinBaoTongXiTong._SystemID)
                {
                    logic = new Logic_YinBaoTongXiTong();
                    logicName = Logic_YinBaoTongXiTong._SystemName;

                    Logic_YinBaoTongXiTong._SystemSignContent = setting.Sign;
                }
                else if (sysID == Logic_CTSXiTong._SystemID)
                {
                    logic = new Logic_CTSXiTong();
                    logicName = Logic_CTSXiTong._SystemName;

                    Logic_CTSXiTong._SystemSignContent = setting.Sign;
                }
                else if (sysID == Logic_CCBSZhongDuanXiTong._SystemID)
                {
                    logic = new Logic_CCBSZhongDuanXiTong();
                    logicName = Logic_CCBSZhongDuanXiTong._SystemName;

                    Logic_BWSystemLogic._SystemSignContent_1 = setting.Sign;
                }
                else
                {
                    MessageBox.Show("Invalid SystemID:" + sysID.ToString());
                    continue;
                }

                string toPath;
                if (!g_subsystemFolders.ContainsKey(sysID))
                {
                    toPath = IMAGE_FOLDER + logicName + "\\";
                    g_subsystemFolders.Add(sysID, toPath);
                }
                else
                {
                    toPath = g_subsystemFolders[sysID];
                }

                DirectoryInfo di = new DirectoryInfo(toPath);
                if (!di.Exists)
                {
                    di.Create();
                }
                
                m_handlers.Add(sysID, new SubSystemHandler(logic, setting, dbSubSystem));
            }
            
            DirectoryInfo unsortedDIR = new DirectoryInfo(IMAGE_FOLDER + "UnSorted" + "\\");
            if (!unsortedDIR.Exists)
            {
                unsortedDIR.Create();
            }
            if (!g_subsystemFolders.ContainsKey(INVALID_SYSTEMID))
                g_subsystemFolders.Add(INVALID_SYSTEMID, IMAGE_FOLDER + "UnSorted" + "\\");

            ANNWrapper.SetErrorRecordFolder(ERRORREC_FOLDER);

            m_handlerThread = new Thread(new ThreadStart(HandlerMain));
            m_handlerThread.Priority = ThreadPriority.AboveNormal;
            m_handlerThread.Start();
        }

        public int SubSystemCount 
        {
            get { return m_handlers.Count; }
        }

        public void StartMonitor(int sysID)
        {
            if (m_Operator == null) return;
            if (m_MonitorJob == null) return;

            SubSystemHandler handler = m_handlers[sysID];
            if (handler != null)
            {
                DAL.MonitorJobDal mJobDal = new JOYFULL.CMPW.DAL.MonitorJobDal();

                MonitorJob job= mJobDal.GetMonitorJobByDate(m_Operator.ID,DateTime.Today);
                if(job==null)
                {
                    job = new MonitorJob();
                    job.OperatorID=m_Operator.ID;
                    job.StartTime = DateTime.Now;
                    job.TaskDate = (int)DateTime.Now.ToOADate();
                   
                    mJobDal.AddMonitorJob(job);
                }
                handler.IsEnable = true;

            }
        }

        public void StopMonitor(int sysID, bool endOfDay )
        {
            if (m_handlers.ContainsKey(sysID))
            {
                m_handlers[sysID].IsEnable = false;
                
                if( endOfDay )
                {
                    CheckOutSubSystem(sysID);
                    m_handlers[ sysID ].RecordLast();
                }
            }
        }



        /// <summary>
        /// 记录Check成功过的系统，只有在检测时间内，并且有Condition检测过才包含
        /// </summary>
        private static List<int> g_realCheckedSysID = new List<int>();

        public List<int> GetMonitoringIdList( int idCenter, int length )
        {
            if (length % 2 == 0)
                return null;
            //if (!g_checkedInSystem.Contains(idCenter) ||
            //    !m_handlers[idCenter].IsEnable)
            //    return null;

            
            List<int> list = new List<int>();
            for (int index = 1; index <= 14; index ++ )
            {
                if (g_checkedInSystem.Contains(index) &&
                    m_handlers.ContainsKey(index) &&
                    m_handlers[index].IsEnable &&
                    g_realCheckedSysID.Contains(index))
                {
                    list.Add( index);
                }
            }
            if (list.Count == 0) return null;
            if (!list.Contains(idCenter))
                idCenter = list[0];//设为优先级最高的已签到系统

            List<int> listSets = new List<int>();
            for (int i = 0; i < length; ++i )
                listSets.AddRange(list);
           
            List<int> arr = new List<int>();

            int indexCenter = listSets.LastIndexOf(idCenter);
            for (int i = 0; i < length/2 ; ++i )
            {
                //arr[i]=list[indexCenter - length / 2 +i];
                arr.Add(listSets[indexCenter - length / 2 + i]);
            }

            //arr[length / 2] = idCenter;
            arr.Add(idCenter);

            int findexCenter = listSets.IndexOf(idCenter);
            for (int i = 0; i < length / 2; ++i)
            {
                //arr[i + length / 2 + 1] = list[findexCenter + i];
                arr.Add(listSets[findexCenter + 1 + i]);
            }
            return arr;
        }

        private void CheckOutSubSystem(int sysID)
        {
            //var smj = m_handlers[sysID].SystemMonitorJob;
            DAL.SubSystemMonitorJobDal dal = new DAL.SubSystemMonitorJobDal();
            var smj = dal.GetByDateAndSystemId(
                DateTime.Today, sysID);
            if( smj != null )
            {
	            smj.SignedOutTime = DateTime.Now;
	            dal.UpdateSubSystemMonitorJob(smj);
            }
            else //从未取到过图像
            {
                smj = new SubSystemMonitorJob();
                smj.SysytemID = sysID;
                smj.SignedInTime = smj.SignedOutTime = DateTime.Now;
                dal.AddSubSystemMonitorJob(smj);
            }
        }
      
        public string GetSystemInfo(int sysID)
        {
            if(! m_handlers.ContainsKey(sysID))
                return string.Empty;
            return m_handlers[sysID].GetSystemInfo();
        }

        public string GetSystemLastCheckInfo(int sysID)
        {
            try
            {
                lock (__syncImage_Lock)
                {
                    return s_sysInfo[sysID];
                }
            }
            catch(Exception e)
            {
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }
            return "";
        }
        public string GetExceptionDesc( int sysID )
        {
            if (!m_handlers.ContainsKey(sysID))
                return string.Empty;

            return m_handlers[sysID].ExceptionDesc;
        }

        public bool IsSystemHasException(int sysID)
        {
            if (!m_handlers.ContainsKey(sysID))
                return false;

            return m_handlers[sysID].HasException();
        }

        public bool IsRiZhongException(int sysID)
        {
            if (!m_handlers.ContainsKey(sysID))
                return false;

            return m_handlers[sysID].IsRiZhongException();
        }

        public int GetFirstExceptionSystemID()
        {
            foreach (KeyValuePair<int, SubSystemHandler> pair in m_handlers)
            {
                if (pair.Value.IsEnable && 
                    ( pair.Value.HasException() || pair.Value.IsRiZhongException() ) )
                    return pair.Key;
            }

            return -1;
        }

        public int GetAlertType( int sysID )
        {
            if( !m_handlers.ContainsKey( sysID ) )
                return 0;
            return m_handlers[sysID].GetAlertType();
        }

        public void LanchAlert(int sysID)
        {
            if(! m_handlers.ContainsKey(sysID))
                return ;

            m_handlers[sysID].LanchAlert();
        }

        public void LanchAlertWithType( int sysID, int alertType )
        {
            if ( !m_handlers.ContainsKey( sysID ) )
                return;
            if ( alertType == 3 )
                alertType = 300 + sysID;
            else if ( alertType == 1 )
                alertType = 100 + sysID;
            m_handlers[ sysID ].LanchAlert( alertType );
        }

        public void StopAlarm(int sysID)
        {
            if (!m_handlers.ContainsKey(sysID))
                return;

            m_handlers[sysID].StopAlert();
        }

        private static int _processingSystemID=INVALID_SYSTEMID;

        private void HandlerMain()
        {          
            
            log.Info("HandlerMain Thread ID:" + GetCurrentThreadId().ToString());
            Bitmap bmpOrig = null;
            while (true)
            {
                int miliSeconds = 100;
                _processingSystemID = INVALID_SYSTEMID;
                try
                {  
                    foreach (KeyValuePair<int, SubSystemHandler> handlerPair in m_handlers)
                    {
                        if (!g_subsystemSignalTime.ContainsKey(handlerPair.Key)) continue;
                        if (!g_checkedInSystem.Contains(handlerPair.Key)) continue;
                        if (!g_updatedSystem.ContainsKey(handlerPair.Key))continue;
                        if (!g_updatedSystem[handlerPair.Key]) continue;

                        TimeSpan now = DateTime.Now.TimeOfDay;
                        long nowCheckMark=0;
                        if (!handlerPair.Value.m_sysTimeCheck.IsTimePointOn(now, out nowCheckMark))
                            continue;
                        
                        int sysID = handlerPair.Key;
                        _processingSystemID = sysID;

                        log.Info("Preprocessing System :" + sysID.ToString());
                        PreprocessThread preprocess = new PreprocessThread(sysID);
                        preprocess.PreprocessMain();
                        preprocess = null;

                        log.Info("Checking System :" + sysID.ToString());

                        
                        bmpOrig = GetSystemScreen(sysID);
                        if (handlerPair.Value.CheckSubsystem())
                        {                          
                            if (sysID != INVALID_SYSTEMID)
                            {
                                if (g_subSystemCheckTime.ContainsKey(sysID))
                                    g_subSystemCheckTime[sysID] = DateTime.Now;
                                else
                                    g_subSystemCheckTime.Add(sysID, DateTime.Now);
                            }

                            lock(__syncImage_Lock)
                            {
                                 Bitmap bmpOld=s_bmpList[sysID];
                                 if (bmpOld != null)
                                 {
                                     bmpOld.Dispose();
                                     bmpOld = null;
                                 }
                                 s_bmpList[sysID] = bmpOrig;
                                 s_sysInfo[sysID] = GetSystemInfo(sysID);
                            }

                            if (!g_realCheckedSysID.Contains(sysID))
                                g_realCheckedSysID.Add(sysID);

                            g_updatedSystem[handlerPair.Key] = false;
                        }
                        else
                        {
                            if(bmpOrig!=null)
                            {
                                bmpOrig.Dispose();
                                bmpOrig = null;
                            }
                            
                            g_updatedSystem[handlerPair.Key] = false;
                        }

                        log.Info("End Checking System :" + handlerPair.Key.ToString());

                        //UpdateSystemBitmap(sysID, s_bmpList[sysID]);//识别数据后再更新系统静态图像
                        //log.Error("End Check System " + handlerPair.Key.ToString().ToString());
                    }
                }
                catch (System.Exception e)
                {
                    if (bmpOrig != null)
                    {
                        bmpOrig.Dispose();
                        bmpOrig = null;
                    }
                    log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                }
                _processingSystemID = INVALID_SYSTEMID;
                Thread.Sleep(miliSeconds);
            }
        }
        public static string GetSystemName( int sysId )
        {
            return m_settings.systems[ sysId ].Name;
        }

        public static int GetSystemId( string name )
        {
            foreach( var pair in m_settings.systems )
            {
                var system = pair.Value;
                if ( system.Name.Contains( name ) )
                    return pair.Key;
            }
            return -1;
        }

        public static string GetSystemFolder(int sysID)
        {
            if (g_subsystemFolders.ContainsKey(sysID))
                return g_subsystemFolders[sysID];

            string toPath = IMAGE_FOLDER + sysID.ToString() + "\\";
            g_subsystemFolders.Add(sysID, toPath);

            DirectoryInfo di = new DirectoryInfo(toPath);
            if (!di.Exists)
            {
                di.Create();
            }
            return toPath;
        }
        
        public static List<int> GetExpiredSystems(long timeSeconds)
        {
            DateTime timenow = DateTime.Now;
            List<int> listSysID = new List<int>();
            foreach (int sysID in g_checkedInSystem)
            {
                if (g_subsystemSignalTime.ContainsKey(sysID))
                {
                    TimeSpan tspan = timenow - g_subsystemSignalTime[sysID];
                    if (tspan.TotalSeconds > timeSeconds)
                        listSysID.Add(sysID);
                }
            }

            return listSysID;
        }
        public static int GetSystemDigitThreshHold(int sysID)
        {
            int digitThreshHold = 180;
            if (Logic_ZhongKeXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_ZhongKeXiTong._SystemDigitThreshHold;
            }
            else if (Logic_ZhengQuanXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_ZhengQuanXiTong._SystemDigitThreshHold;
            }
            else if (Logic_QingSuanXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_QingSuanXiTong._SystemDigitThreshHold;
            }
            else if (Logic_QingSuanZhiLianXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_QingSuanZhiLianXiTong._SystemDigitThreshHold;
            }
            else if (Logic_DaEZhiFu_HuaHuiYeWu._SystemID==sysID)
            {
                digitThreshHold = Logic_DaEZhiFu_HuaHuiYeWu._SystemDigitThreshHold;
            }
            else if (Logic_DaEZhiFu_ShiWuXinXi._SystemID==sysID)
            {
                digitThreshHold = Logic_DaEZhiFu_ShiWuXinXi._SystemDigitThreshHold;
                
            }
            else if (Logic_DaEQianZhi._SystemID==sysID)
            {
                digitThreshHold = Logic_DaEQianZhi._SystemDigitThreshHold;
            }
            else if (Logic_XiaoEZhiLian_HuaHuiYeWu._SystemID==sysID)
            {
                digitThreshHold = Logic_XiaoEZhiLian_HuaHuiYeWu._SystemDigitThreshHold;
            }
            else if (Logic_XiaoEZhiLian_ShiWuXinXi._SystemID==sysID)
            {
                digitThreshHold = Logic_XiaoEZhiLian_ShiWuXinXi._SystemDigitThreshHold;
            }
            else if (Logic_XiaoEQianZhiXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_XiaoEQianZhiXiTong._SystemDigitThreshHold;
            }
            else if (Logic_ZhiPiaoYingXiangXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_ZhiPiaoYingXiangXiTong._SystemDigitThreshHold;
            }
            else if (Logic_YinBaoTongXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_YinBaoTongXiTong._SystemDigitThreshHold;
            }
            else if (Logic_CTSXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_CTSXiTong._SystemDigitThreshHold;
            }
            else if (Logic_CCBSZhongDuanXiTong._SystemID==sysID)
            {
                digitThreshHold = Logic_CCBSZhongDuanXiTong._SystemDigitThreshHold;
            }

            return digitThreshHold;
        }

        public static void UpdateSystemBitmap(int sysID, Bitmap bitSystemOrig)
        {
            log.Info("update system bitmap " + sysID.ToString());
            lock(_syncBitmap_Lock)
            {
                if (bitSystemOrig == null) return;
                System.Drawing.Bitmap bitContent = bitSystemOrig.Clone(
                    new System.Drawing.Rectangle(0, 0, bitSystemOrig.Width, bitSystemOrig.Height),
                    bitSystemOrig.PixelFormat);

                if (Logic_ZhongKeXiTong._SystemID == sysID)
                {
                    Logic_ZhongKeXiTong._SystemBitMap = bitContent;
                }
                else if (Logic_ZhengQuanXiTong._SystemID == sysID)
                {
                    Logic_ZhengQuanXiTong._SystemBitMap = bitContent;
                }
                else if (Logic_QingSuanXiTong._SystemID == sysID)
                {
                    Logic_QingSuanXiTong._SystemBitMap = bitContent;
                }
                else if (Logic_QingSuanZhiLianXiTong._SystemID == sysID)
                {
                    Logic_QingSuanZhiLianXiTong._SystemBitMap = bitContent;
                }
                else if (Logic_DaEZhiFu_HuaHuiYeWu._SystemID == sysID)
                {
                    Logic_DaEZhiFu_HuaHuiYeWu._SystemBitMap = bitContent;
                }
                else if (Logic_DaEZhiFu_ShiWuXinXi._SystemID == sysID)
                {
                    Logic_DaEZhiFu_ShiWuXinXi._SystemBitMap = bitContent;
                }
                else if (Logic_DaEQianZhi._SystemID == sysID)
                {
                    Logic_DaEQianZhi._SystemBitMap = bitContent;
                }
                else if (Logic_XiaoEZhiLian_HuaHuiYeWu._SystemID == sysID)
                {
                    Logic_XiaoEZhiLian_HuaHuiYeWu._SystemBitMap = bitContent;
                }
                else if (Logic_XiaoEZhiLian_ShiWuXinXi._SystemID == sysID)
                {
                    Logic_XiaoEZhiLian_ShiWuXinXi._SystemBitMap = bitContent;
                }
                else if (Logic_XiaoEQianZhiXiTong._SystemID == sysID)
                {
                    Logic_XiaoEQianZhiXiTong._SystemBitMap = bitContent;
                }
                else if (Logic_ZhiPiaoYingXiangXiTong._SystemID == sysID)
                {
                    Logic_ZhiPiaoYingXiangXiTong._SystemBitMap = bitContent;
                }
                else if (Logic_YinBaoTongXiTong._SystemID == sysID)
                {
                    Logic_YinBaoTongXiTong._SystemBitMap = bitContent;
                }
                else if (Logic_CTSXiTong._SystemID == sysID)
                {
                    Logic_CTSXiTong._SystemBitMap = bitContent;
                }
                else if (Logic_CCBSZhongDuanXiTong._SystemID == sysID)
                {
                    Logic_CCBSZhongDuanXiTong._SystemBitMap = bitContent;
                }

                if (g_subsystemSignalTime.ContainsKey(sysID))
                    g_subsystemSignalTime[sysID] = DateTime.Now;
                else
                {
                    log.Info("Update Signal Time sysID :" + sysID.ToString() + " Time:" + DateTime.Now);
                    g_subsystemSignalTime.Add(sysID, DateTime.Now);
                }
            }

        }

        /// <summary>
        /// 获取图像获取分类后(数据可能未识别)的各个系统图像
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetSystemScreen(int sysID)
        {
            //log.Error("get system screen " + sysID.ToString());
            //var bmp = s_bmpList[sysID];
            //if( bmp != null )
            //{
            //    return bmp.Clone( 
            //        new Rectangle( 0, 0, bmp.Width, bmp.Height ), bmp.PixelFormat);
            //}
            //return null;
            lock (_syncBitmap_Lock)
            {
                System.Drawing.Bitmap bitSystemOrig = null;
                if (Logic_ZhongKeXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_ZhongKeXiTong._SystemBitMap;
                }
                else if (Logic_ZhengQuanXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_ZhengQuanXiTong._SystemBitMap;
                }
                else if (Logic_QingSuanXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_QingSuanXiTong._SystemBitMap;
                }
                else if (Logic_QingSuanZhiLianXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_QingSuanZhiLianXiTong._SystemBitMap;
                }
                else if (Logic_DaEZhiFu_HuaHuiYeWu._SystemID == sysID)
                {
                    bitSystemOrig = Logic_DaEZhiFu_HuaHuiYeWu._SystemBitMap;
                }
                else if (Logic_DaEZhiFu_ShiWuXinXi._SystemID == sysID)
                {
                    bitSystemOrig = Logic_DaEZhiFu_ShiWuXinXi._SystemBitMap;
                }
                else if (Logic_DaEQianZhi._SystemID == sysID)
                {
                    bitSystemOrig = Logic_DaEQianZhi._SystemBitMap;
                }
                else if (Logic_XiaoEZhiLian_HuaHuiYeWu._SystemID == sysID)
                {
                    bitSystemOrig = Logic_XiaoEZhiLian_HuaHuiYeWu._SystemBitMap;
                }
                else if (Logic_XiaoEZhiLian_ShiWuXinXi._SystemID == sysID)
                {
                    bitSystemOrig = Logic_XiaoEZhiLian_ShiWuXinXi._SystemBitMap;
                }
                else if (Logic_XiaoEQianZhiXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_XiaoEQianZhiXiTong._SystemBitMap;
                }
                else if (Logic_ZhiPiaoYingXiangXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_ZhiPiaoYingXiangXiTong._SystemBitMap;
                }
                else if (Logic_YinBaoTongXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_YinBaoTongXiTong._SystemBitMap;
                }
                else if (Logic_CTSXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_CTSXiTong._SystemBitMap;
                }
                else if (Logic_CCBSZhongDuanXiTong._SystemID == sysID)
                {
                    bitSystemOrig = Logic_CCBSZhongDuanXiTong._SystemBitMap;
                }

                if (bitSystemOrig != null)
                {
                    System.Drawing.Bitmap bitSystem = bitSystemOrig.Clone(
                         new System.Drawing.Rectangle(0, 0, bitSystemOrig.Width, bitSystemOrig.Height),
                         bitSystemOrig.PixelFormat);
                    return bitSystem;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取数据识别后各个系统的图像
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        private object __syncImage_Lock = new object();

        public Bitmap GetSystemImage( int sysID )
        {
            try
            {
                log.Info("get system image " + sysID.ToString());
                lock (__syncImage_Lock)
                {
                    System.Drawing.Bitmap bitSystemOrig = s_bmpList[sysID];
                    if (bitSystemOrig != null)
                    {
                        System.Drawing.Bitmap bitSystem = bitSystemOrig.Clone(
                             new System.Drawing.Rectangle(0, 0, bitSystemOrig.Width, bitSystemOrig.Height),
                             bitSystemOrig.PixelFormat);
                        return bitSystem;
                    }
                    return null;
                }
            }
            catch(Exception e)
            {
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }
            return null;
        }


        public static List<int> g_checkedInSystem=new List<int>();//用于记录系统是否以签到（数据库已记录)
        public static Dictionary<int, bool> g_updatedSystem=  //用于记录系统是否已取到新图像
            new Dictionary<int,bool>();

        private static object g_checkinLock = new object(); 
        public static void CheckInSystem(int sysID)
        {
            lock (g_checkinLock)
            {
                if (!g_checkedInSystem.Contains(sysID))
                {
                    DAL.SubSystemMonitorJobDal dal = new DAL.SubSystemMonitorJobDal();
                    if (dal.GetByDateAndSystemId(DateTime.Today, sysID) == null)
                    {
                        SubSystemMonitorJob ssmj = new SubSystemMonitorJob();
                        ssmj.SysytemID = sysID;
                        ssmj.SignedInTime = DateTime.Now;
                        dal.AddSubSystemMonitorJob(ssmj);
                    }

                    g_checkedInSystem.Add(sysID);
                }
            }

            if (!g_updatedSystem.ContainsKey(sysID))
                g_updatedSystem.Add(sysID, true);
            else
                g_updatedSystem[sysID] = true;


            BGCapture.Capturer.captureIdleSeconds = 100 + 5 * g_checkedInSystem.Count;
        }

        public static void CheckInSystem_Ex(int sysID)
        {
            lock (g_checkinLock)
            {
              
                DAL.SubSystemMonitorJobDal dal = new DAL.SubSystemMonitorJobDal();
                if (dal.GetByDateAndSystemId(DateTime.Today, sysID)==null)
                {
                   SubSystemMonitorJob ssmj = new SubSystemMonitorJob();
                   ssmj.SysytemID = sysID;
                   ssmj.SignedInTime = DateTime.Now;
                   dal.AddSubSystemMonitorJob(ssmj);
                }
            }
        }
        //public static List<int> GetExpiredSystems(long timeSeconds)
        //{
        //    DateTime timenow = DateTime.Now;
        //    List<int> listSysID = new List<int>();
        //    foreach (int sysID in g_checkedInSystem )
        //    {
        //        if (g_subsystemSignalTime.ContainsKey(sysID))
        //        {
        //            TimeSpan tspan = timenow - g_subsystemSignalTime[sysID];
        //            if(tspan.TotalSeconds>timeSeconds)
        //                listSysID.Add(sysID);
        //        }
        //    }

        //    return listSysID;
        //}

        public static List<int> _preProcessingSystems=new List<int>();
        public static int invalidCacheIndex = 1;

        public static bool IsNeedPreprocess(int sysID)
        {
            bool needPreprocess = true;
            if(sysID == Logic_CCBSZhongDuanXiTong._SystemID ||
               sysID == Logic_XiaoEQianZhiXiTong._SystemID ||
               sysID == Logic_YinBaoTongXiTong._SystemID)
            {
                needPreprocess = false;
            }

            return needPreprocess;
        }
        public static int CheckSystemID(System.Drawing.Bitmap bitContent)
        {
            if (bitContent == null) return INVALID_SYSTEMID;
                                        
            IntPtr hBitMap = IntPtr.Zero;
            int sysID = INVALID_SYSTEMID;

            try
            {
                hBitMap = bitContent.GetHbitmap();

                bool needPreprocess = false;
                if (bitContent.Height > 1000 && bitContent.Width > 1200)
                {
                    if (Logic_ZhiPiaoYingXiangXiTong.CheckSystem(bitContent, hBitMap))
                    {
                        sysID = Logic_ZhiPiaoYingXiangXiTong._SystemID;
                        needPreprocess = true;
                    }
                    else if (Logic_ZhengQuanXiTong.CheckSystem(bitContent, hBitMap))
                    {
                        sysID = Logic_ZhengQuanXiTong._SystemID;
                        needPreprocess = true;
                    }
                }
                else
                {
                    if (sysID == INVALID_SYSTEMID)
                    {
                        if (Logic_CTSXiTong.CheckSystem(bitContent, hBitMap))
                        {
                            sysID = Logic_CTSXiTong._SystemID;
                            needPreprocess = true;
                        }
                        else if (Logic_ZhongKeXiTong.CheckSystem(bitContent, hBitMap))
                        {
                            sysID = Logic_ZhongKeXiTong._SystemID;
                            needPreprocess = true;
                        }
                    }

                    if (sysID == INVALID_SYSTEMID)
                    {
                        sysID = Logic_BWSystemLogic.CheckSystem(bitContent, hBitMap);

                        if (sysID != INVALID_SYSTEMID)
                        {
                            if (sysID == Logic_CCBSZhongDuanXiTong._SystemID)
                                needPreprocess = false;
                            else
                                needPreprocess = true;
                        }
                    }

                    if (sysID == INVALID_SYSTEMID)
                    {

                        if (Logic_DaEQianZhi.CheckSystem(bitContent, hBitMap))
                        {
                            sysID = Logic_DaEQianZhi._SystemID;
                            needPreprocess = true;
                        }
                        else if (Logic_XiaoEQianZhiXiTong.CheckSystem(bitContent, hBitMap))
                        {
                            sysID = Logic_XiaoEQianZhiXiTong._SystemID;
                        }
                        else if (Logic_YinBaoTongXiTong.CheckSystem(bitContent, hBitMap))
                        {
                            sysID = Logic_YinBaoTongXiTong._SystemID;
                        }
                    }
                }
              
                DeleteObject(hBitMap);
                hBitMap = IntPtr.Zero;

                //if (g_updatedSystem.ContainsKey(sysID) && g_updatedSystem[sysID] == true) ;
                if (sysID != INVALID_SYSTEMID && sysID != _processingSystemID)
                {
                    //Save Bitmap to System
                    UpdateSystemBitmap(sysID,bitContent);
                    
                    CheckInSystem(sysID);  
                }        
            }
            catch (System.Exception e)
            {
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);

                if (hBitMap != IntPtr.Zero)
                    DeleteObject(hBitMap);
                sysID = INVALID_SYSTEMID;

                log.Error("\r\n*************************************");
            }

            return sysID;
        }

        private class PreprocessThread
        {
            private int _SystemID = 0;
            public PreprocessThread(int sysID)
            {
                _SystemID = sysID;
            }
            public void PreprocessMain()
            {
                if (!SystemsHandler.IsNeedPreprocess(_SystemID)) return;

                //log.Info("PreprocessThread Thread ID:" + GetCurrentThreadId().ToString() + " SysID:" + _SystemID.ToString());

                if (!SystemsHandler.m_settings.systems.ContainsKey(_SystemID) || SystemsHandler.GetSystemFolder(_SystemID)=="")
                    return;
                System.Drawing.Bitmap bitContent = SystemsHandler.GetSystemScreen(_SystemID);
                if (bitContent == null) return;
               
                SystemsHandler.log.Info("Begin Preprocess System:" + _SystemID);
                IntPtr hbitSystem=IntPtr.Zero; 
                try
                {
                    
                    string systemFolder = SystemsHandler.GetSystemFolder(_SystemID);

                    hbitSystem = bitContent.GetHbitmap();
                    _preProcessingSystems.Add(_SystemID);

                    SystemsSetting.System settings = SystemsHandler.m_settings.systems[_SystemID];
                
                    foreach (KeyValuePair<int, SystemsSetting.Condition> conditionPair in settings.alertConditions)
                    {
                        string conditionFile = systemFolder + conditionPair.Value.Label + ".bmp";
                        Rect range = conditionPair.Value.Range;

                        if (conditionPair.Value.Type==Digit.Digit.DigitType.DIGIT_STRING)
                        {
                            SaveConditionFileText(hbitSystem, range.Left, range.Top, range.Right, range.Bottom, conditionFile);
                        }
                        else if (_SystemID == Logic_XiaoEZhiLian_ShiWuXinXi._SystemID
                            && conditionPair.Value.Label.Contains("未发送笔数"))
                        {
                            string conditionFile1 = systemFolder + conditionPair.Value.Label + "_1.bmp";
                            SaveConditionFile(hbitSystem, range.Left, range.Top, (range.Left + range.Right) / 2, range.Bottom, conditionFile1);

                            string conditionFile2 = systemFolder + conditionPair.Value.Label + "_2.bmp";
                            SaveConditionFile(hbitSystem, (range.Left + range.Right) / 2, range.Top, range.Right, range.Bottom, conditionFile2);

                        }
                        else
                        {
                            SaveConditionFile(hbitSystem, range.Left, range.Top, range.Right, range.Bottom, conditionFile);
                        }
                    }
                    DeleteObject(hbitSystem);
                    bitContent.Dispose();
                    bitContent = null;

                    //if (_SystemID != INVALID_SYSTEMID)
                    //{
                    //    CheckInSystem(_SystemID);
                    //}
                    
                    _preProcessingSystems.Remove(_SystemID);

                }
                catch(System.Exception e)
                {
                    if (hbitSystem != IntPtr.Zero)
                        DeleteObject(hbitSystem);

                    log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                }
                             
                log.Info("End Preprocess System:" + _SystemID.ToString());
            }

            private void SaveConditionFile(IntPtr hbitSystem, int left, int top, int right, int bottom, string conditionFile)
            {
                try
                { 
                    int sysDigitThreshHold = SystemsHandler.GetSystemDigitThreshHold(_SystemID);
                    bool needRevert = false;
                    if (!SystemsHandler.IsSystemBlackText(_SystemID))
                    {
                        needRevert = true;
                    }
                    ANNWrapper.SaveBlockToBMP4(hbitSystem, left, top, right, bottom, conditionFile, sysDigitThreshHold,needRevert);

                    //bool bContinue = true;
                    //bContinue = bContinue && ANNWrapper.SaveBlockToBMP3(hbitSystem, left, top, right, bottom, conditionFile);
                    //if (!SystemsHandler.IsSystemBlackText(_SystemID))
                    //{
                    //    bContinue = bContinue && ANNWrapper.RevertBlackWhiteBMP(conditionFile);
                    //}
                    //bContinue = bContinue && ANNWrapper.BlackWhiteBMP(conditionFile, sysDigitThreshHold);

                    //if (bContinue)
                    //{
                    //    IntPtr hdibHandle = ANNWrapper.ReadDIBFile(conditionFile);
                    //    ANNWrapper.Convert256toGray(hdibHandle);
                    //    ANNWrapper.ConvertGrayToWhiteBlack(hdibHandle);
                    //    //ANNWrapper.GradientSharp(hdibHandle);
                    //    ANNWrapper.RemoveScatterNoise(hdibHandle);
                    //    ANNWrapper.SaveDIB(hdibHandle, conditionFile);
                    //    ANNWrapper.ReleaseDIBFile(hdibHandle);
                    //}
                }
                catch (System.Exception e)
                {
                    log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                    Bitmap bmp = Bitmap.FromHbitmap(hbitSystem);
                    string folder = ConfigurationManager.AppSettings["ExceptionFolder"] +
                        "PreviousProcessError\\";
                    DirectoryInfo di = new DirectoryInfo(folder);
                    if( !di.Exists )
                        di.Create();
                    DateTime now = DateTime.Now;
                    bmp.Save(folder + now.ToString("yyyyMMdd") + "_" +
                        now.ToString("HHmmss") +"_"+left.ToString()+"_"+top.ToString()+"_"+right.ToString()+"_"+bottom.ToString() +".bmp");
                }

            }

            private void SaveConditionFileText(IntPtr hbitSystem, int left, int top, int right, int bottom, string conditionFile)
            {
                bool bContinue = true;
                bContinue = bContinue && ANNWrapper.SaveBlockToBMP3(hbitSystem, left, top, right, bottom, conditionFile);
            }

        }
        public static bool IsSystemBlackText(int _SystemID)
        {
            bool bRet = false;
            if (_SystemID==Logic_ZhongKeXiTong._SystemID ||
                _SystemID == Logic_CTSXiTong._SystemID)
            {
                bRet = true;
            }
            return bRet;
        }
        public static void ANNRecognitionLog(Int32 logType, string message)
        {
            if (logType == ANNWrapper.ANN_LOG_ERROR)
                logANN.Error(message);
            else if (logType == ANNWrapper.ANN_LOG_INFO)
                logANN.Info( message);
            else if (logType == ANNWrapper.ANN_LOG_DEBUG)
                logANN.Debug( message);
            else
                logANN.Fatal( message);
        }

        public static void TCSDKLog(Int32 logType, string message)
        {
            if (logType == TCApp.TCSDKWrapper.TCSDK_LOG_ERROR)
                logTCSDK.Error(message);
            else if (logType == TCApp.TCSDKWrapper.TCSDK_LOG_INFO)
                logTCSDK.Info(message);
            else if (logType == TCApp.TCSDKWrapper.TCSDK_LOG_DEBUG)
                logTCSDK.Debug(message);
            else
                logTCSDK.Fatal(message);
        }

        public void Close()
        {
            for (int i = 1; i <= 14; ++i)
                StopAlarm(i);
            m_handlerThread.Abort();
            m_capture.StopCapture();
        }

        public void Destroy()
        {
            ChannelDSP.ReleaseSource();
        }

        public void OnWindows_Min()
        {

        }
        public void OnWindows_Max()
        {

        }
        
        public void LanchRemindingAlert(string title,string source)
        {
            //int alterType = 4;

            //if (!SystemsHandler.m_settings.alerts.ContainsKey(alterType)) return;

            //SystemsSetting.Alert alt=SystemsHandler.m_settings.alerts[alterType];

            //if(alt!=null)

            try
            {
                int timeSeconds = 86400;
                string file = ALERT_FOLDER + source;

                if (File.Exists(file))
                {
                    altRemindingBuzzer.Play(timeSeconds, title, file);
                }
                else
                {
                    log.Error("Reminding File Not Exist:" + file);
                }
            }
            catch
            {
                log.Error("Error Alter Title:" + title.ToString()+"\tSource:"+source.ToString());
            }
        }

        public void StopRemindingAlarm()
        {
            altRemindingBuzzer.Stop();
        }
    }
}
