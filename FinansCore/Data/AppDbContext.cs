
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using SQLite;

using YurtDisiKazancHesapla.Models;

namespace DTO;

public class AppDbContext : DbContext
{
    //C:\Users\Admin\AppData\Local\Packages\1C7563CB-A2B7-41D9-BFEC-B9C75462F0CF_9zz4h110yvjzm\LocalCache\Local
    public AppDbContext() : base()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //SQLitePCL.Batteries_V2.Init();
        //       this.Database.EnsureDeleted();
        //       this.Database.EnsureCreated();
        //       RelationalDatabaseCreator databaseCreator =
        //(RelationalDatabaseCreator)this.Database.GetService<IDatabaseCreator>();
        //       databaseCreator.();
        //       this.Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    //use sql server

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var sqlitePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FinansDb.db");
        optionsBuilder.UseSqlite($"Data Source={sqlitePath} ");
    }


    public DbSet<Hisse> Hisseler { get; set; }
    public DbSet<Satis> Satislar { get; set; }
    public DbSet<Alis> Alislar { get; set; }
}