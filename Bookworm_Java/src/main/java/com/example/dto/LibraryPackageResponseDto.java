package com.example.dto;

import java.math.BigDecimal;

public class LibraryPackageResponseDto {

    private Integer packageId;
    private String name;
    private BigDecimal cost;
    private Integer bookLimit;
    private Integer validityDays;
    private String description;
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
	public Integer getBookLimit() {
		return bookLimit;
	}
	public void setBookLimit(Integer bookLimit) {
		this.bookLimit = bookLimit;
	}
	public Integer getValidityDays() {
		return validityDays;
	}
	public void setValidityDays(Integer validityDays) {
		this.validityDays = validityDays;
	}
	public String getDescription() {
		return description;
	}
	public void setDescription(String description) {
		this.description = description;
	}

    // getters & setters
}
