package com.example.Controller;

import com.example.models.MyLibrary;
import com.example.Services.MyLibraryService;
import com.example.dto.MyLibraryItemResponseDto;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
//
//@RestController
//@RequestMapping("/api/my-library")
//@CrossOrigin(origins = "http://localhost:5173")
//public class MyLibraryController {
//
//    private final MyLibraryService myLibraryService;
//
//    public MyLibraryController(MyLibraryService myLibraryService) {
//        this.myLibraryService = myLibraryService;
//    }
//
//    // SUBSCRIBE user to library package
//    @PostMapping("/subscribe")
//    public ResponseEntity<MyLibrary> subscribeUser(
//            @RequestBody MyLibrary myLibrary) {
//
//        MyLibrary saved = myLibraryService.subscribeUser(myLibrary);
//        return new ResponseEntity<>(saved, HttpStatus.CREATED);
//    }
//
//    // GET active subscription for user
//    @GetMapping("/active/{userId}")
//    public ResponseEntity<MyLibrary> getActiveSubscription(
//            @PathVariable Integer userId) {
//
//        return ResponseEntity.ok(
//                myLibraryService.getActiveSubscription(userId)
//        );
//    }
//
//    // GET full library history for user
//    @GetMapping("/user/{userId}")
//    public ResponseEntity<List<MyLibrary>> getUserLibrary(
//            @PathVariable Integer userId) {
//
//        return ResponseEntity.ok(
//                myLibraryService.getUserLibrary(userId)
//        );
//    }
//
//    // INCREMENT books taken (borrow)
//    @PutMapping("/{myLibId}/borrow")
//    public ResponseEntity<Void> borrowBook(@PathVariable Integer myLibId) {
//
//        myLibraryService.incrementBooksTaken(myLibId);
//        return ResponseEntity.ok().build();
//    }
//
//    // DECREMENT books taken (return)
//    @PutMapping("/{myLibId}/return")
//    public ResponseEntity<Void> returnBook(@PathVariable Integer myLibId) {
//
//        myLibraryService.decrementBooksTaken(myLibId);
//        return ResponseEntity.ok().build();
//    }
//}



import org.springframework.beans.factory.annotation.Autowired;


import com.example.Repository.UserRepository;
import com.example.models.User;
import com.example.security.JwtUtil;

@RestController
@RequestMapping("/api/my-library")
@CrossOrigin(origins = "http://localhost:5173")
public class MyLibraryController {

    @Autowired
    private MyLibraryService myLibraryService;

    @Autowired
    private JwtUtil jwtUtil;

    @Autowired
    private UserRepository userRepository;

    // ===============================
    // GET USER LIBRARY
    // ===============================
    @GetMapping("/user/{userId}")
    public ResponseEntity<List<MyLibraryItemResponseDto>> getUserLibrary(
            @PathVariable Integer userId
    ) {
        return ResponseEntity.ok(
                myLibraryService.getUserLibrary(userId)
        );
    }

    // ===============================
    // READ BOOK (PDF)
    // ===============================
    @GetMapping("/read/{productId}")
    public ResponseEntity<byte[]> readBook(
            @PathVariable Integer productId,
            @RequestHeader("Authorization") String token
    ) {
        Integer userId = extractUserId(token);

        byte[] pdf =
                myLibraryService.readLibraryBook(userId, productId);

        return ResponseEntity.ok()
                .header("Content-Type", "application/pdf")
                .body(pdf);
    }

    // ===============================
    // JWT → USER ID RESOLUTION
    // ===============================
    private Integer extractUserId(String authHeader) {

        if (authHeader == null || !authHeader.startsWith("Bearer ")) {
            throw new RuntimeException("Missing or invalid Authorization header");
        }

        String token = authHeader.substring(7);

        // JWT stores EMAIL as subject
        String email = jwtUtil.extractUsername(token);

        User user = userRepository.findByUserEmail(email)
                .orElseThrow(() -> new RuntimeException("User not found"));

        return user.getUserId();
    }
}
