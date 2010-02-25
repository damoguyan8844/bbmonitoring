using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JOYFULL.CMPW.Monitor;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using log4net;
using JOYFULL.CMPW.Digit;

namespace MonitorTest
{
    public partial class FormMain : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        static Mutex mutex = new Mutex(true, "{BEB86289-5445-45dc-91B9-2061F11D370F}");
        private static TCApp.TCSDK_LogDelegate _tcsdk_LogHandler = null;
        private static readonly ILog logTCSDK = LogManager.GetLogger(typeof(TCApp.TCSDKWrapper));

        private static ILog log = LogManager.GetLogger(typeof(FormMain));
        private static LogCallbackDelegate logHandler = null;

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
        public static void LoggerFunction(Int32 logTyp, string message)
        {
            if (logTyp == ANNWrapper.ANN_LOG_ERROR)
                log.Error(message);
            else if (logTyp == ANNWrapper.ANN_LOG_INFO)
                log.Info(message);
            else if (logTyp == ANNWrapper.ANN_LOG_DEBUG)
                log.Debug(message);
            else
                log.Fatal(message);
        }
        public FormMain()
        {
            InitializeComponent();

            //Control.CheckForIllegalCrossThreadCalls = false;
            btnStop.Enabled = false;
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                logHandler = new LogCallbackDelegate(LoggerFunction);
                ANNWrapper.SetLogHandler(logHandler);

                DirectoryInfo Dir = new DirectoryInfo(Application.StartupPath + "\\ErrorRec");
                if (!Dir.Exists)
                    Dir.Create();
                ANNWrapper.SetErrorRecordFolder(Application.StartupPath + "\\ErrorRec");

                if (_tcsdk_LogHandler == null)
                {
                    _tcsdk_LogHandler = new TCApp.TCSDK_LogDelegate(TCSDKLog);
                    TCApp.TCSDKWrapper.SetLogHandler(_tcsdk_LogHandler);
                } 

                TCApp.TCSDKWrapper.SetUseCaptureBytes(true);
                init = ChannelDSP.InitDSP(100, Int32.Parse(textIdle.Text), Application.StartupPath);
                if (!init)
                {
                    btnStart.Enabled = false;
                    btnStop.Enabled = false;
                    MessageBox.Show("Init DSP Failure!");

                }
                else
                {
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("采集卡已被占用，请退出其他程序后再运行此程序!");
            }
           
        }
        bool init = false;
        Thread thd = null;
        bool isInCaptureMode = false;

        private void btnStart_Click(object sender, EventArgs e)
        {
            isInCaptureMode = true;

            folder=textFolder.Text;
            idle=Int32.Parse(textIdle.Text);
            if (thd == null)
            {
                thd = new Thread(new ThreadStart(Updater));
                thd.Start();
                thd.Suspend();
                //if (thd.IsAlive)
                //    thd.Abort();
                //thd = null;
            }

            //thd = new Thread(new ThreadStart(Updater));
            {
                thd.Resume();
                btnStop.Enabled =true;
                btnStart.Enabled =false;
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
               
                btnStop.Enabled =false;
                btnStart.Enabled = true;
                if (thd != null)
                {
                    //if (thd.IsAlive)
                    //    thd.Abort();
                    //thd = null;

                    thd.Suspend();
                }

                isInCaptureMode = false;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }   
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            //退出提示，真的要退出，才允许退出 
            //if (MessageBox.Show(this, "你真的要退出？", "提示",
            //                                        MessageBoxButtons.YesNo,
            //                                        MessageBoxIcon.Question) == DialogResult.No)
            {

                btnStop_Click(null, null);
                ChannelDSP.DeInitDSP();
                ChannelDSP.ReleaseSource();

                System.Diagnostics.Process.GetCurrentProcess().Kill();
           //   e.Cancel = true;
            }
        }

