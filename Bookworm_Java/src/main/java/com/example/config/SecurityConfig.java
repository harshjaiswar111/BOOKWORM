package com.example.config;
import jakarta.servlet.http.HttpServletResponse;
import com.example.security.JwtAuthFilter;
import com.example.security.OAuth2SuccessHandler;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.Customizer;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import org.springframework.web.cors.CorsConfiguration;
import org.springframework.web.cors.CorsConfigurationSource;
import org.springframework.web.cors.UrlBasedCorsConfigurationSource;

import java.util.List;

@Configuration
public class SecurityConfig {
	static {
        System.out.println(">>> SecurityConfig LOADED <<<");
    }

    private final JwtAuthFilter jwtAuthFilter;
    private final OAuth2SuccessHandler oAuth2SuccessHandler;

    public SecurityConfig(JwtAuthFilter jwtAuthFilter,
                          OAuth2SuccessHandler oAuth2SuccessHandler) {
        this.jwtAuthFilter = jwtAuthFilter;
        this.oAuth2SuccessHandler = oAuth2SuccessHandler;
    }

    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {

        http
            // ✅ ENABLE CORS (Spring Security 6)
            .cors(Customizer.withDefaults())

            // ✅ Stateless API
            .csrf(csrf -> csrf.disable())
            .sessionManagement(session ->
                session.sessionCreationPolicy(SessionCreationPolicy.STATELESS)
            )

            // ✅ Authorization rules
            .authorizeHttpRequests(auth -> auth
                .requestMatchers("/api/auth/**").permitAll()
                .requestMatchers("/oauth2/**", "/login/**").permitAll()
                .requestMatchers("/api/products/**").permitAll()
                .requestMatchers("/api/generes/**").permitAll()
                .requestMatchers("/api/languages/**").permitAll()
                .requestMatchers("/api/cart/**").hasAnyRole("USER", "ADMIN")
                .requestMatchers("/api/admin/**").hasRole("ADMIN")
                .requestMatchers("/api/checkout/**").hasAnyRole("USER", "ADMIN")
                .requestMatchers("/api/orders/history/**").permitAll()
                .requestMatchers("/api/product-beneficiaries/**").permitAll()
                .requestMatchers("/actuator/health").permitAll()
                .requestMatchers("/actuator/**").permitAll()
                .requestMatchers("/api/library/checkout/**").hasAnyRole("USER", "ADMIN")
                .requestMatchers("/api/library/**").permitAll()               
                .requestMatchers("/api/library-packages/**").permitAll()
                .requestMatchers("/api/my-library/**").permitAll()
                .anyRequest().authenticated()
            )

            // ✅ OAuth2
            .oauth2Login(oauth ->
                oauth.successHandler(oAuth2SuccessHandler)
            )

            // ✅ JWT filter
            .addFilterBefore(jwtAuthFilter, UsernamePasswordAuthenticationFilter.class)

            // ✅ Unauthorized handling
            .exceptionHandling(ex -> ex
                .authenticationEntryPoint(
                    (request, response, authException) ->
                        response.sendError(HttpServletResponse.SC_UNAUTHORIZED, "Unauthorized")
                )
            );

        return http.build();
    }

    // 🔥 THIS IS THE MISSING PIECE (MOST IMPORTANT)
    @Bean
    public CorsConfigurationSource corsConfigurationSource() {

        CorsConfiguration config = new CorsConfiguration();
        config.setAllowedOrigins(List.of("http://localhost:5173"));
        config.setAllowedMethods(List.of("GET", "POST", "PUT", "DELETE", "OPTIONS"));
        config.setAllowedHeaders(List.of("*"));
        config.setAllowCredentials(true);

        UrlBasedCorsConfigurationSource source =
                new UrlBasedCorsConfigurationSource();
        source.registerCorsConfiguration("/**", config);

        return source;
    }
}
