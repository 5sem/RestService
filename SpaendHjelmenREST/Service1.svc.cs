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
            var PictureId = reader.GetInt32(1);
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

            t.PictureId = PictureId;
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


        private static string GetConnectionString()
        {
            var connectionStringSettingsCollection = ConfigurationManager.ConnectionStrings;
            var connectionStringSettings = connectionStringSettingsCollection["CykelDB"];
            return connectionStringSettings.ConnectionString;
        }

        private byte[] ByteConverter(string filepath)
        {
            byte[] Rtn = File.ReadAllBytes(filepath);
            return Rtn;
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

        public void Mikker(PictureDTO pictureDTO)
        {


            const string postpicturesql =
                "INSERT INTO Pictures (Name, Image, TrackId) VALUES (@Name, @Image, @TrackId)";
            using (var DBConnection = new SqlConnection(GetConnectionString()))
            {
                DBConnection.Open();
                using (var SqlCommand = new SqlCommand(postpicturesql, DBConnection))
                {
                    SqlCommand.Parameters.AddWithValue("@Name", pictureDTO.Name);


                    byte[] PostImage = Convert.FromBase64String(pictureDTO.Image);

                    SqlCommand.Parameters.AddWithValue("@Image", PostImage);
                    SqlCommand.Parameters.AddWithValue("@TrackId", pictureDTO.TrackId);
                    SqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
