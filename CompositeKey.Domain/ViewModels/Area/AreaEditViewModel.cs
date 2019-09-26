using System.ComponentModel.DataAnnotations;

namespace CompositeKey.Domain.ViewModels
{
    public class AreaEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }
    }
}
