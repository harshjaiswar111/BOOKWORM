package com.example.models;

import jakarta.persistence.*;
import java.math.BigDecimal;

@Entity
@Table(name = "library_package_purchase_item")
public class LibraryPackagePurchaseItem {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "item_id")
    private Integer itemId;

    // ---------- Relationships ----------

    @ManyToOne(fetch = FetchType.LAZY, optional = false)
    @JoinColumn(name = "purchase_id", nullable = false)
    private LibraryPackagePurchase purchase;

    @ManyToOne(fetch = FetchType.LAZY, optional = false)
    @JoinColumn(name = "product_id", nullable = false)
    private Product product;

    // ---------- Columns (MATCH DB EXACTLY) ----------

    @Column(name = "royalty_percent", precision = 38, scale = 2, nullable = false)
    private BigDecimal royaltyPercent;

    @Column(name = "royalty_amount", precision = 38, scale = 2, nullable = false)
    private BigDecimal royaltyAmount;

    // ---------- Getters & Setters ----------

    public Integer getItemId() {
        return itemId;
    }

    public LibraryPackagePurchase getPurchase() {
        return purchase;
    }

    public void setPurchase(LibraryPackagePurchase purchase) {
        this.purchase = purchase;
    }

    public Product getProduct() {
        return product;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    public BigDecimal getRoyaltyPercent() {
        return royaltyPercent;
    }

    public void setRoyaltyPercent(BigDecimal royaltyPercent) {
        this.royaltyPercent = royaltyPercent;
    }

    public BigDecimal getRoyaltyAmount() {
        return royaltyAmount;
    }

    public void setRoyaltyAmount(BigDecimal royaltyAmount) {
        this.royaltyAmount = royaltyAmount;
    }
}
