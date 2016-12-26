using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JWPush;

namespace JWPush.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            JWPush.PushHelper helper = new PushHelper(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=ZIKGONG;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            var list = helper.GetRegisteredApps();

            if(list != null)
            {
                
            }
            
        }
    }
}
