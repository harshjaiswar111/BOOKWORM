import React from "react";
import { useNavigate } from "react-router-dom";
import { Star } from "lucide-react";

export const SuccessView = () => {
  const navigate = useNavigate();
  return (
    <div className="text-center py-24 animate-in fade-in duration-1000">
      <div className="w-20 h-20 bg-emerald-50 text-emerald-500 rounded-full flex items-center justify-center mx-auto mb-8">
        <Star size={40} fill="currentColor" />
      </div>
      <h1 className="text-4xl font-serif font-bold text-slate-900 mb-4 italic">Payment Successful</h1>
      <p className="text-slate-500 italic mb-12 text-lg">Your sanctuary has expanded. Your books are waiting.</p>
      <button 
        onClick={() => navigate("/shelf")} 
        className="px-12 py-4 bg-slate-900 text-white rounded-full font-bold uppercase text-xs tracking-widest hover:bg-black transition-all shadow-xl"
      >
        Go to My Shelf
      </button>
    </div>
  );
};