using Store_Core7.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_Core7.Model
{
    [Table("Orders")]
    public class OrderModel : IEntity
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
        [Column("GUID")]
        public string? GUID { get; set; }
        [Column("IsDone")]
        public bool IsDone { get; set; }
        [Column("Price")]
        public decimal Price { get; set; }
        [Column("NewLatitude")]
        public string? NewLatitude { get; set; }
        [Column("NewLongitude")]
        public string? NewLongitude { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public string UserId { get; set; }
        public UserModel User { get; set; }
        [ForeignKey("ProductId")]
        [Required]
        public long ProductId { get; set; }
        public ProductModel Product { get; set; }
        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}
