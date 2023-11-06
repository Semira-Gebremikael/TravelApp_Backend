﻿// <auto-generated />
using System;
using DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DbContext.Migrations
{
    [DbContext(typeof(csMainDbContext.SqlServerDbContext))]
    [Migration("20231102221447_initial_migration")]
    partial class initial_migration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DbModel.csAddressDbM", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ZipCode")
                        .HasColumnType("int");

                    b.HasKey("AddressId");

                    b.HasIndex("StreetAddress", "ZipCode", "City", "Country")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("DbModels.csAttractionDbM", b =>
                {
                    b.Property<Guid>("AttractionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressDbMAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttractionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.Property<string>("strCategory")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("AttractionId");

                    b.HasIndex("AddressDbMAddressId");

                    b.ToTable("Attractions");
                });

            modelBuilder.Entity("DbModels.csCommentDbM", b =>
                {
                    b.Property<Guid>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AttractionDbMAttractionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CommentText")
                        .HasColumnType("nvarchar(200)");

                    b.Property<Guid?>("PersonDbMPersonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.HasKey("CommentId");

                    b.HasIndex("AttractionDbMAttractionId");

                    b.HasIndex("PersonDbMPersonId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DbModels.csPersonDbM", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Seeded")
                        .HasColumnType("bit");

                    b.HasKey("PersonId");

                    b.HasIndex("FirstName", "LastName");

                    b.HasIndex("LastName", "FirstName");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("DbModels.csUserDbM", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Models.DTO.vw_attractionsWithNullComments", b =>
                {
                    b.Property<Guid>("AttractionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttractionName")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CommentText")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(200)");

                    b.ToTable((string)null);

                    b.ToView("vw_attractionsWithNullComments", (string)null);
                });

            modelBuilder.Entity("csAttractionDbMcsPersonDbM", b =>
                {
                    b.Property<Guid>("AttractionsDbMAttractionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PersonDbMPersonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AttractionsDbMAttractionId", "PersonDbMPersonId");

                    b.HasIndex("PersonDbMPersonId");

                    b.ToTable("csAttractionDbMcsPersonDbM");
                });

            modelBuilder.Entity("DbModels.csAttractionDbM", b =>
                {
                    b.HasOne("DbModel.csAddressDbM", "AddressDbM")
                        .WithMany("AttractionsDbM")
                        .HasForeignKey("AddressDbMAddressId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("AddressDbM");
                });

            modelBuilder.Entity("DbModels.csCommentDbM", b =>
                {
                    b.HasOne("DbModels.csAttractionDbM", "AttractionDbM")
                        .WithMany("CommentDbM")
                        .HasForeignKey("AttractionDbMAttractionId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DbModels.csPersonDbM", "PersonDbM")
                        .WithMany("CommentsDbM")
                        .HasForeignKey("PersonDbMPersonId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("AttractionDbM");

                    b.Navigation("PersonDbM");
                });

            modelBuilder.Entity("csAttractionDbMcsPersonDbM", b =>
                {
                    b.HasOne("DbModels.csAttractionDbM", null)
                        .WithMany()
                        .HasForeignKey("AttractionsDbMAttractionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DbModels.csPersonDbM", null)
                        .WithMany()
                        .HasForeignKey("PersonDbMPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DbModel.csAddressDbM", b =>
                {
                    b.Navigation("AttractionsDbM");
                });

            modelBuilder.Entity("DbModels.csAttractionDbM", b =>
                {
                    b.Navigation("CommentDbM");
                });

            modelBuilder.Entity("DbModels.csPersonDbM", b =>
                {
                    b.Navigation("CommentsDbM");
                });
#pragma warning restore 612, 618
        }
    }
}
