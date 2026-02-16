package com.example.Controller;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import com.example.Services.LibraryCheckoutService;
import com.example.dto.LibraryCheckoutRequest;

@RestController
@RequestMapping("/api/library")
@CrossOrigin(origins = "http://localhost:5173")
public class LibraryCheckoutController {

    private final LibraryCheckoutService libraryCheckoutService;

    // ✅ Constructor injection (BEST PRACTICE)
    public LibraryCheckoutController(LibraryCheckoutService libraryCheckoutService) {
        this.libraryCheckoutService = libraryCheckoutService;
    }

    @PostMapping("/checkout")
    public ResponseEntity<Void> checkoutLibrary(
            @RequestBody LibraryCheckoutRequest request) {

        libraryCheckoutService.checkout(request);
        return ResponseEntity.ok().build();
    }
}