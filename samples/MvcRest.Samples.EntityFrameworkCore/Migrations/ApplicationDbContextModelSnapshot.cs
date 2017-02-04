using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MvcRest.Samples.EntityFrameworkCore;

namespace MvcRest.Samples.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MvcRest.Samples.EntityFrameworkCore.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ManagerId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("MvcRest.Samples.EntityFrameworkCore.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<DateTime>("HireDate");

                    b.Property<string>("LastName");

                    b.Property<string>("PhoneNumber");

                    b.Property<double>("Salary");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MvcRest.Samples.EntityFrameworkCore.Models.EmployeeJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentId");

                    b.Property<int?>("EmployeeId");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int?>("JobId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("JobId");

                    b.ToTable("EmployeeJobs");
                });

            modelBuilder.Entity("MvcRest.Samples.EntityFrameworkCore.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("MaxSalary");

                    b.Property<double>("MinSalary");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("MvcRest.Samples.EntityFrameworkCore.Models.Department", b =>
                {
                    b.HasOne("MvcRest.Samples.EntityFrameworkCore.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MvcRest.Samples.EntityFrameworkCore.Models.EmployeeJob", b =>
                {
                    b.HasOne("MvcRest.Samples.EntityFrameworkCore.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MvcRest.Samples.EntityFrameworkCore.Models.Employee", "Employee")
                        .WithMany("Jobs")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("MvcRest.Samples.EntityFrameworkCore.Models.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId");
                });
        }
    }
}
