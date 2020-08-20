﻿using System;

namespace TravelBug.Entities
{
    public class Image : Base
    {
        //public Guid ImageId { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public bool Main { get; set; }
        //public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}