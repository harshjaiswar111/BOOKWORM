package com.example.Services;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.example.Repository.BeneficiaryRepository;
import com.example.Repository.LibraryPackagePurchaseRepository;
import com.example.Repository.LibraryPackageRepository;
import com.example.Repository.MyLibraryRepository;
import com.example.Repository.ProductBeneficiaryRepository;
import com.example.Repository.ProductRepository;
import com.example.Repository.TransactionRepository;
import com.example.Repository.UserRepository;
import com.example.dto.LibraryCheckoutRequest;
import com.example.models.Beneficiary;
import com.example.models.LibraryPackage;
import com.example.models.LibraryPackagePurchase;
import com.example.models.MyLibrary;
import com.example.models.Product;
import com.example.models.ProductBeneficiary;
import com.example.models.RoyaltyCalculation;
import com.example.models.Transaction;
import com.example.models.TransactionItem;
import com.example.models.TransactionStatus;
import com.example.models.TransactionType;
import com.example.models.User;
import com.example.Repository.RoyaltyCalculationRespository;
import com.example.Repository.TransactionItemRepository;
//package com.example.Services;
//
//import com.example.Repository.LibraryPackageRepository;
//import com.example.Repository.MyLibraryRepository;
//import com.example.Repository.ProductRepository;
//import com.example.Repository.UserRepository;
//import com.example.dto.LibraryCheckoutRequest;
//import com.example.models.LibraryPackage;
//import com.example.models.MyLibrary;
//import com.example.models.Product;
//import com.example.models.User;
//
//import org.springframework.stereotype.Service;
//import org.springframework.transaction.annotation.Transactional;
//
//import java.time.LocalDate;
//
//@Service
//public class LibraryCheckoutServiceImpl implements LibraryCheckoutService {
//
//    private final UserRepository userRepository;
//    private final LibraryPackageRepository libraryPackageRepository;
//    private final ProductRepository productRepository;
//    private final MyLibraryRepository myLibraryRepository;
//
//    public LibraryCheckoutServiceImpl(
//            UserRepository userRepository,
//            LibraryPackageRepository libraryPackageRepository,
//            ProductRepository productRepository,
//            MyLibraryRepository myLibraryRepository) {
//
//        this.userRepository = userRepository;
//        this.libraryPackageRepository = libraryPackageRepository;
//        this.productRepository = productRepository;
//        this.myLibraryRepository = myLibraryRepository;
//    }
//
//    @Override
//    @Transactional
//    public void checkout(LibraryCheckoutRequest request) {
//
//        // 1️⃣ Fetch user
//        User user = userRepository.findById(request.getUserId())
//                .orElseThrow(() -> new RuntimeException("User not found"));
//
//        // 2️⃣ Fetch library package
//        LibraryPackage libraryPackage = libraryPackageRepository
//                .findById(request.getPackageId())
//                .orElseThrow(() -> new RuntimeException("Library package not found"));
//
//        // 3️⃣ Count already active borrowed books
//        int activeBorrowedBooks =
//                myLibraryRepository.countByUser_UserIdAndEndDateAfter(
//                        user.getUserId(), LocalDate.now());
//
//        int selectedBooks = request.getProductIds().size();
//
//        // 4️⃣ Enforce book limit
//        if (activeBorrowedBooks + selectedBooks > libraryPackage.getBookLimit()) {
//            throw new RuntimeException("Library book limit exceeded");
//        }
//
//        // 5️⃣ Calculate validity dates
//        LocalDate startDate = LocalDate.now();
//        LocalDate endDate = startDate.plusDays(libraryPackage.getValidityDays());
//
//        // 6️⃣ Create MyLibrary entry PER BOOK
//        for (Integer productId : request.getProductIds()) {
//
//            Product product = productRepository.findById(productId)
//                    .orElseThrow(() -> new RuntimeException("Product not found"));
//
//            // Safety check (optional but good)
//            if (!Boolean.TRUE.equals(product.isLibrary())) {
//                throw new RuntimeException(
//                        "Product " + product.getProductName() + " is not available for library");
//            }
//
//            MyLibrary myLibrary = new MyLibrary();
//            myLibrary.setUser(user);
//            myLibrary.setLibraryPackage(libraryPackage);
//            myLibrary.setProduct(product);
//            myLibrary.setStartDate(startDate);
//            myLibrary.setEndDate(endDate);
//
//            // These are duplicated by design (acceptable for this model)
//            myLibrary.setBooksAllowed(libraryPackage.getBookLimit());
//            myLibrary.setBooksTaken(activeBorrowedBooks + selectedBooks);
//
//            myLibraryRepository.save(myLibrary);
//        }
//    }
//}


