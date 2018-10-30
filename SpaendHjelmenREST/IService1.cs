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
        //Track
        #region


        [OperationContract]
        [WebInvoke(
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "Tracks?fragment={fragmentName}"
            )]
        IList<Track> GetTracks(string fragment = null);



        #endregion
    }
}
