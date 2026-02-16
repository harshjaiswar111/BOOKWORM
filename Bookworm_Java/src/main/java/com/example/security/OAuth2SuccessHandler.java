package com.example.security;

import com.example.models.User;
import com.example.Repository.UserRepository;
import jakarta.servlet.http.*;
import org.springframework.security.core.Authentication;
import org.springframework.security.oauth2.core.user.OAuth2User;
import org.springframework.security.web.authentication.AuthenticationSuccessHandler;
import org.springframework.stereotype.Component;

import java.io.IOException;

@Component
public class OAuth2SuccessHandler implements AuthenticationSuccessHandler {

    private final JwtUtil jwtUtil;
    private final UserRepository userRepository;

    public OAuth2SuccessHandler(JwtUtil jwtUtil, UserRepository userRepository) {
        this.jwtUtil = jwtUtil;
        this.userRepository = userRepository;
    }

    @Override
    public void onAuthenticationSuccess(
            HttpServletRequest request,
            HttpServletResponse response,
            Authentication authentication
    ) throws IOException {

        OAuth2User oAuth2User = (OAuth2User) authentication.getPrincipal();

        String email = oAuth2User.getAttribute("email");
        String name  = oAuth2User.getAttribute("name");

        User user = userRepository.findByUserEmail(email)
                .orElseGet(() -> {
                    User u = new User();
                    u.setUserEmail(email);
                    u.setUserName(name);
                    u.setUserPassword("GOOGLE_LOGIN");
                    u.setAdmin(false);
                    return userRepository.save(u);
                });

        String token = jwtUtil.generateToken(user.getUserEmail(), user.isAdmin());

        response.sendRedirect(
        	    "http://localhost:5173/login-success"
        	    + "?token=" + token
        	    + "&userId=" + user.getUserId()
        	);
    }
}
