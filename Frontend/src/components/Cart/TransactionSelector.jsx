
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { BookOpen, Clock } from 'lucide-react';
import { BackButton } from '../UI/Shared';

export const TransactionSelector = ({
  calculateTotal,
  calculateRentalTotal,
  startPaymentFlow, // kept for compatibility; not used inside cards now
  cart,
  setCart          // kept for compatibility; not used here
}) => {
  const navigate = useNavigate();

  const isRentable = cart?.some((item) => item.rentable === true);

  // UPDATED: per-item mode (buy/rent) and rent days
  const [rentDays, setRentDays] = useState({});
  const [modes, setModes] = useState({});

  useEffect(() => {
    const initRentDays = {};
    const initModes = {};

    cart.forEach((item) => {
      if (item.rentable) {
        initRentDays[item.id] = item.selectedDays || item.minRentDays || 7;
        initModes[item.id] = 'buy';
      } else {
        initModes[item.id] = 'buy';
      }
    });

    setRentDays(initRentDays);
    setModes(initModes);
  }, [cart]);

  const getBuyPrice = (item) => item.price ?? 0;

  const getRentPrice = (item) => {
    if (!item.rentable) return 0;
    const days = rentDays[item.id] || item.minRentDays || 7;
    return (item.rentPerDay || 0) * days;
  };

  const getItemSubtotal = (item) => {
    if (!item.rentable) {
      return getBuyPrice(item);
    }
    const mode = modes[item.id] || 'buy';
    if (mode === 'rent') return getRentPrice(item);
    return getBuyPrice(item);
  };

  const finalTotalNumber = cart.reduce(
    (sum, item) => sum + getItemSubtotal(item),
    0
  );
  const finalTotal = finalTotalNumber.toLocaleString('en-IN');

  // 🔥 ADD: Proceed button handler (uses existing final total)
// const handleProceedToPayment = () => {
//   navigate("/payment", {
//     state: {
//       finalAmount: finalTotalNumber, // ✅ SAME value shown in Order Summary
//     },
//   });
// };


const handleProceedToPayment = () => {
  navigate("/payment", {
    state: {
      finalAmount: finalTotalNumber, // ✅ SAME value shown in Order Summary
      rentPayload: cart.map((item) => ({
        productId: item.id,
        mode: modes[item.id] || "buy",
        rentDays:
          modes[item.id] === "rent"
            ? rentDays[item.id]
            : null,
      })),
    },
  });
};


  const increaseDays = (id, minDays) => {
    setRentDays((prev) => {
      const base = prev[id] || minDays || 7;
      return { ...prev, [id]: base + 1 };
    });
  };

  const decreaseDays = (id, minDays) => {
    setRentDays((prev) => {
      const base = prev[id] || minDays || 7;
      const updated = Math.max(minDays || 7, base - 1);
      return { ...prev, [id]: updated };
    });
  };

  return (
    <div className="max-w-5xl mx-auto animate-in fade-in zoom-in-95 duration-500 pb-20 px-4 text-slate-900">
      <BackButton onClick={() => navigate(-1)} label="Back to List" />

      <div className="text-center mb-12">
        <p className="text-[11px] font-semibold tracking-[0.25em] text-slate-400 uppercase mb-2">
          Checkout Options
        </p>
        <h2 className="text-3xl font-serif font-bold mb-2 italic">
          The Moment of Choice
        </h2>
        <p className="text-slate-500 font-serif italic text-base">
          Decide which books become yours forever and which you’ll borrow for a while.
        </p>
      </div>

      {/* MAIN CARDS */}
      <div
        className={`grid gap-8 ${
          isRentable ? 'md:grid-cols-2' : 'md:grid-cols-1 justify-center'
        }`}
      >
        {/* UPDATED: Permanent Collection card styled, informational only */}
        <div className="relative bg-white rounded-[2.2rem] shadow-md border border-slate-100 px-8 py-9 flex flex-col items-center text-center">
          <div className="absolute top-6 left-8 text-[10px] font-semibold tracking-[0.25em] text-slate-400 uppercase">
            Buy
          </div>

          <div className="w-14 h-14 bg-amber-50 rounded-2xl flex items-center justify-center text-[#C5A059] mb-6 shadow-inner">
            <BookOpen size={28} />
          </div>

          <h3 className="text-xl font-serif font-bold mb-3">
            Permanent Collection
          </h3>

          <p className="text-slate-500 text-sm mb-8 italic font-serif max-w-xs">
            Own these treasures forever as part of your sanctuary.
          </p>

          <div className="w-full mt-auto pt-6 border-t border-slate-100 text-center">
            <p className="text-[10px] text-slate-400 font-bold uppercase tracking-[0.25em]">
              All items as buy
            </p>
            <p className="mt-1 text-sm font-sans font-semibold text-slate-700">
              ₹{calculateTotal()}
            </p>
          </div>
        </div>

        {/* UPDATED: Borrowed Journey card styled, keeps rent controls, no payment button */}
        {isRentable && (
          <div className="relative bg-white rounded-[2.2rem] shadow-md border border-slate-100 px-8 py-9 flex flex-col text-left">
            <div className="absolute top-6 left-8 text-[10px] font-semibold tracking-[0.25em] text-slate-400 uppercase">
              Rent
            </div>

            <div className="flex items-center gap-3 mb-6 justify-center">
              <div className="w-12 h-12 bg-blue-50 rounded-2xl flex items-center justify-center text-blue-500 shadow-inner">
                <Clock size={24} />
              </div>
              <div className="text-center">
                <h3 className="text-xl font-serif font-bold mb-1">
                  Borrowed Journey
                </h3>
                <p className="text-slate-500 text-xs italic font-serif">
                  For rentable books, switch to Rent and tune your days.
                </p>
              </div>
            </div>

            {/* RENTABLE ITEMS */}
            <div className="w-full space-y-4">
              {cart
                .filter((item) => item.rentable)
                .map((item) => {
                  const mode = modes[item.id] || 'buy';
                  const days = rentDays[item.id] || item.minRentDays || 7;
                  const buyPrice = getBuyPrice(item);
                  const rentPrice = getRentPrice(item);

                  return (
                    <div
                      key={item.id}
                      className="w-full text-sm text-slate-700 font-serif border border-slate-100 rounded-2xl px-4 py-3 bg-slate-50/60"
                    >
                      <div className="flex justify-between items-center mb-2">
                        <div>
                          <p className="font-semibold">{item.name}</p>
                          <p className="text-[11px] text-slate-400">
                            Rent: ₹{item.rentPerDay}/day
                          </p>
                        </div>

                        {/* Buy / Rent pill toggles */}
                        <div className="inline-flex rounded-full bg-slate-100 p-1">
                          <button
                            type="button"
                            onClick={() =>
                              setModes((prev) => ({
                                ...prev,
                                [item.id]: 'buy',
                              }))
                            }
                            className={`px-3 py-1 rounded-full text-[11px] font-semibold transition-colors ${
                              mode === 'buy'
                                ? 'bg-slate-900 text-white'
                                : 'text-slate-600'
                            }`}
                          >
                            Buy
                          </button>
                          <button
                            type="button"
                            onClick={() =>
                              setModes((prev) => ({
                                ...prev,
                                [item.id]: 'rent',
                              }))
                            }
                            className={`px-3 py-1 rounded-full text-[11px] font-semibold transition-colors ${
                              mode === 'rent'
                                ? 'bg-indigo-500 text-white'
                                : 'text-slate-600'
                            }`}
                          >
                            Rent
                          </button>
                        </div>
                      </div>

                      {mode === 'rent' && (
                        <div className="flex justify-between items-center mt-1">
                          <div className="flex items-center gap-3">
                            <button
                              type="button"
                              onClick={() =>
                                decreaseDays(item.id, item.minRentDays)
                              }
                              className="w-8 h-8 flex items-center justify-center border border-slate-200 rounded-full text-slate-600 hover:bg-white hover:border-slate-300 transition-colors"
                            >
                              −
                            </button>

                            <span className="font-bold">{days} days</span>

                            <button
                              type="button"
                              onClick={() =>
                                increaseDays(item.id, item.minRentDays)
                              }
                              className="w-8 h-8 flex items-center justify-center border border-slate-200 rounded-full text-slate-600 hover:bg-white hover:border-slate-300 transition-colors"
                            >
                              +
                            </button>
                          </div>

                          <span className="font-semibold">
                            ₹{rentPrice.toLocaleString('en-IN')}
                          </span>
                        </div>
                      )}

                      {mode === 'buy' && (
                        <div className="mt-1 text-xs text-slate-500 flex justify-between">
                          <span>Buy price</span>
                          <span>₹{buyPrice.toLocaleString('en-IN')}</span>
                        </div>
                      )}
                    </div>
                  );
                })}
            </div>

            <div className="w-full mt-5 pt-4 border-t border-slate-100 text-xs text-slate-500 font-serif">
              <span className="uppercase tracking-[0.2em] text-slate-400 font-bold">
                All rentable as rent:
              </span>{' '}
              ₹{calculateRentalTotal()}
            </div>
          </div>
        )}
      </div>

            {/* UPDATED: Order Summary remains the single source of truth for final total */}
      <div className="mt-10 bg-white rounded-[2.2rem] shadow-md border border-slate-100 p-6 md:p-7 max-w-4xl mx-auto">
        <p className="text-[11px] font-semibold tracking-[0.25em] text-slate-400 uppercase mb-4">
          Order Summary
        </p>

        <div className="space-y-2 text-sm font-serif">
          {cart.map((item) => {
            const mode = item.rentable ? modes[item.id] || 'buy' : 'buy';
            const subtotal = getItemSubtotal(item);
            const days = rentDays[item.id] || item.minRentDays || 7;

            return (
              <div key={item.id} className="flex justify-between">
                <span>
                  {item.name}{' '}
                  <span className="text-[11px] text-slate-400">
                    {!item.rentable
                      ? '(Buy only)'
                      : mode === 'buy'
                      ? '(Buy)'
                      : `(Rent, ${days} days)`}
                  </span>
                </span>
                <span className="font-semibold">
                  ₹{subtotal.toLocaleString('en-IN')}
                </span>
              </div>
            );
          })}
        </div>

        <div className="mt-5 pt-4 border-t border-slate-100 flex justify-between items-center">
          <span className="text-[11px] uppercase tracking-[0.25em] text-slate-400 font-bold">
            Final Total
          </span>
          <span className="text-xl font-sans font-black text-slate-900">
            ₹{finalTotal}
          </span>
        </div>
      </div>

      {/* 🔥 FINAL PROCEED BUTTON — CORRECT PLACEMENT */}
      <div className="mt-12 flex justify-center">
        <button
          onClick={handleProceedToPayment}
          className="px-12 py-4 rounded-full bg-slate-900 text-white
                     font-semibold tracking-wide shadow-lg
                     hover:bg-slate-800 transition-all
                     active:scale-95"
        >
          Proceed to Payment • ₹{finalTotal}
        </button>
      </div>
    </div>
  );
};