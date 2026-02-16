package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.example.models.Genere;

@Repository
public interface GenereRepository extends JpaRepository<Genere, Integer> {
}
