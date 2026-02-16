package com.example.Controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.example.dto.OrderHistoryDTO;
import com.example.Controller.*;
import com.example.Services.OrderHistoryService;

@RestController
@RequestMapping("/api/orders")
public class OrderHistoryController {
	
	@Autowired
	private OrderHistoryService orderHistoryService;
	
	@GetMapping("/history/{userId}")
	public List<OrderHistoryDTO>getOrderHistory(@PathVariable int userId){
		return orderHistoryService.getOrderHistory(userId);
	}
	
	@GetMapping("/history")
	public List<OrderHistoryDTO>getAllOrders(){
		return orderHistoryService.getAllOrderHistory();
	}

}
