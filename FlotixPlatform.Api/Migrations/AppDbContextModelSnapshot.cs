using System;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FlotixPlatform.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FlotixPlatform.API.Analytics.Domain.Model.Aggregates.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("FilterParams")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("filter_params");

                    b.Property<DateTimeOffset>("GeneratedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("generated_date");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<string>("ReportType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("report_type");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("p_k_reports");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("i_x_reports_owner_id");

                    b.ToTable("reports", "analytics");
                });

            modelBuilder.Entity("FlotixPlatform.API.Commerce.Domain.Model.Aggregates.IotOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset>("OrderDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("order_date");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("status");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("total_amount");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("p_k_iot_orders");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("i_x_iot_orders_owner_id");

                    b.ToTable("iot_orders", "commerce");
                });

            modelBuilder.Entity("FlotixPlatform.API.Fleet.Domain.Model.Aggregates.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<decimal>("CurrentMileage")
                        .HasColumnType("numeric")
                        .HasColumnName("current_mileage");

                    b.Property<Guid?>("DriverId")
                        .HasColumnType("uuid")
                        .HasColumnName("driver_id");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("license_plate");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("owner_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("VehicleType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("vehicle_type");

                    b.HasKey("Id")
                        .HasName("p_k_vehicles");

                    b.HasIndex("DriverId")
                        .HasDatabaseName("i_x_vehicles_driver_id");

                    b.HasIndex("LicensePlate")
                        .IsUnique()
                        .HasDatabaseName("i_x_vehicles_license_plate");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("i_x_vehicles_owner_id");

                    b.ToTable("vehicles", "fleet");
                });

            modelBuilder.Entity("FlotixPlatform.API.Fuel.Domain.Model.Aggregates.FuelRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric")
                        .HasColumnName("cost");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("DriverId")
                        .HasColumnType("uuid")
                        .HasColumnName("driver_id");

                    b.Property<decimal>("Liters")
                        .HasColumnType("numeric")
                        .HasColumnName("liters");

                    b.Property<decimal>("MileageAtRefill")
                        .HasColumnType("numeric")
                        .HasColumnName("mileage_at_refill");

                    b.Property<DateTimeOffset>("RecordedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("recorded_date");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uuid")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id")
                        .HasName("p_k_fuel_records");

                    b.HasIndex("DriverId")
                        .HasDatabaseName("i_x_fuel_records_driver_id");

                    b.HasIndex("VehicleId")
                        .HasDatabaseName("i_x_fuel_records_vehicle_id");

                    b.ToTable("fuel_records", "fuel");
                });

            modelBuilder.Entity("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password_hash");

                    b.Property<string>("RoleType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("role_type");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("p_k__user");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("i_x_users_email");

                    b.HasIndex("RoleType")
                        .HasDatabaseName("i_x_users_role_type");

                    b.ToTable("users", "iam");

                    b.HasDiscriminator<string>("RoleType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("FlotixPlatform.API.Incidents.Domain.Model.Aggregates.Incident", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<Guid>("DriverId")
                        .HasColumnType("uuid")
                        .HasColumnName("driver_id");

                    b.Property<DateTimeOffset>("ReportedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("reported_date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uuid")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id")
                        .HasName("p_k_incidents");

                    b.HasIndex("DriverId")
                        .HasDatabaseName("i_x_incidents_driver_id");

                    b.HasIndex("VehicleId")
                        .HasDatabaseName("i_x_incidents_vehicle_id");

                    b.ToTable("incidents", "incidents");
                });

            modelBuilder.Entity("FlotixPlatform.API.Maintenance.Domain.Model.Aggregates.MaintenanceRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<decimal>("EstimatedCost")
                        .HasColumnType("numeric")
                        .HasColumnName("estimated_cost");

                    b.Property<Guid>("MechanicId")
                        .HasColumnType("uuid")
                        .HasColumnName("mechanic_id");

                    b.Property<DateTimeOffset>("RequestDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("request_date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uuid")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id")
                        .HasName("p_k_maintenance_requests");

                    b.HasIndex("MechanicId")
                        .HasDatabaseName("i_x_maintenance_requests_mechanic_id");

                    b.HasIndex("VehicleId")
                        .HasDatabaseName("i_x_maintenance_requests_vehicle_id");

                    b.ToTable("maintenance_requests", "maintenance");
                });

            modelBuilder.Entity("FlotixPlatform.API.Monitoring.Domain.Model.Aggregates.IotDevice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("boolean")
                        .HasColumnName("is_connected");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("serial_number");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uuid")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id")
                        .HasName("p_k_iot_devices");

                    b.HasIndex("SerialNumber")
                        .IsUnique()
                        .HasDatabaseName("i_x_iot_devices_serial_number");

                    b.HasIndex("VehicleId")
                        .IsUnique()
                        .HasDatabaseName("i_x_iot_devices_vehicle_id");

                    b.ToTable("iot_devices", "monitoring");
                });

            modelBuilder.Entity("FlotixPlatform.API.Monitoring.Domain.Model.Aggregates.TelemetryData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uuid")
                        .HasColumnName("device_id");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("numeric")
                        .HasColumnName("latitude");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("numeric")
                        .HasColumnName("longitude");

                    b.Property<DateTimeOffset>("RecordedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("recorded_at");

                    b.Property<decimal>("Speed")
                        .HasColumnType("numeric")
                        .HasColumnName("speed");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("p_k_telemetry_data");

                    b.HasIndex("DeviceId")
                        .HasDatabaseName("i_x_telemetry_data_device_id");

                    b.ToTable("telemetry_data", "monitoring");
                });

            modelBuilder.Entity("FlotixPlatform.API.Notifications.Domain.Model.Aggregates.Alert", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AlertType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("alert_type");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset>("TriggerDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("trigger_date");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("VehicleId")
                        .HasColumnType("uuid")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id")
                        .HasName("p_k_alerts");

                    b.HasIndex("VehicleId")
                        .HasDatabaseName("i_x_alerts_vehicle_id");

                    b.ToTable("alerts", "notifications");
                });

            modelBuilder.Entity("FlotixPlatform.API.Notifications.Domain.Model.Aggregates.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("AlertId")
                        .HasColumnType("uuid")
                        .HasColumnName("alert_id");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean")
                        .HasColumnName("is_read");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<DateTimeOffset>("SentDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("sent_date");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("p_k_notifications");

                    b.HasIndex("AlertId")
                        .HasDatabaseName("i_x_notifications_alert_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("i_x_notifications_user_id");

                    b.ToTable("notifications", "notifications");
                });

            modelBuilder.Entity("FlotixPlatform.API.IAM.Domain.Model.Aggregates.Driver", b =>
                {
                    b.HasBaseType("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User");

                    b.Property<DateOnly?>("LicenseExpiration")
                        .HasColumnType("date")
                        .HasColumnName("license_expiration");

                    b.Property<string>("LicenseNumber")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("license_number");

                    b.ToTable("users", "iam");

                    b.HasDiscriminator().HasValue("Driver");
                });

            modelBuilder.Entity("FlotixPlatform.API.IAM.Domain.Model.Aggregates.Mechanic", b =>
                {
                    b.HasBaseType("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User");

                    b.Property<string>("WorkshopName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("workshop_name");

                    b.ToTable("users", "iam");

                    b.HasDiscriminator().HasValue("Mechanic");
                });

            modelBuilder.Entity("FlotixPlatform.API.IAM.Domain.Model.Aggregates.Owner", b =>
                {
                    b.HasBaseType("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("company_name");

                    b.ToTable("users", "iam");

                    b.HasDiscriminator().HasValue("Owner");
                });

            modelBuilder.Entity("FlotixPlatform.API.Analytics.Domain.Model.Aggregates.Report", b =>
                {
                    b.HasOne("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_reports__user_owner_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Commerce.Domain.Model.Aggregates.IotOrder", b =>
                {
                    b.HasOne("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_iot_orders__user_owner_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Fleet.Domain.Model.Aggregates.Vehicle", b =>
                {
                    b.HasOne("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("f_k_vehicles__user_driver_id");

                    b.HasOne("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_vehicles__user_owner_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Fuel.Domain.Model.Aggregates.FuelRecord", b =>
                {
                    b.HasOne("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_fuel_records__user_driver_id");

                    b.HasOne("FlotixPlatform.API.Fleet.Domain.Model.Aggregates.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_fuel_records_vehicles_vehicle_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Incidents.Domain.Model.Aggregates.Incident", b =>
                {
                    b.HasOne("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_incidents_users_driver_id");

                    b.HasOne("FlotixPlatform.API.Fleet.Domain.Model.Aggregates.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_incidents_vehicles_vehicle_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Maintenance.Domain.Model.Aggregates.MaintenanceRequest", b =>
                {
                    b.HasOne("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_maintenance_requests_users_mechanic_id");

                    b.HasOne("FlotixPlatform.API.Fleet.Domain.Model.Aggregates.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_maintenance_requests_vehicles_vehicle_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Monitoring.Domain.Model.Aggregates.IotDevice", b =>
                {
                    b.HasOne("FlotixPlatform.API.Fleet.Domain.Model.Aggregates.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_iot_devices_vehicles_vehicle_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Monitoring.Domain.Model.Aggregates.TelemetryData", b =>
                {
                    b.HasOne("FlotixPlatform.API.Monitoring.Domain.Model.Aggregates.IotDevice", null)
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_telemetry_data_iot_devices_device_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Notifications.Domain.Model.Aggregates.Alert", b =>
                {
                    b.HasOne("FlotixPlatform.API.Fleet.Domain.Model.Aggregates.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_alerts_vehicles_vehicle_id");
                });

            modelBuilder.Entity("FlotixPlatform.API.Notifications.Domain.Model.Aggregates.Notification", b =>
                {
                    b.HasOne("FlotixPlatform.API.Notifications.Domain.Model.Aggregates.Alert", null)
                        .WithMany()
                        .HasForeignKey("AlertId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("f_k_notifications_alerts_alert_id");

                    b.HasOne("FlotixPlatform.API.IAM.Domain.Model.Aggregates.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("f_k_notifications_users_user_id");
                });
#pragma warning restore 612, 618
        }
    }
}
