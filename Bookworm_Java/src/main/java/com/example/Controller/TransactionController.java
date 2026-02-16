package com.example.Controller;
import java.util.*;

import org.springframework.web.bind.annotation.*;

import com.example.Repository.TransactionRepository;

import com.example.models.Transaction;

@RestController
@RequestMapping("/api/transactions")
public class TransactionController {

    private final TransactionRepository transactionRepo;

    public TransactionController(TransactionRepository transactionRepo) {
        this.transactionRepo = transactionRepo;
    }

    @GetMapping("/user/{userId}")
    public List<Transaction> getUserTransactions(@PathVariable Long userId) {
        return transactionRepo.findByUserUserId(userId);
    }
}

