using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Cookbook.Models;
using Cookbook.Data;

namespace Cookbook.Data.Migrations
{
    [DbContext(typeof(CookbookContext))]
    [Migration("20160906180237_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Cookbook.Models.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Cookbook.Models.Instruction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("RecipeId");

                    b.Property<string>("Task");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Method");
                });

            modelBuilder.Entity("Cookbook.Models.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Serves");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Cookbook.Models.Ingredient", b =>
                {
                    b.HasOne("Cookbook.Models.Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("Cookbook.Models.Instruction", b =>
                {
                    b.HasOne("Cookbook.Models.Recipe")
                        .WithMany("Method")
                        .HasForeignKey("RecipeId");
                });
        }
    }
}
