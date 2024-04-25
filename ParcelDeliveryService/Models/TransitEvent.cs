using ParcelDeliveryService.Core;

namespace ParcelDeliveryService.Models
{
    public class TransitEvent
    {
        public DateTime TimeStamp { get; set; }
        public string Location { get; set; }
        public TransitEventType Type { get; set; }
    }
}
