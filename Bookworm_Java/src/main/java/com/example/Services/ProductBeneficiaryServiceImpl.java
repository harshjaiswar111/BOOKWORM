package com.example.Services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.example.Repository.ProductBeneficiaryRepository;
import com.example.dto.ProductBeneficiaryDTO;
import com.example.models.ProductBeneficiary;

@Service
public class ProductBeneficiaryServiceImpl 
        implements ProductBeneficiaryService {

    @Autowired
    private ProductBeneficiaryRepository repository;

    @Override
    public List<ProductBeneficiaryDTO> getAllProductBeneficiaries() {
        return repository.findAll().stream().map(pb -> {
            ProductBeneficiaryDTO dto = new ProductBeneficiaryDTO();
            dto.setProdbenId(pb.getProdbenId());
            dto.setBeneficiaryName(pb.getBeneficiary().getBeneficiaryName());
            dto.setProductName(pb.getProduct().getProductName());
            dto.setRoyaltyReceived(pb.getRoyaltyReceived());
            return dto;
        }).toList();
    }
}
