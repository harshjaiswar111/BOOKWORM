using System;
using System.Collections.Generic;
using BookWormNET.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Attribute = BookWormNET.Models.Attribute;

namespace BookWormNET.Data;

public partial class BookwormDbContext : DbContext
{
    public BookwormDbContext()
    {
    }

    public BookwormDbContext(DbContextOptions<BookwormDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Genere> Generes { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LibraryPackage> LibraryPackages { get; set; }

    public virtual DbSet<MyLibrary> MyLibraries { get; set; }

    public virtual DbSet<MyShelf> MyShelves { get; set; }

    public virtual DbSet<PdfBook> PdfBooks { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

    public virtual DbSet<ProductBeneficiary> ProductBeneficiaries { get; set; }

    public virtual DbSet<ProductTypeMaster> ProductTypeMasters { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<RoyaltyCalculation> RoyaltyCalculations { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionItem> TransactionItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<LibraryPackagePurchase> LibraryPackagePurchases { get; set; }
    public virtual DbSet<LibraryPackagePurchaseItem> LibraryPackagePurchaseItems { get; set; }


    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseMySql("server=localhost;port=3306;database=bookwormfinal;user=root;password=Paresh@09;sslmode=None;allowpublickeyretrieval=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.43-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Models.Attribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PRIMARY");

            entity.ToTable("attribute");

            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.AttributeDesc)
                .HasMaxLength(255)
                .HasColumnName("Attribute Desc");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PRIMARY");

            entity.ToTable("author");

            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Bio)
                .HasColumnType("text")
                .HasColumnName("bio");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Beneficiary>(entity =>
        {
            entity.HasKey(e => e.BeneficiaryId).HasName("PRIMARY");

            entity.ToTable("beneficiary");

            entity.HasIndex(e => e.ProductId, "FKbxg0dfum4rlk8wwv9o6y1ma0d");

            entity.Property(e => e.BeneficiaryId).HasColumnName("beneficiary_id");
            entity.Property(e => e.BeneficiaryAccNo)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_acc_no");
            entity.Property(e => e.BeneficiaryAccType)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_acc_type");
            entity.Property(e => e.BeneficiaryBankBranch)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_bank_branch");
            entity.Property(e => e.BeneficiaryBankName)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_bank_name");
            entity.Property(e => e.BeneficiaryContactNo)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_contact_no");
            entity.Property(e => e.BeneficiaryEmailId)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_email_id");
            entity.Property(e => e.BeneficiaryIfsc)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_ifsc");
            entity.Property(e => e.BeneficiaryName)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_name");
            entity.Property(e => e.BeneficiaryPan)
                .HasMaxLength(255)
                .HasColumnName("beneficiary_pan");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Beneficiaries)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKbxg0dfum4rlk8wwv9o6y1ma0d");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PRIMARY");

            entity.ToTable("cart");

            entity.HasIndex(e => e.ProductId, "FK3d704slv66tw6x5hmbm6p2x3u");

            entity.HasIndex(e => e.UserId, "FKl70asp4l4w0jmbm1tqyofho4o");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK3d704slv66tw6x5hmbm6p2x3u");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKl70asp4l4w0jmbm1tqyofho4o");
        });

