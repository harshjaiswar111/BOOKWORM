import React, { useState } from "react";
import ReactDOM from "react-dom";
import { useNavigate } from "react-router-dom";
import { Clock, Library } from "lucide-react";
import { BackButton } from "../UI/Shared";
import { FALLBACK_IMAGE } from "../../data/libraryData";

export const ShelfView = ({ shelf }) => {
  const navigate = useNavigate();
  const handleImgError = (e) => {
    e.target.src = FALLBACK_IMAGE;
  };

  const [pdfUrl, setPdfUrl] = useState(null);
  const [activeBook, setActiveBook] = useState(null);

  const openPdf = async (productId, bookName) => {
    if (!productId) {
      console.error("openPdf called with invalid productId");
      return;
    }

    try {
      const res = await fetch(
        `http://localhost:8080/api/books/${productId}/read`,
        {
          headers: {
            Authorization: `Bearer ${sessionStorage.getItem("token")}`,
          },
        },
      );

      const blob = await res.blob();
      const url = window.URL.createObjectURL(blob);

      setPdfUrl(url);
      setActiveBook(bookName);
    } catch (err) {
      console.error("Failed to open PDF", err);
    }
  };

  return (
    <div className="animate-in fade-in slide-in-from-bottom-4 duration-700 max-w-6xl mx-auto px-4 pb-20">
      <BackButton onClick={() => navigate("/")} label="Back to Hub" />

      <header className="mb-8">
        <h1 className="text-3xl font-serif font-bold text-slate-900 mb-2 italic">
          Private Stacks
        </h1>
        <p className="text-base text-slate-400 italic border-l-4 border-[#C5A059] pl-4">
          A curated reflection of your soul through stories.
        </p>
      </header>

      {/* pdf viewer */}

      {pdfUrl && ReactDOM.createPortal(
        <div className="fixed inset-0 bg-black/90 z-[9999] flex flex-col">
          <div className="flex items-center justify-between bg-slate-900 text-white px-6 py-3 shadow-md">
            <h2 className="font-serif text-lg truncate">{activeBook}</h2>
            <button
              onClick={() => {
                window.URL.revokeObjectURL(pdfUrl);
                setPdfUrl(null);
                setActiveBook(null);
              }}
              className="text-sm font-bold uppercase hover:text-red-400 transition-colors"
            >
              Close
            </button>
          </div>

          <iframe src={pdfUrl} title="PDF Reader" className="flex-1 w-full" />
        </div>,
        document.body
      )}

      {shelf.length > 0 ? (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-8">
          {shelf.map((book, index) => {
            console.log("SHELF ITEM 👉", book);

            const productId =
              book?.product?.productId ??
              book?.productId ??
              book?.bookId ??
              book?.id ??
              null;

            let daysLeft = null;

            if (book.purchaseType == "rent" && book.productExpiryDate) {

              const today = new Date();
              const expiry = new Date(book.productExpiryDate);

              daysLeft = Math.ceil(
                (expiry.getTime() - today.getTime()) / (1000 * 60 * 60 * 24),
              );
            }

            const isRented = book.purchaseType === "rent" || !!book.productExpiryDate;

            return (
              <div
                key={`${productId ?? index}`}
                className="group flex flex-col perspective-container"
              >
                <div className="book-3d-card relative aspect-[2/3] mb-4 transition-all duration-700 transform group-hover:rotate-y-[-10deg]">
                  <div className="absolute inset-0 bg-white rounded-xl shadow-md overflow-hidden z-10 border border-slate-100">
                    <img
                      src={book.image}
                      alt={book.name}
                      className="w-full h-full object-cover"
                      onError={handleImgError}
                    />

                    {/* ⏳ EXPIRY BADGE INSIDE BOOK */}
                    {daysLeft !== null && daysLeft > 0 && (
                      <div
                        className={`absolute bottom-3 right-3 flex items-center gap-1.5 px-4 py-2 rounded-full text-[12px] font-bold backdrop-blur-md shadow-md
 
      ${daysLeft <= 5 ? "bg-red-600/90 text-white" : "bg-slate-900 text-white"}
    `}
                      >
                        <Clock size={12} />
                        {daysLeft} day{daysLeft > 1 ? "s" : ""} left
                      </div>
                    )}

                    <div className="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center p-4">
                      <button
                        onClick={() => openPdf(productId, book.name)}
                        className="bg-white text-slate-900 w-full py-2 rounded-lg font-serif text-sm font-bold shadow-lg active:scale-95"
                      >
                        Read Now
                      </button>
                    </div>
                  </div>

                  <div
                    className={`absolute top-3 right-3 px-2 py-0.5 rounded-full text-[9px] font-black uppercase z-20 backdrop-blur-md ${!isRented
                        ? "bg-amber-100/90 text-amber-700"
                        : "bg-indigo-100/90 text-indigo-700"
                      }`}
                  >
                    {!isRented ? "Eternal" : "Rented"}
                  </div>
                </div>

                <h3 className="font-serif text-lg font-bold text-slate-900 leading-tight mb-1 line-clamp-2">
                  {book.name || book.product?.productName}
                </h3>

                {/* 📅 Expiry date text */}
                {book.purchaseType === "rent" && daysLeft > 0 && (
                  <p
                    className={`text-xs font-bold mt-1 px-1 ${daysLeft <= 5 ? "text-red-600" : "text-slate-500"
                      }`}
                  >
                    ⏳Expires on{" "}
                    {new Date(book.productExpiryDate).toLocaleDateString(
                      "en-IN",
                    )}
                  </p>
                )}

                {/* {book.purchaseType === "rent" && (
                  <div className="flex items-center gap-1.5 text-[10px] text-indigo-500 font-bold uppercase tracking-tight">
                    <Clock size={12} /> 14 Days Left
                  </div>
                )} */}
              </div>
            );
          })}
        </div>
      ) : (
        <div className="text-center py-24 bg-white/40 rounded-3xl border-2 border-dashed border-slate-100">
          <Library size={48} className="mx-auto text-slate-200 mb-4" />
          <p className="text-lg text-slate-400 font-serif italic mb-6">
            Your sanctuary is silent.
          </p>
          <button
            onClick={() => navigate("/products")}
            className="bg-slate-900 text-white px-8 py-3 rounded-full text-xs font-bold uppercase tracking-widest hover:bg-black transition-all"
          >
            Build Collection
          </button>
        </div>
      )}
    </div>
  );
};
