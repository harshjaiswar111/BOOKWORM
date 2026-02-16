import React, { useState } from "react";

// 🔥 ADD
import { useLocation } from "react-router-dom";

import { useNavigate } from "react-router-dom";
import { Lock } from "lucide-react";
import { BackButton } from "../UI/Shared";

export const PaymentView = ({ amount, upiId, setUpiId, onComplete }) => {
  const navigate = useNavigate();
  const [processing, setProcessing] = useState(false);


  // 🔥 ADD
const location = useLocation();
const finalAmountFromRoute = location.state?.finalAmount;
const rentPayload = location.state?.rentPayload;

// ✅ USE routed amount if present, else fallback to prop
const payableAmount = finalAmountFromRoute ?? amount;


  return (
    <div className="max-w-md mx-auto animate-in zoom-in-95 duration-500">
      <BackButton onClick={() => navigate(-1)} label="Back to Choice" />
      <div className="bg-slate-900 text-white p-10 rounded-[2.5rem] shadow-2xl mt-6">
        <div className="flex items-center gap-3 mb-8">
          <div className="w-10 h-10 bg-[#C5A059] rounded-xl flex items-center justify-center"><Lock size={18} /></div>
          <div>
            <h4 className="font-serif font-bold text-sm">Secure Payment</h4>
            <p className="text-[10px] text-slate-400 uppercase tracking-widest">UPI Encryption Active</p>
          </div>
        </div>
        <div className="mb-8">
          <label className="text-[10px] text-slate-400 uppercase font-bold mb-3 block tracking-widest">Enter UPI ID</label>
          <input
            value={upiId}
            onChange={(e) => setUpiId(e.target.value)}
            placeholder="username@bank"
            className="w-full px-5 py-4 rounded-xl bg-white/5 border border-white/10 text-white placeholder:text-white/20 focus:outline-none focus:border-[#C5A059] transition-all"
          />
        </div>
        <div className="mb-10 pb-6 border-b border-white/10 text-center">
          <p className="text-[10px] text-slate-400 uppercase mb-2 tracking-[0.2em]">Total Payable</p>
          <p className="text-4xl font-sans font-black">₹{payableAmount}</p>
        </div>
        <button
          disabled={processing}
          onClick={async () => {
            setProcessing(true);
            try {
              await onComplete(rentPayload);
            } finally {
              setProcessing(false);
            }
          }}
          className={`w-full py-4 rounded-xl font-bold uppercase text-xs tracking-[0.2em] transition-all
    ${processing
              ? "bg-gray-400 cursor-not-allowed"
              : "bg-[#C5A059] hover:bg-[#b38f4d] active:scale-95"
            }`}
        >
          {processing ? "Processing..." : "Pay Now"}
        </button>



        {/* <button onClick={onComplete} className="w-full py-4 bg-[#C5A059] text-white rounded-xl font-bold uppercase text-xs tracking-[0.2em] hover:bg-[#b38f4d] transition-all active:scale-95 shadow-lg shadow-[#C5A059]/20">Pay Now</button> */}
      </div>
    </div>
  );
};