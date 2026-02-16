package com.example.models;

import jakarta.persistence.*;
import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "library_package_purchase")
public class LibraryPackagePurchase {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "purchase_id")
    private Integer purchaseId;

    // ---------- Relationships ----------

    @ManyToOne(fetch = FetchType.LAZY, optional = false)
    @JoinColumn(name = "transaction_id", nullable = false)
    private Transaction transaction;

    @ManyToOne(fetch = FetchType.LAZY, optional = false)
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    @ManyToOne(fetch = FetchType.LAZY, optional = false)
    @JoinColumn(name = "package_id", nullable = false)
    private LibraryPackage libraryPackage;

    // ---------- Columns (MATCH DB EXACTLY) ----------

    @Column(name = "package_price", precision = 38, scale = 2, nullable = false)
    private BigDecimal packagePrice;

    @Column(name = "allowed_books", nullable = false)
    private Integer allowedBooks;

    @Column(name = "avg_book_price", precision = 38, scale = 2, nullable = false)
    private BigDecimal avgBookPrice;

    @Column(name = "purchase_date", nullable = false)
    private LocalDateTime purchaseDate;

    // ---------- Child Items ----------

    @OneToMany(
        mappedBy = "purchase",
        cascade = CascadeType.ALL,
        orphanRemoval = true
    )
    private List<LibraryPackagePurchaseItem> items = new ArrayList<>();

    // ---------- Getters & Setters ----------

    public Integer getPurchaseId() {
        return purchaseId;
    }

    public Transaction getTransaction() {
        return transaction;
    }

    public void setTransaction(Transaction transaction) {
        this.transaction = transaction;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

    public LibraryPackage getLibraryPackage() {
        return libraryPackage;
    }

    public void setLibraryPackage(LibraryPackage libraryPackage) {
        this.libraryPackage = libraryPackage;
    }

    public BigDecimal getPackagePrice() {
        return packagePrice;
    }

    public void setPackagePrice(BigDecimal packagePrice) {
        this.packagePrice = packagePrice;
    }

    public Integer getAllowedBooks() {
        return allowedBooks;
    }

    public void setAllowedBooks(Integer allowedBooks) {
        this.allowedBooks = allowedBooks;
    }

    public BigDecimal getAvgBookPrice() {
        return avgBookPrice;
    }

    public void setAvgBookPrice(BigDecimal avgBookPrice) {
        this.avgBookPrice = avgBookPrice;
    }

    public LocalDateTime getPurchaseDate() {
        return purchaseDate;
    }

    public void setPurchaseDate(LocalDateTime purchaseDate) {
        this.purchaseDate = purchaseDate;
    }

    public List<LibraryPackagePurchaseItem> getItems() {
        return items;
    }
}
