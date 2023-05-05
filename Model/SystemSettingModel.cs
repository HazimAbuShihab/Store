using Store_Core7.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_Core7.Model
{
    [Table("System_Settings")]
    public class SystemSettingModel : IEntity
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
        [Column("Key")]
        [Required]
        public string? Key { get; set; }
        [Column("Value")]
        [Required]
        public string? Value { get; set; }
        [Column("IsActive")]
        public bool IsActive { get; set; }
        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

    }
}
