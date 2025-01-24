using CarPark.Exceptions;
using CarPark.Models;
using CarPark.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarPark.Pages.Cars
{
    public class EditModel : PageModel
    {
        private readonly CarService _service;

        public EditModel(CarService service)
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
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.UpdateAsync(Item);
            }
            catch (BusinessLogicException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
            
            return RedirectToPage("Index");
        }
    }
}
