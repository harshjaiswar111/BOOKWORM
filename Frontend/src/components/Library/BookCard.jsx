import React from "react";
import { Plus } from "lucide-react";
import { FALLBACK_IMAGE } from "../../data/libraryData";

export const BookCard = ({ book, onAddToCart, onOpen, isOwned }) => {
  // ✅ FIX: build correct public image URL
  const imageUrl = book?.productImage
    ? `${import.meta.env.VITE_PUBLIC_URL || ""}${book.productImage}`
    : FALLBACK_IMAGE;

  return (
    <div className="bg-white p-4 rounded-2xl border border-slate-100 shadow-sm hover:shadow-xl transition-all duration-300 hover:-translate-y-2 group cursor-default h-full flex flex-col relative">
      {/* Image Container */}
      <div className="aspect-[2/3] overflow-hidden rounded-xl mb-4 bg-slate-100 relative shadow-inner group-hover:shadow-md transition-shadow">
        <img
          src={imageUrl}
          alt={book.productName}
          onError={(e) => (e.target.src = FALLBACK_IMAGE)}
          className="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-700 ease-out"
        />

        {/* Rent Badge */}
        {book.rentable && (
          <span className="absolute top-2 right-2 bg-indigo-600 text-white text-[10px] font-bold tracking-widest px-2 py-1 rounded-full shadow-md z-10">
            RENT
          </span>
        )}

        {/* Type Badge */}
        <span className="absolute top-2 left-2 px-3 py-1 bg-white/90 backdrop-blur-sm rounded-full text-[10px] font-bold tracking-widest uppercase text-slate-600 border border-slate-100 z-10">
          {book.productType?.typeDesc}
        </span>

        {/* Hover Overlay */}
        <div
          className="absolute inset-0 bg-slate-900/40 opacity-0 group-hover:opacity-100 transition-opacity duration-300 flex items-center justify-center backdrop-blur-[2px]"
        >
          {isOwned ? (
            <div className="bg-white/90 backdrop-blur-md text-slate-900 px-4 py-2 rounded-full font-serif font-bold text-xs flex items-center gap-2 shadow-xl transform translate-y-4 group-hover:translate-y-0 transition-transform duration-300">
              <span className="w-2 h-2 bg-green-500 rounded-full animate-pulse"></span>
              Owned
            </div>
          ) : (
            onAddToCart && (
              <button
                onClick={() => onAddToCart(book)}
                className="bg-white text-slate-900 px-6 py-3 rounded-full font-bold text-xs transform translate-y-8 group-hover:translate-y-0 transition-all duration-500 shadow-xl hover:bg-[#C5A059] hover:text-white flex items-center gap-2"
              >
                <Plus size={16} />
                Add to Cart
              </button>
            )
          )}
        </div>
      </div>

      {/* Info Section */}
      <div className="space-y-0.5 mt-auto flex-1 flex flex-col">
        <p className="text-xs font-bold text-[#C5A059] uppercase tracking-widest line-clamp-1">
          {book.genere?.genereDesc || "Unknown Genre"}
        </p>
        <h3
          onClick={onOpen}
          className="cursor-pointer font-serif font-bold text-lg text-slate-900 leading-tight group-hover:text-[#C5A059] transition-colors line-clamp-2"
        >
          {book.productName}
        </h3>
        <p className="text-sm text-slate-500 font-medium line-clamp-1 mb-2">
          {book.author?.name}
        </p>

        {/* Price Section */}
        {onAddToCart && (
          <div className="mt-auto pt-3 border-t border-slate-100 flex items-center justify-between">
            <div className="flex flex-col">
              {book.productOfferprice && book.discountPercent > 0 ? (
                <>
                  <div className="flex items-center gap-2">
                    <span className="text-2xl font-bold text-slate-900">
                      ₹{book.productOfferprice.toLocaleString("en-IN")}
                    </span>
                    <span className="text-xs font-bold text-green-700 bg-green-100 px-2 py-0.5 rounded">
                      {book.discountPercent}%
                    </span>
                  </div>
                  <span className="text-xs text-slate-400 line-through">
                    ₹{book.productBaseprice?.toLocaleString("en-IN")}
                  </span>
                </>
              ) : (
                <span className="text-2xl font-bold text-slate-900">
                  ₹{book.productBaseprice?.toLocaleString("en-IN")}
                </span>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};
