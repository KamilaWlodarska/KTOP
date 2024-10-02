using Microsoft.EntityFrameworkCore;

namespace KTOPWebAPI.Entities;

public partial class KtopdbContext : DbContext
{
    public KtopdbContext() { }

    public KtopdbContext(DbContextOptions<KtopdbContext> options) : base(options) { }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Home> Homes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersHome> UsersHomes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categories");

            entity.HasIndex(e => e.CategoryName, "UQ__Categories__CategoryName").IsUnique();

            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Home>(entity =>
        {
            entity.HasKey(e => e.HomeId).HasName("PK__Homes");

            entity.Property(e => e.HomeName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Owner).WithMany(p => p.Homes)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Homes__OwnerId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products");

            entity.HasIndex(e => e.CategoryId, "idx_Products__CategoryId");

            entity.HasIndex(e => e.HomeId, "idx_Products__HomeId");

            entity.Property(e => e.ExpiryDate).HasColumnType("date");
            entity.Property(e => e.OpenDate).HasColumnType("date");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PurchaseDate).HasColumnType("date");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__CategoryId");

            entity.HasOne(d => d.Home).WithMany(p => p.Products)
                .HasForeignKey(d => d.HomeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__HomeId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users");

            entity.HasIndex(e => e.Email, "IDX__Users__Email");

            entity.HasIndex(e => e.Email, "UQ__Users__Email").IsUnique();

            entity.HasIndex(e => e.Salt, "UQ__Users__Salt").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsersHome>(entity =>
        {
            entity.HasKey(e => e.UsersHomesId).HasName("PK__UsersHomes");

            entity.HasOne(d => d.Home).WithMany(p => p.UsersHomes)
                .HasForeignKey(d => d.HomeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersHomes__HomeId");

            entity.HasOne(d => d.User).WithMany(p => p.UsersHomes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersHomes__UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
