package com.example.Controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import com.example.Services.ProductBeneficiaryService;
import com.example.dto.ProductBeneficiaryDTO;
import com.example.models.ProductBeneficiary;

@RestController
@RequestMapping("/api/product-beneficiaries")
@CrossOrigin(origins = "*")
public class ProductBeneficiaryController {

    @Autowired
    private ProductBeneficiaryService service;

    @GetMapping
    public List<ProductBeneficiaryDTO> getAll() {
        return service.getAllProductBeneficiaries();
    }
}
