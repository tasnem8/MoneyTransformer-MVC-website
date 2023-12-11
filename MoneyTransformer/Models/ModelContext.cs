using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MoneyTransformer.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Home> Homes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Useraccount> Useraccounts { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    public virtual DbSet<VirtualBank> VirtualBanks { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }
    public virtual DbSet<UserInfo> userinfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##LocalUserDB;PASSWORD=admin;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##LOCALUSERDB")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008392");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Image1path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE1PATH");
            entity.Property(e => e.Image2path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE2PATH");
            entity.Property(e => e.Image3path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE3PATH");
            entity.Property(e => e.Text1)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TEXT1");
            entity.Property(e => e.Text2)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TEXT2");
            entity.Property(e => e.Text3)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TEXT3");
            entity.Property(e => e.Text4)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TEXT4");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008378");

            entity.ToTable("BANKS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Descreption)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("DESCREPTION");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Namee)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAMEE");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008390");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Subject)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
        });

        modelBuilder.Entity<Home>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008394");

            entity.ToTable("HOME");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Facebooklink)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("FACEBOOKLINK");
            entity.Property(e => e.Image1path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE1PATH");
            entity.Property(e => e.Image2path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE2PATH");
            entity.Property(e => e.Image3path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE3PATH");
            entity.Property(e => e.Image4path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE4PATH");
            entity.Property(e => e.Instagramlink)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("INSTAGRAMLINK");
            entity.Property(e => e.Logopath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("LOGOPATH");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Text1)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TEXT1");
            entity.Property(e => e.Text2)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TEXT2");
            entity.Property(e => e.Text3)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TEXT3");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008370");

            entity.ToTable("ROLE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008387");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.TestimonialDate)
                .HasColumnType("DATE")
                .HasColumnName("TESTIMONIAL_DATE");
            entity.Property(e => e.Text)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TEXT");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008388");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008384");

            entity.ToTable("TRANSACTIONS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Amount)
                .HasColumnType("FLOAT")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Commission)
                .HasColumnType("FLOAT")
                .HasColumnName("COMMISSION");
            entity.Property(e => e.RIban)
                .HasColumnType("NUMBER")
                .HasColumnName("R_IBAN");
            entity.Property(e => e.SwalletId)
                .HasColumnType("NUMBER")
                .HasColumnName("SWALLET_ID");
            entity.Property(e => e.TranDate)
                .HasColumnType("DATE")
                .HasColumnName("TRAN_DATE");

            entity.HasOne(d => d.Swallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.SwalletId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008385");
        });

        modelBuilder.Entity<Useraccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008372");

            entity.ToTable("USERACCOUNT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LNAME");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008374");

            entity.ToTable("USERLOGIN");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Passwordd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORDD");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008375");

            entity.HasOne(d => d.User).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008376");
        });

        modelBuilder.Entity<VirtualBank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008396");

            entity.ToTable("VIRTUAL_BANK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasColumnType("FLOAT")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Cardnumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvv)
                .HasColumnType("NUMBER")
                .HasColumnName("CVV");
            entity.Property(e => e.Expireddate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIREDDATE");
            entity.Property(e => e.Owneremanil)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("OWNEREMANIL");
            entity.Property(e => e.Ownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("OWNERNAME");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008380");

            entity.ToTable("WALLET");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasColumnType("FLOAT")
                .HasColumnName("BALANCE");
            entity.Property(e => e.BankId)
                .HasColumnType("NUMBER")
                .HasColumnName("BANK_ID");
            entity.Property(e => e.Datecreate)
                .HasColumnType("DATE")
                .HasColumnName("DATECREATE");
            entity.Property(e => e.Iban)
                .HasColumnType("NUMBER")
                .HasColumnName("IBAN");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Bank).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008382");

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008381");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
