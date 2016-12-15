using Microsoft.VisualStudio.TestTools.UnitTesting;
using WCFServiceWebRole1.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WCFServiceWebRole1.DAO.Tests
{
    [TestClass()]
    public class DataAccessTests
    {
        private readonly string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=data07;AccountKey=h+nxu0B8ghRMuQOL4ZhE9CmV/2BWu9wMnRMEyyH7jFoaeKn9hnOTOjT8DpFzJL1ErFBCOZ7EvKT4dZrQuJHLjA==;";
        private DataAccess objDao;

        public DataAccessTests() {
            objDao = new DataAccess(storageConnectionString);
        }

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void ReadRegistersTest()
        {          
            Assert.IsTrue(objDao.ReadRegisters().Count > 0, "Items readed successfully!");
        }

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void WriteRegisterTest_forCreation()
        {
            objDao.EmptyRegisters();
            Assert.AreEqual(DataAccess.WriteActions.Creation, objDao.WriteRegister(new RequestObject()
            {
                ID = "test",
                Data = "insert",
                Type = "left"
            }));
        }

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void WriteRegisterTest_forUpdate()
        {
            this.WriteRegisterTest_forCreation();

            var results = objDao.WriteRegister(new RequestObject()
            {
                ID = "test",
                Data = "modification",
                Type = "left"
            });

            //Assert 
            Assert.AreEqual(DataAccess.WriteActions.Update, results);
        }

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void RetrieveObjectTest_forExistingObject()
        {
            this.WriteRegisterTest_forCreation();
            DataAccess dao = new DataAccess(storageConnectionString);
            var result = dao.RetrieveObject("test", "left");
            Assert.IsTrue(result.ID == "test");
        }

        [TestMethod()]
        [TestCategory("UnitTest")]
        public void RetrieveObjectTest_forNonExistingObject() {
            DataAccess dao = new DataAccess(storageConnectionString);

            var result = dao.RetrieveObject("nonexisting", "left");

            Assert.IsTrue(result == null);
        }
    }
}