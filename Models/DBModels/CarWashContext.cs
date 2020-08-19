using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarWash.Models.DBModels
{
    public partial class CarWashContext : DbContext
    {
      

        public CarWashContext(DbContextOptions<CarWashContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<CarBrand> CarBrand { get; set; }
        public virtual DbSet<CarModel> CarModel { get; set; }
        public virtual DbSet<CarSize> CarSize { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<HomeScore> HomeScore { get; set; }
        public virtual DbSet<ImageSevice> ImageSevice { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<ModelPackage> ModelPackage { get; set; }
        public virtual DbSet<OthrerImage> OthrerImage { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserLogs> UserLogs { get; set; }
        public virtual DbSet<Wallet> Wallet { get; set; }
        public virtual DbSet<WalletLogs> WalletLogs { get; set; }

   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.VehicleRegistration)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Car)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Car_CarBrand");

                entity.HasOne(d => d.Model_)
                    .WithMany(p => p.Car)
                    .HasForeignKey(d => d.Model_Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Car_CarModel");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Car)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_Car_Province");
            });

            modelBuilder.Entity<CarBrand>(entity =>
            {
                entity.HasKey(e => e.BrandId);

                entity.Property(e => e.BrandName).HasMaxLength(50);
            });

            modelBuilder.Entity<CarModel>(entity =>
            {
                entity.HasKey(e => e.Model_Id);

                entity.Property(e => e.ModelName).HasMaxLength(50);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.CarModel)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_CarModel_CarBrand");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.CarModel)
                    .HasForeignKey(d => d.SizeId)
                    .HasConstraintName("FK_CarModel_CarSize");
            });

            modelBuilder.Entity<CarSize>(entity =>
            {
                entity.HasKey(e => e.SizeId);

                entity.Property(e => e.SizeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.Message).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<HomeScore>(entity =>
            {
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.HomeScore)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_HomeScore_User");
            });

            modelBuilder.Entity<ImageSevice>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.Property(e => e.BackAfter).HasMaxLength(500);

                entity.Property(e => e.BackBefore).HasMaxLength(500);

                entity.Property(e => e.FrontAfter).HasMaxLength(500);

                entity.Property(e => e.FrontBefore).HasMaxLength(500);

                entity.Property(e => e.LaftAfter).HasMaxLength(500);

                entity.Property(e => e.LaftBefore).HasMaxLength(500);

                entity.Property(e => e.RightAfter).HasMaxLength(500);

                entity.Property(e => e.RightBefore).HasMaxLength(500);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.ImageSevice)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_ImageSevice_Job");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.Property(e => e.JobApprove).HasMaxLength(50);

                entity.Property(e => e.Location).HasMaxLength(200);

                entity.Property(e => e.Report).HasMaxLength(200);

                entity.Property(e => e.StatusName).HasMaxLength(50);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_Car");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.JobCustomer)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_User1");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.JobEmployee)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Job_User2");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.PackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_Package");
            });

            modelBuilder.Entity<ModelPackage>(entity =>
            {
                entity.Property(e => e.PackageName).HasMaxLength(50);
            });

            modelBuilder.Entity<OthrerImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.Property(e => e.Image).HasMaxLength(500);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.OthrerImage)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_OthrerImage_Job1");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(e => e.PackageId).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.PackageImage).HasMaxLength(200);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.HasOne(d => d.ModelPackage)
                    .WithMany(p => p.Package)
                    .HasForeignKey(d => d.ModelPackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Package_ModelPackage");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.Package)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Package_CarSize");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.Property(e => e.ProvinceName).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.AspNetRole)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdCardNumber)
                    .IsRequired()
                    .HasMaxLength(13);

                entity.Property(e => e.Image).HasMaxLength(450);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserLogs>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserLogs_User");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.Property(e => e.Balance)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallet)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wallet_User");
            });

            modelBuilder.Entity<WalletLogs>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Confirm)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WalletLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_WalletLogs_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
