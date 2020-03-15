using ERPBO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Common
{
    public class Utility
    {
        public static IEnumerable<Dropdown> ListOfReqStatus()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>() {
                new Dropdown{text="Pending",value="Pending" },
                new Dropdown{text="Recheck",value="Recheck" },
                new Dropdown{text="Approved",value="Approved" },
                new Dropdown{text="Accepted",value="Accepted" },
                new Dropdown{text="Decline",value="Decline" }
            };
            return dropdowns;
        }
    }
}
