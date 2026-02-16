package com.example.Controller;

import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/admin")
public class AdminController {

    @GetMapping("/dashboard")
    public String adminOnly() {
        return "ADMIN ACCESS GRANTED";
    }
}
