package com.example.dto;

public class RegisterRequest {

    private String name;
    private String email;
    private String phone;
    private String address;
    private String password;
    private boolean admin;

    public String getName() { return name; }
    public String getEmail() { return email; }
    public String getPhone() { return phone; }
    public String getAddress() { return address; }
    public String getPassword() { return password; }
    public boolean isAdmin() { return admin; }
}
