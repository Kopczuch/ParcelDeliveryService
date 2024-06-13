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
        DeadlineOver,
        DeadlineExtended,
        InExternalStorage,
        Destroyed,
        Lost
    }
}
