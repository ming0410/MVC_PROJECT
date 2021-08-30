using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameoMvc.Areas.TEST.Models
{
    public class M_TEST001
    {
        public string Name { get; set; }
        public IEnumerable<SelectListItem> MyList { get; set; }
    }
}