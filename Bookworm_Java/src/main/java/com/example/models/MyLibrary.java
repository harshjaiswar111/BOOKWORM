package com.example.models;

import jakarta.persistence.*;
import java.time.LocalDate;

@Entity
@Table(name = "my_library")
public class MyLibrary {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "my_lib_id")
    private Integer myLibId;

    // ===== Relationships =====

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_id", nullable = false)
    private User user;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "package_id", nullable = false)
    private LibraryPackage libraryPackage;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "product_id")
    private Product product;

    // ===== Dates =====

    @Column(name = "start_date")
    private LocalDate startDate;

    @Column(name = "end_date")
    private LocalDate endDate;

    // ===== Subscription Details =====

    @Column(name = "books_allowed", nullable = false)
    private Integer booksAllowed;

    @Column(name = "books_taken", nullable = false)
    private Integer booksTaken;

   

    // ===== Constructors =====

    public MyLibrary() {
    }

   

    // ===== Getters & Setters =====

    public Integer getMyLibId() {
        return myLibId;
    }

    public void setMyLibId(Integer myLibId) {
        this.myLibId = myLibId;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

    public LibraryPackage getLibraryPackage() {
        return libraryPackage;
    }

    public void setLibraryPackage(LibraryPackage libraryPackage) {
        this.libraryPackage = libraryPackage;
    }

    public Product getProduct() {
        return product;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    public LocalDate getStartDate() {
        return startDate;
    }

    public void setStartDate(LocalDate startDate) {
        this.startDate = startDate;
    }

    public LocalDate getEndDate() {
        return endDate;
    }

    public void setEndDate(LocalDate endDate) {
        this.endDate = endDate;
    }

    public Integer getBooksAllowed() {
        return booksAllowed;
    }

    public void setBooksAllowed(Integer booksAllowed) {
        this.booksAllowed = booksAllowed;
    }

    public Integer getBooksTaken() {
        return booksTaken;
    }

    public void setBooksTaken(Integer booksTaken) {
        this.booksTaken = booksTaken;
    }

}