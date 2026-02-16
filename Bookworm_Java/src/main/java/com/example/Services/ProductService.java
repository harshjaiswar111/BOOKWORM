package com.example.Services;
import java.util.List;
import com.example.models.Product;

public interface ProductService {

	    Product saveProduct(Product product);          // save()
	    Product getProductById(Integer id);             // findById()
	    List<Product> getAllProducts();                 // findAll()
	    Product updateProduct(Integer id, Product product); // save()
	    void deleteProduct(Integer id);                 // deleteById()
	    
	    List<Product> searchProductsByName(String name, Integer limit);
	    
	    List<Product> getLibraryProducts();
	    List<Product> filterProducts(String genere, String language);
	}