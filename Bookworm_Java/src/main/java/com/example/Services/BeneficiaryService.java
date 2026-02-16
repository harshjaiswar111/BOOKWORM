package com.example.Services;

import java.util.List;

import com.example.models.Beneficiary;

public interface BeneficiaryService {

    Beneficiary createBeneficiary(Beneficiary beneficiary);

    Beneficiary updateBeneficiary(int id, Beneficiary beneficiary);

    Beneficiary getBeneficiaryById(int id);

    List<Beneficiary> getAllBeneficiaries();

    void deleteBeneficiary(int id);
}
