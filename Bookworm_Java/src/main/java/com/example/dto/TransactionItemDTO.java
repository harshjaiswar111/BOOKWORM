package com.example.dto;

import java.math.BigDecimal;

public class TransactionItemDTO {
	
	private Integer itemID;
	private Long transactionId;
    private int productId;
    private Integer quantity;
    private BigDecimal price;
	public Integer getItemID() {
		return itemID;
	}
	public void setItemID(Integer itemID) {
		this.itemID = itemID;
	}
	public Long getTransactionId() {
		return transactionId;
	}
	public void setTransactionId(Long transactionId) {
		this.transactionId = transactionId;
	}
	public int getProductId() {
		return productId;
	}
	public void setProductId(int i) {
		this.productId = i;
	}
	public Integer getQuantity() {
		return quantity;
	}
	public void setQuantity(Integer quantity) {
		this.quantity = quantity;
	}
	public BigDecimal getPrice() {
		return price;
	}
	public void setPrice(BigDecimal price) {
		this.price = price;
	}
    
    
    
	

}
