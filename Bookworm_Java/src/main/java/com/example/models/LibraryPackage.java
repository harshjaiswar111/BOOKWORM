package com.example.models;

import jakarta.persistence.*;
import java.math.BigDecimal;

@Entity
@Table(name = "library_package")
public class LibraryPackage {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "package_id")
    private Integer packageId;

    @Column(name = "name", nullable = false)
    private String name;

    @Column(name = "cost", nullable = false, precision = 10, scale = 2)
    private BigDecimal cost;

    @Column(name = "validity_days")
    private Integer validityDays;

    @Column(name = "book_limit")
    private Integer bookLimit;

    @Column(name = "description", nullable = false)
    private String description;

    // ===== Constructors =====

    public LibraryPackage() {
    }

  
    // ===== Getters & Setters =====

    public Integer getPackageId() {
        return packageId;
    }

    public void setPackageId(Integer packageId) {
        this.packageId = packageId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public BigDecimal getCost() {
        return cost;
    }

    public void setCost(BigDecimal cost) {
        this.cost = cost;
    }

    public Integer getValidityDays() {
        return validityDays;
    }

    public void setValidityDays(Integer validityDays) {
        this.validityDays = validityDays;
    }

    public Integer getBookLimit() {
        return bookLimit;
    }

    public void setBookLimit(Integer bookLimit) {
        this.bookLimit = bookLimit;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }
}
