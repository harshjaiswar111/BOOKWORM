package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.example.models.ProductBeneficiary;

public interface ProductBeneficiaryRepository extends JpaRepository<ProductBeneficiary, Integer> {

}
