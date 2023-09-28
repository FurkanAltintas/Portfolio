﻿using Portfolio.Core.Entities.Abstract;
using Portfolio.Core.Entities.Concrete;

namespace Portfolio.Entities.Concrete
{
    public class Testimonial : BaseEntity<int>, IEntity
    {
        public string FullName { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}