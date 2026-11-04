using FlotixPlatform.Api.Fuel.Domain.Repositories;
using FlotixPlatform.Api.Fuel.Interfaces.Acl;

namespace FlotixPlatform.Api.Fuel.Application.Acl;

/// <summary>
///     Implementation of the Fuel Control context facade.
/// </summary>
/// <param name="fuelRecordRepository">Repository for accessing fuel record data.</param>
public class FuelContextFacade(IFuelRecordRepository fuelRecordRepository) : IFuelContextFacade
{
    /// <inheritdoc />
    public async Task<IEnumerable<(Guid VehicleId, decimal Liters, decimal Cost)>> FetchFuelRecordsAsync(
        IReadOnlyCollection<Guid> vehicleIds, DateTimeOffset from, DateTimeOffset to,
        CancellationToken cancellationToken = default)
    {
        var fuelRecords = await fuelRecordRepository.FindByVehicleIdsAndRecordedDateRangeAsync(vehicleIds, from, to,
            cancellationToken);
        return fuelRecords.Select(fuelRecord => (fuelRecord.VehicleId, fuelRecord.Liters, fuelRecord.Cost));
    }
}
