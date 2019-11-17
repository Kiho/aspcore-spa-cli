using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpaCliMiddleware.Tests
{
    [TestClass]
    public class PidUtilsTests 
    {
        [TestMethod]
        public void KillPort_8080_KillsSvelteServe()
        {
            bool success = PidUtils.KillPort(8080, true, true);
            Assert.IsTrue(success);
        }
    }
}
