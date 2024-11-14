using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Vfurniture.Reponsitory.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {

                var ext = Path.GetExtension(file.FileName);
                string[] _allowedExtensions = { "jpg", "jpeg", "png" };
                bool res = _allowedExtensions.Any(x => ext.EndsWith(x));
                if (!res)
                {
                    return new ValidationResult("Chỉ được phép có đuôi .jpg, .jpeg, .png, .gif.");
                }
            }
            return ValidationResult.Success;
        }
    }
}