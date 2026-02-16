package com.example.Services;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.stereotype.Service;

import com.example.dto.CheckoutRequest;
import com.example.models.Beneficiary;
import com.example.models.Cart;
import com.example.models.MyShelf;
import com.example.models.Product;
import com.example.models.ProductBeneficiary;
import com.example.models.RoyaltyCalculation;
import com.example.models.Transaction;
import com.example.models.TransactionItem;
import com.example.models.TransactionStatus;
import com.example.models.TransactionType;
import com.example.models.User;
import com.example.Repository.BeneficiaryRepository;
import com.example.Repository.CartRepository;
import com.example.Repository.MyShelfRepository;
import com.example.Repository.ProductBeneficiaryRepository;
import com.example.Repository.RoyaltyCalculationRespository;
import com.example.Repository.TransactionItemRepository;
import com.example.Repository.TransactionRepository;
import com.example.Repository.UserRepository;

import jakarta.transaction.Transactional;

@Service
public class CheckoutService {

	private final UserRepository userRepo;
	private final CartRepository cartRepo;
	private final TransactionRepository transactionRepo;
	private final TransactionItemRepository itemRepo;
	private final MyShelfRepository shelfRepo;
	private final ProductBeneficiaryRepository productBeneficiaryRepo;
	private final BeneficiaryRepository beneficiaryRepository;
	private final RoyaltyCalculationRespository royaltyCalculationRespository;
	private final TransactionPdfService transactionPdfService;
	private final EmailService emailService;
	
	public CheckoutService(UserRepository userRepo, CartRepository cartRepo, TransactionRepository transactionRepo,
			TransactionItemRepository itemRepo, MyShelfRepository shelfRepo,
			ProductBeneficiaryRepository productBeneficiaryRepo, BeneficiaryRepository beneficiaryRepository,
			RoyaltyCalculationRespository royaltyCalculationRespository,
			TransactionPdfService transactionPdfService,
			EmailService emailService) {
		this.userRepo = userRepo;
		this.cartRepo = cartRepo;
		this.transactionRepo = transactionRepo;
		this.itemRepo = itemRepo;
		this.shelfRepo = shelfRepo;
		this.productBeneficiaryRepo = productBeneficiaryRepo;
		this.beneficiaryRepository = beneficiaryRepository;
		this.royaltyCalculationRespository = royaltyCalculationRespository;
		this.transactionPdfService = transactionPdfService;
		this.emailService = emailService;
	}
	
	// ================= PRICE CALCULATION =================

    private BigDecimal calculateItemTotal(
            Product product,
            TransactionType transactionType,
            int quantity,
            Integer rentDays
    ) {

        // ---------- RENT ----------
        if (transactionType == TransactionType.RENT) {

            if (!product.isRentable()) {
                throw new RuntimeException("Product is not rentable");
            }

            if (rentDays == null || rentDays < product.getMinRentDays()) {
                throw new RuntimeException(
                        "Minimum rent days is " + product.getMinRentDays());
            }

            if (product.getRentPerDay() == null) {
                throw new RuntimeException("Rent price per day not available");
            }

            return product.getRentPerDay()
                    .multiply(BigDecimal.valueOf(rentDays))
                    .multiply(BigDecimal.valueOf(quantity));
        }

        // ---------- BUY ----------
        BigDecimal unitPrice;

        // Offer price
        if (product.getProductOfferprice() != null &&
            product.getProductOffPriceExpirydate() != null &&
            product.getProductOffPriceExpirydate().isAfter(LocalDate.now())) {

            unitPrice = product.getProductOfferprice();
        }
        // Discount percent
        else if (product.getDiscountPercent() != null &&
                 product.getDiscountPercent().compareTo(BigDecimal.ZERO) > 0) {

            BigDecimal discount =
                    product.getProductBaseprice()
                           .multiply(product.getDiscountPercent())
                           .divide(BigDecimal.valueOf(100), 2, RoundingMode.HALF_UP);

            unitPrice = product.getProductBaseprice().subtract(discount);
        }
        // Base price
        else {
            unitPrice = product.getProductBaseprice();
        }

        return unitPrice.multiply(BigDecimal.valueOf(quantity));
    }


	

