using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RepertoireManagementWeb.Pages.BandPages; 
public class AddMemberModel : PageModel
{
    [BindProperty]
    public Guid BandId { get; set; }

    [BindProperty]
    public Guid UserId { get; set; }

    public List<SelectListItem> UserOptions { get; set; }

    public void OnGet(Guid bandId)
    {
        BandId = bandId;
        UserOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "User 1" },
                new SelectListItem { Value = Guid.NewGuid().ToString(), Text = "User 2" }
            };
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Logic to add the user to the band goes here  

        return RedirectToPage("Band");
    }
}
