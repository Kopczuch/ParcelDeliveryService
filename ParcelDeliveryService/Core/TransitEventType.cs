namespace ParcelDeliveryService.Core
{
    public enum TransitEventType
    {
        ReadyToBeSent,
        SentFromParcelLocker,
        SentFromParcelStorage,
        ReceivedByCourier,
        InTransit,
        InStorage,
        ReadyForDelivery,
        ReadyForPickUp,
        PickedUp,
        DeadlineApproaches,
        DeadlineOver,
        ReturningToStorage,
        ReturningToSender,
        InExternalStorage,
        Destroyed,
        Lost
    }
}
