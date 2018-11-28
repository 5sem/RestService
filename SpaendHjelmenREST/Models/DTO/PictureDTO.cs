using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpaendHjelmenREST.Models.DTO
{
    public class PictureDTO
    {
        public virtual string Name { get; set; }
        public virtual string Image { get; set; }
        public virtual int TrackId { get; set; }

        public PictureDTO()
        {

        }
    }
}