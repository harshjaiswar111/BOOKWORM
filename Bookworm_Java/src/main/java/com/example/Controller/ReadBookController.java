package com.example.Controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

import com.example.Services.ReadBookService;
import com.example.models.ReadBook;

@RestController
@RequestMapping("/api/books")
public class ReadBookController {

	@Autowired
    private ReadBookService service;

	@PostMapping("/{productId}/upload")
    public ResponseEntity<String> uploadPdf(
            @PathVariable int productId,
            @RequestParam("file") MultipartFile file) {

        try {
            service.savePdf(file, productId);
            return ResponseEntity.ok("PDF uploaded successfully");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Upload failed");
        }
    }
    
    @GetMapping("/{productId}/read")
    public ResponseEntity<byte[]> readBook(@PathVariable int productId) {

        ReadBook book = service.getPdfByProductId(productId);

        return ResponseEntity.ok()
                .header("Content-Type", "application/pdf")
                .header("Content-Disposition",
                        "inline; filename=\"" + book.getFileName() + "\"")
                .body(book.getPdfData());
    }
    
}
