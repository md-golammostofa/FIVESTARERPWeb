using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Production.ViewModels
{
   public class VmReqDetails
    {
        public long ReqDetailId { get; set; }
        [Range(1,long.MaxValue)]
        public long? ItemTypeId { get; set; }
        [StringLength(100)]
        public string ItemType { get; set; }
        [Range(1, long.MaxValue)]
        public long? ItemId { get; set; }
        [StringLength(100)]
        public string Item { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        //[Range(1, long.MaxValue)]
        public long? UnitId { get; set; }
        [StringLength(100)]
        public string UnitSymbol { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public long OrganizationId { get; set; }
        [Range(1, long.MaxValue)]
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        [Range(1, long.MaxValue)]
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
    public class VmReqInfo
    {
        public long ReqInfoId { get; set; }
        [Range(1, long.MaxValue)]
        public long? LineId { get; set; }
        public string LineNumber { get; set; }
        [Range(1, long.MaxValue)]
        public long? WarehouseId { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
        public List<VmReqDetails> ReqDetails { get; set; }
    }
}
