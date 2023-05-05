using Store_Core7.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store_Core7.Model
{
    [Table("ExceptionLogs")]
    public class LogModel : IEntity
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
        [Column("ControllerName")]
        public string? ControllerName { get; set; }
        [Column("ErrorMessage")]
        public string? ErrorMessage { get; set; }
        [Column("StackTrace")]
        public string? StackTrace { get; set; }
        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}
