import api from "../api/axios";


// Get the user's shelf
export const getShelfAPI = (userId) => {
    return api.get(`/shelf/${userId}`);
};
