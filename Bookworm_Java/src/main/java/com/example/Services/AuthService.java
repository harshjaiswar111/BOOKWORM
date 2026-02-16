package com.example.Services;

import com.example.models.User;
import com.example.Repository.UserRepository;
import com.example.dto.AuthResponse;
import com.example.dto.LoginRequest;
import com.example.dto.RegisterRequest;
import com.example.security.JwtUtil;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.LocalDate;

@Service
public class AuthService {

    private final UserRepository userRepo;
    private final BCryptPasswordEncoder encoder;
    private final JwtUtil jwtUtil;

    public AuthService(UserRepository userRepo,
                       BCryptPasswordEncoder encoder,
                       JwtUtil jwtUtil) {
        this.userRepo = userRepo;
        this.encoder = encoder;
        this.jwtUtil = jwtUtil;
    }

    public void register(RegisterRequest request) {

        User user = new User();
        user.setUserName(request.getName());
        user.setUserEmail(request.getEmail());
        user.setUserPhone(request.getPhone());
        user.setUserAddress(request.getAddress());
        user.setUserPassword(encoder.encode(request.getPassword()));
        user.setJoinDate(LocalDate.now());
        user.setAdmin(request.isAdmin());

        userRepo.save(user);
    }
    
    
    // ✅ UPDATED: return token + userId
    public AuthResponse login(LoginRequest request) {

        User user = userRepo.findByUserEmail(request.getEmail())
                .orElseThrow(() -> new RuntimeException("User not found"));

        if (!encoder.matches(request.getPassword(), user.getUserPassword())) {
            throw new RuntimeException("Invalid credentials");
        }

        String token = jwtUtil.generateToken(user.getUserEmail(), user.isAdmin());

        // ⚠️ If your User entity has getId() then replace getUserId() with getId()
        return new AuthResponse(token, user.getUserId());
    }

//    public String login(LoginRequest request) {
//
//        User user = userRepo.findByUserEmail(request.getEmail())
//                .orElseThrow(() -> new RuntimeException("User not found"));
//
//        if (!encoder.matches(request.getPassword(), user.getUserPassword())) {
//            throw new RuntimeException("Invalid credentials");
//        }
//
//        return jwtUtil.generateToken(user.getUserEmail(), user.isAdmin());
//    }
}
