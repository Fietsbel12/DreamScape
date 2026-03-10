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
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<TradeItem> TradeItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=dreamscape;user=root;password=",
                ServerVersion.Parse("8.0.30")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Inventory)
                .WithOne(i => i.User);

            modelBuilder.Entity<Inventory>()
                .HasMany(i => i.InventoryItems)
                .WithOne(ii => ii.Inventory);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.InventoryItems)
                .WithOne(ii => ii.Item);

            modelBuilder.Entity<Trade>()
                .HasMany(t => t.TradeItems)
                .WithOne(ti => ti.Trade);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.TradeItems)
                .WithOne(ti => ti.Item);

            // Roles data
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Player" },
                new Role { RoleId = 2, Name = "Admin" }
            );

            // Users data
            modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, Username = "Admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"), RoleId = 2 },
            new User { UserId = 2, Username = "Fietsbel12", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ewa123"), RoleId = 1 },
            new User { UserId = 3, Username = "DragonSlayer", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Dragon123"), RoleId = 1 },
            new User { UserId = 4, Username = "ShadowWolf", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Wolf123"), RoleId = 1 },
            new User { UserId = 5, Username = "StarForge", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Star123"), RoleId = 1 }
        );

            // Items data
            modelBuilder.Entity<Item>().HasData(
                new Item { ItemId = 1, Name = "Iron Sword", Description = "Just a basic sword", Type = "Weapon", Rarity = "Common", Strength = 40, Speed = 20, Durability = 60, MagicalProperty = "None" },
                new Item { ItemId = 2, Name = "Magic Wand", Description = "A wand infused with magic.", Type = "Weapon", Rarity = "Rare", Strength = 20, Speed = 40, Durability = 30, MagicalProperty = "Fire" },
                new Item { ItemId = 3, Name = "Shadow Dagger", Description = "A blade forged in darkness.", Type = "Weapon", Rarity = "Uncommon", Strength = 35, Speed = 55, Durability = 25, MagicalProperty = "Shadow" },
                new Item { ItemId = 4, Name = "Crystal Staff", Description = "Channels arcane energy.", Type = "Weapon", Rarity = "Epic", Strength = 15, Speed = 30, Durability = 40, MagicalProperty = "Ice" },
                new Item { ItemId = 5, Name = "Thunder Axe", Description = "Crackles with lightning.", Type = "Weapon", Rarity = "Rare", Strength = 60, Speed = 15, Durability = 70, MagicalProperty = "Lightning" },
                new Item { ItemId = 6, Name = "Iron Shield", Description = "A sturdy shield.", Type = "Armor", Rarity = "Common", Strength = 10, Speed = 5, Durability = 90, MagicalProperty = "None" },
                new Item { ItemId = 7, Name = "Enchanted Robe", Description = "Glows with a faint blue light.", Type = "Armor", Rarity = "Uncommon", Strength = 5, Speed = 25, Durability = 35, MagicalProperty = "Arcane" },
                new Item { ItemId = 8, Name = "Dragon Scale Armor", Description = "Forged from dragon scales.", Type = "Armor", Rarity = "Legendary", Strength = 50, Speed = 20, Durability = 100, MagicalProperty = "Fire" },
                new Item { ItemId = 9, Name = "Health Potion", Description = "Restores health instantly.", Type = "Potion", Rarity = "Common", Strength = 0, Speed = 0, Durability = 1, MagicalProperty = "Healing" },
                new Item { ItemId = 10, Name = "Speed Elixir", Description = "Temporarily boosts speed.", Type = "Potion", Rarity = "Uncommon", Strength = 0, Speed = 50, Durability = 1, MagicalProperty = "Haste" }
            );

            // Inventory data
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory { InventoryId = 1, UserId = 1 },
                new Inventory { InventoryId = 2, UserId = 2 },
                new Inventory { InventoryId = 3, UserId = 3 },
                new Inventory { InventoryId = 4, UserId = 4 },
                new Inventory { InventoryId = 5, UserId = 5 }
            );

            // InventoryItem data
            modelBuilder.Entity<InventoryItem>().HasData(
                new InventoryItem { InventoryItemId = 1, InventoryId = 1, ItemId = 1, Quantity = 1 }, // Admin       → Iron Sword
                new InventoryItem { InventoryItemId = 2, InventoryId = 2, ItemId = 2, Quantity = 1 }, // Fietsbel12  → Magic Wand
                new InventoryItem { InventoryItemId = 3, InventoryId = 2, ItemId = 9, Quantity = 3 }, // Fietsbel12  → Health Potion x3
                new InventoryItem { InventoryItemId = 4, InventoryId = 3, ItemId = 3, Quantity = 1 }, // DragonSlayer → Shadow Dagger
                new InventoryItem { InventoryItemId = 5, InventoryId = 3, ItemId = 6, Quantity = 1 }, // DragonSlayer → Iron Shield
                new InventoryItem { InventoryItemId = 6, InventoryId = 4, ItemId = 5, Quantity = 1 }, // ShadowWolf  → Thunder Axe
                new InventoryItem { InventoryItemId = 7, InventoryId = 4, ItemId = 10, Quantity = 2 }, // ShadowWolf  → Speed Elixir x2
                new InventoryItem { InventoryItemId = 8, InventoryId = 5, ItemId = 4, Quantity = 1 }, // StarForge   → Crystal Staff
                new InventoryItem { InventoryItemId = 9, InventoryId = 5, ItemId = 7, Quantity = 1 }, // StarForge   → Enchanted Robe
                new InventoryItem { InventoryItemId = 10, InventoryId = 5, ItemId = 8, Quantity = 1 }  // StarForge   → Dragon Scale Armor
            );

            modelBuilder.Entity<Trade>().HasData(
                new Trade
                {
                    TradeId = 1,
                    SenderId = 2,   // Fietsbel12
                    ReceiverId = 1, // Admin
                    Status = TradeStatus.Pending
                },
                new Trade
                {
                    TradeId = 2,
                    SenderId = 3,   // DragonSlayer
                    ReceiverId = 1, // Admin
                    Status = TradeStatus.Pending
                }
            );
        }
    }
}
