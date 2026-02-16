package com.example.Controller;

import java.util.List;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.example.models.Product;
import com.example.Services.ProductService;

@RestController	
@RequestMapping("/api/products")
public class ProductController {

	private final ProductService productService;

    // Constructor Injection
    public ProductController(ProductService productService) {
        this.productService = productService;
    }
    
    // READ ALL
    @GetMapping
    public ResponseEntity<List<Product>> getAllProducts() {
        return ResponseEntity.ok(productService.getAllProducts());
    }
    
    // READ BY ID
    @GetMapping("/{id}")
    public ResponseEntity<Product> getProductById(@PathVariable Integer id) {
        Product product = productService.getProductById(id);
        return ResponseEntity.ok(product);
    }
    
    @PostMapping
    public ResponseEntity<Product> createProduct(@RequestBody Product product) {
        Product saved = productService.saveProduct(product);
        return new ResponseEntity<>(saved, HttpStatus.CREATED);
    }
    
    // PARTIAL UPDATE (PATCH)
    @PatchMapping("/{id}")
    public ResponseEntity<Product> updateProduct (@PathVariable Integer id, @RequestBody Product product) {

        Product updated = productService.updateProduct(id, product);
        return ResponseEntity.ok(updated);
    }
    
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteProduct(@PathVariable Integer id) {
        productService.deleteProduct(id);
        return ResponseEntity.noContent().build();
    }
    
    
    @GetMapping("/search")
    public ResponseEntity<List<Product>> searchProducts(@RequestParam(required = false) String name,
@RequestParam(required = false) Integer limit) {

        return ResponseEntity.ok(
                productService.searchProductsByName(name, limit)
        );
    }
    
    
    
    @GetMapping("/secure-test")
    public String secureApi() {
        return "JWT WORKING - ACCESS GRANTED";
    }
    
    @GetMapping("/library")
    public ResponseEntity<List<Product>> libraryProducts() {
        return ResponseEntity.ok(
                productService.getLibraryProducts()
        );
    }
    
    
    @GetMapping("/filter")
    public ResponseEntity<List<Product>> filterProducts(
            @RequestParam(required = false) String genere,
            @RequestParam(required = false) String language
    ) {
        return ResponseEntity.ok(
                productService.filterProducts(genere, language)
        );
    }

    
}