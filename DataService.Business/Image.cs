﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.Business
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int  Width { get; set; }
        public int Height { get; set; }
        public string Extension { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
