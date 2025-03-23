using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkshopManager.api.Repos.Interfaces;

namespace WorkshopManager.api.Repos
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly WorkshopContext _context;

        public VehicleRepository(WorkshopContext context)
        {
            _context = context;
        }

     
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

       
        public async Task<Vehicle> GetVehicleByIdAsync(Guid id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

    
        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }


        public async Task<Guid> GetOrAddVehicleAsync(string vin, string make, string model, int year, string licensePlate)
        {
           
            var existingVehicle = await _context.Vehicles
                .Where(v => v.VIN == vin)  
                .FirstOrDefaultAsync();

            if (existingVehicle != null)
            {
            
                return existingVehicle.Id;
            }
            else
            {
               
                var newVehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),  
                    VIN = vin,
                    Make = make,
                    Model = model,
                    Year = year,
                    LicensePlate = licensePlate
                };

                await _context.Vehicles.AddAsync(newVehicle);
                await _context.SaveChangesAsync();

                return newVehicle.Id;
            }
        }
    }
}
