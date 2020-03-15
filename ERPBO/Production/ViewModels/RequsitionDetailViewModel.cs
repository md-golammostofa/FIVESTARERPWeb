using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
   public class RequsitionDetailViewModel
    {
        public long ReqDetailId { get; set; }
        [Range(1,long.MaxValue)]
        public long ItemTypeId { get; set; }
        [Range(1, long.MaxValue)]
        public long ItemId { get; set; }
        [Range(1, long.MaxValue)]
        public long UnitId { get; set; }
        [Range(1, long.MaxValue)]
        public long Quantity { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        //navi
        [Range(1, long.MaxValue)]
        public long ReqInfoId { get; set; }

        //custom
        [StringLength(100)]
        public string ItemTypeName { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(100)]
        public string UnitName { get; set; }
    }
}
