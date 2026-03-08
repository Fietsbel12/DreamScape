using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<TradeItem> TradeItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=dreamscape;user=root;password=",
                ServerVersion.Parse("8.0.30")
            );
        }

        // Roles data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<Role>().HasData(
                    new Role { RoleId = 1, Name = "Player" },
                    new Role { RoleId = 2, Name = "Admin" }
                );

            // Users data
                modelBuilder.Entity<User>().HasData(
                    new User { UserId = 1, Username = "Admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"), RoleId = 1 },
                    new User { UserId = 2, Username = "Fietsbel12", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ewa123"), RoleId = 2 }
                );

            // Items data
                modelBuilder.Entity<Item>().HasData(
                    new Item 
                    { 
                        ItemId = 1, 
                        Name = "Iron Sword", 
                        Description = "Just a basic sword", 
                        Type = "Weapon", 
                        Rarity = "Common", 
                        Strength = 40, 
                        Speed = 20,
                        Durability = 60,
                        MagicalProperty = "None"
                    },
                    new Item
                    {
                        ItemId = 2,
                        Name = "Magic Wand",
                        Description = "A wand infused with magic.",
                        Type = "Weapon",
                        Rarity = "Rare",
                        Strength = 20,
                        Speed = 40,
                        Durability = 30,
                        MagicalProperty = "Fire"
                    }
                );

                modelBuilder.Entity<Inventory>().HasData(
                    new Inventory
                    {
                        InventoryId = 2,
                        UserId = 2,
                        ItemId = 2,
                        Quantity = 1
                    }
                );

        }



    }
}
