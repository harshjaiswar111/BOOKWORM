package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.example.models.LibraryPackage;

import java.util.Optional;

@Repository
public interface LibraryPackageRepository extends JpaRepository<LibraryPackage, Integer> {

    // Find package by name (Silver / Gold / Premium)
    Optional<LibraryPackage> findByName(String name);

    // Check if package exists by name
    boolean existsByName(String name);
}