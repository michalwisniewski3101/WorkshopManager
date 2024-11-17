namespace WorkshopManager.api.Repos.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(Guid id);
        Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
        Task<Guid> GetOrAddVehicleAsync(string vin, string make, string model, int year, string licensePlate);
    }
}
