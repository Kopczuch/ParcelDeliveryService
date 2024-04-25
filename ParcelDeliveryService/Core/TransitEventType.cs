namespace ParcelDeliveryService.Core
{
    public enum TransitEventType
    {
        Registered,
        Deposited,
        ReceivedFromSenderLocker,
        InStorage,
        InTransit,
        ReadyForPickUp,
        PickedUp,
        DeadlineApproaches,
        DeadlineOver,
        InExternalStorage,
        Destroyed,
        Lost
    }
}
