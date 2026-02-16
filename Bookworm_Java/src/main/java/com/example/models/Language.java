package com.example.models;

import jakarta.persistence.*;

@Entity
@Table(name = "language")
public class Language {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "Language_id")
    private int languageId;

    @Column(name = "Language_Desc", length = 50)
    private String languageDesc;

    // -------- Constructors --------

    public Language() {
    }

    

    // -------- Getters and Setters --------

    public int getLanguageId() {
        return languageId;
    }

    public void setLanguageId(int languageId) {
        this.languageId = languageId;
    }

    public String getLanguageDesc() {
        return languageDesc;
    }

    public void setLanguageDesc(String languageDesc) {
        this.languageDesc = languageDesc;
    }
}