	@Transactional
    public void checkout(CheckoutRequest request) {
		System.out.println("rentDays from request = " + request.getRentDays());
		
        // 1️⃣ Fetch user
        User user = userRepo.findById(request.getUserId())
                .orElseThrow(() -> new RuntimeException("User not found"));

        // 2️⃣ Fetch cart items (SOURCE OF TRUTH)
        //List<Cart> cartItems = cartRepo.findByUser_UserId(user.getUserId());
        List<Cart> cartItems = cartRepo.findByUser(user);

        if (cartItems.isEmpty()) {
            throw new RuntimeException("Cart is empty");
        }
        
     // 3️⃣ Decide transaction type
        TransactionType transactionType =
                request.getRentDays() != null
                        ? TransactionType.RENT
                        : TransactionType.BUY;

        // 3️⃣ Create transaction (PENDING)
        Transaction transaction = new Transaction();
        transaction.setUser(user);
        transaction.setTransactionType(transactionType);
        transaction.setStatus(TransactionStatus.PENDING);
        transaction = transactionRepo.save(transaction);

        BigDecimal total = BigDecimal.ZERO;


        // 4️⃣ Create transaction items from cart
        for (Cart cart : cartItems) {

            Product product = cart.getProduct(); // MUST NOT be null
            int qty = cart.getQty();
            
            BigDecimal itemTotal = calculateItemTotal(
                    product,
                    transactionType,
                    qty,
                    request.getRentDays()
            );

            // Store UNIT PRICE in transaction item
            BigDecimal unitPrice;
            if (transactionType == TransactionType.RENT) {
                unitPrice = product.getRentPerDay();
            } else {
                unitPrice = itemTotal.divide(
                        BigDecimal.valueOf(qty),
                        2,
                        RoundingMode.HALF_UP
                );
            }

            
            
//            BigDecimal sellingPrice =
//                    product.getProductOfferprice() != null
//                    ? product.getProductOfferprice()
//                    : product.getProductBaseprice();

            TransactionItem item = new TransactionItem();
            item.setTransaction(transaction);
            item.setProduct(product);
            item.setPrice(unitPrice);
            item.setQuantity(cart.getQty());

            itemRepo.save(item);

//            BigDecimal price = sellingPrice;
//            BigDecimal itemTotal =
//                    price.multiply(BigDecimal.valueOf(cart.getQty()));

            total = total.add(itemTotal);

            
            
            // ================= 🔥 ROYALTY LOGIC START =================

            // 1️⃣ Fetch beneficiaries for this product
            if(product.getRoyaltyPercent() != null) {
            	
            	List<Beneficiary> beneficiaries =
            			beneficiaryRepository.findByProduct_ProductId(product.getProductId());
            
            
            	if(!beneficiaries.isEmpty()) {
            
            	// total royalty for a product
            		BigDecimal totalRoyalty = itemTotal
            				.multiply(product.getRoyaltyPercent())
            				.divide(BigDecimal.valueOf(100));
            	
            	
            		RoyaltyCalculation rc = new RoyaltyCalculation();
            		rc.setProduct(product);
            		rc.setInvoiceDetail(item);
            		rc.setRoyaltyPercent(product.getRoyaltyPercent());
            		rc.setTotalAmount(itemTotal);
            		rc.setTotalRoyalty(totalRoyalty);
            	
            		rc = royaltyCalculationRespository.save(rc);
            	
            		// 3️⃣ Split equally
            		BigDecimal perBeneficiaryRoyalty =
            				totalRoyalty.divide(
                                BigDecimal.valueOf(beneficiaries.size()),2,RoundingMode.HALF_UP);
                
            		// 4️⃣ Insert ONE ROW PER BENEFICIARY
            		for (Beneficiary beneficiary : beneficiaries) {

                    ProductBeneficiary pbm =
                            new ProductBeneficiary();

//                    pbm.set(transaction);
                    pbm.setProduct(product);
                    pbm.setBeneficiary(beneficiary);
                    pbm.setRoyaltyCalculation(rc);
                    pbm.setRoyaltyReceived(perBeneficiaryRoyalty);
                    
                    productBeneficiaryRepo.save(pbm);
            
            		}
            
            	}

            }
            
            

            
            boolean exists = shelfRepo
            	    .existsByUser_UserIdAndProduct_ProductId(
            	        user.getUserId(),
            	        product.getProductId()
            	    );

            	if (!exists) {

            	    MyShelf shelf = new MyShelf();
            	    shelf.setUser(user);
            	    shelf.setProduct(product);

            	    // rent expiry logic
            	    if (Boolean.TRUE.equals(product.isRentable())) {
            	        shelf.setProductExpiryDate(
            	            LocalDateTime.now().plusDays(product.getMinRentDays())
            	        );
            	    }

            	    shelfRepo.save(shelf);

            	} else {
            	    System.out.println("Book already present in shelf → skip");
            	}
            
            
            
//            // 5️⃣ Add to shelf
//            MyShelf shelf = new MyShelf();
//            shelf.setUser(user);
//            shelf.setProduct(product);
//            
//            shelfRepo.save(shelf);
        }

        // 6️⃣ Update transaction
        transaction.setTotalAmount(total);
        transaction.setStatus(TransactionStatus.SUCCESS);
        transactionRepo.save(transaction);
        
//      pdf
      List<TransactionItem> items =
              itemRepo.findByTransaction(transaction);

      byte[] pdfBytes = transactionPdfService.generateInvoice(transaction, items);
      
      
//      for email
      emailService.sendTransactionSuccessEmail(
              transaction.getUser().getUserEmail(),
              transaction,
              pdfBytes
      );

        // 7️⃣ Clear cart
        //cartRepo.deleteByUser_UserId(user.getUserId());
        cartRepo.deleteByUser(user);
     
	}
}

