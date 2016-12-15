using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFServiceWebRole1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

namespace WCFServiceWebRole1.Tests
{
    [TestClass()]
    public class Service1Tests
    {
        //local endpoint
        //private readonly string baseUrl = "http://localhost:56755/Service.svc/";

        //cloud endpoint
        private readonly string baseUrl = "http://diffed.cloudapp.net/Service.svc/";

        private HttpWebResponse myWebClient(string urlendpoint, string dataContent, string method)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(urlendpoint));            
            httpWebRequest.Method = method;
            httpWebRequest.ContentType = "application/json";

            if (method == "PUT")
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(dataContent);

                Stream newStream = httpWebRequest.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }        
                        

            return (HttpWebResponse)httpWebRequest.GetResponse();
        }

        [TestMethod()]
        [TestCategory("IntegrationTest")]
        public void CreateLeftSideTest()
        {
            //test json input
            string json = @"{""Data"":""bTFQcnUzNzA=""}";
            var result = myWebClient(baseUrl + "v1/diff/itest/left", json, "PUT");

            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created);
        }



        [TestMethod()]
        [TestCategory("IntegrationTest")]
        public void CreateRightSideTest()
        {
            //test json input
            string json = @"{""Data"":""bTFQcnUzNzA=""}";
            var result = myWebClient(baseUrl + "v1/diff/itest/right", json, "PUT");

            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod()]
        [TestCategory("IntegrationTest")]
        public void GetDifferencesTest_forEquals()
        {
            string expectedResponse = @"{""diffResultType"":""Equals""}";
            string leftjson = @"{""Data"":""aW50ZWdyYXRpb24=""}"; //testing
            string rightjson = @"{""Data"":""aW50ZWdyYXRpb25=""}"; //testing

            var result = myWebClient(baseUrl + "v1/diff/itest/left", leftjson, "PUT");
            if (result.StatusCode == HttpStatusCode.Created)
            {
                result = myWebClient(baseUrl + "v1/diff/itest/right", rightjson, "PUT");
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    result = myWebClient(baseUrl + "v1/diff/itest","","GET");

                    //read content
                    var stream = result.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();

                    //parse content to json
                    var parse = "";
                    Assert.AreEqual(content,expectedResponse);
                    return;
                }
            }
            Assert.Fail();
        }

        [TestMethod()]
        [TestCategory("IntegrationTest")]
        public void GetDifferencesTest_forContentDiff()
        {
            string expectedResponse = @"{""diffResultType"":""ContentDoNotMatch"",""diffs"":[{""offset"":0,""length"":1},{""offset"":2,""length"":2}]}";
            string leftjson = @"{""Data"":""dGVzdA==""}"; //test
            string rightjson = @"{""Data"":""cmVkbw==""}"; //redo

            var result = myWebClient(baseUrl + "v1/diff/itest/left", leftjson, "PUT");
            if (result.StatusCode == HttpStatusCode.Created)
            {
                result = myWebClient(baseUrl + "v1/diff/itest/right", rightjson, "PUT");
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    result = myWebClient(baseUrl + "v1/diff/itest", "", "GET");

                    //read content
                    var stream = result.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();

                    //compare two json strings                    
                    Assert.AreEqual(content, expectedResponse);
                    return;
                }
            }
            Assert.Fail();
        }

        [TestMethod()]
        [TestCategory("IntegrationTest")]
        public void GetDifferencesTest_forSizeMatch()
        {
            string expectedResponse = @"{""diffResultType"":""SizeDoNotMatch""}";
            string leftjson = @"{""Data"":""aW50ZWdyYXRpb24=""}"; //integration
            string rightjson = @"{""Data"":""dGVzdA==""}"; //test

            var result = myWebClient(baseUrl + "v1/diff/itest/left", leftjson, "PUT");
            if (result.StatusCode == HttpStatusCode.Created)
            {
                result = myWebClient(baseUrl + "v1/diff/itest/right", rightjson, "PUT");
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    result = myWebClient(baseUrl + "v1/diff/itest", "", "GET");

                    //read content
                    var stream = result.GetResponseStream();
                    var sr = new StreamReader(stream);
                    var content = sr.ReadToEnd();

                    //parse content to json                    
                    Assert.AreEqual(content, expectedResponse);
                    return;
                }
            }
            Assert.Fail();
        }
    }
}