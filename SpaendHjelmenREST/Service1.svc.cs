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

namespace SpaendHjelmenREST
{
    public class Service1 : IService1
    {
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

        private static Picture ReadPicture(IDataRecord reader)
        {

            var Id = reader.GetInt32(0);
            var Name = reader.GetString(1);

            var blob = new byte[(reader.GetBytes(0, 0, null, 0, int.MaxValue))];
            reader.GetBytes(0, 0, blob, 0, blob.Length);

            var TrackId = reader.GetInt32(3);

            Picture t = new Picture(Id);

            t.Id = Id;
            t.Name = Name;
            t.Image = blob;
            t.TrackId = TrackId;

            return t;
        }

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
    }
}
