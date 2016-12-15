using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFServiceWebRole1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceWebRole1.Utils.Tests
{
    [TestClass()]    
    public class DataAccessTests
    {
        [TestMethod()]
        [TestCategory("UnitTest")]
        public void DecodeTest()
        {
            //declare a dummy plain text
            string dummy = "myencodedTest";
            //Encode a dummy text en Base64
            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(dummy);
            string encodedText = Convert.ToBase64String(encodedBytes);

            string decoded = Utils.Base64.Decode(encodedText);

            Assert.AreEqual(decoded, dummy);
        }
    }
}