import axios from "axios";

const BASE_URL = "http://localhost:8080/api/orders/history";

export const getOrderHistoryAPI = () => {
  const user = JSON.parse(sessionStorage.getItem("user"));

  if (!user?.userId) {
    console.error("No userId found in sessionStorage");
    return Promise.resolve({ data: [] });
  }

  return axios.get(`${BASE_URL}/${user.userId}`);
};
