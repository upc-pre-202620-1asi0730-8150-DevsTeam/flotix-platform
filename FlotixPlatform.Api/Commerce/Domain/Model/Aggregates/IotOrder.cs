using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Commerce.Domain.Model.Aggregates;

/// <summary>
///     Represents an order of IoT devices placed by an owner.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateIotOrderCommand and updated through an UpdateIotOrderCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class IotOrder
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected IotOrder()
    {
    }

    /// <summary>
    ///     Creates a new IotOrder from a creation command.
    /// </summary>
    /// <param name="command">The CreateIotOrderCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public IotOrder(CreateIotOrderCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.OwnerId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.Quantity);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Quantity, 1000);
        ArgumentOutOfRangeException.ThrowIfNegative(command.TotalAmount);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.TotalAmount, 1000000m);

        OwnerId = command.OwnerId;
        Quantity = command.Quantity;
        TotalAmount = command.TotalAmount;
        Status = OrderStatus.PendingPayment;
        OrderDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this IoT order.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the owner user.
    /// </summary>
    public Guid OwnerId { get; private set; }

    /// <summary>
    ///     Gets the number of devices ordered.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    ///     Gets the total amount, priced from the catalog.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    ///     Gets the current status of the IoT order.
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    ///     Gets the date the order was placed.
    /// </summary>
    public DateTimeOffset OrderDate { get; private set; }

    /// <summary>
    ///     Updates the IoT order information from an update command.
    /// </summary>
    /// <param name="command">The UpdateIotOrderCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the update breaks the rule: A delivered or cancelled order cannot change its status.</exception>
    public void UpdateInformation(UpdateIotOrderCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled && command.Status != Status)
            throw new InvalidOperationException("A delivered or cancelled order cannot change its status.");

        Status = command.Status;
    }
}
