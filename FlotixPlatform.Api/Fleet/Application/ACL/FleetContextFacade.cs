using FlotixPlatform.Api.Fleet.Domain.Repositories;
using FlotixPlatform.Api.Fleet.Interfaces.ACL;

namespace FlotixPlatform.API.Fleet.Application.ACL;

/// <summary>
///     Implementation of the Vehicle &amp; Fleet Management context facade.
/// </summary>
/// <param name="vehicleRepository">Repository for accessing vehicle data.</param>
public class FleetContextFacade(IVehicleRepository vehicleRepository) : IFleetContextFacade
{
    /// <inheritdoc />
    public async Task<IReadOnlyDictionary<Guid, string>> FetchLicensePlatesByOwnerIdAsync(Guid ownerId,
        CancellationToken cancellationToken = default)
    {
        var vehicles = await vehicleRepository.FindByOwnerIdAsync(ownerId, cancellationToken);
        return vehicles.ToDictionary(vehicle => vehicle.Id, vehicle => vehicle.LicensePlate.Value);
    }
}