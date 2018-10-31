using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models
{
    public class Track
    {
        public virtual int Id { get; set; }
        public virtual int PictureId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Info { get; set; }
        public virtual double Longitude { get; set; }
        public virtual double Latitude { get; set; }
        public virtual int PostalCode { get; set; }
        public virtual string City { get; set; }
        public virtual string Address { get; set; }
        public virtual string Colorcode { get; set; }
        public virtual double Length { get; set; }
        public virtual double MaxHeight { get; set; }
        public virtual string ParkInfo { get; set; }
        public virtual string Regional { get; set; }


        public Track()
        {

        }

        public Track(int id)
        {
            this.Id = id;
        }
    }
}