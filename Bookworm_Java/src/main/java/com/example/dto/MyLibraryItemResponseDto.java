package com.example.dto;

import java.time.LocalDate;

public class MyLibraryItemResponseDto {

    private Integer packageId;
    private LocalDate startDate;
    private LocalDate endDate;
    private ProductResponseDto product;
	public Integer getPackageId() {
		return packageId;
	}
	public void setPackageId(Integer packageId) {
		this.packageId = packageId;
	}
	public LocalDate getStartDate() {
		return startDate;
	}
	public void setStartDate(LocalDate startDate) {
		this.startDate = startDate;
	}
	public LocalDate getEndDate() {
		return endDate;
	}
	public void setEndDate(LocalDate endDate) {
		this.endDate = endDate;
	}
	public ProductResponseDto getProduct() {
		return product;
	}
	public void setProduct(ProductResponseDto product) {
		this.product = product;
	}

    // getters & setters
}
