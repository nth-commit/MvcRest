using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using QueryableStrings.EntityFrameworkCore;

namespace QueryableStrings.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170204164741_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("QueryableStrings.EntityFrameworkCore.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ManagerId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("QueryableStrings.EntityFrameworkCore.Models.Employee", b =>
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

            modelBuilder.Entity("QueryableStrings.EntityFrameworkCore.Models.EmployeeJob", b =>
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

            modelBuilder.Entity("QueryableStrings.EntityFrameworkCore.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("MaxSalary");

                    b.Property<double>("MinSalary");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("QueryableStrings.EntityFrameworkCore.Models.Department", b =>
                {
                    b.HasOne("QueryableStrings.EntityFrameworkCore.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QueryableStrings.EntityFrameworkCore.Models.EmployeeJob", b =>
                {
                    b.HasOne("QueryableStrings.EntityFrameworkCore.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QueryableStrings.EntityFrameworkCore.Models.Employee", "Employee")
                        .WithMany("Jobs")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("QueryableStrings.EntityFrameworkCore.Models.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId");
                });
        }
    }
}
