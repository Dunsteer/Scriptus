using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Models.Shared
{
    public class PagerBase
    {
        public PagerBase()
        {
        }

        public bool SortOrder { get; set; } = true;
        public string SortMember { get; set; }

        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int TotalCount { get; set; }

        public string Term { get; set; }
    }
}
