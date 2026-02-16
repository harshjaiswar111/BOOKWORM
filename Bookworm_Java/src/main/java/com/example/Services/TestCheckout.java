//package com.example.Services;
//
//import static org.junit.Assert.assertEquals;
//import static org.mockito.Mockito.atLeastOnce;
//import static org.mockito.Mockito.verify;
//import static org.mockito.Mockito.when;
//import static org.mockito.ArgumentMatchers.any;
//
//
//import java.math.BigDecimal;
//import java.util.List;
//import java.util.Optional;
//
//import org.junit.jupiter.api.BeforeEach;
//import org.junit.jupiter.api.Test;
//import org.mockito.InjectMocks;
//import org.mockito.Mock;
//import org.mockito.MockitoAnnotations;
//import org.mockito.junit.*;
//import org.mockito.MockitoAnnotations;
//import static org.junit.jupiter.api.Assertions.*;
//
//import org.junit.jupiter.api.extension.ExtendWith;
//
//import com.example.Repository.CartRepository;
//import com.example.Repository.MyShelfRepository;
//import com.example.Repository.TransactionItemRepository;
//import com.example.Repository.TransactionRepository;
//import com.example.Repository.UserRepository;
//import com.example.dto.CheckoutRequest;
//import com.example.models.Cart;
//import com.example.models.Product;
//import com.example.models.Transaction;
//import com.example.models.TransactionItem;
//import com.example.models.TransactionStatus;
//import com.example.models.User;
//
//import org.mockito.junit.jupiter.MockitoExtension;
//
//@ExtendWith(MockitoExtension.class)
//class TestCheckout {
//
//    @InjectMocks
//    private CheckoutService checkoutService;
//
//    @Mock
//    private TransactionRepository transactionRepo;
//    
//    @Mock
//    private UserRepository userRepo;
//
//    @Mock
//    private TransactionItemRepository itemRepo;
//    
//    @Mock
//    private CartRepository cartRepo;
//
//    @Mock
//    private MyShelfRepository shelfRepo;
//
//    @Mock
//    private TransactionPdfService transactionPdfService;
//
//    @Mock
//    private EmailService emailService;
//    
//    
//    @Test
//    void shouldCheckoutSuccessfully() {
//
//        // 1️⃣ Dummy user
//        User user = new User();
//        user.setUserId(1);
//        user.setUserEmail("test@gmail.com");
//        user.setUserName("Test User");
//
//        // 2️⃣ Dummy product
//        Product product = new Product();
//        product.setProductId(1);
//        product.setProductBaseprice(BigDecimal.valueOf(500));
//
//        // 3️⃣ Cart item
//        Cart cart = new Cart();
//        cart.setUser(user);
//        cart.setProduct(product);
//        cart.setQty(1);
//
//        // 4️⃣ Checkout request
//        CheckoutRequest request = new CheckoutRequest();
//        request.setUserId(1);
//        request.setProductIds(List.of(1));
//
//        // 5️⃣ Mock repository calls
//        when(userRepo.findById(1))
//                .thenReturn(Optional.of(user));
//
//        when(cartRepo.findByUser(user))
//                .thenReturn(List.of(cart));
//
//        when(transactionRepo.save(any(Transaction.class)))
//                .thenAnswer(inv -> inv.getArgument(0));
//
//        // 6️⃣ Call method under test
//        checkoutService.checkout(request);
//
//        // 7️⃣ Verify transaction saved
//        verify(transactionRepo, atLeastOnce())
//        .save(any(Transaction.class));
//
//    }
//}
