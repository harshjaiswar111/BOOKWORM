package com.example.Scheduler;

import com.example.Repository.MyShelfRepository;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Component;

import java.time.LocalDateTime;

@Component
public class ShelfExpiryScheduler {

    private final MyShelfRepository shelfRepository;

    public ShelfExpiryScheduler(MyShelfRepository shelfRepository) {
        this.shelfRepository = shelfRepository;
    }

    // runs every 1 minute
    @Scheduled(fixedRate = 60000000)
    public void removeExpiredShelfItems() {

        shelfRepository.deleteByProductExpiryDateBefore(LocalDateTime.now());

        System.out.println("Expired shelf rows cleaned");
    }
}