@Service
public class LibraryCheckoutServiceImpl implements LibraryCheckoutService {

    private final UserRepository userRepository;
    private final LibraryPackageRepository libraryPackageRepository;
    private final LibraryPackagePurchaseRepository libraryPackagePurchaseRepository;
    private final ProductRepository productRepository;
    private final MyLibraryRepository myLibraryRepository;
    private final TransactionRepository transactionRepository;
    private final RoyaltyCalculationRespository royaltyCalculationRepository;
    
    private final LibraryInvoicePdfService libraryInvoicePdfService;
    private final EmailService emailService;
    private final TransactionItemRepository transactionItemRepository;
    private final BeneficiaryRepository beneficiaryRepository;
    private final ProductBeneficiaryRepository productBeneficiaryRepository;



    public LibraryCheckoutServiceImpl(
            UserRepository userRepository,
            LibraryPackageRepository libraryPackageRepository,
            LibraryPackagePurchaseRepository libraryPackagePurchaseRepository,
            ProductRepository productRepository,
            MyLibraryRepository myLibraryRepository,
            TransactionRepository transactionRepository,
            RoyaltyCalculationRespository royaltyCalculationRepository,
            LibraryInvoicePdfService libraryInvoicePdfService,
            EmailService emailService,
            TransactionItemRepository transactionItemRepository,
            BeneficiaryRepository beneficiaryRepository,
            ProductBeneficiaryRepository productBeneficiaryRepository
            
    ) {
        this.userRepository = userRepository;
        this.libraryPackageRepository = libraryPackageRepository;
        this.libraryPackagePurchaseRepository = libraryPackagePurchaseRepository;
        this.productRepository = productRepository;
        this.myLibraryRepository = myLibraryRepository;
        this.transactionRepository = transactionRepository;
        this.royaltyCalculationRepository = royaltyCalculationRepository;
		this.libraryInvoicePdfService = libraryInvoicePdfService;
		this.emailService = emailService;
		this.transactionItemRepository = transactionItemRepository;
		this.beneficiaryRepository = beneficiaryRepository;
		this.productBeneficiaryRepository = productBeneficiaryRepository;

		
    }	

