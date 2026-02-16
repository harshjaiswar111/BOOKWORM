import React from "react";
import { X } from "lucide-react";
import { FALLBACK_IMAGE } from "../../data/libraryData";

export const ProductModal = ({ book, onClose }) => {
  console.log("BOOK IN MODAL:", book);
  return (

    <div className="fixed inset-0 z-50 bg-black/60 backdrop-blur-sm flex items-center justify-center px-4">

      <div className="bg-white max-w-3xl w-full rounded-2xl shadow-2xl overflow-hidden relative animate-in fade-in zoom-in duration-300">

        {/* Close */}
        <button
          onClick={onClose}
          className="absolute top-4 right-4 text-slate-500 hover:text-black"
        >
          <X size={20} />
        </button>

        <div className="flex flex-col md:flex-row">

          {/* Image */}
          <img
            src={book.productImage || FALLBACK_IMAGE}
            alt={book.productName}
            className="w-full md:w-1/3 object-cover"
          />

          {/* Content */}
          <div className="p-6 flex flex-col gap-4">
            <h2 className="font-serif text-2xl font-bold">
              {book.productName}
            </h2>

            <p className="italic text-slate-500">
              {book.author?.name}
            </p>


            <div className="text-sm text-slate-600 space-y-1 font-serif">
              <p>
                <span className="font-semibold">Language:</span>{" "}
                {book.language?.languageDesc}
              </p>

              <p>
                <span className="font-semibold">Genre:</span>{" "}
                {book.genere?.genereDesc}
              </p>

              <p>
                <span className="font-semibold">ISBN:</span>{" "}
                {book.productIsbn}
              </p>

              <p>
                <span className="font-semibold">Publisher:</span>{" "}
                {book.publisher?.name}
              </p>

              <p>
                <span className="font-semibold">Contact:</span>{" "}
                {book.publisher?.email}
              </p>
            </div>


            <p className="text-slate-700 leading-relaxed text-sm">
              {book.productDescriptionLong}
            </p>

            <div className="mt-4 flex items-center justify-between">
              <span className="text-xl font-bold">
                ₹{book.productBaseprice?.toLocaleString("en-IN")}
              </span>

              <button
                onClick={() => onClose()}
                className="px-6 py-3 bg-black text-white rounded-full hover:bg-[#C5A059]"
              >
                Close
              </button>
            </div>
          </div>
        </div>

      </div>
    </div>
  );
};