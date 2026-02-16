import React from "react";
import { FALLBACK_IMAGE } from "../../data/libraryData";

export const LibraryBookCard = ({ book, onOpen, isSelected }) => {
  const imageUrl = book?.productImage
    ? `${import.meta.env.VITE_PUBLIC_URL || ""}${book.productImage}`
    : FALLBACK_IMAGE;

  // 🟡 Calculate remaining days (DISPLAY ONLY)
  let daysLeft = null;
  if (book.endDate) {
    const today = new Date();
    const end = new Date(book.endDate);

    daysLeft = Math.ceil(
      (end.getTime() - today.getTime()) / (1000 * 60 * 60 * 24)
    );
  }

  return (
    <div
      onClick={onOpen}
      className={`
        group p-3 rounded-2xl transition cursor-pointer
        hover:bg-slate-50
        ${isSelected ? "ring-2 ring-[#C5A059] bg-[#C5A059]/10" : ""}
      `}
    >
      {/* Image */}
      <div
        className="relative w-full overflow-hidden rounded-2xl shadow-md bg-white mb-4"
        style={{ aspectRatio: "2/3" }}
      >
        <img
          src={imageUrl}
          alt={book.productName}
          onError={(e) => (e.target.src = FALLBACK_IMAGE)}
          className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-105"
        />

        {/* 🔴 Remaining days badge */}
        {daysLeft !== null && daysLeft > 0 && (
          <span
            className={`absolute bottom-3 right-3 text-[12px] font-bold px-4 py-2 rounded-full ${daysLeft <= 5
              ? "bg-red-600 text-white"
              : "bg-slate-900 text-white"
              }`}
          >
            {daysLeft} day{daysLeft > 1 ? "s" : ""} left
          </span>
        )}
      </div>

      {/* Book Name */}
      <h3 className="font-serif text-base font-bold leading-snug line-clamp-2 px-1">
        {book.productName}
      </h3>

      {/* Author */}
      <p className="text-slate-500 italic text-sm font-serif mt-1 px-1">
        {book.author?.name}
      </p>

      {/* 📅 Expiry date text */}
      {book.endDate && daysLeft > 0 && (
        <p
          className={`text-xs font-bold mt-1 px-1 ${daysLeft <= 5 ? "text-red-600" : "text-slate-500"
            }`}
        >
          Expires on{" "}
          {new Date(book.endDate).toLocaleDateString("en-IN")}
        </p>
      )}
    </div>
  );
};
