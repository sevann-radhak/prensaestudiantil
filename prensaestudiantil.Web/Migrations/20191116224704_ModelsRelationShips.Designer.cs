﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using prensaestudiantil.Web.Data;

namespace prensaestudiantil.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20191116224704_ModelsRelationShips")]
    partial class ModelsRelationShips
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("prensaestudiantil.Web.Data.Entities.Publication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author")
                        .HasMaxLength(60);

                    b.Property<string>("Body");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Footer")
                        .HasMaxLength(250);

                    b.Property<string>("Header")
                        .IsRequired();

                    b.Property<string>("ImageDescription")
                        .HasMaxLength(150);

                    b.Property<string>("ImagePath");

                    b.Property<DateTime?>("LastUpdate");

                    b.Property<int?>("PublicationCategoryId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int?>("WriterId");

                    b.HasKey("Id");

                    b.HasIndex("PublicationCategoryId");

                    b.HasIndex("WriterId");

                    b.ToTable("Publications");
                });

            modelBuilder.Entity("prensaestudiantil.Web.Data.Entities.PublicationCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("PublicationCategories");
                });

            modelBuilder.Entity("prensaestudiantil.Web.Data.Entities.PublicationImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("ImageUrl");

                    b.Property<int?>("PublicationId");

                    b.HasKey("Id");

                    b.HasIndex("PublicationId");

                    b.ToTable("PublicationImages");
                });

            modelBuilder.Entity("prensaestudiantil.Web.Data.Entities.Writer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(100);

                    b.Property<string>("CellPhone")
                        .HasMaxLength(20);

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.ToTable("Writers");
                });

            modelBuilder.Entity("prensaestudiantil.Web.Data.Entities.YoutubeVideo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasMaxLength(11);

                    b.Property<int?>("WriterId");

                    b.HasKey("Id");

                    b.HasIndex("WriterId");

                    b.ToTable("YoutubeVideos");
                });

            modelBuilder.Entity("prensaestudiantil.Web.Data.Entities.Publication", b =>
                {
                    b.HasOne("prensaestudiantil.Web.Data.Entities.PublicationCategory", "PublicationCategory")
                        .WithMany("Publications")
                        .HasForeignKey("PublicationCategoryId");

                    b.HasOne("prensaestudiantil.Web.Data.Entities.Writer", "Writer")
                        .WithMany("Publications")
                        .HasForeignKey("WriterId");
                });

            modelBuilder.Entity("prensaestudiantil.Web.Data.Entities.PublicationImage", b =>
                {
                    b.HasOne("prensaestudiantil.Web.Data.Entities.Publication", "Publication")
                        .WithMany("PublicationImages")
                        .HasForeignKey("PublicationId");
                });

            modelBuilder.Entity("prensaestudiantil.Web.Data.Entities.YoutubeVideo", b =>
                {
                    b.HasOne("prensaestudiantil.Web.Data.Entities.Writer", "Writer")
                        .WithMany("YoutubeVideos")
                        .HasForeignKey("WriterId");
                });
#pragma warning restore 612, 618
        }
    }
}