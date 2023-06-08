using Course.Web.Models.Courses;
using FluentValidation;

namespace Course.Web.Validator
{
    public class CourseUpdateInputValidator : AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {

            RuleFor(x => x.Id).NotEmpty().WithMessage("Kurs Id boş geçilemez");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kurs Adını Giriniz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Kurs Açıklaması Giriniz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).NotEmpty().WithMessage("Kurs Süresi Boş Olamaz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Kurs Ücreti Boş Olamaz..").ScalePrecision(2, 6).WithMessage("Kurs Ücretini uygun formatta giriniz");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori Seçiniz");

        }
    }
}
