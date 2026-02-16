import React from "react";
import { useNavigate } from "react-router-dom";
import { FALLBACK_IMAGE } from "../../data/libraryData";

export const PackageCard = ({ pkg, isOwned }) => {
  const navigate = useNavigate();

  const openLibrary = (pkg) => {
    navigate(`/library/${pkg.packageId}`);
  };

  return (
    <div className="w-52 bg-white rounded-xl shadow-md hover:shadow-xl transition p-4">

      {/* IMAGE */}
      <img
        src={FALLBACK_IMAGE}
        alt={pkg.packageName}
        className="h-52 w-full object-cover rounded-lg mb-4"
      />

      {/* PACKAGE NAME */}
      <h3 className="font-serif font-semibold text-slate-900 text-center">
        {pkg.packageName}
      </h3>

      {/* Book limit */}
      <p className="text-sm text-slate-500 text-center mt-1">
        validity {pkg.validityDays} days
      </p>

      {/* ACCESS COUNT */}
      <p className="text-sm text-slate-500 text-center mt-1">
        Access to {pkg.bookLimit} books
      </p>


      {/* PRICE */}
      <p className="text-center font-bold text-slate-800 mt-2">
        ₹{pkg.cost}
      </p>

      {/* ACTION */}
      <div className="flex justify-center mt-4">
        <button
          onClick={() => !isOwned && openLibrary(pkg)}
          disabled={isOwned}
          className={`px-4 py-2 rounded-lg text-white transition ${isOwned
            ? "bg-slate-400 cursor-not-allowed"
            : "bg-slate-900 hover:bg-slate-700"
            }`}
        >
          {isOwned ? "Already Owned" : "View Library"}
        </button>
      </div>
    </div>
  );
};
