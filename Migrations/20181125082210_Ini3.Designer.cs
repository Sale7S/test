﻿// <auto-generated />
using System;
using COCAS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace COCAS.Migrations
{
    [DbContext(typeof(COCASContext))]
    [Migration("20181125082210_Ini3")]
    partial class Ini3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("COCAS.Models.Course", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DepartmentCode")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Code");

                    b.HasIndex("DepartmentCode");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("COCAS.Models.Department", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Code");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("COCAS.Models.Form", b =>
                {
                    b.Property<string>("Title")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Title");

                    b.HasIndex("Type");

                    b.ToTable("Form");
                });

            modelBuilder.Entity("COCAS.Models.FormType", b =>
                {
                    b.Property<string>("Type")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Type");

                    b.ToTable("FormType");
                });

            modelBuilder.Entity("COCAS.Models.HoD", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DepartmentCode")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("DepartmentCode");

                    b.ToTable("HoD");
                });

            modelBuilder.Entity("COCAS.Models.Instructor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DepartmentCode")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("DepartmentCode");

                    b.ToTable("Instructor");
                });

            modelBuilder.Entity("COCAS.Models.Request", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FormTitle")
                        .IsRequired();

                    b.Property<string>("SectionNumber");

                    b.Property<string>("StudentID")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("FormTitle");

                    b.HasIndex("SectionNumber");

                    b.HasIndex("StudentID");

                    b.ToTable("Request");
                });

            modelBuilder.Entity("COCAS.Models.Response", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Reason");

                    b.Property<int>("RequestID");

                    b.Property<bool>("Status");

                    b.HasKey("ID");

                    b.HasIndex("RequestID");

                    b.ToTable("Response");
                });

            modelBuilder.Entity("COCAS.Models.Schedule", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CourseCode")
                        .IsRequired();

                    b.Property<string>("SectionNumber");

                    b.Property<string>("StudentID");

                    b.HasKey("ID");

                    b.HasIndex("SectionNumber");

                    b.HasIndex("StudentID");

                    b.ToTable("Schedule");
                });

            modelBuilder.Entity("COCAS.Models.Section", b =>
                {
                    b.Property<string>("Number")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Activity")
                        .IsRequired();

                    b.Property<string>("CourseCode")
                        .IsRequired();

                    b.Property<int>("Day");

                    b.Property<int>("Duration");

                    b.Property<string>("EndTime");

                    b.Property<string>("FinalExam");

                    b.Property<int?>("InstructorID");

                    b.Property<string>("Place");

                    b.Property<string>("StartTime");

                    b.HasKey("Number");

                    b.HasIndex("CourseCode");

                    b.HasIndex("InstructorID");

                    b.ToTable("Section");
                });

            modelBuilder.Entity("COCAS.Models.Student", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DepartmentCode")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("DepartmentCode");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("COCAS.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Username");

                    b.HasIndex("Type");

                    b.ToTable("User");
                });

            modelBuilder.Entity("COCAS.Models.UserType", b =>
                {
                    b.Property<string>("Type")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Type");

                    b.ToTable("UserType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("COCAS.Models.Course", b =>
                {
                    b.HasOne("COCAS.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("COCAS.Models.Form", b =>
                {
                    b.HasOne("COCAS.Models.FormType", "FormType")
                        .WithMany()
                        .HasForeignKey("Type")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("COCAS.Models.HoD", b =>
                {
                    b.HasOne("COCAS.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("COCAS.Models.Instructor", b =>
                {
                    b.HasOne("COCAS.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("COCAS.Models.Request", b =>
                {
                    b.HasOne("COCAS.Models.Form", "Form")
                        .WithMany()
                        .HasForeignKey("FormTitle")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("COCAS.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionNumber");

                    b.HasOne("COCAS.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("COCAS.Models.Response", b =>
                {
                    b.HasOne("COCAS.Models.Request", "Request")
                        .WithMany()
                        .HasForeignKey("RequestID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("COCAS.Models.Schedule", b =>
                {
                    b.HasOne("COCAS.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionNumber");

                    b.HasOne("COCAS.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentID");
                });

            modelBuilder.Entity("COCAS.Models.Section", b =>
                {
                    b.HasOne("COCAS.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseCode")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("COCAS.Models.Instructor", "Instructor")
                        .WithMany()
                        .HasForeignKey("InstructorID");
                });

            modelBuilder.Entity("COCAS.Models.Student", b =>
                {
                    b.HasOne("COCAS.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentCode")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("COCAS.Models.User", b =>
                {
                    b.HasOne("COCAS.Models.UserType", "UserType")
                        .WithMany()
                        .HasForeignKey("Type")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
