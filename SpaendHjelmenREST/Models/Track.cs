using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Track
    {
        public int Id { get; set; }
        public int PictureId { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Address { get; set; }
        public string Colorcode { get; set; }
        public double Length { get; set; }
        public double MaxHeight { get; set; }
        public string ParkInfo { get; set; }
        public string Regional { get; set; }

    }
}