    @Override
    @Transactional
    public void checkout(LibraryCheckoutRequest request) {

        if (request.getProductIds() == null || request.getProductIds().isEmpty()) {
            throw new RuntimeException("No books selected");
        }

        User user = userRepository.findById(request.getUserId())
                .orElseThrow(() -> new RuntimeException("User not found"));

     // ✅ Block multiple active packages (SAFE – Java-side date logic)
        List<LibraryPackagePurchase> previousPurchases =
                libraryPackagePurchaseRepository
                        .findByUser_UserIdOrderByPurchaseDateDesc(user.getUserId());

//        if (!previousPurchases.isEmpty()) {
//
//            LibraryPackagePurchase latest = previousPurchases.get(0);
//
//            LocalDateTime expiry =
//                    latest.getPurchaseDate()
//                          .plusDays(
//                              latest.getLibraryPackage().getValidityDays()
//                          );
//
//            if (expiry.isAfter(LocalDateTime.now())) {
//                throw new RuntimeException(
//                        "User already has an active library package"
//                );
//            }
//        }

        LocalDateTime now = LocalDateTime.now();

//        for (LibraryPackagePurchase purchase : previousPurchases) {
//
//            LocalDateTime expiry =
//                    purchase.getPurchaseDate()
//                            .plusDays(
//                                purchase.getLibraryPackage().getValidityDays()
//                            );
//
//            if (expiry.isAfter(now)) {
//                throw new RuntimeException(
//                        "User already has an active library package"
//                );
//            }
//        }
        
//        if (!previousPurchases.isEmpty()) {
//
//            LibraryPackagePurchase latest = previousPurchases.get(0);
//
//            LocalDateTime expiry =
//                    latest.getPurchaseDate()
//                          .plusDays(
//                              latest.getLibraryPackage().getValidityDays()
//                          );
//
//            if (expiry.isAfter(now)) {
//                throw new RuntimeException(
//                        "User already has an active library package"
//                );
//            }
//        }
        
        
        LibraryPackagePurchase activePurchase = null;

        if (!previousPurchases.isEmpty()) {

            LibraryPackagePurchase latest = previousPurchases.get(0);

            LocalDateTime expiry =
                latest.getPurchaseDate()
                      .plusDays(
                          latest.getLibraryPackage().getValidityDays()
                      );

            // ✅ Same package AND not expired → reuse
            if (expiry.isAfter(now)
                && latest.getLibraryPackage().getPackageId()
                         .equals(request.getPackageId())) {

                activePurchase = latest;
            }
        }

        

        LibraryPackage libraryPackage =
                libraryPackageRepository.findById(request.getPackageId())
                        .orElseThrow(() -> new RuntimeException("Library package not found"));

        int bookLimit = libraryPackage.getBookLimit();

        List<Product> products =
                productRepository.findAllById(request.getProductIds());

        if (products.size() != request.getProductIds().size()) {
            throw new RuntimeException("Invalid product selection");
        }

        if (products.size() > bookLimit) {
            throw new RuntimeException("Library book limit exceeded");
        }

        // 1️⃣ Create Transaction
        Transaction transaction = new Transaction();
        transaction.setUser(user);
        transaction.setTransactionType(TransactionType.BUY);
        transaction.setStatus(TransactionStatus.PENDING);
        transaction.setTotalAmount(libraryPackage.getCost());
        transaction.setCreatedAt(LocalDateTime.now());
        transactionRepository.save(transaction);
        
        if (bookLimit <= 0) {
            throw new RuntimeException("Invalid library package configuration");
        }

        // 2️⃣ Create Package Purchase
        BigDecimal avgBookPrice =
                libraryPackage.getCost()
                        .divide(BigDecimal.valueOf(bookLimit),
                        		2,
                                RoundingMode.HALF_EVEN);

        
        LibraryPackagePurchase purchase;

        if (activePurchase != null) {
            // ✅ Reuse existing purchase
            purchase = activePurchase;
        } else {
         purchase = new LibraryPackagePurchase();
        purchase.setUser(user);
        purchase.setTransaction(transaction);
        purchase.setLibraryPackage(libraryPackage);
        purchase.setPackagePrice(libraryPackage.getCost());
        purchase.setAllowedBooks(bookLimit);
        purchase.setAvgBookPrice(avgBookPrice);
        purchase.setPurchaseDate(LocalDateTime.now());
        libraryPackagePurchaseRepository.save(purchase);
        }
        
        
        
        LocalDate today = LocalDate.now();
        LocalDate endDate = today.plusDays(libraryPackage.getValidityDays());
        
        

        //int booksTaken = 0;
        
        int booksAlreadyTaken =
        	    myLibraryRepository.countByUser_UserIdAndEndDateAfter(
        	        user.getUserId(),
        	        LocalDate.now()
        	    );
        
     // ✅ Enforce total limit BEFORE adding anything
        if (booksAlreadyTaken + products.size() > purchase.getAllowedBooks()) {
            throw new RuntimeException("Library book limit exceeded");
        }

        int booksTaken = booksAlreadyTaken;


        for (Product product : products) {

            if (!Boolean.TRUE.equals(product.isLibrary())) {
                throw new RuntimeException(
                        product.getProductName() + " is not library-enabled");
            }

            boolean alreadyOwned =
                    myLibraryRepository
                            .existsByUser_UserIdAndProduct_ProductIdAndEndDateAfter(
                                    user.getUserId(),
                                    product.getProductId(),
                                    today
                            );

            if (alreadyOwned) {
                throw new RuntimeException(
                        product.getProductName() + " already in library");
            }

            booksTaken++;

            BigDecimal royaltyPercent =
                    product.getRoyaltyPercent() != null
                            ? product.getRoyaltyPercent()
                            : BigDecimal.ZERO;

            BigDecimal royaltyAmount =
                    avgBookPrice
                            .multiply(royaltyPercent)
                            .divide(BigDecimal.valueOf(100),
                            		2,
                                    RoundingMode.HALF_EVEN);

//            if (royaltyPercent.compareTo(BigDecimal.ZERO) > 0) {
//                RoyaltyCalculation royalty = new RoyaltyCalculation();
//                royalty.setProduct(product);
//                royalty.setRoyaltyPercent(royaltyPercent);
//                royalty.setTotalAmount(avgBookPrice);
//                royalty.setTotalRoyalty(royaltyAmount);
//                royalty.setRoycalTranDate(today);
//                royaltyCalculationRepository.save(royalty);
//            }

         // 1️⃣ Create TransactionItem (REQUIRED for royalty)
            TransactionItem item = new TransactionItem();
            item.setTransaction(transaction);
            item.setProduct(product);
            item.setPrice(avgBookPrice);
            item.setQuantity(1);

            transactionItemRepository.save(item);
            
            

            // 2️⃣ Create RoyaltyCalculation ONLY if royalty > 0
//            if (royaltyPercent.compareTo(BigDecimal.ZERO) > 0) {
//
//                RoyaltyCalculation royalty = new RoyaltyCalculation();
//                royalty.setInvoiceDetail(item);          // 🔑 CRITICAL FIX
//                royalty.setProduct(product);
//                royalty.setRoyaltyPercent(royaltyPercent);
//                royalty.setTotalAmount(avgBookPrice);
//                royalty.setTotalRoyalty(royaltyAmount);
//                royalty.setRoycalTranDate(today);
//
//                royaltyCalculationRepository.save(royalty);
//            }
            
            
            
         // 2️⃣ Create RoyaltyCalculation + ProductBeneficiary
            if (royaltyPercent.compareTo(BigDecimal.ZERO) > 0) {

                // 1️⃣ Save RoyaltyCalculation
                RoyaltyCalculation royalty = new RoyaltyCalculation();
                royalty.setInvoiceDetail(item);
                royalty.setProduct(product);
                royalty.setRoyaltyPercent(royaltyPercent);
                royalty.setTotalAmount(avgBookPrice);
                royalty.setTotalRoyalty(royaltyAmount);
                royalty.setRoycalTranDate(today);

                royaltyCalculationRepository.save(royalty);

                // 2️⃣ Fetch beneficiaries for this product
                List<Beneficiary> beneficiaries =
                        beneficiaryRepository.findByProduct_ProductId(
                                product.getProductId()
                        );

                if (beneficiaries.isEmpty()) {
                    throw new RuntimeException(
                        "No beneficiary configured for product: "
                        + product.getProductName()
                    );
                }

                // 3️⃣ Create ProductBeneficiary record
                for (Beneficiary beneficiary : beneficiaries) {

                    ProductBeneficiary pb = new ProductBeneficiary();
                    pb.setBeneficiary(beneficiary);
                    pb.setProduct(product);
                    pb.setRoyaltyCalculation(royalty);
                    pb.setRoyaltyReceived(royaltyAmount);

                    productBeneficiaryRepository.save(pb);
                }
            }


            MyLibrary myLibrary = new MyLibrary();
            myLibrary.setUser(user);
            myLibrary.setLibraryPackage(libraryPackage);
            myLibrary.setProduct(product);
            myLibrary.setStartDate(today);
            myLibrary.setEndDate(endDate);
            myLibrary.setBooksAllowed(bookLimit);
            myLibrary.setBooksTaken(booksTaken);
            myLibraryRepository.save(myLibrary);
        }

        transaction.setStatus(TransactionStatus.SUCCESS);
        transactionRepository.save(transaction);
        
        
     // ===============================
     // 📄 Generate Library Invoice PDF
     // ===============================
     byte[] invoicePdf =
             libraryInvoicePdfService.generateLibraryInvoice(purchase);

     // ===============================
     // 📧 Send Email with Invoice
     // ===============================
     emailService.sendTransactionSuccessEmail(
             user.getUserEmail(),
             transaction,
             invoicePdf
    		 );
    }
}
