package com.example.models;

import jakarta.persistence.*;

@Entity
@Table(name = "beneficiary")
public class Beneficiary {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "beneficiary_id")
    private int beneficiaryId;

    @Column(name = "beneficiary_name")
    private String beneficiaryName;

    @Column(name = "beneficiary_email_id")
    private String beneficiaryEmailId;

    @Column(name = "beneficiary_contact_no")
    private String beneficiaryContactNo;

    @Column(name = "beneficiary_bank_name")
    private String beneficiaryBankName;

    @Column(name = "beneficiary_bank_branch")
    private String beneficiaryBankBranch;

    @Column(name = "beneficiary_ifsc")
    private String beneficiaryIfsc;

    @Column(name = "beneficiary_acc_no")
    private String beneficiaryAccNo;

    @Column(name = "beneficiary_acc_type")
    private String beneficiaryAccType;

    @Column(name = "beneficiary_pan")
    private String beneficiaryPan;
    
    @ManyToOne
    @JoinColumn(name = "product_id", nullable = false)
    private Product product;


    // -------- No-Arg Constructor (REQUIRED by JPA) --------



	public Beneficiary() {
    }

    // -------- Getters and Setters --------

    public int getBeneficiaryId() {
        return beneficiaryId;
    }

    public void setBeneficiaryId(int beneficiaryId) {
        this.beneficiaryId = beneficiaryId;
    }

    public String getBeneficiaryName() {
        return beneficiaryName;
    }

    public void setBeneficiaryName(String beneficiaryName) {
        this.beneficiaryName = beneficiaryName;
    }

    public String getBeneficiaryEmailId() {
        return beneficiaryEmailId;
    }

    public void setBeneficiaryEmailId(String beneficiaryEmailId) {
        this.beneficiaryEmailId = beneficiaryEmailId;
    }

    public String getBeneficiaryContactNo() {
        return beneficiaryContactNo;
    }

    public void setBeneficiaryContactNo(String beneficiaryContactNo) {
        this.beneficiaryContactNo = beneficiaryContactNo;
    }

    public String getBeneficiaryBankName() {
        return beneficiaryBankName;
    }

    public void setBeneficiaryBankName(String beneficiaryBankName) {
        this.beneficiaryBankName = beneficiaryBankName;
    }

    public String getBeneficiaryBankBranch() {
        return beneficiaryBankBranch;
    }

    public void setBeneficiaryBankBranch(String beneficiaryBankBranch) {
        this.beneficiaryBankBranch = beneficiaryBankBranch;
    }

    public String getBeneficiaryIfsc() {
        return beneficiaryIfsc;
    }

    public void setBeneficiaryIfsc(String beneficiaryIfsc) {
        this.beneficiaryIfsc = beneficiaryIfsc;
    }

    public String getBeneficiaryAccNo() {
        return beneficiaryAccNo;
    }

    public void setBeneficiaryAccNo(String beneficiaryAccNo) {
        this.beneficiaryAccNo = beneficiaryAccNo;
    }

    public String getBeneficiaryAccType() {
        return beneficiaryAccType;
    }

    public void setBeneficiaryAccType(String beneficiaryAccType) {
        this.beneficiaryAccType = beneficiaryAccType;
    }

    public String getBeneficiaryPan() {
        return beneficiaryPan;
    }

    public void setBeneficiaryPan(String beneficiaryPan) {
        this.beneficiaryPan = beneficiaryPan;
    }
    
    public Product getProductId() {
		return product;
	}

	public void setProductId(Product productId) {
		this.product = productId;
	}
}
