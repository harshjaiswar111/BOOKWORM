package com.example.Repository;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.example.models.Beneficiary;

@Repository
public interface BeneficiaryRepository extends JpaRepository<Beneficiary, Integer> {

    // You can add custom query methods later if needed
    // Example:
    // Beneficiary findByBeneficiaryEmailId(String emailId);
	List<Beneficiary> findByProduct_ProductId(Integer productId);
}
