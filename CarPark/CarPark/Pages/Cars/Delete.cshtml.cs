using CarPark.Models;
using CarPark.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace CarPark.Pages.Cars
{
    public class DeleteModel : PageModel
    {
        private readonly CarService _service;

        public DeleteModel(CarService service)
        {
            _service = service;
        }

        [BindProperty]
        public Car Item { get; set; }

        public async Task OnGetAsync(string id)
        {
            Item = await _service.GetByIdAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _service.DeleteAsync(Item.Id);
            return RedirectToPage("Index");
        }
    }
}
