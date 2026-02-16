package com.example.models;

import jakarta.persistence.*;
import java.time.LocalDate;
import java.time.LocalDateTime;

@Entity
@Table(name = "my_shelf")
public class MyShelf {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "shelf_id")
    private int shelfId;

    // -------- Relationship with User --------

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    // -------- Relationship with Product --------

    @ManyToOne
    @JoinColumn(name = "product_id", nullable = false)
    private Product product;

    // -------- Product Expiry Date --------

    @Column(name = "product_expiry_date")
    private LocalDateTime productExpiryDate;

    // -------- Constructors --------

    public MyShelf() {
    }

   

    // -------- Getters and Setters --------

    public int getShelfId() {
        return shelfId;
    }

    public void setShelfId(int shelfId) {
        this.shelfId = shelfId;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

    public Product getProduct() {
        return product;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    public LocalDateTime getProductExpiryDate() {
        return productExpiryDate;
    }

    public void setProductExpiryDate(LocalDateTime productExpiryDate) {
        this.productExpiryDate = productExpiryDate;
    }
}
