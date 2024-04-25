using ParcelDeliveryService.Core;

namespace ParcelDeliveryService.Models
{
    public class Slot
    {
        public int Id { get; set; }
        public Size Size { get; set; }
        public int? ParcelId { get; set; }
        public VacancyState Vacancy { get; set; }
    }
}
