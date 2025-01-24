using CarPark.Exceptions;
using CarPark.Models;
using CarPark.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarPark.Pages.Cars
{
    public class CreateModel : PageModel
    {
        private readonly CarService _service;

        public CreateModel(CarService service)
        {
            _service = service;
        }

        [BindProperty]
        public Car Item { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _service.AddAsync(Item);
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
