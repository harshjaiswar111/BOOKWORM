package com.example.Controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import com.example.Services.BeneficiaryService;
import com.example.models.Beneficiary;

@RestController
@RequestMapping("/api/beneficiaries")
@CrossOrigin(origins = "*")
public class BeneficiaryController {

    @Autowired
    private BeneficiaryService beneficiaryService;

    @PostMapping
    public Beneficiary createBeneficiary(@RequestBody Beneficiary beneficiary) {
        return beneficiaryService.createBeneficiary(beneficiary);
    }

    @PutMapping("/{id}")
    public Beneficiary updateBeneficiary(@PathVariable int id,
                                         @RequestBody Beneficiary beneficiary) {
        return beneficiaryService.updateBeneficiary(id, beneficiary);
    }

    @GetMapping("/{id}")
    public Beneficiary getBeneficiaryById(@PathVariable int id) {
        return beneficiaryService.getBeneficiaryById(id);
    }

    @GetMapping
    public List<Beneficiary> getAllBeneficiaries() {
        return beneficiaryService.getAllBeneficiaries();
    }

    @DeleteMapping("/{id}")
    public String deleteBeneficiary(@PathVariable int id) {
        beneficiaryService.deleteBeneficiary(id);
        return "Beneficiary deleted successfully with id: " + id;
    }
}
