package com.example.dto;

import java.util.List;

import com.example.models.TransactionType;

public class CheckoutRequest {

    private Integer userId;
    private List<Integer> productIds;
    private TransactionType transactionType;
    private Integer rentDays;

    public Integer getUserId() {
        return userId;
    }

    public void setUserId(Integer userId) {
        this.userId = userId;
    }

    public List<Integer> getProductIds() {
        return productIds;
    }

    public void setProductIds(List<Integer> productIds) {
        this.productIds = productIds;
    }
    
    
    public Integer getRentDays() {
        return rentDays;
    }

    public void setRentDays(Integer rentDays) {
        this.rentDays = rentDays;
    }

}

