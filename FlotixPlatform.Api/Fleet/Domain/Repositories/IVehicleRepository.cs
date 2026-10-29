using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Domain.Model.ValueObjects;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Fleet.Domain.Repositories;

/// <summary>
///     The vehicle repository contract.
/// </summary>
public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    /// <summary>
    ///     Find vehicles by owner ID
    /// </summary>
    /// <param name="ownerId">The owner ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the vehicle objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Vehicle>> FindByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find vehicles by driver ID
    /// </summary>
    /// <param name="driverId">The driver ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the vehicle objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Vehicle>> FindByDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Check whether a vehicle with the given license plate exists
    /// </summary>
    /// <param name="licensePlate">The license plate to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     True if a vehicle with the license plate exists, false otherwise.
    /// </returns>
    Task<bool> ExistsByLicensePlateAsync(LicensePlate licensePlate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find a vehicle by license plate
    /// </summary>
    /// <param name="licensePlate">The license plate to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     The vehicle object if found, or null otherwise.
    /// </returns>
    Task<Vehicle?> FindByLicensePlateAsync(LicensePlate licensePlate, CancellationToken cancellationToken = default);
}