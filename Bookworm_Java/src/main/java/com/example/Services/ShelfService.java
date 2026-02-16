package com.example.Services;

import com.example.Repository.MyShelfRepository;
import com.example.models.Product;
import com.example.models.MyShelf;
import com.example.models.User;
import com.example.Repository.ProductRepository;
import com.example.Repository.MyShelfRepository;
import com.example.Repository.UserRepository;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;

@Service
public class ShelfService {

    private final MyShelfRepository MyShelfRepository;
    private final UserRepository userRepository;
    private final ProductRepository productRepository;

    public ShelfService(MyShelfRepository shelfRepository,
                        UserRepository userRepository,
                        ProductRepository productRepository) {
        this.MyShelfRepository = shelfRepository;
        this.userRepository = userRepository;
        this.productRepository = productRepository;
    }
    
    
 // ✅ READ
    public List<MyShelf> getShelfByUser(Integer userId) {
        return MyShelfRepository.findByUser_UserId(userId);
    }

    // ✅ CREATE (purchase / rent add to shelf)
    public MyShelf addToShelf(Integer userId, Integer productId, Integer rentDays) {

        if (MyShelfRepository.existsByUser_UserIdAndProduct_ProductId(userId, productId)) {
            throw new RuntimeException("This product is already in shelf!");
        }

        User user = userRepository.findById(userId)
                .orElseThrow(() -> new RuntimeException("User not found"));

        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new RuntimeException("Product not found"));

        
        
        
        
        MyShelf shelf = new MyShelf();
        shelf.setUser(user);
        shelf.setProduct(product);

        // rentDays => expiry date time set
        if (rentDays != null && rentDays > 0) {
            shelf.setProductExpiryDate(LocalDateTime.now().plusDays(rentDays));
        } else {
            shelf.setProductExpiryDate(null); // purchase case
        }

        return MyShelfRepository.save(shelf);
    }

    // ✅ DELETE (remove from shelf)
    public void deleteShelfItem(Integer shelfId) {
        MyShelfRepository.deleteById(shelfId);
    }

    // ✅ delete all expired rent items
    public void deleteExpiredItems() {
        MyShelfRepository.deleteByProductExpiryDateBefore(LocalDateTime.now());
    }
    
    
    

//    // ✅ ADD TO SHELF AFTER TRANSACTION SUCCESS
//    public MyShelf addToShelfAfterTransaction(Integer userId, Integer productId, Integer rentDays) {
//
//        // duplicate check
//        if (shelfRepository.existsByUser_UserIdAndProduct_ProductId(userId, productId)) {
//            throw new RuntimeException("Already exists in shelf");
//        }
//
//        User user = userRepository.findById(userId)
//                .orElseThrow(() -> new RuntimeException("User not found"));
//
//        Product product = productRepository.findById(productId)
//                .orElseThrow(() -> new RuntimeException("Product not found"));
//
//        MyShelf shelf = new MyShelf();
//        shelf.setUser(user);
//        shelf.setProduct(product);
//
//        // rentDays => expiry date time set
//       
//
//        return shelfRepository.save(shelf);
//    }
}
