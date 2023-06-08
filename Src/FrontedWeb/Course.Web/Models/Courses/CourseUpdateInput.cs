using System.ComponentModel.DataAnnotations;

namespace Course.Web.Models.Courses
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }

        [Display(Name = "Kurs Adı")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Kurs Açıklama")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Kurs Ücreti")]
        [Required]
        public decimal Price { get; set; }

        public string UserId { get; set; }
        public string Picture { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name="Kurs Resmi")]
        public IFormFile PhotoUrlFile { get; set; }

        [Display(Name = "Kategori Adı")]
        [Required]
        public string CategoryId { get; set; }
    }
}