        modelBuilder.Entity<Genere>(entity =>
        {
            entity.HasKey(e => e.GenereId).HasName("PRIMARY");

            entity.ToTable("genere");

            entity.Property(e => e.GenereId).HasColumnName("genere_id");
            entity.Property(e => e.GenereDesc)
                .HasMaxLength(50)
                .HasColumnName("genere_desc");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PRIMARY");

            entity.ToTable("language");

            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.LanguageDesc)
                .HasMaxLength(50)
                .HasColumnName("language_desc");
        });

        modelBuilder.Entity<LibraryPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PRIMARY");

            entity.ToTable("library_package");

            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.BookLimit).HasColumnName("book_limit");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ValidityDays).HasColumnName("validity_days");
        });

        modelBuilder.Entity<MyLibrary>(entity =>
        {
            entity.HasKey(e => e.MyLibId).HasName("PRIMARY");

            entity.ToTable("my_library");

            entity.HasIndex(e => e.ProductId, "FKip8g980dpsj8kyuc29yhjvcmq");

            entity.HasIndex(e => e.UserId, "FKj1bkdl8fyy4p3omi59flyqq00");

            entity.HasIndex(e => e.PackageId, "FKjbvvfdmgstwlwn8ntsl3biw4c");

            entity.Property(e => e.MyLibId).HasColumnName("my_lib_id");
            entity.Property(e => e.BooksAllowed).HasColumnName("books_allowed");
            entity.Property(e => e.BooksTaken).HasColumnName("books_taken");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Package).WithMany(p => p.MyLibraries)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKjbvvfdmgstwlwn8ntsl3biw4c");

            entity.HasOne(d => d.Product).WithMany(p => p.MyLibraries)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FKip8g980dpsj8kyuc29yhjvcmq");

            entity.HasOne(d => d.User).WithMany(p => p.MyLibraries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKj1bkdl8fyy4p3omi59flyqq00");
        });

        modelBuilder.Entity<MyShelf>(entity =>
        {
            entity.HasKey(e => e.ShelfId).HasName("PRIMARY");

            entity.ToTable("my_shelf");

            entity.HasIndex(e => e.ProductId, "FKi9jt4pby2o2jrf15ltnwth8bk");

            entity.HasIndex(e => e.UserId, "FKph5mtfix6ti7nwnsf4xn59a1m");

            entity.Property(e => e.ShelfId).HasColumnName("shelf_id");
            entity.Property(e => e.ProductExpiryDate)
                .HasMaxLength(6)
                .HasColumnName("product_expiry_date");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.MyShelves)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKi9jt4pby2o2jrf15ltnwth8bk");

            entity.HasOne(d => d.User).WithMany(p => p.MyShelves)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKph5mtfix6ti7nwnsf4xn59a1m");
        });

        modelBuilder.Entity<PdfBook>(entity =>
        {
            entity.HasKey(e => e.PdfId).HasName("PRIMARY");

            entity.ToTable("pdf_book");

            entity.HasIndex(e => e.ProductId, "FKbj6akvkak2ehhya0wnsf0weht");

            entity.Property(e => e.PdfId).HasColumnName("pdf_id");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.PdfData).HasColumnName("pdf_data");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithMany(p => p.PdfBooks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKbj6akvkak2ehhya0wnsf0weht");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.ProductLang, "FK4t9pvspkgp6ev2o32evhevy48");

            entity.HasIndex(e => e.ProductPublisher, "FKb8lf9e3p2wad5qyyu08jx6ebs");

            entity.HasIndex(e => e.ProductType, "FKij1k07u3o1luyr3duuk4glewn");

            entity.HasIndex(e => e.AttributeId, "FKk3j4x5gp4cs69mquqe893i3hl");

            entity.HasIndex(e => e.ProductGenere, "FKk630ngm1ti3hucpttardvn910");

            entity.HasIndex(e => e.ProductAuthor, "FKn0k3dgs9nbb0tnolhg1af4s83");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.DiscountPercent)
                .HasPrecision(5, 2)
                .HasColumnName("discount_percent");
            entity.Property(e => e.IsLibrary)
                .HasColumnType("bit(1)")
                .HasColumnName("is_library");
            entity.Property(e => e.IsRentable)
                .HasColumnType("bit(1)")
                .HasColumnName("is_rentable");
            entity.Property(e => e.MinRentDays).HasColumnName("min_rent_days");
            entity.Property(e => e.ProductAuthor).HasColumnName("product_author");
            entity.Property(e => e.ProductBaseprice)
                .HasPrecision(38, 2)
                .HasColumnName("product_baseprice");
            entity.Property(e => e.ProductDescriptionLong)
                .HasColumnType("text")
                .HasColumnName("product_description_long");
            entity.Property(e => e.ProductDescriptionShort)
                .HasMaxLength(255)
                .HasColumnName("product_description_short");
            entity.Property(e => e.ProductGenere).HasColumnName("product_genere");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(255)
                .HasColumnName("product_image");
            entity.Property(e => e.ProductIsbn)
                .HasMaxLength(255)
                .HasColumnName("product_isbn");
            entity.Property(e => e.ProductLang).HasColumnName("product_lang");
            entity.Property(e => e.ProductName)
                .HasMaxLength(150)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductOffPriceExpirydate).HasColumnName("product_off_price_expirydate");
            entity.Property(e => e.ProductOfferprice)
                .HasPrecision(38, 2)
                .HasColumnName("product_offerprice");
            entity.Property(e => e.ProductPublisher).HasColumnName("product_publisher");
            entity.Property(e => e.ProductType).HasColumnName("product_type");
            entity.Property(e => e.RentPerDay)
                .HasPrecision(5, 2)
                .HasColumnName("rent_per_day");
            entity.Property(e => e.RoyaltyPercent)
                .HasPrecision(38, 2)
                .HasColumnName("royalty_percent");

            entity.HasOne(d => d.Attribute).WithMany(p => p.Products)
                .HasForeignKey(d => d.AttributeId)
                .HasConstraintName("FKk3j4x5gp4cs69mquqe893i3hl");

            entity.HasOne(d => d.ProductAuthorNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductAuthor)
                .HasConstraintName("FKn0k3dgs9nbb0tnolhg1af4s83");

            entity.HasOne(d => d.ProductGenereNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductGenere)
                .HasConstraintName("FKk630ngm1ti3hucpttardvn910");

            entity.HasOne(d => d.ProductLangNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductLang)
                .HasConstraintName("FK4t9pvspkgp6ev2o32evhevy48");

            entity.HasOne(d => d.ProductPublisherNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductPublisher)
                .HasConstraintName("FKb8lf9e3p2wad5qyyu08jx6ebs");

            entity.HasOne(d => d.ProductTypeNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductType)
                .HasConstraintName("FKij1k07u3o1luyr3duuk4glewn");
        });

        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.HasKey(e => e.ProdAttId).HasName("PRIMARY");

            entity.ToTable("product_attribute");

            entity.HasIndex(e => e.AttributeId, "FKefc9famxhv98xs6686269a79");

            entity.HasIndex(e => e.ProductId, "FKlefs59y5kmsbu017n1wp10gf2");

            entity.Property(e => e.ProdAttId).HasColumnName("prod_att_id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.AtttributeValue)
                .HasMaxLength(255)
                .HasColumnName("atttribute_value");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Attribute).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKefc9famxhv98xs6686269a79");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAttributes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKlefs59y5kmsbu017n1wp10gf2");
        });

        modelBuilder.Entity<ProductBeneficiary>(entity =>
        {
            entity.HasKey(e => e.ProdbenId).HasName("PRIMARY");

            entity.ToTable("product_beneficiary");

            entity.HasIndex(e => e.BeneficiaryId, "FKcry2xtp9lj2xd0a4ua4l5y6en");

            entity.HasIndex(e => e.ProductId, "FKkhfp9o212p3x1e5rhts34qqli");

            entity.HasIndex(e => e.RoycalId, "FKkx1d3hr3vyno6q02do5ueygs4");

            entity.Property(e => e.ProdbenId).HasColumnName("prodben_id");
            entity.Property(e => e.BeneficiaryId).HasColumnName("beneficiary_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RoyaltyReceived)
                .HasPrecision(38, 2)
                .HasColumnName("royalty_received");
            entity.Property(e => e.RoycalId).HasColumnName("roycal_id");

            entity.HasOne(d => d.Beneficiary).WithMany(p => p.ProductBeneficiaries)
                .HasForeignKey(d => d.BeneficiaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKcry2xtp9lj2xd0a4ua4l5y6en");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductBeneficiaries)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKkhfp9o212p3x1e5rhts34qqli");

            entity.HasOne(d => d.Roycal).WithMany(p => p.ProductBeneficiaries)
                .HasForeignKey(d => d.RoycalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKkx1d3hr3vyno6q02do5ueygs4");
        });

        modelBuilder.Entity<ProductTypeMaster>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");

            entity.ToTable("product_type_master");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.TypeDesc)
                .HasMaxLength(50)
                .HasColumnName("type_desc");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PRIMARY");

            entity.ToTable("publisher");

            entity.HasIndex(e => e.Email, "UKtq31gshjc2w4bjif7cw51o25").IsUnique();

            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RoyaltyCalculation>(entity =>
        {
            entity.HasKey(e => e.RoycalId).HasName("PRIMARY");

            entity.ToTable("royalty_calculation");

            entity.HasIndex(e => e.ProductId, "FK9vc0u607pogh735h76dqmth3l");

            entity.HasIndex(e => e.ItemId, "FKh50sq6rrpjk0h50de0d82ip3l");

            entity.Property(e => e.RoycalId).HasColumnName("roycal_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RoyaltyPercent)
                .HasPrecision(38, 2)
                .HasColumnName("royalty_percent");
            entity.Property(e => e.RoycalTrandate).HasColumnName("roycal_trandate");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(38, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.TotalRoyalty)
                .HasPrecision(38, 2)
                .HasColumnName("total_royalty");

            //entity.HasOne(d => d.Item).WithMany(p => p.RoyaltyCalculations)
            //    .HasForeignKey(d => d.ItemId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FKh50sq6rrpjk0h50de0d82ip3l");

            entity.HasOne(d => d.Product).WithMany(p => p.RoyaltyCalculations)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK9vc0u607pogh735h76dqmth3l");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");

            entity.ToTable("transactions");

            entity.HasIndex(e => e.UserId, "FK9e5ssu5c6n40gw5bgt5dg4mph");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(6)
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasColumnType("enum('FAILED','PENDING','SUCCESS')")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(38, 2)
                .HasColumnName("total_amount");
            entity.Property(e => e.TransactionType)
                .HasColumnType("enum('BUY','RENT')")
                .HasColumnName("transaction_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK9e5ssu5c6n40gw5bgt5dg4mph");
        });

        modelBuilder.Entity<TransactionItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("transaction_items");

            entity.HasIndex(e => e.ProductId, "FKaghtk4pgcgrsf2c4n1rueb613");

            entity.HasIndex(e => e.TransactionId, "FKfaqqkmi2ahnahay1ciwffqwyp");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Price)
                .HasPrecision(38, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FKaghtk4pgcgrsf2c4n1rueb613");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionItems)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FKfaqqkmi2ahnahay1ciwffqwyp");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserEmail, "UKj09k2v8lxofv2vecxu2hde9so").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IsAdmin)
                .HasColumnType("bit(1)")
                .HasColumnName("is_admin");
            entity.Property(e => e.JoinDate).HasColumnName("join_date");
            entity.Property(e => e.UserAddress)
                .HasColumnType("text")
                .HasColumnName("user_address");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(80)
                .HasColumnName("user_email");
            entity.Property(e => e.UserName)
                .HasMaxLength(80)
                .HasColumnName("user_name");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .HasColumnName("user_password");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(20)
                .HasColumnName("user_phone");
        });

        modelBuilder.Entity<LibraryPackagePurchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PRIMARY");
            entity.ToTable("library_package_purchase");

            entity.Property(e => e.PurchaseId).HasColumnName("purchase_id");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");

            entity.Property(e => e.PackagePrice)
                .HasPrecision(38, 2)
                .HasColumnName("package_price");

            entity.Property(e => e.AllowedBooks)
                .HasColumnName("allowed_books");

            entity.Property(e => e.AvgBookPrice)
                .HasPrecision(38, 2)
                .HasColumnName("avg_book_price");

            entity.Property(e => e.PurchaseDate)
                .HasColumnName("purchase_date");

            entity.HasOne(d => d.Transaction)
                .WithMany()
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("fk_lpp_transaction");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_lpp_user");

            entity.HasOne(d => d.Package)
                .WithMany()
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("fk_lpp_package");
        });

        modelBuilder.Entity<LibraryPackagePurchaseItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");
            entity.ToTable("library_package_purchase_item");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.PurchaseId).HasColumnName("purchase_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.Property(e => e.RoyaltyPercent)
                .HasPrecision(38, 2)
                .HasColumnName("royalty_percent");

            entity.Property(e => e.RoyaltyAmount)
                .HasPrecision(38, 2)
                .HasColumnName("royalty_amount");

            entity.HasOne(d => d.Purchase)
                .WithMany(p => p.Items)
                .HasForeignKey(d => d.PurchaseId)
                .HasConstraintName("fk_lppi_purchase");

            entity.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_lppi_product");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
