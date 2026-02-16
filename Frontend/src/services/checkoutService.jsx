import api from "../api/axios";

// Record a new transaction
export const checkoutAPI = (transactionData) => {
    return api.post("/checkout", transactionData);
};
