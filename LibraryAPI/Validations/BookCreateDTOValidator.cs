using FluentValidation;
using LibraryAPI.DTOs;

namespace LibraryAPI.Validations
{
    public class BookCreateDTOValidator : AbstractValidator<BookCreateDTO>
    {
        public BookCreateDTOValidator()
        {
            // 1. شروط اسم الكتاب
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("اسم الكتاب مطلوب ولا يمكن أن يكون فارغاً.")
                .Length(3, 100).WithMessage("اسم الكتاب يجب أن يتكون من 3 إلى 100 حرف.");

            // 2. شروط رقم المؤلف
            RuleFor(x => x.AuthorId)
                .NotEmpty().WithMessage("رقم المؤلف مطلوب.")
                .GreaterThan(0).WithMessage("رقم المؤلف يجب أن يكون أكبر من الصفر.");

            // 3. شروط الصورة (لو تم إرسالها)
            RuleFor(x => x.ImageFile)
                // شرط الحجم: 10 ميجا بايت (بنحولها لـ بايت)
                .Must(file => file == null || file.Length <= 10 * 1024 * 1024)
                .WithMessage("حجم الصورة يجب ألا يتعدى 10 ميجا بايت.")

                // شرط الامتداد
                .Must(file =>
                {
                    if (file == null) return true; // لو مفيش صورة، عدي الشرط

                    var extension = Path.GetExtension(file.FileName).ToLower();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("صيغة الملف المرفوع غير مدعومة. مسموح فقط بصيغ: jpg, jpeg, png.");
        }
    }
} 