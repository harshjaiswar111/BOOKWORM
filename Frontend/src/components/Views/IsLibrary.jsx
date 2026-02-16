import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { LibraryBookCard } from "../Library/LibraryBookCard";
import { useNavigate } from "react-router-dom";


export const LibraryView = () => {
  const { packageId } = useParams();
  const navigate = useNavigate();

  /* ================================================================================================================================================================== */
  /* ================================================================================================================================================================== */

  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [bookLimit, setBookLimit] = useState(0);
  const [packageDetails, setPackageDetails] = useState(null);
  const [selectedBooks, setSelectedBooks] = useState([]);
  const [ownedBookIds, setOwnedBookIds] = useState(new Set());
  const [ownedBookMap, setOwnedBookMap] = useState(new Map());

  // New State for package tracking
  const [ownedCountForPackage, setOwnedCountForPackage] = useState(0);

  // handle selected books
  const handleBookSelect = (book) => {
    const bookId = Number(book.productId);

    // 🔒 Already owned
    if (ownedBookMap.has(bookId)) {
      const source = ownedBookMap.get(bookId);

      if (source === "SHELF") {
        alert("📚 This book is already present in your Shelf");
      } else if (source === "LIBRARY") {
        alert("📦 This book is already present in your Library");
      }
      return;
    }

    // 🔁 Toggle selection
    const alreadySelected = selectedBooks.some(
      (b) => Number(b.productId) === bookId,
    );

    if (alreadySelected) {
      setSelectedBooks((prev) =>
        prev.filter((b) => Number(b.productId) !== bookId),
      );
      return;
    }

    // 🚫 Limit reached
    // MODIFIED: Check total (selected + already owned in this package)
    if ((selectedBooks.length + ownedCountForPackage) >= bookLimit) {
      alert(`Book limit reached for this package. You already own ${ownedCountForPackage} books.`);
      return;
    }

    setSelectedBooks((prev) => [...prev, book]);
  };

  const handlePurchase = () => {
    navigate("/confirm-buy", {
      state: {
        packageId,
        selectedBooks,
        cost: packageDetails?.cost,
        isTopUp: ownedCountForPackage > 0,
      },
    });
  };

  // fetching shelf books
  useEffect(() => {
    const user = JSON.parse(sessionStorage.getItem("user"));
    const userId = user?.userId;
    if (!userId) return;

    fetch(`http://localhost:8080/api/shelf/${userId}`, {
      headers: {
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      },
    })
      .then((res) => res.json())
      .then((data) => {
        setOwnedBookMap((prev) => {
          const map = new Map(prev);
          data.forEach((item) => {
            map.set(Number(item.product.productId), "SHELF");
          });
          return map;
        });
      });
  }, []);

  // fetching my_library books
  useEffect(() => {
    const user = JSON.parse(sessionStorage.getItem("user"));
    const userId = user?.userId;
    if (!userId) return;

    fetch(`http://localhost:8080/api/my-library/user/${userId}`, {
      headers: {
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      },
    })
      .then((res) => res.json())
      .then((data) => {
        let count = 0;
        const libraryItemsMap = new Map();

        data.forEach((item) => {
          libraryItemsMap.set(Number(item.product.productId), "LIBRARY");

          // Check if this book belongs to the current package
          const pkgId =
            item.packageId ||
            item.libraryPackage?.packageId ||
            item.product?.packageId ||
            item.product?.libraryPackage?.packageId;

          if (pkgId && Number(pkgId) === Number(packageId)) {
            count++;
          }
        });

        setOwnedBookMap((prev) => {
          const map = new Map(prev);
          libraryItemsMap.forEach((val, key) => {
            map.set(key, val);
          });
          return map;
        });

        setOwnedCountForPackage(count);
      });
  }, [packageId]); // Re-run if packageId changes

  useEffect(() => {
    if (!packageId) return;

    fetch(`http://localhost:8080/api/library-packages/${packageId}`)
      .then((res) => res.json())
      .then((data) => {
        setBookLimit(data.bookLimit);
        setPackageDetails(data);
      })
      .catch((err) => {
        console.error("Failed to load package", err);
      });
  }, [packageId]);

  useEffect(() => {
    setLoading(true);
    fetch(`http://localhost:8080/api/products/library`)
      .then((res) => res.json())
      .then((data) => {
        setBooks(data);
        setLoading(false);
      })
      .catch(() => setLoading(false));
  }, [packageId]);

  // for olready present books in Shelf
  useEffect(() => {
    const user = JSON.parse(sessionStorage.getItem("user"));
    const userId = user?.userId;

    if (!userId) return;

    fetch(`http://localhost:8080/api/my-library/user/${userId}`, {
      headers: {
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      },
    })
      .then((res) => res.json())
      .then((data) => {
        // data = [{ product: {...} }, ...]
        const ids = new Set(data.map((item) => item.product.productId));
        setOwnedBookIds(ids);
      })
      .catch((err) => {
        console.error("Failed to load owned books", err);
      });
  }, []);

  if (loading) {
    return (
      <p className="text-center mt-20 text-slate-500">
        Loading your library...
      </p>
    );
  }

  // console.log("packageId:", packageId);

  return (
    <div className="max-w-7xl mx-auto px-8 py-10">
      <h1 className="text-4xl font-serif font-bold text-slate-900 mb-8">
        Browse Library Collection
      </h1>
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-8">
        {books.map((book) => (
          <LibraryBookCard
            key={book.productId}
            book={book}
            onOpen={() => handleBookSelect(book)}
            isSelected={selectedBooks.some(
              (b) => Number(b.productId) === Number(book.productId),
            )}
            isDisabled={
              ownedBookMap.has(Number(book.productId)) ||
              ((selectedBooks.length + ownedCountForPackage) >= bookLimit &&
                !selectedBooks.some(
                  (b) => Number(b.productId) === Number(book.productId),
                ))
            }
          />
        ))}
      </div>

      {selectedBooks.length > 0 && (
        <div className="mt-12 bg-slate-900 border border-[#C5A059]/30 rounded-2xl shadow-xl px-8 py-6 relative overflow-hidden">
          {/* Background Glow */}
          <div className="absolute top-0 right-0 w-64 h-64 bg-[#C5A059]/10 rounded-full blur-3xl -translate-y-1/2 translate-x-1/2 pointer-events-none" />

          <div className="flex flex-wrap gap-6 justify-between items-center relative z-10">
            {/* Count */}
            <div>
              <p className="font-serif text-xl text-white">
                Selected Books:{" "}
                <span className="font-bold text-[#C5A059]">
                  {selectedBooks.length}
                </span>
                {" / "}
                <span className="text-slate-500">
                  {Math.max(0, bookLimit - ownedCountForPackage)}
                </span>
              </p>
              <p className="text-sm text-slate-400 mt-1">
                You already own <span className="text-white font-medium">{ownedCountForPackage}</span> books from this package.
              </p>
            </div>

            {/* Actions */}
            <div className="flex gap-4 items-center">
              <button
                onClick={() => setSelectedBooks([])}
                className="px-6 py-2.5 rounded-full border border-slate-700 text-slate-300 hover:bg-slate-800 hover:text-white transition font-medium text-sm"
              >
                Clear Selection
              </button>

              <button
                onClick={handlePurchase}
                className="px-8 py-3 rounded-full bg-[#C5A059] text-white font-bold hover:bg-[#b08d4b] shadow-lg hover:shadow-[#C5A059]/40 transition transform hover:-translate-y-0.5 flex items-center gap-2"
              >
                {ownedCountForPackage > 0 ? "ADD TO LIBRARY" : "PURCHASE PACKAGE"}
              </button>
            </div>
          </div>
        </div>
      )}

    </div>
  );
};
