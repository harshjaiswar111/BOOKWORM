package com.example.Controller;

import com.example.models.LibraryPackage;
import com.example.Services.LibraryPackageService;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/library-packages")
@CrossOrigin(origins = "http://localhost:5173")
public class LibraryPackageController {

    private final LibraryPackageService libraryPackageService;

    public LibraryPackageController(LibraryPackageService libraryPackageService) {
        this.libraryPackageService = libraryPackageService;
    }

    // CREATE package
    @PostMapping
    public ResponseEntity<LibraryPackage> createPackage(
            @RequestBody LibraryPackage libraryPackage) {

        LibraryPackage saved = libraryPackageService.createPackage(libraryPackage);
        return new ResponseEntity<>(saved, HttpStatus.CREATED);
    }

    // GET all packages
    @GetMapping
    public ResponseEntity<List<LibraryPackage>> getAllPackages() {
        return ResponseEntity.ok(libraryPackageService.getAllPackages());
    }

    // GET package by ID
    @GetMapping("/{id}")
    public ResponseEntity<LibraryPackage> getPackageById(@PathVariable Integer id) {
        return ResponseEntity.ok(libraryPackageService.getPackageById(id));
    }

    // GET package by name
    @GetMapping("/name/{name}")
    public ResponseEntity<LibraryPackage> getPackageByName(@PathVariable String name) {
        return ResponseEntity.ok(libraryPackageService.getPackageByName(name));
    }

    // DELETE package
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deletePackage(@PathVariable Integer id) {
        libraryPackageService.deletePackage(id);
        return ResponseEntity.noContent().build();
    }
}