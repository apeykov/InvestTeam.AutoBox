﻿// <auto-generated />
using System;
using InvestTeam.AutoBox.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InvestTeam.AutoBox.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Common.AppSetting", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Value")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Key", "Value");

                    b.ToTable("AppSettings");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.Error", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ErrorText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorType")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("HistoryID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HistoryID");

                    b.ToTable("Errors");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Commandlet")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("ObjectID")
                        .HasColumnType("int");

                    b.Property<string>("ObjectIdentity")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("ObjectType")
                        .HasColumnType("int");

                    b.Property<string>("Parameters")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<Guid>("RunID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Source")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("History");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.HistoryDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("HistoryId")
                        .HasColumnType("int");

                    b.Property<string>("Information")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Warning")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HistoryId");

                    b.ToTable("HistoryDetails");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Identity")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<long>("SequenceNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VechicleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Identity")
                        .IsUnique();

                    b.HasIndex("VechicleId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.RESTLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("HistoryID")
                        .HasColumnType("int");

                    b.Property<string>("Request")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HistoryID");

                    b.ToTable("RestLogs");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.Vechicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<string>("Identity")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Identity")
                        .IsUnique();

                    b.ToTable("Vechicles");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.Error", b =>
                {
                    b.HasOne("InvestTeam.AutoBox.Domain.Entities.History", null)
                        .WithMany("Errors")
                        .HasForeignKey("HistoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.HistoryDetails", b =>
                {
                    b.HasOne("InvestTeam.AutoBox.Domain.Entities.History", "History")
                        .WithMany("HistoryDetails")
                        .HasForeignKey("HistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("History");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.Order", b =>
                {
                    b.HasOne("InvestTeam.AutoBox.Domain.Entities.Vechicle", "Vechicle")
                        .WithMany("Orders")
                        .HasForeignKey("VechicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vechicle");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.RESTLog", b =>
                {
                    b.HasOne("InvestTeam.AutoBox.Domain.Entities.History", "History")
                        .WithMany("RESTLogs")
                        .HasForeignKey("HistoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("History");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.History", b =>
                {
                    b.Navigation("Errors");

                    b.Navigation("HistoryDetails");

                    b.Navigation("RESTLogs");
                });

            modelBuilder.Entity("InvestTeam.AutoBox.Domain.Entities.Vechicle", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
