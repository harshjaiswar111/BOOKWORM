//import axios from "axios";
import api from "../api/axios";
const API_URL = "http://localhost:8080/api/cart";

// ADD item to cart
export const addToCartAPI = (userId, productId, qty = 1) => {
  //const token = sessionStorage.getItem("token");

  return api.post(
    `/cart/add`,
    {
      userId,
      productId,
      qty,
    }
  );
};

// REMOVE item from cart (future use)

export const removeFromCartAPI = (cartId) => {
  return api.delete(`/cart/remove/${cartId}`);
};



// export const removeFromCartAPI = (userId, bookId) => {
//   return axios.delete(`${API_URL}/remove`, {
//     data: { userId, bookId }
//   });
// };

// GET cart items (future use)


export const getCartAPI = (userId) => {
  return api.get(`/cart/${userId}`);
};

// export const getCartAPI = (userId) => {
//   return axios.get(`${API_URL}/${userId}`);
// };
