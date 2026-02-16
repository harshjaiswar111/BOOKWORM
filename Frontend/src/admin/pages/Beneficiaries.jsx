
import React, { useEffect, useState } from "react";
import api from "../../api/axios";

export default function Beneficiary() {
  const [beneficiaries, setBeneficiaries] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchBeneficiaries();
  }, []);

  const fetchBeneficiaries = async () => {
    try {
      const res = await api.get("/product-beneficiaries");
      console.log("BENEFICIARY RESPONSE:", res.data);
      const data = Array.isArray(res.data) ? res.data : [res.data];
      setBeneficiaries(data);
    } catch (error) {
      console.error("Failed to fetch beneficiaries", error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <p className="text-center mt-10">Loading beneficiaries...</p>;
  }

  return (
  <div className="max-w-7xl mx-auto p-6">
    <h1 className="text-3xl font-bold mb-8 text-slate-800">
      Beneficiary Earnings
    </h1>

    {beneficiaries.length === 0 ? (
      <p className="text-slate-500 text-center">
        No beneficiary records found.
      </p>
    ) : (
      <div className="overflow-x-auto rounded-xl border border-slate-200 shadow-sm">
        <table className="w-full border-collapse">
          <thead className="bg-slate-100">
            <tr>
              <th className="px-6 py-4 text-left text-sm font-semibold text-slate-600">
                #
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-slate-600">
                Beneficiary Name
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-slate-600">
                Product Name
              </th>
              <th className="px-6 py-4 text-right text-sm font-semibold text-slate-600">
                Royalty Received (₹)
              </th>
            </tr>
          </thead>

          <tbody className="divide-y divide-slate-100 bg-white">
            {beneficiaries.map((item, index) => (
              <tr
                key={item.prodbenId}
                className="hover:bg-slate-50 transition-colors"
              >
                <td className="px-6 py-4 text-sm text-slate-500">
                  {index + 1}
                </td>

                <td className="px-6 py-4 text-sm font-medium text-slate-800">
                  {item.beneficiaryName}
                </td>

                <td className="px-6 py-4 text-sm text-slate-700">
                  {item.productName}
                </td>

                <td className="px-6 py-4 text-sm font-semibold text-right text-green-600">
                  ₹ {item.royaltyReceived}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    )}
  </div>
);

}