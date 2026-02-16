package com.example.Services;

import java.util.List;

import com.example.models.Cart;
import com.example.models.Product;
import com.example.models.User;

public interface CartService {

    Cart addToCart(User user, Product product, Integer qty);

    List<Cart> getCartByUser(User user);

    Cart updateQty(Integer cartId, Integer qty);

    void removeItem(Integer cartId);

    void clearCart(User user);
}