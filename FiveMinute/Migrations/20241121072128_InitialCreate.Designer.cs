﻿// <auto-generated />
using System;
using FiveMinute.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FiveMinute.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241121072128_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FiveMinutes.Models.Answer", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FMTestId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("FiveMinutes.Models.AppUser", b =>
                {
                    b.Property<string>("FMTestId")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FMTestId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("FiveMinutes.Models.FiveMinuteTestResult", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<int>("FiveMinuteTemplateId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PassTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("FMTestId");

                    b.ToTable("FiveMinuteResults");
                });

            modelBuilder.Entity("FiveMinutes.Models.FiveMinuteTemplate", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("OriginId")
                        .HasColumnType("integer");

                    b.Property<bool>("ShowInProfile")
                        .HasColumnType("boolean");

                    b.Property<string>("UserOwnerId")
                        .HasColumnType("text");

                    b.HasKey("FMTestId");

                    b.HasIndex("OriginId");

                    b.HasIndex("UserOwnerId");

                    b.ToTable("FiveMinuteTemplates");
                });

            modelBuilder.Entity("FiveMinutes.Models.FiveMinuteTest", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<int?>("AttachedFMTId")
                        .HasColumnType("integer");

                    b.Property<int>("FolderId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int?>("UserOrganizerId")
                        .HasColumnType("integer");

                    b.HasKey("FMTestId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("AttachedFMTId");

                    b.HasIndex("FolderId");

                    b.ToTable("FiveMinuteTests");
                });

            modelBuilder.Entity("FiveMinutes.Models.Folder", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentFolderId")
                        .HasColumnType("integer");

                    b.HasKey("FMTestId");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("FiveMinutes.Models.Question", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<int>("FiveMinuteTemplateId")
                        .HasColumnType("integer");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ResponseType")
                        .HasColumnType("integer");

                    b.HasKey("FMTestId");

                    b.HasIndex("FiveMinuteTemplateId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("FiveMinutes.Models.UserAnswer", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<int?>("FiveMinuteResultId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<int>("QuestionPosition")
                        .HasColumnType("integer");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FMTestId");

                    b.HasIndex("FiveMinuteResultId");

                    b.ToTable("UserAnswer");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("FMTestId")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("FMTestId");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FMTestId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("FMTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("FMTestId"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("FMTestId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FiveMinutes.Models.Answer", b =>
                {
                    b.HasOne("FiveMinutes.Models.Question", null)
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FiveMinutes.Models.FiveMinuteTemplate", b =>
                {
                    b.HasOne("FiveMinutes.Models.FiveMinuteTemplate", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginId");

                    b.HasOne("FiveMinutes.Models.AppUser", "UserOwner")
                        .WithMany("FMTs")
                        .HasForeignKey("UserOwnerId");

                    b.Navigation("Origin");

                    b.Navigation("UserOwner");
                });

            modelBuilder.Entity("FiveMinutes.Models.FiveMinuteTest", b =>
                {
                    b.HasOne("FiveMinutes.Models.AppUser", null)
                        .WithMany("Tests")
                        .HasForeignKey("AppUserId");

                    b.HasOne("FiveMinutes.Models.FiveMinuteTemplate", "AttachedFMT")
                        .WithMany()
                        .HasForeignKey("AttachedFMTId");

                    b.HasOne("FiveMinutes.Models.Folder", "Folder")
                        .WithMany("Tests")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AttachedFMT");

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("FiveMinutes.Models.Folder", b =>
                {
                    b.HasOne("FiveMinutes.Models.Folder", "ParentFolder")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentFolderId");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("FiveMinutes.Models.Question", b =>
                {
                    b.HasOne("FiveMinutes.Models.FiveMinuteTemplate", "FiveMinuteTemplate")
                        .WithMany("Questions")
                        .HasForeignKey("FiveMinuteTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FiveMinuteTemplate");
                });

            modelBuilder.Entity("FiveMinutes.Models.UserAnswer", b =>
                {
                    b.HasOne("FiveMinutes.Models.FiveMinuteTestResult", null)
                        .WithMany("Answers")
                        .HasForeignKey("FiveMinuteResultId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FiveMinutes.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FiveMinutes.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FiveMinutes.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FiveMinutes.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FiveMinutes.Models.AppUser", b =>
                {
                    b.Navigation("FMTs");

                    b.Navigation("Tests");
                });

            modelBuilder.Entity("FiveMinutes.Models.FiveMinuteTestResult", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("FiveMinutes.Models.FiveMinuteTemplate", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("FiveMinutes.Models.Folder", b =>
                {
                    b.Navigation("SubFolders");

                    b.Navigation("Tests");
                });

            modelBuilder.Entity("FiveMinutes.Models.Question", b =>
                {
                    b.Navigation("Answers");
                });
#pragma warning restore 612, 618
        }
    }
}
