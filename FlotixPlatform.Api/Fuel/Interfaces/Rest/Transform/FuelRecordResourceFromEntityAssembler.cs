using FlotixPlatform.Api.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.Api.Fuel.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Fuel.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a FuelRecordResource from a FuelRecord.
/// </summary>
public static class FuelRecordResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a FuelRecordResource from a FuelRecord.
    /// </summary>
    /// <param name="entity">The FuelRecord entity</param>
    /// <returns>
    ///     A FuelRecordResource assembled from the FuelRecord
    /// </returns>
    public static FuelRecordResource ToResourceFromEntity(FuelRecord entity) =>
        new(entity.Id, entity.VehicleId, entity.DriverId, entity.Liters, entity.Cost, entity.MileageAtRefill, entity.RecordedDate);
}
