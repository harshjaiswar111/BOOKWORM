package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.example.models.MyShelf;

import java.time.LocalDateTime;
import java.util.List;

public interface MyShelfRepository extends JpaRepository<MyShelf, Integer> {

    List<MyShelf> findByUser_UserId(Integer userId);
    boolean existsByUser_UserIdAndProduct_ProductId(Integer userId, Integer productId);

    // ✅ delete all expired rent items
    void deleteByProductExpiryDateBefore(LocalDateTime dateTime);
}

