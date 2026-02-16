package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.example.models.Transaction;

import java.util.List;

public interface TransactionRepository extends JpaRepository<Transaction, Long> {

    List<Transaction> findByUserUserId(Long userId);
}

