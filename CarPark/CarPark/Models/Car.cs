using System.ComponentModel.DataAnnotations;

namespace CarPark.Models
{
    public class Car : BaseModel
    {
        [Required(ErrorMessage = "Введите наименование")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Введите производителя")]
        public required string Make { get; set; }
    }
}
