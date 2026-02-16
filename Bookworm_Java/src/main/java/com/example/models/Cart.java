package com.example.models;

import jakarta.persistence.*;

@Entity
@Table(name = "cart")
public class Cart {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "cartId")
    private int cartId;

    // -------- Relationship with User --------

    @ManyToOne
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    // -------- Relationship with Product --------

    @ManyToOne
    @JoinColumn(name = "product_id", nullable = false)
    private Product product;

    // -------- Quantity --------

    @Column(name = "qty", nullable = false)
    private int qty;

    // -------- Constructors --------

    public Cart() {
    }

    

    // -------- Getters and Setters --------

    public int getCartId() {
        return cartId;
    }

    public void setCartId(int cartId) {
        this.cartId = cartId;
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

    public int getQty() {
        return qty;
    }

    public void setQty(int qty) {
        this.qty = qty;
    }
}
