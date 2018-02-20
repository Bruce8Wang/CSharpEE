using System;
using System.ComponentModel.DataAnnotations;

namespace ITSM.Models
{
    public class FeedBack
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string Company { get; set; }
        public string EMail { get; set; }
        public string Mobile { get; set; }
        public string Content { get; set; }
        public string FilePath { get; set; }
        public DateTime CreateTime { get; set; }

    }
}