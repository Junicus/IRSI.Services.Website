using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.Models.AVT
{
    public class AVTPaged
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public IEnumerable<AVTPagedItem> Avts { get; set; }
        public string _Prev { get; set; }
        public string _Next { get; set; }
    }
}
