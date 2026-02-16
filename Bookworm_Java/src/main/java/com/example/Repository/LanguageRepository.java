package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.example.models.Language;

@Repository
public interface LanguageRepository extends JpaRepository<Language, Integer> {
}
