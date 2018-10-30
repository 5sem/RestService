using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
            const string sqlString = "SELECT * FROM Tracks";

            using(SqlConnection sqlConnection = new SqlConnection(GetConnectionString()))
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
            t.Colorcode = ColorCode;
            t.Length = Lenght;
            t.MaxHeight = MaxHeight;
            t.ParkInfo = ParkInfo;
            t.Regional = Regional;

            return t;
        }

        private static string GetConnectionString()
        {
            var connectionStringSettingsCollection = ConfigurationManager.ConnectionStrings;
            var connectionStringSettings = connectionStringSettingsCollection["CykelDB"];
            return connectionStringSettings.ConnectionString;
        }
    }
}
