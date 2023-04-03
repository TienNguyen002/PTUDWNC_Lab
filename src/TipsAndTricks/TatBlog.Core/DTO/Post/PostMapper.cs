﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO.Post
{
    public class PostMapper
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string UrlSlug { get; set; }
        public string CategoryName { get; set; }
        public string ViewCount { get; set; }
        public string Tags { get; set; }

        public override string ToString()
        {
            return string.Format("{0,-5}{1,-20}{2,-30}{3,-20}{4,10}",
                Id, Title, ShortDescription, UrlSlug, ViewCount);
        }
    }
}