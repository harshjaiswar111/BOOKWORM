import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { BackButton } from "../UI/Shared";
import { BookOpen, Check, ShieldCheck } from "lucide-react";

export const ConfirmBuy = () => {
  const { state } = useLocation();
  const navigate = useNavigate();
  const [processing, setProcessing] = useState(false);

  // Redirect if no state (e.g. direct access)
  if (!state) {
    navigate("/my-library");
    return null;
  }

  const { packageId, selectedBooks, cost, isTopUp } = state;

  const confirmBuy = async () => {
    setProcessing(true);
    try {
      const token = sessionStorage.getItem("token");
      const user = JSON.parse(sessionStorage.getItem("user"));

      // ... (rest of logic)

      if (!user || !user.userId) {
        alert("User not logged in");
        return;
      }

      const payload = {
        userId: user.userId,
        packageId: packageId,
        productIds: selectedBooks.map((b) => b.productId),
      };

      console.log("CHECKOUT PAYLOAD:", payload);

      const res = await fetch("http://localhost:8080/api/library/checkout", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        throw new Error("Checkout failed");
      }

      // Success
      navigate("/my-library");
    } catch (err) {
      console.error(err);
      alert("Failed to confirm purchase. Please try again.");
    } finally {
      setProcessing(false);
    }
  };

  return (
    <div className="max-w-5xl mx-auto px-6 py-12 animate-in fade-in duration-500">
      <BackButton onClick={() => navigate(-1)} label="Back to Selection" />

      <div className="mt-8 grid grid-cols-1 md:grid-cols-3 gap-12">
        {/* LEFT COLUMN: ITEM LIST */}
        <div className="md:col-span-2">
          <h1 className="text-3xl font-serif font-bold text-slate-900 mb-2">
            Confirm Selection
          </h1>
          <p className="text-slate-500 mb-8 font-serif italic">
            Review the books you are adding to your library package.
          </p>

          <div className="space-y-4">
            {selectedBooks.map((book) => (
              <div
                key={book.productId}
                className="flex items-start gap-4 p-4 border border-slate-100 rounded-2xl bg-white shadow-sm hover:shadow-md transition-shadow"
              >
                {/* Thumbnail placeholder or book icon */}
                <div className="w-12 h-16 bg-slate-100 rounded-lg flex-shrink-0 flex items-center justify-center text-slate-400">
                  <BookOpen size={20} />
                </div>

                <div>
                  <h3 className="font-bold text-slate-900 font-serif">
                    {book.productName}
                  </h3>
                  <p className="text-xs text-slate-500 uppercase tracking-wider mt-1">
                    {book.author?.name || "Unknown Author"}
                  </p>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* RIGHT COLUMN: ACTION CARD */}
        <div>
          <div className="bg-slate-900 text-white p-8 rounded-[2.5rem] shadow-2xl sticky top-8">
            <div className="flex items-center gap-3 mb-8">
              <div className="w-10 h-10 bg-[#C5A059] rounded-xl flex items-center justify-center text-slate-900">
                <ShieldCheck size={20} />
              </div>
              <div>
                <h4 className="font-serif font-bold text-sm">
                  Library Update
                </h4>
                <p className="text-[10px] text-slate-400 uppercase tracking-widest">
                  Secure Action
                </p>
              </div>
            </div>

            <div className="mb-8 space-y-4">
              <div className="flex justify-between items-center text-sm border-b border-white/10 pb-4">
                <span className="text-slate-400">Package ID</span>
                <span className="font-mono">{packageId}</span>
              </div>
              <div className="flex justify-between items-center text-sm border-b border-white/10 pb-4">
                <span className="text-slate-400">Books Selected</span>
                <span className="font-bold text-[#C5A059]">
                  {selectedBooks.length}
                </span>
              </div>
              <div className="flex justify-between items-center text-sm pb-2">
                <span className="text-slate-400">
                  {isTopUp ? "Additional Cost" : "Package Price"}
                </span>
                <span className="font-bold text-white text-lg">
                  {isTopUp ? "Included" : `₹${cost || 0}`}
                </span>
              </div>
            </div>

            <button
              onClick={confirmBuy}
              disabled={processing}
              className={`w-full py-4 rounded-xl font-bold uppercase text-xs tracking-[0.2em] transition-all flex items-center justify-center gap-2
                ${processing
                  ? "bg-slate-700 cursor-not-allowed text-slate-400"
                  : "bg-[#C5A059] hover:bg-[#b38f4d] active:scale-95 text-slate-900"
                }`}
            >
              {processing ? (
                "Processing..."
              ) : (
                <>
                  Confirm & Add <Check size={16} />
                </>
              )}
            </button>

            <p className="text-[10px] text-slate-500 text-center mt-6 leading-relaxed">
              By confirming, these books will be added to your library package immediately.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};
