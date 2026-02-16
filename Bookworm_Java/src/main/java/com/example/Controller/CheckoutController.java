package com.example.Controller;

import com.example.dto.CheckoutRequest;
import com.example.Services.CheckoutService;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api")
public class CheckoutController {

    private final CheckoutService checkoutService;

    public CheckoutController(CheckoutService checkoutService) {
        this.checkoutService = checkoutService;
    }

    @PostMapping("/checkout")
    public String checkout(@RequestBody CheckoutRequest request) {
        checkoutService.checkout(request);
        return "Checkout successful";
    }
}

