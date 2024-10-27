using Microsoft.EntityFrameworkCore;
using WorkshopManager.api.Database;

public class WorkshopContext : DbContext
{
    public WorkshopContext(DbContextOptions<WorkshopContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceSchedule> ServiceSchedules { get; set; }
    public DbSet<Mechanic> Mechanics { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<MechanicSpecialty> MechanicSpecialties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MechanicSpecialty>().ToTable("dict_MechanicSpecialties");
    }
}
