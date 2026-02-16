import React from "react";
import { useNavigate } from "react-router-dom";

export const NotFoundView = () => {
  const navigate = useNavigate();
  return (
    <div className="text-center py-24 bg-white rounded-[3rem] border border-slate-100 shadow-sm">
      <h1 className="text-9xl font-serif font-black text-slate-100 mb-4">404</h1>
      <h2 className="text-3xl font-serif font-bold text-slate-900 mb-4 italic">This story hasn't been written yet.</h2>
      <p className="text-slate-400 italic mb-10 font-serif">The path you followed does not exist in our library.</p>
      <button 
        onClick={() => navigate("/")} 
        className="px-10 py-3 bg-[#C5A059] text-white rounded-full font-bold uppercase tracking-widest text-xs hover:bg-[#b38f4d] transition-all"
      >
        Return Home
      </button>
    </div>
  );
};