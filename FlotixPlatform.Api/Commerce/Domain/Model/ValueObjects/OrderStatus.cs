using System.Text.Json.Serialization;

namespace FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;

/// <summary>
///     Represents the fulfillment status of an IoT order.
/// </summary>
/// <remarks>
///     Serialized with the Spanish names used by the Flotix front end and persisted by member name.
/// </remarks>
public enum OrderStatus
{
    /// <summary>
    ///     The order is waiting for its payment.
    /// </summary>
    [JsonStringEnumMemberName("Pendiente de pago")]
    PendingPayment,

    /// <summary>
    ///     The order is being prepared.
    /// </summary>
    [JsonStringEnumMemberName("En preparación")]
    Preparing,

    /// <summary>
    ///     The order has been delivered.
    /// </summary>
    [JsonStringEnumMemberName("Entregado")]
    Delivered,

    /// <summary>
    ///     The order has been cancelled.
    /// </summary>
    [JsonStringEnumMemberName("Cancelado")]
    Cancelled
}
