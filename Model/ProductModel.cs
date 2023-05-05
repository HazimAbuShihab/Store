using Store_Core7.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_Core7.Model
{
    [Table("Products")]
    public class ProductModel : IEntity
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
        [Column("ProductName")]
        public string? ProductName { get; set; }
        [Column("ProductBarCode")]
        public string? ProductBarCode { get; set; }
        [Column("ProductPrice")]
        public decimal ProductPrice { get; set; }
        [Column("ProductImageName")]
        public string? ProductImageName { get; set; }
        [Column("OldPrice")]
        public Nullable<decimal> OldPrice { get; set; }
        [Column("IsOffered")]
        public Nullable<bool> IsOffered { get; set; }
        [Column("PercentageDiscount")]
        public Nullable<int> PercentageDiscount { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; }
        [ForeignKey("CategoryId")]
        public long CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedOn")]
        public Nullable<DateTime> UpdatedOn { get; set; }
        public ICollection<OrderModel>? orders { get; set; }
    }
}