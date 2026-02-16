import React, { useState, useEffect } from "react";
import { Package, BookOpen } from "lucide-react";
import { PackageCard } from "../Library/PackageCard";
import { BookCard } from "../Library/BookCard";

export const LibraryPackageView = () => {
  /* ================= STATE ================= */
  const [packages, setPackages] = useState([]);
  const [books, setBooks] = useState([]);
  const [viewMode, setViewMode] = useState("packages"); // "packages" | "books"
  const [loading, setLoading] = useState(true);
  const [ownedPackageIds, setOwnedPackageIds] = useState(new Set());

  useEffect(() => {
    fetchPackages();
    fetchBooks();
    fetchUserLibrary();
  }, []);

  const fetchPackages = () => {
    setLoading(true);
    fetch("http://localhost:8080/api/library-packages")
      .then((res) => res.json())
      .then((data) => {
        setPackages(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Failed to fetch packages", err);
        setLoading(false);
      });
  };

  const fetchBooks = () => {
    // Fetch books available for library preview
    fetch("http://localhost:8080/api/products/library")
      .then((res) => res.json())
      .then((data) => {
        setBooks(data);
      })
      .catch((err) => console.error("Failed to fetch library books", err));
  };

  const fetchUserLibrary = () => {
    const user = JSON.parse(sessionStorage.getItem("user"));
    if (!user?.userId) return;

    fetch(`http://localhost:8080/api/my-library/user/${user.userId}`, {
      headers: {
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      },
    })
      .then((res) => res.json())
      .then((data) => {
        const owned = new Set();
        data.forEach((item) => {
          // Robust package ID check
          const pkgId =
            item.packageId ||
            item.libraryPackage?.packageId ||
            item.product?.packageId ||
            item.product?.libraryPackage?.packageId;

          if (pkgId) {
            owned.add(Number(pkgId));
          }
        });
        setOwnedPackageIds(owned);
      })
      .catch((err) => console.error("Failed to fetch user library", err));
  };

  if (loading) {
    return (
      <p className="text-center mt-20 text-slate-500">
        Loading packages...
      </p>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-8 py-8">

      {/* HEADER + TOGGLE */}
      <div className="flex flex-col md:flex-row justify-between items-center mb-10 gap-6">
        <h1 className="text-4xl font-serif font-bold text-slate-900">
          {viewMode === "packages" ? "Select Packages" : "Library Preview"}
        </h1>

        <div className="flex bg-white p-2 rounded-full border border-slate-100 shadow-sm ring-1 ring-slate-100">
          <button
            onClick={() => setViewMode("packages")}
            className={`flex items-center gap-2 px-8 py-3 rounded-full text-sm font-bold transition-all duration-500 ease-out ${viewMode === "packages"
              ? "bg-[#C5A059] text-white shadow-lg shadow-orange-500/20 transform scale-105"
              : "text-slate-500 hover:text-slate-800 hover:bg-slate-50"
              }`}
          >
            <Package size={18} />
            Packages
          </button>
          <button
            onClick={() => setViewMode("books")}
            className={`flex items-center gap-2 px-8 py-3 rounded-full text-sm font-bold transition-all duration-500 ease-out ${viewMode === "books"
              ? "bg-[#C5A059] text-white shadow-lg shadow-orange-500/20 transform scale-105"
              : "text-slate-500 hover:text-slate-800 hover:bg-slate-50"
              }`}
          >
            <BookOpen size={18} />
            Preview Books
          </button>
        </div>
      </div>

      {/* CONTENT GRID */}
      {viewMode === "packages" ? (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-4 gap-12 justify-items-center">
          {packages.map((pkg) => (
            <PackageCard
              key={pkg.packageId}
              pkg={pkg}
              isOwned={ownedPackageIds.has(pkg.packageId)}
            />
          ))}
        </div>
      ) : (
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-12">
          {books.map((book) => (
            <BookCard
              key={book.productId}
              book={book}
              onOpen={() => { }} // No-op, just viewing
            // onAddToCart is intentionally OMITTED
            />
          ))}
        </div>
      )}
    </div>
  );
};
