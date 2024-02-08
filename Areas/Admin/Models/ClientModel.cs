using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeProjectCore.Areas.Admin.Models
{
    [Table("ClientTable")]
 
    public class ClientModel
    {
        [Key]
        public int ClientId {  get; set; }
        public string? ClientName { get; set; }
        public string? ClientAddress { get; set; }

        public string? ClientEmail { get; set; }
        public string? ClientPassword { get; set; }
        public string? ClientContact { get; set; }
        public string? ClientContactA { get; set; }
        public string? ClientLogo { get; set; }
        public DateTime ClientDate { get; set; }

        public int ClientActive { get; set; }
    }
}
