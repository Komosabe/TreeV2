using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TreeV2.NodeDto
{
    public class EditNodeDto
    {
        [Required]
        public int? SelectedNodeId { get; set; }
        [Required(ErrorMessage = "Please, enter the name")]
        [StringLength(20, MinimumLength = 2)]
        public string? Name { get; set; }
        public string? ParentNode { get; set; }

        public List<SelectListItem>? Parent { get; set; }
    }
}
