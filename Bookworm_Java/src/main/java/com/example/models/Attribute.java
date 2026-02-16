package com.example.models;

import jakarta.persistence.*;

@Entity
@Table(name = "attribute")
public class Attribute {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Attribute_Id")
    private int attributeId;

    @Column(name = "Attribute Desc", length = 255)
    private String attributeDesc;

    // -------- Constructors --------

    public Attribute() {
    }

   
    // -------- Getters and Setters --------

    public int getAttributeId() {
        return attributeId;
    }

    public void setAttributeId(int attributeId) {
        this.attributeId = attributeId;
    }

    public String getAttributeDesc() {
        return attributeDesc;
    }

    public void setAttributeDesc(String attributeDesc) {
        this.attributeDesc = attributeDesc;
    }
}
