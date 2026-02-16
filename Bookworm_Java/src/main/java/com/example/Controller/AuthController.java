package com.example.Controller;

import com.example.Services.AuthService;
import com.example.dto.AuthResponse;
import com.example.dto.LoginRequest;
import com.example.dto.RegisterRequest;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/auth")
@CrossOrigin(origins = "http://localhost:5173")
public class AuthController {

    private final AuthService service;

    public AuthController(AuthService service) {
        this.service = service;
    }

    @PostMapping("/register")
    public String register(@RequestBody RegisterRequest request) {
        service.register(request);
        return "User Registered Successfully";
    }
    
    
    @PostMapping("/login")
    public AuthResponse login(@RequestBody LoginRequest request) {
        return service.login(request);
    }

//    @PostMapping("/login")
//    public AuthResponse login(@RequestBody LoginRequest request) {
//        return new AuthResponse(service.login(request));
//    }
}
