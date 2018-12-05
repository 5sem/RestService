using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SpaendHjelmenREST.Models;
using SpaendHjelmenREST.Models.DTO;

namespace SpaendHjelmenREST
{
    public class Service1 : IService1
    {
        #region Tracks

        public IList<Track> GetTracks()
        {
            string sqlString = "SELECT * FROM Tracks";

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var trackList = new List<Track>();
                        while (reader.Read())
                        {
                            var track = ReadTrack(reader);
                            trackList.Add(track);
                        }
                        return trackList;
                    }
                }
            }
        }


        public Track GetTrackById(string id)
        {
            string sqlString = $"SELECT * FROM Tracks WHERE Id = '{id}'";

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var track = ReadTrack(reader);
                            return track;
                        }
                        return null;
                    }
                }
            }
        }

        private static Track ReadTrack(IDataRecord reader)
        {
            var Id = reader.GetInt32(0);
            //var PictureId = reader.GetInt32(1);
            var Name = reader.GetString(2);
            var Info = reader.GetString(3);
            var Longitude = reader.GetDouble(4);
            var Latitude = reader.GetDouble(5);
            var PostalCode = reader.GetInt32(6);
            var City = reader.GetString(7);
            var Address = reader.GetString(8);
            var ColorCode = reader.GetString(9);
            var Lenght = reader.GetDouble(10);
            var MaxHeight = reader.GetDouble(11);
            var ParkInfo = reader.GetString(12);
            var Regional = reader.GetString(13);

            Track t = new Track(Id);

            // t.PictureId = PictureId;
            t.Name = Name;
            t.Info = Info;
            t.Longitude = Longitude;
            t.Latitude = Latitude;
            t.PostalCode = PostalCode;
            t.City = City;
            t.Address = Address;
            t.ColorCode = ColorCode;
            t.Length = Lenght;
            t.MaxHeight = MaxHeight;
            t.ParkInfo = ParkInfo;
            t.Regional = Regional;

            return t;
        }

        #endregion

        #region Comments

        public int PostComment(Comment comment)
        {
            const string postsql =
                "INSERT INTO Comments (UserId, TrackId, UserComment) values (@UserId, @TrackId, @UserComment)";

            //check userid ok
            //burde tjek via token i send request
            if (CheckUserId().FindAll(x => x.Id.Equals(comment.UserId)).Count > 0)
            {
                using (var DBConnection = new SqlConnection(GetConnectionString()))
                {
                    DBConnection.Open();
                    using (var PostCommand = new SqlCommand(postsql, DBConnection))
                    {
                        PostCommand.Parameters.AddWithValue("@UserId", comment.UserId);
                        PostCommand.Parameters.AddWithValue("@TrackId", comment.TrackId);
                        PostCommand.Parameters.AddWithValue("@UserComment", comment.UserComment);
                        var rowsaffected = PostCommand.ExecuteNonQuery();
                        return rowsaffected;
                    }
                }
            }

            return 403; //denied


        }


        public IList<Comment> GetCommentsByTrackId(string TrackId)
        {
            const string GetCommentSql = "SELECT * From Comments where TrackId=@TrackId";
            using (var SqlConnection = new SqlConnection(GetConnectionString()))
            {
                SqlConnection.Open();
                using (var sqlCommand = new SqlCommand(GetCommentSql, SqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@TrackId", TrackId);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var _Comments = new List<Comment>();
                        while (reader.Read())
                        {
                            var _Comment = CommentReader(reader);
                            _Comments.Add(_Comment);
                        }

                        return _Comments;
                    }
                }
            }
        }



        public IList<Comment> GetComments()
        {
            const string GetCommentSql = "SELECT * From Comments";
            using (var SqlConnection = new SqlConnection(GetConnectionString()))
            {
                SqlConnection.Open();
                using (var sqlCommand = new SqlCommand(GetCommentSql, SqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var _Comments = new List<Comment>();
                        while (reader.Read())
                        {
                            var _Comment = CommentReader(reader);
                            _Comments.Add(_Comment);
                        }

                        return _Comments;
                    }
                }
            }
        }

        public int DeleteComment(string id)
        {
            string DeleteCommentSql = $"DELETE FROM Comments where id = @id";
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(DeleteCommentSql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.ExecuteNonQuery();
                    return 204; //resource deleted successfully
                }
            }
        }


        public int UpdateComment(Comment comment, string commentid)
        {
            const string UpdateCommentSql = "UPDATE comments SET UserId = @UserId, TrackId = @TrackId, Edited = @Edited, UserComment = @UserComment WHERE Id = @id";
            DateTime now = DateTime.Now;
            using (var dbcon = new SqlConnection(GetConnectionString()))
            {
                dbcon.Open();
                using (var sqlcommand = new SqlCommand(UpdateCommentSql, dbcon))
                {
                    sqlcommand.Parameters.AddWithValue("@UserId", comment.UserId);
                    sqlcommand.Parameters.AddWithValue("@TrackId", comment.TrackId);
                    sqlcommand.Parameters.AddWithValue("@Edited", now);
                    sqlcommand.Parameters.AddWithValue("@UserComment", comment.UserComment);
                    sqlcommand.Parameters.AddWithValue("@id", commentid);
                    sqlcommand.ExecuteNonQuery();
                    return 204;
                }
            }
        }


        private List<UserDTO> CheckUserId()
        {
            const string userDtosql = "SELECT Id, AuthToken From Users";
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(userDtosql, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var _UserList = new List<UserDTO>();
                        while (reader.Read())
                        {
                            var _user = UserDTOReader(reader);
                            _UserList.Add(_user);
                        }

                        return _UserList;
                    }
                }
            }

        }



        private static Comment CommentReader(IDataRecord reader)
        {
            var Id = reader.GetInt32(0);
            var UserId = reader.GetInt32(1);
            var TrackId = reader.GetInt32(2);
            var UserComment = reader.GetString(3);
            var Created = reader.GetDateTime(4);
            var Edited = reader.GetDateTime(5);

            var _Comment = new Comment
            {
                Id = Id,
                UserId = UserId,
                TrackId = TrackId,
                UserComment = UserComment,
                Created = Created,
                Edited = Edited
            };
            return _Comment;
        }


        private static UserDTO UserDTOReader(IDataRecord reader)
        {
            var Id = reader.GetInt32(0);
            var AuthToken = reader.GetString(1);

            var _UserDTO = new UserDTO { Id = Id, AuthToken = AuthToken };
            return _UserDTO;
        }

        #endregion

        #region Picture

        public IList<Picture> GetPictures(string trackid)
        {
            string sqlString = $"SELECT * FROM Pictures WHERE TrackId = '{trackid}'";
            using (var sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(sqlString, sqlConnection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        List<Picture> picList = new List<Picture>();
                        while (reader.Read())
                        {
                            Picture picture = new Picture();
                            picture.Id = reader.GetInt32(0);
                            picture.Name = reader.GetString(1);
                            picture.Image = (byte[])reader.GetValue(2);
                            picture.TrackId = reader.GetInt32(3);

                            picList.Add(picture);
                        }
                        return picList;
                    }
                }
            }

        }




        public void PostPicture(string filepath)
        {
            const string sqlcommand = "INSERT INTO Pictures (Image) Values (@file)";
            byte[] file;
            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }

                using (var dbcon = new SqlConnection(GetConnectionString()))
                {
                    dbcon.Open();
                    using (var PostCommand = new SqlCommand(sqlcommand, dbcon))
                    {
                        PostCommand.Parameters.Add("@file", SqlDbType.VarBinary, file.Length).Value = file;
                        PostCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        #endregion

        #region User

        public User GetUserById(string id)
        {
            string sqlString = $"SELECT * FROM Users WHERE Id = '{id}'";

            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection))
                {
                    using(var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = UserReader(reader);
                            return user;
                        }
                        return null;
                    }
                }
            }
        }

        public int UpdateDescription(User user, string id)
        {
            //TODO: billede
            const string PutUserSql = "UPDATE Users SET Description = @Description, Privacy = @Privacy WHERE Id = @Id";
            using (SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
            {
               sqlConnection.Open();
                using (var sqlcommand = new SqlCommand(PutUserSql, sqlConnection))
                {
                    sqlcommand.Parameters.AddWithValue("@Description", user.Description);
                    sqlcommand.Parameters.AddWithValue("@Privacy", user.Privacy);
                    sqlcommand.Parameters.AddWithValue("@Id", id);
                    return 204;
                }
            }
        }

        #endregion


        public int GetTrackRating(string trackid)
        {
            int _total = 0;
            const string GetTrackRatingSql = "SELECT * FROM Rating where TrackId = @TrackId";
            using (var DBcon = new SqlConnection(GetConnectionString()))
            {
                DBcon.Open();
                using (var SqlCommand = new SqlCommand(GetTrackRatingSql, DBcon))
                {
                    SqlCommand.Parameters.AddWithValue("@TrackId", trackid);

                    using (var reader = SqlCommand.ExecuteReader())
                    {
                        List<Rating> _ratings = new List<Rating>();
                        while (reader.Read())
                        {
                            var _rating = RatingReader(reader);
                            _ratings.Add(_rating);
                        }


                        foreach (var i in _ratings)
                        {
                            _total = _total + i.UserRating;
                        }

                        if (_total <= 0)
                        {
                            return 0;
                        }
                        return _total / _ratings.Count;
                    }

                }
            }
        }

        public int GetPersonalTrackRating(string userid, string trackid)
        {
            const string GetPersonalRating = "SELECT UserRating FROM Rating where UserId = @userid AND TrackId = @trackid";
            using (var dbcon = new SqlConnection(GetConnectionString()))
            {
                dbcon.Open();
                using (var sqlcommand = new SqlCommand(GetPersonalRating, dbcon))
                {
                    sqlcommand.Parameters.AddWithValue("@userid", userid);
                    sqlcommand.Parameters.AddWithValue("@trackid", trackid);
                    using (var reader = sqlcommand.ExecuteReader())
                    {
                        int _rating = 0;
                        while (reader.Read())
                        {
                            _rating = reader.GetInt32(0);
                        }
                        return _rating;
                    }
                }
            }
        }

        public int PostTrackRating(Rating rating)
        {
            if (GetPersonalTrackRating(rating.UserId.ToString(), rating.TrackId.ToString()) != null)
            {

                const string PostTrackRating =
                    "INSERT INTO Rating (UserId, TrackId, UserRating) values (@UserId, @TrackId, @UserRating)";

                //check userid ok
                //burde tjek via token i send request
                if (CheckUserId().FindAll(x => x.Id.Equals(rating.UserId)).Count > 0)
                {
                    using (var DBConnection = new SqlConnection(GetConnectionString()))
                    {
                        DBConnection.Open();
                        using (var PostCommand = new SqlCommand(PostTrackRating, DBConnection))
                        {
                            PostCommand.Parameters.AddWithValue("@UserId", rating.UserId);
                            PostCommand.Parameters.AddWithValue("@TrackId", rating.TrackId);
                            PostCommand.Parameters.AddWithValue("@UserRating", rating.UserRating);
                            PostCommand.ExecuteNonQuery();
                            return 201;
                        }
                    }
                }
            }
            //put her
            const string puttrackrating = "UPDATE Rating SET UserRating = @userrating where UserId = @userid AND TrackId = @trackid";
            using (var dbcon = new SqlConnection(GetConnectionString()))
            {
                dbcon.Open();
                using (dbcon)
                {
                    using (var sqlcommand = new SqlCommand(puttrackrating, dbcon))
                    {
                        sqlcommand.Parameters.AddWithValue("@userrating", rating.UserRating);
                        sqlcommand.Parameters.AddWithValue("@userid", rating.UserId);
                        sqlcommand.Parameters.AddWithValue("@trackid", rating.TrackId);
                        sqlcommand.ExecuteNonQuery();
                        return 201;
                    }
                }
            }
        }

        private Rating RatingReader(IDataRecord reader)
        {
            var Id = reader.GetInt32(0);
            var UserId = reader.GetInt32(1);
            var TrackId = reader.GetInt32(2);
            var UserRating = reader.GetInt32(3);

            var _rating = new Rating { Id = Id, UserId = UserId, TrackId = TrackId, UserRating = UserRating };
            return _rating;
        }

        private User UserReader(IDataRecord reader)
        {
            var Id = reader.GetInt32(0);
            var AuthToken = reader.GetString(1);
            var UserName = reader.GetString(2);
            //var Image = reader.GetByte(3);
            var Description = reader.GetString(4);
            var Privacy = reader.GetBoolean(5);

            var user = new User { Id = Id, AuthToken = AuthToken, UserName = UserName, Description = Description, Privacy = Privacy};
            return user;
        }


        private static string GetConnectionString()
        {
            var connectionStringSettingsCollection = ConfigurationManager.ConnectionStrings;
            var connectionStringSettings = connectionStringSettingsCollection["CykelDB"];
            return connectionStringSettings.ConnectionString;
        }

    }
}