package com.example.dto;

public class AuthResponse {

    private String token;
    private Integer userId;

    public AuthResponse(String token, Integer userId) {
        this.token = token;
        this.userId = userId;
    }

    public String getToken() {
        return token;
    }

    public Integer getUserId() {
        return userId;
    }
}


//package com.example.dto;
//
//public class AuthResponse {
//
//  private String token;
//
//  public AuthResponse(String token) {
//      this.token = token;
//  }
//
//  public String getToken() {
//      return token;
//  }
//}
