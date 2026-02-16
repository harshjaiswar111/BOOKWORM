package com.example.models;

import jakarta.persistence.*;
import java.math.BigDecimal;
import java.time.LocalDate;

@Entity
@Table(name = "product")
public class Product {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "product_id")
    private int productId;

    @Column(name = "product_name", length = 150, nullable = false)
    private String productName;

    // -------- Relationships (Foreign Keys) --------

    @ManyToOne
    @JoinColumn(name = "product_type")
    private Product_Type productType;

    @ManyToOne
    @JoinColumn(name = "product_author")
    private Author author;

    @ManyToOne
    @JoinColumn(name = "product_publisher")
    private Publisher publisher;

    @ManyToOne
    @JoinColumn(name = "product_lang")
    private Language language;

    @ManyToOne
    @JoinColumn(name = "product_genere")
    private Genere genere;

    @ManyToOne
    @JoinColumn(name = "Attribute_Id")
    private Attribute attribute;

    // -------- Pricing --------

    @Column(name = "product_baseprice", nullable = false)
    private BigDecimal productBaseprice;

    @Column(name = "product_offerprice")
    private BigDecimal productOfferprice;

    @Column(name = "discount_percent", precision = 5, scale = 2)
    private BigDecimal discountPercent;

    @Column(name = "royalty_percent")
    private BigDecimal royaltyPercent;

    // -------- Dates --------

    @Column(name = "product_off_price_expirydate")
    private LocalDate productOffPriceExpirydate;

    // -------- Descriptions --------

    @Column(name = "product_description_short")
    private String productDescriptionShort;

    @Column(name = "product_description_long", columnDefinition = "TEXT")
    private String productDescriptionLong;

    // -------- Other Details --------

    @Column(name = "product_isbn", nullable = false)
    private String productIsbn;

    @Column(name = "is_rentable")
    private Boolean rentable;

    @Column(name = "is_library")
    private Boolean library;

    @Column(name = "rent_per_day", precision = 5, scale = 2)
    private BigDecimal rentPerDay;

    @Column(name = "min_rent_days")
    private int minRentDays;

    @Column(name = "product_Image")
    private String productImage;

    // -------- Constructors --------

    public Product() {
    }

    // -------- Getters and Setters --------

    public int getProductId() {
        return productId;
    }

    public void setProductId(int productId) {
        this.productId = productId;
    }

    public String getProductName() {
        return productName;
    }

    public void setProductName(String productName) {
        this.productName = productName;
    }

    public Product_Type getProductType() {
        return productType;
    }

    public void setProductType(Product_Type productType) {
        this.productType = productType;
    }

    public Author getAuthor() {
        return author;
    }

    public void setAuthor(Author author) {
        this.author = author;
    }

    public Publisher getPublisher() {
        return publisher;
    }

    public void setPublisher(Publisher publisher) {
        this.publisher = publisher;
    }

    public Language getLanguage() {
        return language;
    }

    public void setLanguage(Language language) {
        this.language = language;
    }

    public Genere getGenere() {
        return genere;
    }

    public void setGenere(Genere genere) {
        this.genere = genere;
    }

    public Attribute getAttribute() {
        return attribute;
    }

    public void setAttribute(Attribute attribute) {
        this.attribute = attribute;
    }

    public BigDecimal getProductBaseprice() {
        return productBaseprice;
    }

    public void setProductBaseprice(BigDecimal productBaseprice) {
        this.productBaseprice = productBaseprice;
    }

    public BigDecimal getProductOfferprice() {
        return productOfferprice;
    }

    public void setProductOfferprice(BigDecimal productOfferprice) {
        this.productOfferprice = productOfferprice;
    }

    public BigDecimal getDiscountPercent() {
        return discountPercent;
    }

    public void setDiscountPercent(BigDecimal discountPercent) {
        this.discountPercent = discountPercent;
    }

    public BigDecimal getRoyaltyPercent() {
        return royaltyPercent;
    }

    public void setRoyaltyPercent(BigDecimal royaltyPercent) {
        this.royaltyPercent = royaltyPercent;
    }

    public LocalDate getProductOffPriceExpirydate() {
        return productOffPriceExpirydate;
    }

    public void setProductOffPriceExpirydate(LocalDate productOffPriceExpirydate) {
        this.productOffPriceExpirydate = productOffPriceExpirydate;
    }

    public String getProductDescriptionShort() {
        return productDescriptionShort;
    }

    public void setProductDescriptionShort(String productDescriptionShort) {
        this.productDescriptionShort = productDescriptionShort;
    }

    public String getProductDescriptionLong() {
        return productDescriptionLong;
    }

    public void setProductDescriptionLong(String productDescriptionLong) {
        this.productDescriptionLong = productDescriptionLong;
    }

    public String getProductIsbn() {
        return productIsbn;
    }

    public void setProductIsbn(String productIsbn) {
        this.productIsbn = productIsbn;
    }

    public Boolean isRentable() {
        return rentable;
    }

    public void setRentable(boolean rentable) {
        this.rentable = rentable;
    }

    public Boolean isLibrary() {
        return library;
    }

    public void setLibrary(boolean library) {
        this.library = library;
    }

    public BigDecimal getRentPerDay() {
        return rentPerDay;
    }

    public void setRentPerDay(BigDecimal rentPerDay) {
        this.rentPerDay = rentPerDay;
    }

    public int getMinRentDays() {
        return minRentDays;
    }

    public void setMinRentDays(int minRentDays) {
        this.minRentDays = minRentDays;
    }

    public String getProductImage() {
        return productImage;
    }

    public void setProductImage(String productImage) {
        this.productImage = productImage;
    }
}
