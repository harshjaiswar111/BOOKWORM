package com.example.Services;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.example.models.Cart;
import com.example.models.Product;
import com.example.models.User;
import com.example.Repository.CartRepository;

@Service
public class CartServiceImpl implements CartService {

    @Autowired
    private CartRepository cartRepository;

    @Override
    public Cart addToCart(User user, Product product, Integer qty) {

        Optional<Cart> existing =
            cartRepository.findByUserAndProduct(user, product);

        if (existing.isPresent()) {
            Cart cart = existing.get();
            cart.setQty(cart.getQty() + qty);
            return cartRepository.save(cart);
        }

        Cart cart = new Cart();
        cart.setUser(user);
        cart.setProduct(product);
        cart.setQty(qty);

        return cartRepository.save(cart);
    }

    @Override
    public List<Cart> getCartByUser(User user) {
        return cartRepository.findByUser(user);
    }

    @Override
    public Cart updateQty(Integer cartId, Integer qty) {
        Cart cart = cartRepository.findById(cartId)
            .orElseThrow(() -> new RuntimeException("Cart item not found"));

        cart.setQty(qty);
        return cartRepository.save(cart);
    }

    @Override
    public void removeItem(Integer cartId) {
        cartRepository.deleteById(cartId);
    }

    @Override
    public void clearCart(User user) {
        List<Cart> cartItems = cartRepository.findByUser(user);
        cartRepository.deleteAll(cartItems);
    }
}