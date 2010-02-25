using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using TCApp;
using System.Drawing.Imaging;

namespace JOYFULL.CMPW.Monitor
{
    public class ChannelDSP
    {

        private static byte[] capture_buffer = new byte[1600 * 1200 * 4];

        public static System.Drawing.Bitmap ChanelCapture(int channelID)
        {
            string fileName = ChanelCaptureFile(channelID);
            
            if(fileName.Contains(".bmp"))
                return new System.Drawing.Bitmap(fileName);
            
            return null;
            
            //return new Bitmap("C:\\CMPW\\Test.bmp");
            //return GetBitmap( 100, 100 );
            //return GetBitmap(channelID, 704, 576);
        }
        public static void NextCapture()
        {
            TCSDKWrapper.NextCapture();
        }

        public static IntPtr GetChanelCaptureHDIB(int channelID)
        {
            return TCSDKWrapper.GetLatestCaptureHDIB();
        }
        public static string ChanelCaptureFile(int channelID)
        {
            //return "C:\\CMPW\\Test.bmp";
            byte[] tempParas = new byte[256];
            if(TCSDKWrapper.GetLatestCapture(tempParas))
            {
                string fileName = System.Text.Encoding.GetEncoding("GB2312").GetString(tempParas, 0, tempParas.Length);
                if (fileName.Contains(".bmp"))
                    return fileName.Substring(0,fileName.IndexOf('\0'));
            }
            return "";
        }

        public static System.Drawing.Bitmap GetChanelCaptureBitmap(int channelID , out Int32 captureIndex)
        {
            //unsafe
            {
                Int32 mapHeight=0;
                Int32 mapWidth=0;
                captureIndex = 0;
                if (TCSDKWrapper.GetLatestCaptureBytes(ref captureIndex, ref mapWidth, ref mapHeight, capture_buffer)) 
                {
                    Rectangle rect = new Rectangle(0, 0, mapWidth, mapHeight);
                    
                    Bitmap bmp = new Bitmap(mapWidth, mapHeight, PixelFormat.Format24bppRgb);
                    
                    // Lock the bitmap's bits.  
                    BitmapData bmp_data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);

                    int bytes = bmp_data.Stride * bmp.Height;

                    // Get the address of the first line.
                    IntPtr ptr = bmp_data.Scan0;

                    // Copy the RGB values back to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(capture_buffer, 0, ptr, bytes);

                    // Unlock the bits.
                    bmp.UnlockBits(bmp_data);

                    //byte* bmpPtr = (byte*)(bmp_data.Scan0);
                    //int ind = 0;
                    //for (int r = 0; r < mapHeight; r++)
                    //{
                    //    for (int k = 0; k < mapWidth; k++)
                    //    {
                    //        bmpPtr[ind + 0] = capture_buffer[ind + 0];
                    //        bmpPtr[ind + 1] = capture_buffer[ind + 1];
                    //        bmpPtr[ind + 2] = capture_buffer[ind + 2];

                    //        ind += 3;
                    //    }
                    //}
                    return bmp;
                }
            }
            return null;
        }
        //public static System.Drawing.Bitmap ChanelCapture_EX(int channelID,int width,int height)
        //{
        //    return GetBitmap(channelID, width, height);
        //}

        public static void NewCapture(string fileName)
        {

        }

        private static int initDSPCount = 0;
        private static Dictionary<int, ChannelInfo> channels = new Dictionary<int, ChannelInfo>();

        public static bool InitDSP(int lngMaxCachePic,int frameRate,string shareFolder)
        {
            try
            {
                TCSDKWrapper.SetUseCaptureBytes(true);

                if(initDSPCount>0) return true;
                
                if(frameRate<50) frameRate = 50;

                if(TCSDKWrapper.InitCard(frameRate) != 0)
                    return false;

                if(TCSDKWrapper.StartCapture(lngMaxCachePic, shareFolder, new TCApp.CallbackDelegate(NewCapture)) != 0) 
                    return false;

                initDSPCount++;

                return true;
            }
            catch (Exception exp)
            {
                //SystemsHandler.LogError("Init DSP Failure:" + exp.Message);
            }
            return false;
        }
        public static void DeInitDSP()
        {
            initDSPCount--;
            if (initDSPCount == 0)
            {
                TCSDKWrapper.StopCapture();
            }
        }  
      
        public static void ReleaseSource()
        {
            TCSDKWrapper.ReleaseCard();
        }
    }
}
