using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Course.Web.Models.Courses
{
    public class CourseCreateInput
    {

        [DisplayName("Kurs Adı")]

        [Required]
        public string Name { get; set; }

        [DisplayName("Kurs Açıklama")]

        [Required]
        public string Description { get; set; }

        [DisplayName("Kurs Ücreti")]

        [Required]
        public decimal Price { get; set; }

        public string UserId { get; set; }
        public string Picture { get; set; }
        public FeatureViewModel Feature { get; set; }

        [DisplayName("Kategori Adı")]

        [Required]
        public string CategoryId { get; set; }

        [Display(Name ="Kurs Resmi")]
        public IFormFile PhotoUrlFile { get; set; }
    }
}
