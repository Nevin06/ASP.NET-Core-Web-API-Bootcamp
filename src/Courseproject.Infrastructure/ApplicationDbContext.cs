using Courseproject.Common.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Courseproject.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { 
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite("Filename=Courseproject.db"); //89
        base.OnConfiguring(optionsBuilder);
        //89
        //Hard coding the filename of our db, need to make it dynamic so that we can change values 
        //without having to recompile the whole application => use environment variables
        //configure environment variables to Azure
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Address>().HasKey(e => e.Id); //Primary Key
        builder.Entity<Employee>().HasKey(e => e.Id);
        builder.Entity<Team>().HasKey(e => e.Id);
        builder.Entity<Job>().HasKey(e => e.Id);

        //relations
        //EF takes care of everything needed in the db to represent this relationship in the db
        builder.Entity<Employee>().HasOne(e => e.Address);
        builder.Entity<Employee>().HasOne(e => e.Job);
        builder.Entity<Team>().HasMany(e => e.Employees).WithMany(e => e.Teams);

        base.OnModelCreating(builder); 
    }
}