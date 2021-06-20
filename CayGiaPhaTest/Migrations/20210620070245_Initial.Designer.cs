﻿// <auto-generated />
using System;
using CayGiaPhaTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CayGiaPhaTest.Migrations
{
    [DbContext(typeof(GPDbContext))]
    [Migration("20210620070245_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CayGiaPhaTest.Parents", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FatherId")
                        .HasColumnType("int");

                    b.Property<int?>("MotherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FatherId");

                    b.HasIndex("MotherId");

                    b.ToTable("Parents");
                });

            modelBuilder.Entity("CayGiaPhaTest.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentsId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ParentsId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CayGiaPhaTest.Parents", b =>
                {
                    b.HasOne("CayGiaPhaTest.User", "Father")
                        .WithMany("Fathers")
                        .HasForeignKey("FatherId");

                    b.HasOne("CayGiaPhaTest.User", "Mother")
                        .WithMany("Mothers")
                        .HasForeignKey("MotherId")
                        .OnDelete(DeleteBehavior.ClientNoAction);

                    b.Navigation("Father");

                    b.Navigation("Mother");
                });

            modelBuilder.Entity("CayGiaPhaTest.User", b =>
                {
                    b.HasOne("CayGiaPhaTest.Parents", "Parents")
                        .WithMany("Children")
                        .HasForeignKey("ParentsId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Parents");
                });

            modelBuilder.Entity("CayGiaPhaTest.Parents", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("CayGiaPhaTest.User", b =>
                {
                    b.Navigation("Fathers");

                    b.Navigation("Mothers");
                });
#pragma warning restore 612, 618
        }
    }
}
