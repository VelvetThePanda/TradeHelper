﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradeHelper.Data;

#nullable disable

namespace TradeHelper.Migrations
{
    [DbContext(typeof(TradeContext))]
    [Migration("20220515202635_FixCreationTimestamps")]
    partial class FixCreationTimestamps
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("TradeHelper.Data.Models.TradeOffer", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ClaimedAt")
                        .HasColumnType("TEXT");

                    b.Property<ulong?>("ClaimerID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("GuildID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Offering")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("OwnerID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Requesting")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ClaimerID");

                    b.HasIndex("OwnerID");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("TradeHelper.Data.Models.TradeUser", b =>
                {
                    b.Property<ulong>("ID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Reputation")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TradeHelper.Data.Models.TradeOffer", b =>
                {
                    b.HasOne("TradeHelper.Data.Models.TradeUser", "Claimer")
                        .WithMany("ClaimedTrades")
                        .HasForeignKey("ClaimerID");

                    b.HasOne("TradeHelper.Data.Models.TradeUser", "Owner")
                        .WithMany("TradeOffers")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Claimer");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TradeHelper.Data.Models.TradeUser", b =>
                {
                    b.Navigation("ClaimedTrades");

                    b.Navigation("TradeOffers");
                });
#pragma warning restore 612, 618
        }
    }
}
