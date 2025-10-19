using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace airlines.Models;

public partial class AirlinesContext : DbContext
{
    public AirlinesContext()
    {
    }

    public AirlinesContext(DbContextOptions<AirlinesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=.;initial catalog=airlines;user id=sa;password=aptech; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__City__3214EC0759E8D110");

            entity.ToTable("City");

            entity.Property(e => e.Namee)
                .HasMaxLength(250)
                .HasColumnName("namee");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__class__3214EC070A3DE235");

            entity.ToTable("class");

            entity.Property(e => e.Namee)
                .HasMaxLength(250)
                .HasColumnName("namee");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Flight__3214EC07524A5B2D");

            entity.ToTable("Flight");

            entity.Property(e => e.ArrivalTime)
                .HasColumnType("datetime")
                .HasColumnName("arrivalTime");
            entity.Property(e => e.BasePrice).HasColumnName("basePrice");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.DepartureTime)
                .HasColumnType("datetime")
                .HasColumnName("departureTime");
            entity.Property(e => e.DestinationCity).HasColumnName("destinationCity");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(10)
                .HasColumnName("flightNumber");
            entity.Property(e => e.OriginCity).HasColumnName("originCity");

            entity.HasOne(d => d.DestinationCityNavigation).WithMany(p => p.FlightDestinationCityNavigations)
                .HasForeignKey(d => d.DestinationCity)
                .HasConstraintName("FK_Flight_City_Destination");

            entity.HasOne(d => d.OriginCityNavigation).WithMany(p => p.FlightOriginCityNavigations)
                .HasForeignKey(d => d.OriginCity)
                .HasConstraintName("FK_Flight_City_Origin");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07709F1915");

            entity.ToTable("login");

            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Logins)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_login_ToTable");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07E7A08D8F");

            entity.ToTable("reservation");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.CreditCardUsed)
                .HasMaxLength(250)
                .HasColumnName("creditCardUsed");
            entity.Property(e => e.Cstatus)
                .HasMaxLength(50)
                .HasColumnName("cstatus");
            entity.Property(e => e.DepartureDate)
                .HasColumnType("date")
                .HasColumnName("departureDate");
            entity.Property(e => e.Fid).HasColumnName("fid");
            entity.Property(e => e.PesCount).HasColumnName("pes_count");
            entity.Property(e => e.ReservationDate)
                .HasColumnType("datetime")
                .HasColumnName("reservationDate");
            entity.Property(e => e.ReturnDate)
                .HasColumnType("date")
                .HasColumnName("returnDate");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");
            entity.Property(e => e.TripType)
                .HasMaxLength(50)
                .HasColumnName("trip_type");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Class).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_reservation_ToTable_1");

            entity.HasOne(d => d.FidNavigation).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Fid)
                .HasConstraintName("FK_reservation_ToTable");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK_reservation_ToTable_2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07D5FA2B83");

            entity.ToTable("Role");

            entity.Property(e => e.Namee)
                .HasMaxLength(250)
                .HasColumnName("namee");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Table__3214EC071ACB872B");

            entity.ToTable("Table");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