        delegate void SetTextCallback(string text);
        private void SetCaptureTextThreadSafe(string text)
        {
            if (this.textCapture.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetCaptureTextThreadSafe);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textCapture.Text = text;
            }
        }

        public void SetImageLocationThreadSafe(
        string file)
        {

            if (this.pictureBoxCapture.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetImageLocationThreadSafe);
                this.Invoke(d, new object[] { file });
            }
            else
            {
            //    this.pictureBoxCapture.ImageLocation = file;

                if (!File.Exists(file)) return;
                if (pictureBoxCapture.Image != null)
                    pictureBoxCapture.Image.Dispose();
                pictureBoxCapture.Image = new Bitmap(file);
            }
        }

        int index = 0;
        string folder="";
        int idle = 1000;

        void Updater()
        {
            pictureBoxCapture.SizeMode=PictureBoxSizeMode.StretchImage;

            DirectoryInfo Dir = new DirectoryInfo(Application.StartupPath + "\\" + folder);
            if (!Dir.Exists)
            {
                Dir.Create();
            }

            //try
            //{

            //    DirectoryInfo Dir = new DirectoryInfo("C:\\Temp");
            
            //    foreach (FileInfo f in Dir.GetFiles("*.bmp"))
            //    {
            //        Bitmap bitCapture = new Bitmap(f.FullName);
            //        if(bitCapture!=null)
            //        {
            //            pictureBoxCapture.Image =bitCapture;
            //        }
            //        Thread.Sleep(1000);
            //    }
            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}   
            try
            {
                Int32 captureIndex = 0;

                while (true)
                {
                    //textBox1.Update();
                    //textBox1.Text = ChannelDSP.ChanelCaptureFile(0);
                    //Byte[] capture = new Byte[1600 * 1200 * 4];
                    /*IntPtr hDiB = ChannelDSP.GetChanelCaptureHDIB(0);*/

                    //TCApp.TCSDKWrapper.GetLatestCaptureByte(capture);

                    Bitmap  bitCapture = ChannelDSP.GetChanelCaptureBitmap(0, out captureIndex);
                    if (bitCapture != null)
                    {
                        string fileName = Application.StartupPath + "\\" + folder +"\\" + index.ToString() + "_capture_" + captureIndex.ToString() + ".bmp";
                        bitCapture.Save(fileName);
                        SetImageLocationThreadSafe(fileName);
                        SetCaptureTextThreadSafe(fileName);
                        
                        bitCapture.Dispose();
                        bitCapture = null;
                    }

                    index++;
                    Thread.Sleep(idle);

                    if (index > idle)
                        index = 0;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }   
        }

        private void textCapture_TextChanged(object sender, EventArgs e)
        {
            if (!isInCaptureMode)
            {
              //  pictureBoxCapture.ImageLocation = textCapture.Text;
            //}
            //else
            //{
                string strFile = textCapture.Text;
                if (!strFile.Contains(":"))
                    strFile = Application.StartupPath + "\\" + strFile;
                if (!File.Exists(strFile)) return;
                if (pictureBoxCapture.Image != null)
                    pictureBoxCapture.Image.Dispose();
                pictureBoxCapture.Image = new Bitmap(strFile);
            }
        }

        private void btnWhiteText_Click(object sender, EventArgs e)
        {
            string strRect = textBoxRect.Text;
            strRect=strRect.Replace(",", ";");

            string[] strArray = strRect.Split(';');

            if (strArray.Length < 4)
            {
                MessageBox.Show("错误的输入范围！");
                return;
            }
            string fileLocation =textCapture.Text;
            if (!fileLocation.Contains(":"))
                fileLocation = Application.StartupPath + "\\"+fileLocation;

            string strTemp = Application.StartupPath + "\\sample.bmp";
            string strTempTif = Application.StartupPath + "\\sample.tif";
            ANNWrapper.SaveBlockToBMP2(fileLocation, Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2]), Int32.Parse(strArray[3]), strTemp);
            ANNWrapper.RevertBlackWhiteBMP(strTemp);

            ANNWrapper.BlackWhiteBMP(strTemp, Int32.Parse(textBlackWhite.Text));

            ANNWrapper.ConvertBMP2TIF(strTemp, strTempTif);

            string strContent;
            byte[] tempParas = new byte[1024];
            if (ANNWrapper.OCRFile(strTempTif, tempParas))
            {
                strContent = System.Text.Encoding.GetEncoding("GB2312").GetString(tempParas, 0, tempParas.Length);
                textOCRContent.Text = strContent.Substring(0, strContent.IndexOf("\0"));
            }
            else
            {
                MessageBox.Show("OCRFile Failure!");
            }
        }

        private void btnBlackText_Click(object sender, EventArgs e)
        {
            string strRect = textBoxRect.Text;
            strRect = strRect.Replace(",", ";");

            string[] strArray = strRect.Split(';');

            if (strArray.Length < 4)
            {
                MessageBox.Show("错误的输入范围！");
                return;
            }
            string fileLocation = textCapture.Text;
            if (!fileLocation.Contains(":"))
                fileLocation = Application.StartupPath + "\\" + fileLocation;

            string strTemp = Application.StartupPath + "\\sample.bmp";
            string strTempTif = Application.StartupPath + "\\sample.tif";
            ANNWrapper.SaveBlockToBMP2(fileLocation, Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2]), Int32.Parse(strArray[3]), strTemp);
            //ANNWrapper.RevertBlackWhiteBMP(strTemp);

            ANNWrapper.BlackWhiteBMP(strTemp, Int32.Parse(textBlackWhite.Text));

            ANNWrapper.ConvertBMP2TIF(strTemp, strTempTif);

            string strContent;
            byte[] tempParas = new byte[1024];
            if (ANNWrapper.OCRFile(strTempTif, tempParas))
            {
                strContent = System.Text.Encoding.GetEncoding("GB2312").GetString(tempParas, 0, tempParas.Length);
                textOCRContent.Text = strContent.Substring(0, strContent.IndexOf("\0"));
            }
            else
            {
                MessageBox.Show("OCRFile Failure!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strRect = textBoxRect.Text;
            strRect = strRect.Replace(",", ";");

            string[] strArray = strRect.Split(';');

            if (strArray.Length < 4)
            {
                MessageBox.Show("错误的输入范围！");
                return;
            }

            string fileLocation = textCapture.Text;
            if (!fileLocation.Contains(":"))
                fileLocation = Application.StartupPath + "\\" + fileLocation;

            string strTemp = Application.StartupPath + "\\sample.bmp";
            ANNWrapper.SaveBlockToBMP2(fileLocation, Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2]), Int32.Parse(strArray[3]), strTemp);
            ANNWrapper.RevertBlackWhiteBMP(strTemp);

            ANNWrapper.BlackWhiteBMP(strTemp, Int32.Parse(textBlackWhite.Text));
            ANNTestRecognition();
        }

        private void ANNTestRecognition()
        {
             try
                {
                    int[] intRes = new int[64];
                    string strTemp = Application.StartupPath + "\\sample.bmp";
                    ANNWrapper.BlackWhiteBMP(strTemp, Int32.Parse(textBlackWhite.Text));

                    IntPtr hdibHandle = ANNWrapper.ReadDIBFile(strTemp);

                    ANNWrapper.Convert256toGray(hdibHandle);

                    ANNWrapper.SaveDIB(hdibHandle, Application.StartupPath + "\\Convert256toGray.bmp");

                    ANNWrapper.ConvertGrayToWhiteBlack(hdibHandle);

                    ANNWrapper.SaveDIB(hdibHandle, Application.StartupPath + "\\ConvertGrayToWhiteBlack.bmp");

                    //ANNWrapper.GradientSharp(hdibHandle);
                    ANNWrapper.RemoveScatterNoise(hdibHandle);

                    ANNWrapper.SaveDIB(hdibHandle, Application.StartupPath + "\\RemoveScatterNoise.bmp");

                    //ANNWrapper.SlopeAdjust(hdibHandle);

                    Int32 charRectID = ANNWrapper.CharSegment(hdibHandle);

                    if (charRectID >= 0)
                    {
                        ANNWrapper.LoadBPParameters(Application.StartupPath + "\\BWParas.dat");

                        //ANNWrapper.StdDIBbyRect(hdibHandle, charRectID, 16, 16);
                        IntPtr newHdibHandle = ANNWrapper.AutoAlign(hdibHandle, charRectID);
                        ANNWrapper.SaveDIB(newHdibHandle, Application.StartupPath + "\\AutoAlign.bmp");
                        //charRectID = ANNWrapper.CharSegment(newHdibHandle);

                        if (charRectID >= 0)
                        {
                            //ANNWrapper.SaveSegment(newHdibHandle, charRectID, Application.StartupPath + "\\");
                            if (ANNWrapper.Recognition_EX(newHdibHandle, charRectID, intRes))
                            {
                                string res = "";
                                foreach (int value in intRes)
                                {
                                    if (value == -1)
                                        break;
                                    res += value.ToString();
                                }

                                textOCRContent.Text=res.ToString(); 
                            }
                            else
                            {
                                MessageBox.Show("Recognition Failure" + "\r\n");
                            }
                        }

                        ANNWrapper.ReleaseDIBFile(newHdibHandle);
                    }
                    else
                    {
                        MessageBox.Show("CharSegment Step False" + "\r\n");
                    }

                    ANNWrapper.ReleaseDIBFile(hdibHandle);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
    
        private void btnBlackDigit_Click(object sender, EventArgs e)
        {

            string strRect = textBoxRect.Text;
            strRect = strRect.Replace(",", ";");

            string[] strArray = strRect.Split(';');

            if (strArray.Length < 4)
            {
                MessageBox.Show("错误的输入范围！");
                return;
            }

            string fileLocation = textCapture.Text;
            if (!fileLocation.Contains(":"))
                fileLocation = Application.StartupPath + "\\" + fileLocation;

            string strTemp = Application.StartupPath + "\\sample.bmp";
            ANNWrapper.SaveBlockToBMP2(fileLocation, Int32.Parse(strArray[0]), Int32.Parse(strArray[1]), Int32.Parse(strArray[2]), Int32.Parse(strArray[3]), strTemp);
            //ANNWrapper.RevertBlackWhiteBMP(strTemp);
            ANNWrapper.BlackWhiteBMP(strTemp, Int32.Parse(textBlackWhite.Text));
            ANNTestRecognition();
        }

        private void pictureBoxCapture_Click(object sender, EventArgs e)
        {

        }
    }
}
