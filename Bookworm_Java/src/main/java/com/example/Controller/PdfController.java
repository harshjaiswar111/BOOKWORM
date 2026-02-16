package com.example.Controller;

import java.util.List;

import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.example.Repository.TransactionItemRepository;
import com.example.Repository.TransactionRepository;
import com.example.Services.TransactionPdfService;
import com.example.models.Transaction;
import com.example.models.TransactionItem;
import com.example.models.TransactionStatus;

@RestController
@RequestMapping("/api/invoice")
@CrossOrigin(origins = "http://localhost:5173")
public class PdfController {

    private final TransactionRepository transactionRepo;
    private final TransactionItemRepository itemRepo;
    private final TransactionPdfService pdfService;

    public PdfController(TransactionRepository transactionRepo,
                             TransactionItemRepository itemRepo,
                             TransactionPdfService pdfService) {
        this.transactionRepo = transactionRepo;
        this.itemRepo = itemRepo;
        this.pdfService = pdfService;
    }

    @GetMapping("/{transactionId}")
    public ResponseEntity<byte[]> downloadInvoice(
            @PathVariable("transactionId") Long transactionId) {

        Transaction transaction = transactionRepo.findById(transactionId)
                .orElseThrow(() -> new RuntimeException("Transaction not found"));

        if (transaction.getStatus() != TransactionStatus.SUCCESS) {
            throw new RuntimeException("Invoice available only after success");
        }

        List<TransactionItem> items = itemRepo.findByTransaction(transaction);

        byte[] pdf = pdfService.generateInvoice(transaction, items);

        return ResponseEntity.ok()
                .header("Content-Disposition",
                        "attachment; filename=invoice_" + transactionId + ".pdf")
                .contentType(MediaType.APPLICATION_PDF)
                .body(pdf);
    }
}