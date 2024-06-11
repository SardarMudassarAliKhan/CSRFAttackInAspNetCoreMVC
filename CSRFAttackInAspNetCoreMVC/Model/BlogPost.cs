using System.ComponentModel.DataAnnotations;

namespace CSRFAttackInAspNetCoreMVC.Model
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }
        public DateTime Created { get; set; }
    }
}
