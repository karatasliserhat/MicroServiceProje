using Microsoft.Build.Framework;
using System.ComponentModel;

namespace Course.Web.Models.Courses
{
    public class FeatureViewModel
    {
        [DisplayName("Kurs Süresi")]

        [Required]
        public int Duration { get; set; }
    }
}
