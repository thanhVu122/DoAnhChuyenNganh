using System.ComponentModel.DataAnnotations;

namespace Vfurniture.Reponsitory.Validation
{
    public class fileExtentionsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var ext = Path.GetExtension(file.FileName);
                string[] extension = { "jpg", "png", "jpeg" };
                bool res = extension.Any(x => ext.EndsWith(x));
                if (!res)
                {
                    return new ValidationResult("phần mở rộng được phép là ");
                }
            }
            return ValidationResult.Success;
        }
    }
}
