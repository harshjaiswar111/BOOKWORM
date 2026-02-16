package com.example.Services;

import com.example.models.MyLibrary;
import com.example.Repository.MyLibraryRepository;
import com.example.Services.MyLibraryService;
import com.example.dto.MyLibraryItemResponseDto;
import com.example.dto.ProductResponseDto;

import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.util.List;
//
//@Service
//public class MyLibraryServiceImpl implements MyLibraryService {
//
//    private final MyLibraryRepository myLibraryRepository;
//
//    public MyLibraryServiceImpl(MyLibraryRepository myLibraryRepository) {
//        this.myLibraryRepository = myLibraryRepository;
//    }
//
//    @Override
//    public MyLibrary subscribeUser(MyLibrary myLibrary) {
//
//        LocalDate today = LocalDate.now();
//
//        // Prevent multiple active subscriptions
//        boolean hasActive = myLibraryRepository
//                .existsByUser_UserIdAndEndDateAfter(
//                        myLibrary.getUser().getUserId(), today);
//
//        if (hasActive) {
//            throw new RuntimeException("User already has an active subscription");
//        }
//
//        // Auto-set start date if missing
//        if (myLibrary.getStartDate() == null) {
//            myLibrary.setStartDate(today);
//        }
//
//        return myLibraryRepository.save(myLibrary);
//    }
//
//    @Override
//    public MyLibrary getActiveSubscription(Integer userId) {
//
//        LocalDate today = LocalDate.now();
//
//        return myLibraryRepository
//                .findByUser_UserIdAndEndDateAfter(userId, today)
//                .orElseThrow(() -> new RuntimeException("No active subscription found"));
//    }
//
//    @Override
//    public List<MyLibrary> getUserLibrary(Integer userId) {
//        return myLibraryRepository.findByUser_UserId(userId);
//    }
//
//    @Override
//    public void incrementBooksTaken(Integer myLibId) {
//
//        MyLibrary myLibrary = myLibraryRepository.findById(myLibId)
//                .orElseThrow(() -> new RuntimeException("Library record not found"));
//
//        if (myLibrary.getBooksTaken() >= myLibrary.getBooksAllowed()) {
//            throw new RuntimeException("Book limit reached");
//        }
//
//        myLibrary.setBooksTaken(myLibrary.getBooksTaken() + 1);
//        myLibraryRepository.save(myLibrary);
//    }
//
//    @Override
//    public void decrementBooksTaken(Integer myLibId) {
//
//        MyLibrary myLibrary = myLibraryRepository.findById(myLibId)
//                .orElseThrow(() -> new RuntimeException("Library record not found"));
//
//        if (myLibrary.getBooksTaken() > 0) {
//            myLibrary.setBooksTaken(myLibrary.getBooksTaken() - 1);
//            myLibraryRepository.save(myLibrary);
//        }
//    }
//}





import org.springframework.beans.factory.annotation.Autowired;

import com.example.models.Product;

@Service
public class MyLibraryServiceImpl implements MyLibraryService {

    @Autowired
    private MyLibraryRepository myLibraryRepository;

    @Autowired
    private ReadBookService readBookService;

    @Override
    public List<MyLibraryItemResponseDto> getUserLibrary(Integer userId) {

        LocalDate today = LocalDate.now();

        return myLibraryRepository
                .findByUser_UserIdAndEndDateAfter(userId, today)
                .stream()
                .map(this::toDto)
                .toList();
    }

    @Override
    public byte[] readLibraryBook(Integer userId, Integer productId) {

        LocalDate today = LocalDate.now();

        boolean allowed =
                myLibraryRepository
                        .existsByUser_UserIdAndProduct_ProductIdAndEndDateAfter(
                                userId,
                                productId,
                                today
                        );

        if (!allowed) {
            throw new RuntimeException("You do not have access to this book");
        }

        return readBookService.readBook(productId);
    }

    // ===============================
    // ENTITY → DTO MAPPING
    // ===============================
    private MyLibraryItemResponseDto toDto(MyLibrary ml) {

        Product product = ml.getProduct();

        ProductResponseDto productDto = new ProductResponseDto();
        productDto.setProductId(product.getProductId());
        productDto.setProductName(product.getProductName());
        productDto.setProductImage(product.getProductImage());

        if (product.getAuthor() != null) {
            productDto.setAuthorName(product.getAuthor().getName());
        }

        MyLibraryItemResponseDto dto = new MyLibraryItemResponseDto();
        dto.setPackageId(ml.getLibraryPackage().getPackageId());
        dto.setStartDate(ml.getStartDate());
        dto.setEndDate(ml.getEndDate());
        dto.setProduct(productDto);

        return dto;
    }
}

