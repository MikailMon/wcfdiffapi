using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web.Hosting;
using WCFServiceWebRole1.DAO;
using WCFServiceWebRole1.Utils;
namespace WCFServiceWebRole1
{
    public class Service : IService
    {
        /// <summary>
        /// Clear all existing registration
        /// </summary>
        public void ClearRegisters()
        {
            //get the current request operation context object           
            DataAccess objDao = new DataAccess();
            objDao.EmptyRegisters();            

        }

        /// <summary>
        /// Create left/right side from PUT request into global json array
        /// </summary>
        /// <param name="id">Request identifier</param>
        /// <param name="side">Left/Right side</param>
        /// <param name="request">Data json input</param>
        public void CreateLeftRightSide(string id, string side, RequestObject request)
        {
            //get the current request operation context object
            WebOperationContext operationContext = WebOperationContext.Current;

            //Check if the side value has a valid parameter, only left or right are accepted
            if (side == "left" || side == "right")
            {
                try
                {
                    //Declare data access object for accesing global json file
                    DataAccess objDao = new DataAccess();

                    //Write new json entry           
                    objDao.WriteRegister(new RequestObject()
                    {
                        Data = Base64.Decode(request.Data),
                        ID = id,
                        Type = side
                    });

                    //Change teh default Status code from 200 to 201                    
                    operationContext.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Created;
                }
                catch (Exception)
                {
                    //If any exception is catching return a 500 status code
                    operationContext.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                //If side parameter has invalid value return a 501 status code
                operationContext.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NotImplemented;
            }
        }    

        /// <summary>
        /// Generate result base on the id value and the previous left/right side created 
        /// </summary>
        /// <param name="id">Identifier for analysis</param>
        /// <returns>Complex object with details</returns>
        public ResponseObject GetDifferences(string id)
        {
            //Declare data access object for accesing global json file
            DataAccess objDao = new DataAccess();

            //Retrive value from left side by identifier
            RequestObject objLeftSide = objDao.RetrieveObject(id, "left");

            //validate if id has left side
            if (objLeftSide == null)
            {
                //if leftside object is null then return a message and exit
                return new ResponseObject() { DiffResultType = "IDHasNoLeftSide" };
            }

            RequestObject objRightSide = objDao.RetrieveObject(id, "right");

            //validate if id has right side
            if (objRightSide == null)
            {
                //if rightside object is null then return a message and exit
                return new ResponseObject() { DiffResultType = "IDHasNoRightSide" };
            }
            //Declare response complex object
            ResponseObject objResponse = new ResponseObject();

            //First evaluate Equals lenght results
            if (objLeftSide.Data == objRightSide.Data)
            {
                objResponse.DiffResultType = "Equals";
            } 
            //Evaluate differences in content
            else if (objLeftSide.Data.Length == objRightSide.Data.Length) {
                //variable to retrieve indicate if there is a mismatch and control the lenght counter
                bool boolMismatch = false;
                //integer variable used to increment lenght where a mismatch is found
                int intLenghtCounter = 0;

                //Complex list value for response details
                List<Diffs> lstDiffs = new List<Diffs>();

                objResponse.DiffResultType = "ContentDoNotMatch";
                //Evaluate every char value into string request
                for (int index = 0; index < objLeftSide.Data.Length; index++)
                {
                    //if the first mismatch is founded (no consecutive)
                    if (objLeftSide.Data[index] != objRightSide.Data[index] && !boolMismatch)
                    {
                        //indicate there is a mismatch
                        boolMismatch = true;                        
                        lstDiffs.Add(new Diffs()
                        {
                            Offset = index,
                              Length = ++intLenghtCounter
                        });                      
                    } //if a consecutive mismatch is founded
                    else if (objLeftSide.Data[index] != objRightSide.Data[index] && boolMismatch) {
                        lstDiffs.Last().Length = ++intLenghtCounter;
                    }
                    else //char values are equals
                    {
                        boolMismatch = false;
                        intLenghtCounter = 0;
                    }
                }
                //setting details to response complex object
                objResponse.Diffs = lstDiffs;
            }

            //Evaluate differences in length
            else if (objLeftSide.Data.Length != objRightSide.Data.Length) {
                objResponse.DiffResultType = "SizeDoNotMatch";
            }

            //Return Final Value
            return objResponse;           
        }
    }
}
