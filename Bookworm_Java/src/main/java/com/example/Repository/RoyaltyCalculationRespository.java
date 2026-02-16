package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.example.models.RoyaltyCalculation;

public interface RoyaltyCalculationRespository extends JpaRepository<RoyaltyCalculation, Integer> {

}
