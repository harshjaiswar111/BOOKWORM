package com.example.Services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.example.dto.TransactionItemDTO;
import com.example.models.*;
import com.example.Repository.*;

@Service
public class TransactionItemService {

    @Autowired
    private TransactionItemRepository transactionItemRepository;

    // GET ALL transaction items
    public List<TransactionItemDTO> getAllItems() {
        return transactionItemRepository.findAll()
                .stream()
                .map(this::mapToDTO)
                .toList();
    }

    // GET items by transaction_id
    public List<TransactionItemDTO> getItemsByTransactionId(Long transactionId) {
        return transactionItemRepository
                .findByTransactionTransactionId(transactionId)
                .stream()
                .map(this::mapToDTO)
                .toList();
    }

    private TransactionItemDTO mapToDTO(TransactionItem item) {
        TransactionItemDTO dto = new TransactionItemDTO();
        dto.setItemID(item.getItemId());
        dto.setTransactionId(item.getTransaction().getTransactionId());
        dto.setProductId(item.getProduct().getProductId());
        dto.setQuantity(item.getQuantity());
        dto.setPrice(item.getPrice());
        return dto;
    }
}
