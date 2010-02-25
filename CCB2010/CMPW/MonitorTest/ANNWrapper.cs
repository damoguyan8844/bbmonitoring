using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace JOYFULL.CMPW.Digit
{
    //#define  LOG_ERROR 0x0000
    //#define  LOG_INFO 0x0010
    //#define  LOG_DEBUG 0x0100

    public delegate void LogCallbackDelegate(Int32 logType, string newCapture);

    public class ANNWrapper
    {
        public static readonly Int32 ANN_LOG_ERROR = 0x0000;
        public static readonly Int32 ANN_LOG_INFO = 0x0010;
        public static readonly Int32 ANN_LOG_DEBUG = 0x0100;

        [DllImport("ANNRecognition.dll",EntryPoint = "SetLogHandler")]
        public static extern void SetLogHandler( LogCallbackDelegate logger);

        [DllImport("ANNRecognition.dll", EntryPoint = "ConvertJPEG2BMP")]
        public static extern bool ConvertJPEG2BMP(string jpegFile,string bmpFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "ConvertBMP2TIF")]
        public static extern bool ConvertBMP2TIF(string bmpFile, string tifFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "BlackWhiteBMP")]
        public static extern bool BlackWhiteBMP(string bmpFile, int threshold);

        [DllImport("ANNRecognition.dll", EntryPoint = "RevertBlackWhiteBMP")]
        public static extern bool RevertBlackWhiteBMP(string bmpFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "SaveBlockToBMP")]
        public static extern bool SaveBlockToBMP(string bmpFile, 
                double leftRate, double topRate, double rightRate, double bottomRate,
                string blockBMPFile);
        
        [DllImport("ANNRecognition.dll", EntryPoint = "SaveBlockToBMP2")]
        public static extern bool SaveBlockToBMP2(string bmpFile,
                Int32 leftRate, Int32 topRate, Int32 rightRate, Int32 bottomRate,
                string blockBMPFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "SaveBlockToBMP3")]
        public static extern bool SaveBlockToBMP3(IntPtr hInputDIB,
                Int32 leftRate, Int32 topRate, Int32 rightRate, Int32 bottomRate,
                string blockBMPFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "OCRFile")]
        public static extern bool OCRFile(string bmpFile, byte[] content);

        [DllImport("ANNRecognition.dll", EntryPoint = "ReadDIBFile")]
        public static extern IntPtr ReadDIBFile( string fileName);

        [DllImport("ANNRecognition.dll", EntryPoint = "SaveDIB")]
        public static extern bool SaveDIB(IntPtr hInputDIB, string fileName);

        [DllImport("ANNRecognition.dll", EntryPoint = "BPEncode")]
        public static extern bool BPEncode(IntPtr hInputDIB, double[] outCode, Int32 top, Int32 left, Int32 right, Int32 bottom, string gridFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "LoadBPParameters")]
        public static extern bool LoadBPParameters(string paraFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "SaveBPParameters")]
        public static extern bool SaveBPParameters(string paraFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "PrintBPParameters")]
        public static extern bool PrintBPParameters(string txtParaFile);

        [DllImport("ANNRecognition.dll", EntryPoint = "InitTrainBPLearnSpeed")]
        public static extern bool InitTrainBPLearnSpeed(double learningSpeed);

        [DllImport("ANNRecognition.dll", EntryPoint = "InitBPParameters")]
        public static extern bool InitBPParameters(Int32 inputDim, Int32 implicitDim, Int32 outputDim);

        [DllImport("ANNRecognition.dll", EntryPoint = "Training")]
        public static extern double Training(double[] input, double[] dest);

        [DllImport("ANNRecognition.dll", EntryPoint = "Recognition")]
        public static extern bool Recognition(double[] input, [MarshalAs(UnmanagedType.LPArray, SizeConst = 4)]  double[] result);

        [DllImport("ANNRecognition.dll", EntryPoint = "Recognition_EX")]
        public static extern bool Recognition_EX(IntPtr hInputDIB, Int32 charRectID, [MarshalAs(UnmanagedType.LPArray, SizeConst = 50)]  int[] result);

        [DllImport("ANNRecognition.dll", EntryPoint = "ReleaseDIBFile")]
        public static extern bool ReleaseDIBFile(IntPtr hInputDIB);

        [DllImport("ANNRecognition.dll", EntryPoint = "Convert256toGray")]
        public static extern void Convert256toGray(IntPtr hInputDIB);

        [DllImport("ANNRecognition.dll", EntryPoint = "ConvertGrayToWhiteBlack")]
        public static extern void ConvertGrayToWhiteBlack(IntPtr hInputDIB);

        [DllImport("ANNRecognition.dll", EntryPoint = "GradientSharp")]
        public static extern void GradientSharp(IntPtr hInputDIB);

        [DllImport("ANNRecognition.dll", EntryPoint = "RemoveScatterNoise")]
        public static extern void RemoveScatterNoise(IntPtr hInputDIB);

        [DllImport("ANNRecognition.dll", EntryPoint = "SlopeAdjust")]
        public static extern void SlopeAdjust(IntPtr hInputDIB);

        [DllImport("ANNRecognition.dll", EntryPoint = "CharSegment")]
        public static extern Int32 CharSegment(IntPtr hInputDIB);

        [DllImport("ANNRecognition.dll", EntryPoint = "StdDIBbyRect")]
        public static extern void StdDIBbyRect(IntPtr hInputDIB, Int32 charRectID, int tarWidth, int tarHeight);

        [DllImport("ANNRecognition.dll", EntryPoint = "StdDIB")]
        public static extern void StdDIB(IntPtr hInputDIB, int tarWidth, int tarHeight);

        [DllImport("ANNRecognition.dll", EntryPoint = "AutoAlign")]
        public static extern IntPtr AutoAlign(IntPtr hInputDIB, Int32 charRectID);

        [DllImport("ANNRecognition.dll", EntryPoint = "SaveSegment")]
        public static extern void SaveSegment(IntPtr hInputDIB, Int32 charRectID, string toFolder);

        [DllImport("ANNRecognition.dll", EntryPoint = "SetErrorRecordFolder")]
        public static extern bool SetErrorRecordFolder(string errorRecFolder);

        [DllImport("ANNRecognition.dll", EntryPoint = "RecognitionWhiteText")]
        public static extern bool RecognitionWhiteText(IntPtr hInputDIB, Int32 left, Int32 top, Int32 right,
            Int32 bottom, Int32 threshHold, string tifFileName, byte[] data );

        [DllImport("ANNRecognition.dll", EntryPoint = "RecognitionBlackText")]
        public static extern bool RecognitionBlackText(IntPtr hInputDIB, Int32 left, Int32 top, Int32 right,
            Int32 bottom, Int32 threshHold, string tifFileName, byte[] data);

    }
}
