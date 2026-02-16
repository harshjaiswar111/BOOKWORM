package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;

import com.example.models.ReadBook;

public interface ReadBookRepository extends JpaRepository<ReadBook, Integer>{

	ReadBook findByProduct_ProductId(int productId);
}
