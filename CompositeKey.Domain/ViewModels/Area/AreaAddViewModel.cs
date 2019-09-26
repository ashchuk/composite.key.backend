using System.ComponentModel.DataAnnotations;

namespace CompositeKey.Domain.ViewModels
{
    public class AreaAddViewModel
    {
        [Required, StringLength(200)]
        public string Name { get; set; }
    }
}
