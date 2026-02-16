package com.example.Repository;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.example.models.LibraryPackagePurchase;

@Repository
public interface LibraryPackagePurchaseRepository
        extends JpaRepository<LibraryPackagePurchase, Integer> {

    // ❗ Check if user already has an active package
	List<LibraryPackagePurchase> findByUser_UserIdOrderByPurchaseDateDesc(
            Integer userId
    );
}
