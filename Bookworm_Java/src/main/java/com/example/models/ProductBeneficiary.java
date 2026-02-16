package com.example.models;

import jakarta.persistence.*;
import java.math.BigDecimal;

@Entity
@Table(name = "product_beneficiary")
public class ProductBeneficiary {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "prodben_id")
    private int prodbenId;

    // -------- Relationship with Beneficiary --------

    @ManyToOne
    @JoinColumn(name = "beneficiary_id", nullable = false)
    private Beneficiary beneficiary;

    // -------- Relationship with Product --------

    @ManyToOne
    @JoinColumn(name = "product_id", nullable = false)
    private Product product;

    // -------- Relationship with Royalty Calculation --------

    @ManyToOne
    @JoinColumn(name = "roycal_id", nullable = false)
    private RoyaltyCalculation royaltyCalculation;

    // -------- Royalty Received --------

    @Column(name = "royalty_received")
    private BigDecimal royaltyReceived;

    // -------- No-Arg Constructor (REQUIRED by JPA) --------

    public ProductBeneficiary() {
    }

    // -------- Getters and Setters --------

    public int getProdbenId() {
        return prodbenId;
    }

    public void setProdbenId(int prodbenId) {
        this.prodbenId = prodbenId;
    }

    public Beneficiary getBeneficiary() {
        return beneficiary;
    }

    public void setBeneficiary(Beneficiary beneficiary) {
        this.beneficiary = beneficiary;
    }

    public Product getProduct() {
        return product;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    public RoyaltyCalculation getRoyaltyCalculation() {
        return royaltyCalculation;
    }

    public void setRoyaltyCalculation(RoyaltyCalculation royaltyCalculation) {
        this.royaltyCalculation = royaltyCalculation;
    }

    public BigDecimal getRoyaltyReceived() {
        return royaltyReceived;
    }

    public void setRoyaltyReceived(BigDecimal royaltyReceived) {
        this.royaltyReceived = royaltyReceived;
    }
}
