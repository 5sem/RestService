using SpaendHjelmenREST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SpaendHjelmenREST
{
    [ServiceContract]
    public interface IService1
    {
        
        #region Track


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "tracks/")]
        IList<Track> GetTracks();


        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "tracks/{id}")]
        Track GetTrackById(string id);


        #endregion
    }
}
