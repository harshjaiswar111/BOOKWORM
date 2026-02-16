import { useEffect, useState } from "react";
import ReactDOM from "react-dom";
import { useNavigate } from "react-router-dom";
import { Library } from "lucide-react";
import { LibraryBookCard } from "../Library/LibraryBookCard";

export const MyLibrary = () => {
  const navigate = useNavigate();
  const [books, setBooks] = useState([]);
  const [pdfUrl, setPdfUrl] = useState(null);
  const [activeBook, setActiveBook] = useState(null);
  const [loading, setLoading] = useState(true);
  const [incompletePackages, setIncompletePackages] = useState([]);


  useEffect(() => {
    const user = JSON.parse(sessionStorage.getItem("user"));
    const userId = user?.userId;

    if (!userId) {
      setLoading(false);
      return;
    }
    setLoading(true);

    // 1. Fetch Packages (to know limits)
    // 2. Fetch User Library (to know what they have)
    Promise.all([
      fetch("http://localhost:8080/api/library-packages").then((res) =>
        res.json()
      ),
      fetch(`http://localhost:8080/api/my-library/user/${userId}`, {
        headers: {
          Authorization: `Bearer ${sessionStorage.getItem("token")}`,
        },
      }).then((res) => res.json()),
    ])
      .then(([packagesData, libraryData]) => {

        // A. Process Library Books
        const booksWithExpiry = libraryData.map((item) => {
          // Robust package ID search
          const foundPackageId =
            item.packageId ||
            item.libraryPackage?.packageId ||
            item.product?.packageId ||
            item.product?.libraryPackage?.packageId;

          return {
            ...item.product, // product fields
            startDate: item.startDate,
            endDate: item.endDate,
            packageId: foundPackageId ? Number(foundPackageId) : null,
          };
        });

        setBooks(booksWithExpiry);

        // B. Calculate Incomplete Packages
        // Map: packageId -> count of books owned
        const ownedCounts = {};
        libraryData.forEach((item) => {
          const foundPackageId =
            item.packageId ||
            item.libraryPackage?.packageId ||
            item.product?.packageId ||
            item.product?.libraryPackage?.packageId;

          if (foundPackageId) {
            const pid = Number(foundPackageId);
            ownedCounts[pid] = (ownedCounts[pid] || 0) + 1;
          }
        });

        const incomplete = [];
        // Check identifying valid packages user has interacted with
        // We only care about packages the user *has* books from, OR active subscriptions (if we had that data).
        // For now, we iterate over packages we found in the library.
        Object.keys(ownedCounts).forEach((pkgIdStr) => {
          const pkgId = Number(pkgIdStr);
          const pkgInfo = packagesData.find((p) => p.packageId === pkgId);
          const ownedCount = ownedCounts[pkgId];

          if (pkgInfo && ownedCount < pkgInfo.bookLimit) {
            incomplete.push({
              packageId: pkgId,
              packageName: pkgInfo.packageName,
              limit: pkgInfo.bookLimit,
              owned: ownedCount,
              remaining: pkgInfo.bookLimit - ownedCount,
            });
          }
        });

        setIncompletePackages(incomplete);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setLoading(false);
      });
  }, []);

  // ✅ SAME logic as ShelfView
  const openPdf = async (book) => {
    try {
      console.log("TOKEN USED:", sessionStorage.getItem("token"));
      const res = await fetch(
        `http://localhost:8080/api/my-library/read/${book.productId}`,
        {
          headers: {
            Authorization: `Bearer ${sessionStorage.getItem("token")}`,
          },
        }
      );

      const blob = await res.blob();
      const url = URL.createObjectURL(blob);

      setPdfUrl(url);
      setActiveBook(book.productName);
    } catch (err) {
      console.error("Failed to open PDF", err);
    }
  };

  return (
    <div className="max-w-7xl mx-auto px-8">
      <h1 className="text-3xl font-serif font-bold mb-10">My Library</h1>

      {/* ⚠️ INCOMPLETE PACKAGES NOTIFICATION */}
      {incompletePackages.length > 0 && (
        <div className="mb-10 grid gap-4">
          {incompletePackages.map((pkg) => (
            <div
              key={pkg.packageId}
              className="bg-slate-900 border border-[#C5A059]/30 rounded-2xl p-6 flex flex-col sm:flex-row justify-between items-center shadow-lg relative overflow-hidden"
            >
              <div className="absolute top-0 right-0 w-32 h-32 bg-[#C5A059]/10 rounded-full blur-2xl -translate-y-1/2 translate-x-1/2" />

              <div className="relative z-10 text-center sm:text-left">
                <h3 className="text-xl font-serif font-bold text-white mb-1">
                  Complete Your <span className="text-[#C5A059]">{pkg.packageName}</span> Collection
                </h3>
                <p className="text-slate-300 text-sm">
                  You can add <span className="text-[#C5A059] font-bold">{pkg.remaining}</span> more books to this package.
                </p>
              </div>
              <button
                onClick={() => navigate(`/library/${pkg.packageId}`)}
                className="mt-6 sm:mt-0 px-8 py-3 bg-[#C5A059] hover:bg-[#b08d4b] text-white font-bold text-sm rounded-full transition-all shadow-lg hover:shadow-[#C5A059]/40 hover:-translate-y-0.5"
              >
                SELECT BOOKS
              </button>
            </div>
          ))}
        </div>
      )}

      {/* 📄 PDF VIEWER (Portal) */}
      {pdfUrl && ReactDOM.createPortal(
        <div className="fixed inset-0 bg-black/90 z-[9999] flex flex-col">
          <div className="flex items-center justify-between bg-slate-900 text-white px-6 py-3 shadow-md">
            <h2 className="font-serif text-lg truncate">{activeBook}</h2>
            <button
              onClick={() => {
                URL.revokeObjectURL(pdfUrl);
                setPdfUrl(null);
                setActiveBook(null);
              }}
              className="text-sm font-bold hover:text-red-400 transition-colors"
            >
              Close
            </button>
          </div>

          <iframe src={pdfUrl} title="PDF Reader" className="flex-1 w-full" />
        </div>,
        document.body
      )}

      {/* 📖 BOOKS GRID */}
      {!loading && books.length === 0 ? (
        <div className="text-center py-24 bg-white/40 rounded-3xl border-2 border-dashed border-slate-100">
          <Library size={48} className="mx-auto text-slate-200 mb-4" />
          <p className="text-lg text-slate-400 font-serif italic mb-6">
            Your sanctuary is silent.
          </p>
          <button
            onClick={() => navigate("/library")}
            className="bg-slate-900 text-white px-8 py-3 rounded-full text-xs font-bold uppercase tracking-widest hover:bg-black transition-all"
          >
            Build Collection
          </button>
        </div>
      ) : (
        <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-8">
          {books.map((book, index) => (
            <LibraryBookCard
              key={index}
              book={book}
              onOpen={() => openPdf(book)}
              isSelected={false}
            />
          ))}
        </div>
      )}
    </div>
  );
};
