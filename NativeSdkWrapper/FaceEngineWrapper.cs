using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using NativeSdkWrapper.Model;

namespace NativeSdkWrapper
{
    public class FaceEngineWrapper : IDisposable
    {
        private const string FaceEngineLibrary = @"libarcsoft_face_engine.dll";
        private const int DefaultScale = 32;
        private const int DefaultMaxFaceNum = 5;

        private IntPtr _engine;

        #region DllImport
        [DllImport(FaceEngineLibrary, EntryPoint = "ASFOnlineActivation",
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long ActivateOnline(string appId, string sdkKey);

        [DllImport(FaceEngineLibrary, EntryPoint = "ASFGetActiveFileInfo",
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long GetActiveFileInfo(ref AS_ActiveFileInfo asActiveFileInfo);

        [DllImport(FaceEngineLibrary, EntryPoint = "ASFInitEngine", 
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long InitEngine(AS_DetectMode asDetectMode, AS_OrientPriority asOrientPriority,
            int scaleVal, int maxNum, AS_CombinedMask asCombinedMask, out IntPtr engine);

        [DllImport(FaceEngineLibrary, EntryPoint = "ASFDetectFaces",
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long DetectFaces(IntPtr engine, int width, int height, int format,
             IntPtr imgData,  ref AS_MultiFaceInfo asMultiFaceInfos, AS_DetectModel asDetectModel);

        [DllImport(FaceEngineLibrary, EntryPoint = "ASFDetectFacesEx",
            CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern long DetectFaces(IntPtr engine, int width, int height, int format,
            IntPtr imgData, ref AS_MultiFaceInfo asMultiFaceInfos, AS_DetectModel asDetectModel);

        [DllImport(FaceEngineLibrary, EntryPoint = "ASFUninitEngine")]
        internal static extern long UninitEngine(IntPtr engine);
        #endregion

        public FaceEngineWrapper()
        {
            _engine = IntPtr.Zero;
        }

        public long Activate(string appId, string sdkKey)
        {
            return ActivateOnline(appId, sdkKey);
        }

        public ActiveFileInfo GetActiveFileInfo()
        {
            AS_ActiveFileInfo asAfi = new AS_ActiveFileInfo();
            GetActiveFileInfo(ref asAfi);

            return new ActiveFileInfo()
            {
                StartTime = Marshal.PtrToStringAnsi(asAfi.StartTime),
                EndTime = Marshal.PtrToStringAnsi(asAfi.EndTime),
                Platform = Marshal.PtrToStringAnsi(asAfi.Platform),
                SdkType = Marshal.PtrToStringAnsi(asAfi.SdkType),
                AppId = Marshal.PtrToStringAnsi(asAfi.AppId),
                SdkKey = Marshal.PtrToStringAnsi(asAfi.SdkKey),
                SdkVersion = Marshal.PtrToStringAnsi(asAfi.SdkVersion),
                FileVersion = Marshal.PtrToStringAnsi(asAfi.FileVersion)
            };
        }

        public long Initialize()
        {
            return InitEngine(
                AS_DetectMode.DETECT_MODE_IMAGE,
                AS_OrientPriority.OP_0_ONLY,
                DefaultScale,
                DefaultMaxFaceNum,
                AS_CombinedMask.FACE_DETECT | AS_CombinedMask.AGE,
                out _engine
            );
        }

        public long DetectFaces(int width, int height, IntPtr imgData, out List<FaceInfo> fis)
        {
            AS_MultiFaceInfo asInfo = new AS_MultiFaceInfo();
            var ret = DetectFaces(_engine, width, height, 513, imgData, ref asInfo,
                AS_DetectModel.DETECT_MODEL_RGB);

            fis = new List<FaceInfo>();
            var faceRectSize = Marshal.SizeOf(typeof(AS_FaceRect));
            var faceOrientSize = Marshal.SizeOf(typeof(int));
            var faceIdSize = Marshal.SizeOf(typeof(int));

            for (int i = 0; i < asInfo.FaceNumber; i++)
            {
                IntPtr faceRectPtr = asInfo.FaceRect + i * faceRectSize;
                IntPtr faceOrientPtr = asInfo.FaceOrient + i * faceOrientSize;

                AS_FaceRect faceRect = Marshal.PtrToStructure<AS_FaceRect>(faceRectPtr);
                int faceOrient = Marshal.PtrToStructure<int>(faceOrientPtr);

                int faceId = -1;
                if (asInfo.FaceID != IntPtr.Zero)
                {
                    IntPtr faceIdPtr = asInfo.FaceID + i * faceIdSize;
                    faceId = Marshal.PtrToStructure<int>(faceIdPtr);
                }

                fis.Add(new FaceInfo()
                {
                    Top = faceRect.Top,
                    Left = faceRect.Left,
                    Right = faceRect.Right,
                    Bottom = faceRect.Bottom,
                    Orient = faceOrient,
                    Id = faceId
                });
            }

            return ret;
        }


        #region Dispose
        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
            UninitEngine(_engine);
        }

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FaceEngineWrapper()
        {
            Dispose(false);
        } 
        #endregion
    }
}
