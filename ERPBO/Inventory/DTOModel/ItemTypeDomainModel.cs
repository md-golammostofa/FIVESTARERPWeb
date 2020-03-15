using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DTOModel
{
   public class ItemTypeDTO
    {
        public long ItemId { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long WarehouseId { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }

        // Custom Property
        [StringLength(10)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string WarehouseName { get; set; }
    }
}
