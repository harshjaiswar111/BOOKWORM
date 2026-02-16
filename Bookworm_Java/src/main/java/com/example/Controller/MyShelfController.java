package com.example.Controller;

import com.example.Services.ShelfService;
import com.example.models.MyShelf;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/shelf")
@CrossOrigin(origins = {"http://localhost:5173"})
public class MyShelfController {

    private final ShelfService myShelfService;

    public MyShelfController(ShelfService myShelfService) {
        this.myShelfService = myShelfService;
    }

    @GetMapping("/{userId}")
    public List<MyShelf> getShelfItems(@PathVariable("userId") Integer userId) {
        return myShelfService.getShelfByUser(userId);
    }

    @DeleteMapping("/delete/{shelfId}")
    public String deleteShelfItem(@PathVariable("shelfId") Integer shelfId) {
        myShelfService.deleteShelfItem(shelfId);
        return "Deleted Successfully";
    }
}
