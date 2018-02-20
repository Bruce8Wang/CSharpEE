using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    public class SuperUser
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }

        //权限等级
        public long PermLevl { get; set; }
    }
}