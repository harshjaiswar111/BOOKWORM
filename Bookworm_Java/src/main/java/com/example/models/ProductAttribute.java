package com.example.models;

import jakarta.persistence.*;

@Entity
@Table(name = "product_attribute")
public class ProductAttribute {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "prod_att_id")
    private int prodAttId;

    // -------- Relationship with Product --------

    @ManyToOne
    @JoinColumn(name = "product_id", nullable = false)
    private Product product;

    // -------- Relationship with Attribute --------

    @ManyToOne
    @JoinColumn(name = "attribute_id", nullable = false)
    private Attribute attribute;

    // -------- Attribute Value --------

    @Column(name = "atttribute_value")
    private String attributeValue;

    // -------- Constructors --------

    public ProductAttribute() {
    }

    

    // -------- Getters and Setters --------

    public int getProdAttId() {
        return prodAttId;
    }

    public void setProdAttId(int prodAttId) {
        this.prodAttId = prodAttId;
    }

    public Product getProduct() {
        return product;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    public Attribute getAttribute() {
        return attribute;
    }

    public void setAttribute(Attribute attribute) {
        this.attribute = attribute;
    }

    public String getAttributeValue() {
        return attributeValue;
    }

    public void setAttributeValue(String attributeValue) {
        this.attributeValue = attributeValue;
    }
}
