using Store_Core7.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_Core7.Model
{
    [Table("Categories")]
    public class CategoryModel : IEntity
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
        [Column("CategoryName")]
        [StringLength(50)]
        public string? CategoryName { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; }
        public ICollection<ProductModel>? products { get; set; }
    }
}
