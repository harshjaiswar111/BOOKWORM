package com.example.models;

import jakarta.persistence.*;

@Entity
@Table(name = "product_type_master")
public class Product_Type {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Type_Id")
    private Integer typeId;

    @Column(name = "Type_Desc", length = 50)
    private String typeDesc;

    // -------- Constructors --------

    public Product_Type() {
    }

   

    // -------- Getters and Setters --------

    public Integer getTypeId() {
        return typeId;
    }

    public void setTypeId(Integer typeId) {
        this.typeId = typeId;
    }

    public String getTypeDesc() {
        return typeDesc;
    }

    public void setTypeDesc(String typeDesc) {
        this.typeDesc = typeDesc;
    }
}

