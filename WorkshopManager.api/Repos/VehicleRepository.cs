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

        // Pobierz wszystkie pojazdy
        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // Pobierz pojazd po ID
        public async Task<Vehicle> GetVehicleByIdAsync(Guid id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        // Dodaj nowy pojazd
        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        // Sprawdź, czy pojazd o danym VIN istnieje, jeśli nie, dodaj nowy
        public async Task<Guid> GetOrAddVehicleAsync(string vin, string make, string model, int year, string licensePlate)
        {
            // Sprawdzenie, czy pojazd o danym VIN istnieje
            var existingVehicle = await _context.Vehicles
                .Where(v => v.VIN == vin)  // Szukamy po VIN
                .FirstOrDefaultAsync();

            if (existingVehicle != null)
            {
                // Pojazd istnieje, zwrócimy jego ID
                return existingVehicle.Id;
            }
            else
            {
                // Pojazd nie istnieje, tworzymy nowy
                var newVehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),  // Tworzymy nowe ID
                    VIN = vin,
                    Make = make,
                    Model = model,
                    Year = year,
                    LicensePlate = licensePlate
                };

                await _context.Vehicles.AddAsync(newVehicle);
                await _context.SaveChangesAsync();

                // Zwracamy ID nowo dodanego pojazdu
                return newVehicle.Id;
            }
        }
    }
}
