package com.example.Services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.example.dto.OrderHistoryDTO;
import com.example.Repository.*;

@Service
public class OrderHistoryService{
	
	@Autowired
	private TransactionItemRepository transactionItemRepositroy;
	
	public List<OrderHistoryDTO>getOrderHistory(int userId){
		return transactionItemRepositroy.findOrderHistoryByUserId(userId);	
	}
	
	public List<OrderHistoryDTO>getAllOrderHistory(){
		return transactionItemRepositroy.findAllOrderHistory();
	}

}
