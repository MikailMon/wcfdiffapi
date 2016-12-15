using Microsoft.WindowsAzure.ServiceRuntime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.File;

namespace WCFServiceWebRole1.DAO
{
    /// <summary>
    /// Class that helps to get connectivity to persistent data
    /// </summary>
    public class DataAccess
    {            
        //object that access to store space profile on azure
        private CloudStorageAccount _storageAccount;

        //object that access to public file on azure
        private CloudFileClient _fileClient;

        /// <summary>
        /// DataAccess constructor for main program
        /// </summary>
        public DataAccess()
        {

            // Parse the connection string and return a reference to the storage account.            
            _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"].ToString());

            // Create a CloudFileClient object for credentialed access to File storage.
            _fileClient = _storageAccount.CreateCloudFileClient();
        }

        /// <summary>
        /// DataAccess constructor for unit test program
        /// </summary>
        /// <param name="storageConnectionString">Storage connection string</param>
        public DataAccess(string storageConnectionString)
        {
            // Parse the connection string and return a reference to the storage account.            
            _storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create a CloudFileClient object for credentialed access to File storage.
            _fileClient = _storageAccount.CreateCloudFileClient();
        }

        /// <summary>
        /// Get data.json cloud file on azure 
        /// </summary>
        /// <returns></returns>
        private CloudFile GetCloudFile() {
            //Object that manage the share space profile on azure
            CloudFileShare fileShare = _fileClient.GetShareReference("data07");

            // Get a reference to the root directory for the share.
            CloudFileDirectory rootDir = fileShare.GetRootDirectoryReference();

            // Get a reference to the file we created previously.
            CloudFile cloudFile = rootDir.GetFileReference("data.json");

            return cloudFile;
        }

        /// <summary>
        /// Read existing registrations for all left and right side request
        /// </summary>
        /// <returns>All existing registration</returns>
        public List<RequestObject> ReadRegisters() {

            string jsonContent = "";     
            
            //Download json file from cloud and save it in memory
            jsonContent = GetCloudFile().DownloadTextAsync().Result;

            //string jsonContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<RequestObject>>(jsonContent);          

        }

        /// <summary>
        /// Write new request into persistent data json file on Azure
        /// </summary>
        /// <param name="objNewRequest">New complex request object to save</param>
        /// <returns>Action result type to specified the insertion or update</returns>
        public WriteActions WriteRegister(RequestObject objNewRequest)
        {
            
            //Read registration file 
            List<RequestObject> objListRequest = ReadRegisters();
            
            //Specify if values is the request is alredy exists in the data file
            bool boolIsNew = true;
            WriteActions actionResult = WriteActions.None;

            //Check if ID exists on registers
            foreach (var item in objListRequest)
            {
                if (objNewRequest.ID == item.ID && objNewRequest.Type == item.Type)
                {
                    //update existing request
                    boolIsNew = false;
                    item.Data = objNewRequest.Data;
                    actionResult = WriteActions.Update;
                }
            }

            //if theres is no update on existing request
            if (boolIsNew)
            {
                objListRequest.Add(objNewRequest);
                actionResult = WriteActions.Creation;            
            }            

            //Upload new content in data file on azure
            GetCloudFile().UploadText(JsonConvert.SerializeObject(objListRequest));                      

            return actionResult;
        }

        /// <summary>
        /// Retreive a specific register base on id
        /// </summary>
        /// <param name="id">Register indetification</param>
        /// <param name="type">Type: left or right</param>
        /// <returns></returns>
        public RequestObject RetrieveObject(string id, string  type) {
            //get all exisiting registers
            List<RequestObject> objListRequest = ReadRegisters();
            //Query data an return results
            return objListRequest.Where(x => x.ID == id && x.Type == type).FirstOrDefault();
        }

        /// <summary>
        /// Clear all registration in data json file
        /// </summary>        
        public WriteActions EmptyRegisters()
        {
            //emtpy data list 
            List<RequestObject> objListRequest = new List<RequestObject>();            

            //Upload new content in data file on azure
            GetCloudFile().UploadText(JsonConvert.SerializeObject(objListRequest));

            return WriteActions.Clear;
        }

        /// <summary>
        /// Types of write action
        /// </summary>
        public enum WriteActions
        {
            None,
            Creation,
            Update,
            Clear
        }


    }
}