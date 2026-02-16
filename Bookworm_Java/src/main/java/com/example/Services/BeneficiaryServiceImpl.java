package com.example.Services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.example.Repository.BeneficiaryRepository;
import com.example.Services.BeneficiaryService;
import com.example.models.Beneficiary;

@Service
public class BeneficiaryServiceImpl implements BeneficiaryService {

    @Autowired
    private BeneficiaryRepository beneficiaryRepository;

    @Override
    public Beneficiary createBeneficiary(Beneficiary beneficiary) {
        return beneficiaryRepository.save(beneficiary);
    }

    @Override
    public Beneficiary updateBeneficiary(int id, Beneficiary beneficiary) {
        Beneficiary existing = beneficiaryRepository.findById(id).orElse(null);

        if (existing != null) {
            existing.setBeneficiaryName(beneficiary.getBeneficiaryName());
            existing.setBeneficiaryEmailId(beneficiary.getBeneficiaryEmailId());
            existing.setBeneficiaryContactNo(beneficiary.getBeneficiaryContactNo());
            existing.setBeneficiaryBankName(beneficiary.getBeneficiaryBankName());
            existing.setBeneficiaryBankBranch(beneficiary.getBeneficiaryBankBranch());
            existing.setBeneficiaryIfsc(beneficiary.getBeneficiaryIfsc());
            existing.setBeneficiaryAccNo(beneficiary.getBeneficiaryAccNo());
            existing.setBeneficiaryAccType(beneficiary.getBeneficiaryAccType());
            existing.setBeneficiaryPan(beneficiary.getBeneficiaryPan());

            return beneficiaryRepository.save(existing);
        }

        return null; // or throw custom exception
    }

    @Override
    public Beneficiary getBeneficiaryById(int id) {
        return beneficiaryRepository.findById(id).orElse(null);
    }

    @Override
    public List<Beneficiary> getAllBeneficiaries() {
        return beneficiaryRepository.findAll();
    }

    @Override
    public void deleteBeneficiary(int id) {
        beneficiaryRepository.deleteById(id);
    }
}
