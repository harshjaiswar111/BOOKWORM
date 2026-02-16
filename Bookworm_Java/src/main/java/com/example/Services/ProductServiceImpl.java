package com.example.Services;

import java.util.List;

import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;

import com.example.models.Product;
import com.example.Repository.ProductRepository;

@Service
public class ProductServiceImpl implements ProductService {

    private final ProductRepository productRepository;

    // ✅ Constructor Injection (REQUIRED for final fields)
    public ProductServiceImpl(ProductRepository productRepository) {
        this.productRepository = productRepository;
    }

    // CREATE
    @Override
    public Product saveProduct(Product product) {
        return productRepository.save(product);
    }

    // READ by ID
    @Override
    public Product getProductById(Integer id) {
        return productRepository.findById(id).orElse(null);
    }

    // READ all
    @Override
    public List<Product> getAllProducts() {
        return productRepository.findAll();
    }

    // UPDATE
    @Override
    public Product updateProduct(Integer id, Product product) {

        Product existing = getProductById(id);

        // -------- Basic Info --------
        if (product.getProductName() != null)
            existing.setProductName(product.getProductName());

        if (product.getProductDescriptionShort() != null)
            existing.setProductDescriptionShort(product.getProductDescriptionShort());

        if (product.getProductDescriptionLong() != null)
            existing.setProductDescriptionLong(product.getProductDescriptionLong());

        if (product.getProductImage() != null)
            existing.setProductImage(product.getProductImage());

        if (product.getProductIsbn() != null)
            existing.setProductIsbn(product.getProductIsbn());

        // -------- Pricing --------
        if (product.getProductBaseprice() != null)
            existing.setProductBaseprice(product.getProductBaseprice());

        if (product.getProductOfferprice() != null)
            existing.setProductOfferprice(product.getProductOfferprice());

        if (product.getDiscountPercent() != null)
            existing.setDiscountPercent(product.getDiscountPercent());

        if (product.getRoyaltyPercent() != null)
            existing.setRoyaltyPercent(product.getRoyaltyPercent());

        if (product.getProductOffPriceExpirydate() != null)
            existing.setProductOffPriceExpirydate(product.getProductOffPriceExpirydate());

        // -------- Relationships --------
        if (product.getProductType() != null)
            existing.setProductType(product.getProductType());

        if (product.getAuthor() != null)
            existing.setAuthor(product.getAuthor());

        if (product.getPublisher() != null)
            existing.setPublisher(product.getPublisher());

        if (product.getLanguage() != null)
            existing.setLanguage(product.getLanguage());

        if (product.getGenere() != null)
            existing.setGenere(product.getGenere());

        if (product.getAttribute() != null)
            existing.setAttribute(product.getAttribute());

        // -------- Rental / Library --------
        // NOTE: boolean primitives cannot be null → always overwrite
        existing.setRentable(product.isRentable());
        existing.setLibrary(product.isLibrary());

        if (product.getRentPerDay() != null)
            existing.setRentPerDay(product.getRentPerDay());

        if (product.getMinRentDays() > 0)
            existing.setMinRentDays(product.getMinRentDays());

        return productRepository.save(existing);
    }

    // DELETE
    @Override
    public void deleteProduct(Integer id) {
        productRepository.deleteById(id);
    }

	@Override
	public List<Product> searchProductsByName(String name, Integer limit) {

	    // 1️ Normalize input (avoid null & extra spaces)
	    String query = (name == null) ? "" : name.trim();

	    // 2️ Empty search → return nothing (safe for live search)
	    if (query.isEmpty()) {
	        return List.of();
	    }

	    // 3️ Protect DB with safe limit (default 10, max 50)
	    int safeLimit = (limit == null)
	            ? 10
	            : Math.min(Math.max(limit, 1), 50);

	    // 4️ Perform limited, case-insensitive search
	    return productRepository.findByProductNameContainingIgnoreCase(query,PageRequest.of(0, safeLimit)
	    );
	}
	
	
	
	@Override
	public List<Product> getLibraryProducts() {
	    return productRepository.findByLibraryTrue();
	}
	
	
	@Override
	public List<Product> filterProducts(String genere, String language) {

	    boolean hasGenere = genere != null && !genere.isBlank();
	    boolean hasLanguage = language != null && !language.isBlank();

	    // ✅ BOTH dropdowns selected
	    if (hasGenere && hasLanguage) {
	        return productRepository
	                .findByGenere_GenereDescAndLanguage_LanguageDescAndLibraryTrue(
	                        genere,
	                        language
	                );
	    }

	    // ✅ ONLY genre selected
	    if (hasGenere) {
	        return productRepository
	                .findByGenere_GenereDescAndLibraryTrue(genere);
	    }

	    // ✅ ONLY language selected
	    if (hasLanguage) {
	        return productRepository
	                .findByLanguage_LanguageDescAndLibraryTrue(language);
	    }

	    // ✅ NONE selected → show all library books
	    return productRepository.findByLibraryTrue();
	}

    
}