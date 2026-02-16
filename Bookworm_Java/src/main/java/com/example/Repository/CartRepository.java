package com.example.Repository;

import java.util.List;
import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.example.models.Cart;
import com.example.models.Product;
import com.example.models.User;

@Repository
public interface CartRepository extends JpaRepository<Cart, Integer> {

    Optional<Cart> findByUserAndProduct(User user, Product product);

    List<Cart> findByUser(User user);
    
    void deleteByUser(User user);
}
