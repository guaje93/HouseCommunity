﻿// <auto-generated />
using System;
using HouseCommunity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HouseCommunity.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201220205929_Add damage")]
    partial class Adddamage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HouseCommunity.Model.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("HouseCommunity.Model.Announcement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("HouseCommunity.Model.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<double>("ColdWaterEstimatedUsageForOneHuman")
                        .HasColumnType("float");

                    b.Property<int?>("CostId")
                        .HasColumnType("int");

                    b.Property<double>("HeatingEstimatedUsageForOneHuman")
                        .HasColumnType("float");

                    b.Property<double>("HotWaterEstimatedUsageForOneHuman")
                        .HasColumnType("float");

                    b.Property<int?>("HouseManagerId")
                        .HasColumnType("int");

                    b.Property<int>("HousingDevelopmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CostId");

                    b.HasIndex("HouseManagerId");

                    b.HasIndex("HousingDevelopmentId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("HouseCommunity.Model.Cost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AdministrationUnitCost")
                        .HasColumnType("float");

                    b.Property<double>("ColdWaterUnitCost")
                        .HasColumnType("float");

                    b.Property<double>("GarbageUnitCost")
                        .HasColumnType("float");

                    b.Property<double>("HeatingUnitCost")
                        .HasColumnType("float");

                    b.Property<double>("HotWaterUnitCost")
                        .HasColumnType("float");

                    b.Property<double>("OperatingUnitCost")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Cost");
                });

            modelBuilder.Entity("HouseCommunity.Model.Damage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BuildingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RequestCreatorId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("RequestCreatorId");

                    b.ToTable("Damages");
                });

            modelBuilder.Entity("HouseCommunity.Model.Flat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Area")
                        .HasColumnType("float");

                    b.Property<int>("BuildingId")
                        .HasColumnType("int");

                    b.Property<double>("ColdWaterEstimatedUsage")
                        .HasColumnType("float");

                    b.Property<string>("FlatNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("HeatingEstimatedUsage")
                        .HasColumnType("float");

                    b.Property<double>("HotWaterEstimatedUsage")
                        .HasColumnType("float");

                    b.Property<int>("ResidentsAmount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.ToTable("Flats");
                });

            modelBuilder.Entity("HouseCommunity.Model.HousingDevelopment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HousingDevelopments");
                });

            modelBuilder.Entity("HouseCommunity.Model.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AcceptanceDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("CurrentValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("EndPeriodDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FlatId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("LastValue")
                        .HasColumnType("float");

                    b.Property<int>("MediaType")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartPeriodDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FlatId");

                    b.ToTable("MediaHistory");
                });

            modelBuilder.Entity("HouseCommunity.Model.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DetailsId")
                        .HasColumnType("int");

                    b.Property<int?>("FlatId")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentBookDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PaymentDeadline")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DetailsId");

                    b.HasIndex("FlatId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("HouseCommunity.Model.PaymentDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdministrationDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("AdministrationValue")
                        .HasColumnType("float");

                    b.Property<string>("ColdWaterDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ColdWaterValue")
                        .HasColumnType("float");

                    b.Property<string>("GarbageDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("GarbageValue")
                        .HasColumnType("float");

                    b.Property<string>("HeatingDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HeatingRefundDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("HeatingRefundValue")
                        .HasColumnType("float");

                    b.Property<double>("HeatingValue")
                        .HasColumnType("float");

                    b.Property<string>("HotWaterDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("HotWaterValue")
                        .HasColumnType("float");

                    b.Property<string>("OperatingCostDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("OperatingCostValue")
                        .HasColumnType("float");

                    b.Property<string>("WaterRefundDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("WaterRefundValue")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("PaymentDetail");
                });

            modelBuilder.Entity("HouseCommunity.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FlatId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlatId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HouseCommunity.Model.UserAnnouncement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnnouncementId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnnouncementId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAnnouncements");
                });

            modelBuilder.Entity("HouseCommunity.Model.Building", b =>
                {
                    b.HasOne("HouseCommunity.Model.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseCommunity.Model.Cost", "Cost")
                        .WithMany()
                        .HasForeignKey("CostId");

                    b.HasOne("HouseCommunity.Model.User", "HouseManager")
                        .WithMany()
                        .HasForeignKey("HouseManagerId");

                    b.HasOne("HouseCommunity.Model.HousingDevelopment", "HousingDevelopment")
                        .WithMany("Buildings")
                        .HasForeignKey("HousingDevelopmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HouseCommunity.Model.Damage", b =>
                {
                    b.HasOne("HouseCommunity.Model.Building", "Building")
                        .WithMany()
                        .HasForeignKey("BuildingId");

                    b.HasOne("HouseCommunity.Model.User", "RequestCreator")
                        .WithMany()
                        .HasForeignKey("RequestCreatorId");
                });

            modelBuilder.Entity("HouseCommunity.Model.Flat", b =>
                {
                    b.HasOne("HouseCommunity.Model.Building", "Building")
                        .WithMany("Flats")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HouseCommunity.Model.Media", b =>
                {
                    b.HasOne("HouseCommunity.Model.Flat", "Flat")
                        .WithMany("MediaHistory")
                        .HasForeignKey("FlatId");
                });

            modelBuilder.Entity("HouseCommunity.Model.Payment", b =>
                {
                    b.HasOne("HouseCommunity.Model.PaymentDetail", "Details")
                        .WithMany()
                        .HasForeignKey("DetailsId");

                    b.HasOne("HouseCommunity.Model.Flat", "Flat")
                        .WithMany("Payments")
                        .HasForeignKey("FlatId");
                });

            modelBuilder.Entity("HouseCommunity.Model.User", b =>
                {
                    b.HasOne("HouseCommunity.Model.Flat", "Flat")
                        .WithMany("Residents")
                        .HasForeignKey("FlatId");
                });

            modelBuilder.Entity("HouseCommunity.Model.UserAnnouncement", b =>
                {
                    b.HasOne("HouseCommunity.Model.Announcement", "Announcement")
                        .WithMany("UserAnnouncements")
                        .HasForeignKey("AnnouncementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HouseCommunity.Model.User", "User")
                        .WithMany("UserAnnouncements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
