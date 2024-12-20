using System.ComponentModel.DataAnnotations;

namespace SuParty.Models
{
    public class SampleModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
