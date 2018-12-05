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

        #region Comment

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "comments")]
        int PostComment(Comment comment);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "comments")]
        IList<Comment> GetComments();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "comments/{TrackId}")]
        IList<Comment> GetCommentsByTrackId(string TrackId);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "comments/{id}")]
        int DeleteComment(string id);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "comments/{commentid}")]
        int UpdateComment(Comment comment, string commentid);

        #endregion

        #region Post

        [OperationContract]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "Pictures/?filepath={filepath}")]
        void PostPicture(string filepath);


        [OperationContract]
        [WebInvoke(Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "pictures/{trackid}")]
        IList<Picture> GetPictures(string trackid);

        #endregion

        #region Rating

        [OperationContract]
        [WebInvoke(Method = "GET",
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "Rating/{trackid}")]
        int GetTrackRating(string trackid);

        [OperationContract]
        [WebInvoke(Method = "GET",
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "Rating/personlig/{userid}/{trackid}")]
        int GetPersonalTrackRating(string userid, string trackid);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "Rating")]
        int PostTrackRating(Rating rating);

        #endregion

        #region User

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "users/{id}")]
        User GetUserById(string id);

        #endregion


    }
}
