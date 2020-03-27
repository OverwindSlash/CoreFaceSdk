using System;
using System.Runtime.InteropServices;

namespace NativeSdkWrapper.Model
{
    public enum AS_DetectModel : uint
    {
        DETECT_MODEL_RGB = 0x1	//RGB图像检测模型
    }
    
    public enum AS_DetectMode : uint
    {
        DETECT_MODE_VIDEO = 0x00000000,    //Video模式，一般用于多帧连续检测
        DETECT_MODE_IMAGE = 0xFFFFFFFF     //Image模式，一般用于静态图的单次检测

    };

    public enum AS_OrientPriority : uint
    {
        OP_0_ONLY = 0x1,    // 常规预览下正方向
        OP_90_ONLY = 0x2,   // 基于0°逆时针旋转90°的方向
        OP_270_ONLY = 0x3,  // 基于0°逆时针旋转270°的方向
        OP_180_ONLY = 0x4,  // 基于0°旋转180°的方向（逆时针、顺时针效果一样）
        OP_ALL_OUT = 0x5    // 全角度
    };

    [Flags]
    public enum AS_CombinedMask
    {
        NONE = 0x00000000,	            //无属性
        FACE_DETECT = 0x00000001,	    //此处detect可以是tracking或者detection两个引擎之一，具体的选择由detect mode 确定
        FACERECOGNITION = 0x00000004,	//人脸特征
        AGE = 0x00000008,	            //年龄
        GENDER = 0x00000010,	        //性别
        FACE3DANGLE = 0x00000020,	    //3D角度
        LIVENESS = 0x00000080,	        //RGB活体
        IR_LIVENESS = 0x00000400	    //IR活体
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AS_ActiveFileInfo
    {
        public IntPtr StartTime;
        public IntPtr EndTime;
        public IntPtr Platform;
        public IntPtr SdkType;
        public IntPtr AppId;
        public IntPtr SdkKey;
        public IntPtr SdkVersion;
        public IntPtr FileVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AS_MultiFaceInfo
    {
        public IntPtr FaceRect;
        public IntPtr FaceOrient;
        public int FaceNumber;
        public IntPtr FaceID;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AS_FaceRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ASVl_OffScreen
    {
        public uint U32PixelArrayFormat;
        public int I32Width;
        public int I32Height;

        public IntPtr[] Ppu8Plane;
        public int[] Pi32Pitch;


    }
}
