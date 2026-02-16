package com.example.Services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import com.example.Repository.ProductRepository;
import com.example.Repository.ReadBookRepository;
import com.example.models.Product;
import com.example.models.ReadBook;

@Service
public class ReadBookService {

	@Autowired
    private ReadBookRepository readBookRepository;

    @Autowired
    private ProductRepository productRepository;

    public void savePdf(MultipartFile file, int productId) throws Exception {

        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new RuntimeException("Book not found"));

        ReadBook readBook = new ReadBook();
        readBook.setFileName(file.getOriginalFilename());
        readBook.setPdfData(file.getBytes());
        readBook.setProduct(product);

        readBookRepository.save(readBook);
    }

    public ReadBook getPdfByProductId(int productId) {
        return readBookRepository.findByProduct_ProductId(productId);
    }
    
    
 // ✅ ADD THIS METHOD (THIS FIXES EVERYTHING)
    public byte[] readBook(int productId) {

        ReadBook readBook = readBookRepository.findByProduct_ProductId(productId);

        if (readBook == null || readBook.getPdfData() == null) {
            throw new RuntimeException("PDF not found for this book");
        }

        return readBook.getPdfData();
    }
}
