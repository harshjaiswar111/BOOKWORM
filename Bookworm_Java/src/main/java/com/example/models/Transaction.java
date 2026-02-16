package com.example.models;

import jakarta.persistence.*;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;



@Entity
@Table(name = "transactions")
public class Transaction {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long transactionId;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    private BigDecimal totalAmount;

    @Enumerated(EnumType.STRING)
    private TransactionStatus status;

    private LocalDateTime createdAt = LocalDateTime.now();

    @OneToMany(mappedBy = "transaction", cascade = CascadeType.ALL)
    private List<TransactionItem> items;
    
    @Enumerated(EnumType.STRING)
    @Column(name = "transaction_type")
    private TransactionType transactionType;


    // getters and setters
    
    public Long getTransactionId() {
        return transactionId;
    }

    public User getUser() {
        return user;
    }

    public BigDecimal getTotalAmount() {
        return totalAmount;
    }

    public TransactionStatus getStatus() {
        return status;
    }

    public LocalDateTime getCreatedAt() {
        return createdAt;
    }

    public List<TransactionItem> getItems() {
        return items;
    }

    // ===== SETTERS =====

    public void setTransactionId(Long transactionId) {
        this.transactionId = transactionId;
    }

    public void setUser(User user) {
        this.user = user;
    }

    public void setTotalAmount(BigDecimal totalAmount) {
        this.totalAmount = totalAmount;
    }

    public void setStatus(TransactionStatus status) {
        this.status = status;
    }

    public void setCreatedAt(LocalDateTime createdAt) {
        this.createdAt = createdAt;
    }

    public void setItems(List<TransactionItem> items) {
        this.items = items;
    }
    
    
    public TransactionType getTransactionType() {
        return transactionType;
    }

    public void setTransactionType(TransactionType transactionType) {
        this.transactionType = transactionType;
    }

}

