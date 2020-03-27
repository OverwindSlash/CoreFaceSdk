using System.Collections.Generic;

namespace NativeSdkWrapper.Model
{
    public class ActiveFileInfo
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Platform { get; set; }
        public string SdkType { get; set; }
        public string AppId { get; set; }
        public string SdkKey { get; set; }
        public string SdkVersion { get; set; }
        public string FileVersion { get; set; }
    }

    public class FaceInfo
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Orient { get; set; }
        public int Id { get; set; }
    }
}
