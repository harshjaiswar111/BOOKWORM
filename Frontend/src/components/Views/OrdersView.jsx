import { useEffect, useState } from "react";
import { getOrderHistoryAPI } from "../../services/orderhistoryservice";

export const OrdersView = () => {
  const [orders, setOrders] = useState([]);

  useEffect(() => {
    getOrderHistoryAPI()
      .then(res => setOrders(res.data))
      .catch(err => console.error(err));
  }, []);

  if (orders.length === 0) {
    return (
      <div className="max-w-5xl mx-auto mt-16 text-slate-500">
        No orders yet 📦
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto mt-16 px-4">
      <h2 className="text-2xl font-serif font-bold mb-6">
        My Orders
      </h2>

      <div className="overflow-x-auto bg-white rounded-2xl shadow border border-slate-100">
        <table className="min-w-full border-collapse">
          <thead className="bg-[#F8F9FA]">
            <tr>
              <th className="px-6 py-4 text-left text-xs font-bold uppercase tracking-widest text-slate-600">
                Transaction ID
              </th>
              <th className="px-6 py-4 text-left text-xs font-bold uppercase tracking-widest text-slate-600">
                Product
              </th>
              <th className="px-6 py-4 text-left text-xs font-bold uppercase tracking-widest text-slate-600">
                Amount
              </th>
              <th className="px-6 py-4 text-left text-xs font-bold uppercase tracking-widest text-slate-600">
                Ordered On
              </th>
            </tr>
          </thead>

          <tbody>
            {orders.map((order, index) => (
              <tr
                key={index}
                className="border-t border-slate-100 hover:bg-[#FAFAFA] transition-colors"
              >
                <td className="px-6 py-4 text-sm font-medium text-slate-700">
                  #{order.transactionId}
                </td>

                <td className="px-6 py-4 text-sm text-slate-700">
                  {order.productName}
                </td>

                <td className="px-6 py-4 text-sm font-semibold text-slate-800">
                  ₹{Number(order.amount).toLocaleString("en-IN")}
                </td>

                <td className="px-6 py-4 text-sm text-slate-500">
                  {new Date(order.orderDate).toLocaleString("en-IN", {
                    dateStyle: "medium",
                    timeStyle: "short",
                  })}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};
