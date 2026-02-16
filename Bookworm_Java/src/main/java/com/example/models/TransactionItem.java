package com.example.models;

import java.math.BigDecimal;

import com.example.models.Transaction;

import jakarta.persistence.*;

@Entity
@Table(name = "transaction_items")
public class TransactionItem {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer itemId;

    @ManyToOne
    @JoinColumn(name = "transaction_id")
    private Transaction transaction;

    @ManyToOne
    @JoinColumn(name = "product_id")
    private Product product;

    private BigDecimal price;

    private Integer quantity;

    // getters and setters
    
    public Integer getItemId() {
        return itemId;
    }

    public Transaction getTransaction() {
        return transaction;
    }

    public Product getProduct() {
        return product;
    }

    public BigDecimal getPrice() {
        return price;
    }

    public Integer getQuantity() {
        return quantity;
    }

    // ===== SETTERS =====

    public void setItemId(Integer itemId) {
        this.itemId = itemId;
    }

    public void setTransaction(Transaction transaction) {
        this.transaction = transaction;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    public void setPrice(BigDecimal price) {
        this.price = price;
    }

    public void setQuantity(Integer quantity) {
        this.quantity = quantity;
    }
}
