using System.Collections.Generic;

namespace CompositeKey.Domain.ViewModels
{
    public class ErrorResponseViewModel
    {
        public string ErrorCode { get; set; }
        public string Description { get; set; }
        public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
