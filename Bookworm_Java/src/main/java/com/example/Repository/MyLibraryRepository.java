//	package com.example.Repository;
//	
//	import org.springframework.data.jpa.repository.JpaRepository;
//	import org.springframework.stereotype.Repository;
//	
//	import com.example.models.MyLibrary;
//	
//	import java.time.LocalDate;
//	import java.util.List;
//	import java.util.Optional;
//	
//	@Repository
//	public interface MyLibraryRepository extends JpaRepository<MyLibrary, Integer> {
//	
//	    // Get all library records for a user
//	    List<MyLibrary> findByUser_UserId(Integer userId);
//	
//	    // Get active subscription for a user (based on date)
//	    Optional<MyLibrary> findByUser_UserIdAndEndDateAfter(
//	            Integer userId,
//	            LocalDate today
//	    );
//	
//	    // Check if user already has an active subscription
//	    boolean existsByUser_UserIdAndEndDateAfter(
//	            Integer userId,
//	            LocalDate today
//	    );
//	
//	    // Find expired subscriptions
//	    List<MyLibrary> findByEndDateBefore(LocalDate today);
//	    
//	    int countByUser_UserIdAndEndDateAfter(
//	            Integer userId,
//	            LocalDate today
//	    );
//	}

package com.example.Repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.example.models.MyLibrary;

import java.time.LocalDate;
import java.util.List;

@Repository
public interface MyLibraryRepository
        extends JpaRepository<MyLibrary, Integer> {

    // All active library books for a user
    List<MyLibrary> findByUser_UserIdAndEndDateAfter(
            Integer userId,
            LocalDate today
    );

    // Count active books for limit enforcement
    int countByUser_UserIdAndEndDateAfter(
            Integer userId,
            LocalDate today
    );

    // Check if a specific book is already owned
    boolean existsByUser_UserIdAndProduct_ProductIdAndEndDateAfter(
            Integer userId,
            Integer productId,
            LocalDate today
    );
}
