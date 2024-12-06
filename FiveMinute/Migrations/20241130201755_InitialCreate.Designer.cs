﻿// <auto-generated />
using System;
using System.Collections.Generic;
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
    [Migration("20241130201755_InitialCreate")]
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

            modelBuilder.Entity("FiveMinute.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("FiveMinute.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
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

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

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

                    b.HasKey("Id");

                    b.HasIndex("OriginId");

                    b.HasIndex("UserOwnerId");

                    b.ToTable("FiveMinuteTemplates");
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FiveMinuteTemplateId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<int>>("PositionsToInclude")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("UserOrganizerId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FiveMinuteTemplateId");

                    b.HasIndex("UserOrganizerId");

                    b.ToTable("FiveMinuteTests");
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTestResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUserId")
                        .HasColumnType("text");

                    b.Property<int>("FiveMinuteTestId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PassTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("FiveMinuteTestId");

                    b.ToTable("FiveMinuteResults");
                });

            modelBuilder.Entity("FiveMinute.Models.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentFolderId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("FiveMinute.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FiveMinuteTemplateId")
                        .HasColumnType("integer");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ResponseType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FiveMinuteTemplateId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("FiveMinute.Models.UserAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("FiveMinuteTestResultId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

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

                    b.HasKey("Id");

                    b.HasIndex("FiveMinuteTestResultId");

                    b.ToTable("UserAnswer");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
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

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

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

            modelBuilder.Entity("FiveMinute.Models.Answer", b =>
                {
                    b.HasOne("FiveMinute.Models.Question", null)
                        .WithMany("AnswerOptions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTemplate", b =>
                {
                    b.HasOne("FiveMinute.Models.FiveMinuteTemplate", "Origin")
                        .WithMany()
                        .HasForeignKey("OriginId");

                    b.HasOne("FiveMinute.Models.AppUser", "UserOwner")
                        .WithMany("FMTTemplates")
                        .HasForeignKey("UserOwnerId");

                    b.Navigation("Origin");

                    b.Navigation("UserOwner");
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTest", b =>
                {
                    b.HasOne("FiveMinute.Models.FiveMinuteTemplate", "FiveMinuteTemplate")
                        .WithMany()
                        .HasForeignKey("FiveMinuteTemplateId");

                    b.HasOne("FiveMinute.Models.AppUser", "UserOrganizer")
                        .WithMany("FMTests")
                        .HasForeignKey("UserOrganizerId");

                    b.Navigation("FiveMinuteTemplate");

                    b.Navigation("UserOrganizer");
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTestResult", b =>
                {
                    b.HasOne("FiveMinute.Models.AppUser", null)
                        .WithMany("PassedTests")
                        .HasForeignKey("AppUserId");

                    b.HasOne("FiveMinute.Models.FiveMinuteTest", null)
                        .WithMany("Results")
                        .HasForeignKey("FiveMinuteTestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FiveMinute.Models.Folder", b =>
                {
                    b.HasOne("FiveMinute.Models.Folder", "ParentFolder")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentFolderId");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("FiveMinute.Models.Question", b =>
                {
                    b.HasOne("FiveMinute.Models.FiveMinuteTemplate", "FiveMinuteTemplate")
                        .WithMany("Questions")
                        .HasForeignKey("FiveMinuteTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FiveMinuteTemplate");
                });

            modelBuilder.Entity("FiveMinute.Models.UserAnswer", b =>
                {
                    b.HasOne("FiveMinute.Models.FiveMinuteTestResult", null)
                        .WithMany("Answers")
                        .HasForeignKey("FiveMinuteTestResultId");
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
                    b.HasOne("FiveMinute.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FiveMinute.Models.AppUser", null)
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

                    b.HasOne("FiveMinute.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FiveMinute.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FiveMinute.Models.AppUser", b =>
                {
                    b.Navigation("FMTTemplates");

                    b.Navigation("FMTests");

                    b.Navigation("PassedTests");
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTemplate", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTest", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("FiveMinute.Models.FiveMinuteTestResult", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("FiveMinute.Models.Folder", b =>
                {
                    b.Navigation("SubFolders");
                });

            modelBuilder.Entity("FiveMinute.Models.Question", b =>
                {
                    b.Navigation("AnswerOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
