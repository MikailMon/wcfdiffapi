using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1
{
    [ServiceContract]
    public interface IService
    {
       
        // TODO: Add your service operations here
        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/v1/diff/{id}/{side}")]
        void CreateLeftRightSide(string id, string side, RequestObject request);

        
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/v1/diff/{id}")]
        ResponseObject GetDifferences(string id);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/v1/clear")]
        void ClearRegisters();

    }

    /// <summary>
    /// Request object to manipulate json input by serialization
    /// </summary>
    [DataContract]
    public class RequestObject {
        string _ID;
        string _type;
        string _data;       
              
        //Request Identification
        [DataMember]
        public string ID
        {
            get
            {
                return _ID;
            }

            set
            {
                _ID = value;
            }
        }

        //Request Type: left or right
        [DataMember]
        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        //Main content request data
        [DataMember]
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }

    /// <summary>
    /// Response object send it to the client by json serialization
    /// </summary>
    [DataContract]
    public class ResponseObject {
        string _diffResultType;
        List<Diffs> _diffs;
        //Operation message
        [DataMember(Name = "diffResultType", Order = 1)]
        public string DiffResultType
        {
            get
            {
                return _diffResultType;
            }

            set
            {
                _diffResultType = value;
            }
        }
        [DataMember(EmitDefaultValue = false, Name = "diffs", Order = 2)]
        public List<Diffs> Diffs
        {
            get
            {
                return _diffs;
            }

            set
            {
                _diffs = value;
            }
        }
    }

    /// <summary>
    /// Contents the differences detail on response
    /// </summary>
    [DataContract]
    public class Diffs {
        int _offset;
        int _length;
        [DataMember(Name = "offset", Order = 1)]
        public int Offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value;
            }
        }
        [DataMember(Name = "length", Order = 2)]
        public int Length
        {
            get
            {
                return _length;
            }

            set
            {
                _length = value;
            }
        }
    }
 
}
