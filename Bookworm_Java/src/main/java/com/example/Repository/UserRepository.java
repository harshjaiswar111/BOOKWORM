package com.example.Repository;

import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.example.models.User;

public interface UserRepository extends JpaRepository<User, Integer> {
	Optional<User> findByUserEmail(String userEmail);
}

