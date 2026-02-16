package com.example.models;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.Lob;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

@Entity
@Table(name = "pdfBook")
public class ReadBook {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "pdf_Id")
	private int pdfId;
	
	@Lob
	@Column(name = "pdf_data", columnDefinition = "LONGBLOB")
	private byte[] pdfData;
	
	@Column(name = "file_name")
	private String fileName;
	
	@ManyToOne
	@JoinColumn(name = "product_id", nullable = false)
	private Product product;

	public int getPdfId() {
		return pdfId;
	}

	public void setPdfId(int pdfId) {
		this.pdfId = pdfId;
	}

	public byte[] getPdfData() {
		return pdfData;
	}

	public void setPdfData(byte[] pdfData) {
		this.pdfData = pdfData;
	}

	public Product getProduct() {
		return product;
	}

	public void setProduct(Product product) {
		this.product = product;
	}

	public String getFileName() {
		return fileName;
	}

	public void setFileName(String fileName) {
		this.fileName = fileName;
	}
	
	
}
