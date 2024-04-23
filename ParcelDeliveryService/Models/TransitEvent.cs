using System.Net;
using System.Net.Sockets;

namespace ParcelDeliveryService.Models
{
    public class TransitEvent
    {
        public DateTime TimeStamp { get; set; }
        public Address Location { get; set; }
        public TransitEvent Type { get; set; }
    }
}
