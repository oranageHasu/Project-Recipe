﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RecipeService.Classes;

namespace RecipeService.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200514223202_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("RecipeService.Models.Ingredient", b =>
                {
                    b.Property<Guid?>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RecipeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredients");

                    b.HasData(
                        new
                        {
                            IngredientId = new Guid("29346366-c8bb-42f4-a3d8-3c69d9896b73"),
                            RecipeId = new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"),
                            Value = "Water"
                        },
                        new
                        {
                            IngredientId = new Guid("9756a3d4-87a8-4e4e-9ced-c3dd35c38067"),
                            RecipeId = new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"),
                            Value = "Flour"
                        },
                        new
                        {
                            IngredientId = new Guid("7d30c147-6405-4f53-b941-cca64c6afe3b"),
                            RecipeId = new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"),
                            Value = "Salt"
                        },
                        new
                        {
                            IngredientId = new Guid("659e5798-a2fa-4405-883d-8b12ffe109ef"),
                            RecipeId = new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"),
                            Value = "Water"
                        },
                        new
                        {
                            IngredientId = new Guid("0425b446-866f-4316-84ec-f33eacec5605"),
                            RecipeId = new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"),
                            Value = "Flour"
                        },
                        new
                        {
                            IngredientId = new Guid("f6ef45f0-172a-4e84-8b63-ff6b704cfcdc"),
                            RecipeId = new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"),
                            Value = "Salt"
                        },
                        new
                        {
                            IngredientId = new Guid("debee29b-de56-46a5-acf1-73d242d22727"),
                            RecipeId = new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"),
                            Value = "Water"
                        },
                        new
                        {
                            IngredientId = new Guid("f19633c8-1893-47fa-9ae2-7ca6f3176361"),
                            RecipeId = new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"),
                            Value = "Flour"
                        },
                        new
                        {
                            IngredientId = new Guid("639aa8f9-f129-460e-8437-eac45558b320"),
                            RecipeId = new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"),
                            Value = "Salt"
                        });
                });

            modelBuilder.Entity("RecipeService.Models.Instruction", b =>
                {
                    b.Property<Guid?>("InstructionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("RecipeId")
                        .HasColumnType("uuid");

                    b.Property<int>("SortOrder")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("InstructionId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Instructions");

                    b.HasData(
                        new
                        {
                            InstructionId = new Guid("a3f12a50-0613-4f39-8dd9-c2265d994ab9"),
                            RecipeId = new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"),
                            SortOrder = 1,
                            Value = "Preheat oven to 450F."
                        },
                        new
                        {
                            InstructionId = new Guid("2613dc32-9b1b-4307-930f-20be88eb0b14"),
                            RecipeId = new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"),
                            SortOrder = 2,
                            Value = "Insert food."
                        },
                        new
                        {
                            InstructionId = new Guid("10db9bee-f528-44fb-9f66-0f5219194366"),
                            RecipeId = new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"),
                            SortOrder = 3,
                            Value = "Take out food after 15 minutes."
                        },
                        new
                        {
                            InstructionId = new Guid("34fb44b8-d23c-429e-9290-dad66ddd1f9f"),
                            RecipeId = new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"),
                            SortOrder = 1,
                            Value = "Preheat oven to 450F."
                        },
                        new
                        {
                            InstructionId = new Guid("6c43866b-1297-449d-9319-341a5146ab8f"),
                            RecipeId = new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"),
                            SortOrder = 2,
                            Value = "Insert food."
                        },
                        new
                        {
                            InstructionId = new Guid("0cbe392c-e893-46b8-89ba-ab5e9ce80977"),
                            RecipeId = new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"),
                            SortOrder = 3,
                            Value = "Take out food after 15 minutes."
                        },
                        new
                        {
                            InstructionId = new Guid("9da11bcf-b092-4aba-b46a-54b43e7227df"),
                            RecipeId = new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"),
                            SortOrder = 1,
                            Value = "Preheat oven to 450F."
                        },
                        new
                        {
                            InstructionId = new Guid("93d529aa-e50f-4f2d-8dfa-61e3753daf3d"),
                            RecipeId = new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"),
                            SortOrder = 2,
                            Value = "Insert food."
                        },
                        new
                        {
                            InstructionId = new Guid("44325639-afc7-43ad-8701-a17e88cf5b04"),
                            RecipeId = new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"),
                            SortOrder = 3,
                            Value = "Take out food after 15 minutes."
                        });
                });

            modelBuilder.Entity("RecipeService.Models.Recipe", b =>
                {
                    b.Property<Guid?>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Notes")
                        .HasColumnType("text");

                    b.Property<decimal>("Rating")
                        .HasColumnType("numeric");

                    b.HasKey("RecipeId");

                    b.ToTable("Recipes");

                    b.HasData(
                        new
                        {
                            RecipeId = new Guid("bf6a3c4e-cfe3-4cec-a0fa-ff9cfa75a341"),
                            IsDeleted = false,
                            Name = "Pho",
                            Notes = "For best results, use shank and knee bones.",
                            Rating = 4.7m
                        },
                        new
                        {
                            RecipeId = new Guid("14e30840-cc7f-458a-bd46-360d1084d9d3"),
                            IsDeleted = false,
                            Name = "Polish Perogies",
                            Notes = "For best results, choose potatoes that have as little water in them as possible such as Russets.",
                            Rating = 4.1m
                        },
                        new
                        {
                            RecipeId = new Guid("0e2e377b-1d1b-4720-8d1d-be23c293181b"),
                            IsDeleted = false,
                            Name = "Bread",
                            Notes = "Use active yeast for best results.",
                            Rating = 3.2m
                        });
                });

            modelBuilder.Entity("RecipeService.Models.User", b =>
                {
                    b.Property<Guid?>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("96d9a443-db55-4881-9b8c-4318eb5ae17a"),
                            Password = "admin",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("RecipeService.Models.Ingredient", b =>
                {
                    b.HasOne("RecipeService.Models.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("RecipeService.Models.Instruction", b =>
                {
                    b.HasOne("RecipeService.Models.Recipe", "Recipe")
                        .WithMany("Instructions")
                        .HasForeignKey("RecipeId");
                });
#pragma warning restore 612, 618
        }
    }
}