﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace momentum_api.Migrations
{
    [DbContext(typeof(MomentumDBContext))]
    [Migration("20240911162225_removeIdsFromModels")]
    partial class removeIdsFromModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GoalDoc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("GoalDocs");
                });

            modelBuilder.Entity("GoalDoc", b =>
                {
                    b.OwnsOne("Goal", "Goal", b1 =>
                        {
                            b1.Property<int>("GoalDocId")
                                .HasColumnType("integer");

                            b1.Property<string>("Description")
                                .HasColumnType("text");

                            b1.Property<bool>("IsComplete")
                                .HasColumnType("boolean");

                            b1.Property<string>("Name")
                                .HasColumnType("text");

                            b1.HasKey("GoalDocId");

                            b1.ToTable("GoalDocs");

                            b1.ToJson("Goal");

                            b1.WithOwner()
                                .HasForeignKey("GoalDocId");

                            b1.OwnsMany("Habit", "Habits", b2 =>
                                {
                                    b2.Property<int>("GoalDocId")
                                        .HasColumnType("integer");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<DateTime>("Date")
                                        .HasColumnType("timestamp with time zone");

                                    b2.Property<bool>("IsComplete")
                                        .HasColumnType("boolean");

                                    b2.Property<string>("Name")
                                        .HasColumnType("text");

                                    b2.HasKey("GoalDocId", "Id");

                                    b2.ToTable("GoalDocs");

                                    b2.WithOwner()
                                        .HasForeignKey("GoalDocId");
                                });

                            b1.Navigation("Habits");
                        });

                    b.Navigation("Goal");
                });
#pragma warning restore 612, 618
        }
    }
}