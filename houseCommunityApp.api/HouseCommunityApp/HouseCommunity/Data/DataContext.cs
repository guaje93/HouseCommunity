﻿using HouseCommunity.Model;
using Microsoft.EntityFrameworkCore;

namespace HouseCommunity.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Flat> Flats { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Media> MediaHistory { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<HousingDevelopment> HousingDevelopments { get; set; }
        public DbSet<UserAnnouncement> UserAnnouncements { get; set; }

    }
}
