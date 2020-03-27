using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace NativeSdkWrapper.Tests
{
    [TestClass]
    public class FaceEngineTest
    {
        [TestMethod]
        public void TestActivate()
        {
            using (FaceEngineWrapper wrapper = new FaceEngineWrapper())
            {
                long result = wrapper.Activate("B2MXNUkvHuw6QFjCgsUaHb4Kfmz2mcP5WA3muXGnH4GX",
                    "4juepd7jPdTcUU9AnVmUiBBWwiRPjsPayR5cVXY9zy9L");
                Assert.AreEqual(90114, result);
            }
        }

        [TestMethod]
        public void TestGetActiveFileInfo()
        {
            using (FaceEngineWrapper wrapper = new FaceEngineWrapper())
            {
                var result = wrapper.GetActiveFileInfo();
                Assert.AreEqual("ArcFace", result.SdkType);
            }
        }

        [TestMethod]
        public void TestInitEngine()
        {
            using (FaceEngineWrapper wrapper = new FaceEngineWrapper())
            {
                long result = wrapper.Initialize();
                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public void TestDetectFaces()
        {
            using (FaceEngineWrapper wrapper = new FaceEngineWrapper())
            {
                long result = wrapper.Initialize();
                Assert.AreEqual(0, result);

                Mat mat1 = new Mat("Images/1-2.jpg");
                long result1 = wrapper.DetectFaces(mat1.Width, mat1.Height, mat1.Data, out var multiFaceInfos1);
                Assert.AreEqual(1, multiFaceInfos1.Count);

                Mat mat2 = new Mat("Images/trudeau-faceapp.jpg");
                long result2 = wrapper.DetectFaces(mat2.Width, mat2.Height, mat2.Data, out var multiFaceInfos2);
                Assert.AreEqual(2, multiFaceInfos2.Count);
            }
        }
    }
}
