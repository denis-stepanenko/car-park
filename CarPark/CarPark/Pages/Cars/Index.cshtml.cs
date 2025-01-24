using CarPark.Models;
using CarPark.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarPark.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly CarService _service;

        public IndexModel(CarService service)
        {
            _service = service;
        }

        public PaginatedResult<Car> Data { get; set; }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            Data = await _service.GetPaginatedAsync(pageNumber);
        }
    }
}
