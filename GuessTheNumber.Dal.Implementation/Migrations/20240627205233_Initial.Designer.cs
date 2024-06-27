﻿// <auto-generated />
using System;
using GuessTheNumber.Dal.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GuessTheNumber.Dal.Implementation.Migrations
{
    [DbContext(typeof(GameSessionWriteContext))]
    [Migration("20240627205233_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("GuessTheNumber.Domain.Entities.GameSession", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("AttemptCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetValue")
                        .HasColumnType("INTEGER");

                    b.HasKey("SessionId");

                    b.ToTable("GameSessions", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
