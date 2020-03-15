using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBO.Inventory.DomainModels
{
    [Table("tblItemTypes")]
    public class ItemType
    {
        [Key]
        public long ItemId { get; set; }
        [StringLength(100)]
        public string ItemName { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationId { get; set; }
        public long? EUserId { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public long? UpUserId { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<Item> Items { get; set; }


        // Navigation Property

        [ForeignKey("Warehouse")]
        public long WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
