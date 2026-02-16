import React from 'react';
import { ArrowLeft, Bookmark } from 'lucide-react';

export const BackButton = ({ onClick, label }) => (
  <button
    onClick={onClick}
    className="flex items-center gap-2 text-slate-400 hover:text-[#C5A059] mb-10 transition-all font-serif italic text-sm group"
  >
    <ArrowLeft size={16} className="group-hover:-translate-x-1 transition-transform" /> {label}
  </button>
);

export const OwnershipModal = ({ bookName, purchaseType, onClose, onGoToShelf }) => (
  <div className="fixed inset-0 z-[100] flex items-center justify-center p-6 bg-slate-900/60 backdrop-blur-sm">
    <div className="bg-white max-w-md w-full rounded-3xl p-10 shadow-2xl text-center border-t-4 border-[#C5A059] animate-in zoom-in-95 duration-300">
      <div className="w-16 h-16 bg-amber-50 text-[#C5A059] rounded-full flex items-center justify-center mx-auto mb-6">
        <Bookmark size={32} />
      </div>
      <h3 className="text-2xl font-serif font-bold text-slate-900 mb-2">Already in Your Sanctuary</h3>
      <p className="text-slate-500 font-serif italic mb-8">
        You have already {purchaseType === 'buy' ? 'purchased' : 'rented'} <span className="text-slate-900 font-bold">"{bookName}"</span>.
      </p>
      <div className="flex flex-col gap-3">
        <button onClick={onGoToShelf} className="w-full py-4 bg-slate-900 text-white rounded-xl font-bold hover:bg-black transition-all shadow-lg">
          Go to My Shelf
        </button>
        <button onClick={onClose} className="w-full py-3 text-slate-400 hover:text-slate-900 underline underline-offset-4">
          Stay in Library
        </button>
      </div>
    </div>
  </div>
);