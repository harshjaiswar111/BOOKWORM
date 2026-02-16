package com.example.Controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.example.models.Cart;
import com.example.models.Product;
import com.example.models.User;
import com.example.Repository.ProductRepository;
import com.example.Repository.UserRepository;
import com.example.Services.CartService;

import com.example.dto.AddToCartRequest;
import com.example.dto.DeleteCartItemRequest;
import com.example.dto.UpdateCartQtyRequest;

@RestController
@RequestMapping("/api/cart")
@CrossOrigin(origins = "http://localhost:5173")
public class CartController {

    @Autowired
    private CartService cartService;

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private ProductRepository productRepository;
    
    @PostMapping("/add")
    public Cart addToCart(@RequestBody AddToCartRequest request) {
    	
    	
    	if (request.getUserId() == null) {
            throw new RuntimeException("userId is missing in request");
        }
        if (request.getProductId() == null) {
            throw new RuntimeException("productId is missing in request");
        }
        if (request.getQty() == null) {
            request.setQty(1);
        }

        System.out.println("USER ID = " + request.getUserId());
        System.out.println("PRODUCT ID = " + request.getProductId());
        System.out.println("QTY = " + request.getQty());

        User user = userRepository.findById(request.getUserId())
            .orElseThrow(() -> new RuntimeException("User not found"));

        Product product = productRepository.findById(request.getProductId())
            .orElseThrow(() -> new RuntimeException("Product not found"));

        return cartService.addToCart(user, product, request.getQty());
    }

    @GetMapping("/{userId}")
    public List<Cart> getCart(@PathVariable("userId") Integer userId) {

        User user = userRepository.findById(userId)
                .orElseThrow(() -> new RuntimeException("User not found"));

        return cartService.getCartByUser(user);
    }
    
    @PutMapping("/{cartId}")
    public Cart updateQty(
            @PathVariable("cartId") Integer cartId,
            @RequestBody UpdateCartQtyRequest request) {

        return cartService.updateQty(cartId, request.getQty());
    }
    
    
    
    @DeleteMapping("/remove/{cartId}")
    public ResponseEntity<String> removeFromCart(@PathVariable int cartId) {
        cartService.removeItem(cartId);
        return ResponseEntity.ok("Removed");
    }
    
//    @DeleteMapping("/remove")
//    public void removeItem(@RequestBody DeleteCartItemRequest request) {
//
//        cartService.removeItem(request.getCartId());
//    }

}