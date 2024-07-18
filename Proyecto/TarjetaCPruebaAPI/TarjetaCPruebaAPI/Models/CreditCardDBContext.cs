using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TarjetaCPruebaAPI.Models
{
    public partial class CreditCardDBContext : DbContext
    {
        public CreditCardDBContext()
        {
        }

        public CreditCardDBContext(DbContextOptions<CreditCardDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CreditCard> CreditCards { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreditCard>(entity =>
            {
                entity.HasKey(e => e.CardId)
                    .HasName("PK__CreditCa__BDF201DDAC62E917");

                entity.HasIndex(e => e.CardNumber, "UQ__CreditCa__1E6E0AF4CDF092CA")
                    .IsUnique();

                entity.Property(e => e.CardId).HasColumnName("card_id");

                entity.Property(e => e.AvailableBalance)
                    .HasColumnType("decimal(19, 2)")
                    .HasColumnName("available_balance")
                    .HasComputedColumnSql("([credit_limit]-[current_balance])", true);

                entity.Property(e => e.CardNumber)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("card_number");

                entity.Property(e => e.CreditLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("credit_limit");

                entity.Property(e => e.CurrentBalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("current_balance")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CreditCards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CreditCar__user___4E88ABD4");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.CardId).HasColumnName("card_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("transaction_date");

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("transaction_type");

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacti__card___5165187F");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E61644F35CFB8")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("last_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
