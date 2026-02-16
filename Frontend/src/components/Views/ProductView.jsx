import React, { useState, useEffect } from "react";
import { BookCard } from "../Library/BookCard";
import { ProductModal } from "../Library/ProductModal";
import { useLocation } from "react-router-dom";


export const ProductView = ({ onAddToCart }) => {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedBook, setSelectedBook] = useState(null);
  const [searchText, setSearchText] = useState("");
  const location = useLocation();
  const [generes, setGeneres] = useState([]);
  const [languages, setLanguages] = useState([]); // Added languages state
  const [selectedGenere, setSelectedGenere] = useState("");
  const [selectedLanguage, setSelectedLanguage] = useState("");
  const [ownedBookIds, setOwnedBookIds] = useState(new Set());

  useEffect(() => {
    fetchAllProducts();
    fetchAllGeneres();
    fetchAllLanguages(); // Fetch languages
    fetchUserLibrary();
  }, []);

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
        const ids = new Set(data.map((item) => Number(item.product.productId)));
        setOwnedBookIds(ids);
      })
      .catch((err) => console.error("Failed to fetch library", err));
  };

  const fetchFilteredProducts = (genere, language) => {
    setBooks([]);
    setLoading(true);

    const params = new URLSearchParams();
    if (genere) params.append("genere", genere);
    if (language) params.append("language", language);

    fetch(`http://localhost:8080/api/products/filter?${params.toString()}`)
      .then((res) => res.json())
      .then((data) => {
        setBooks(Array.isArray(data) ? data : []);
        setLoading(false);
      })
      .catch(() => {
        setBooks([]);
        setLoading(false);
      });
  };

  const fetchAllGeneres = () => {
    fetch("http://localhost:8080/api/generes")
      .then((res) => res.json())
      .then((data) => setGeneres(data))
      .catch((err) => console.error("Failed to load generes", err));
  };

  const fetchAllLanguages = () => {
    fetch("http://localhost:8080/api/languages")
      .then((res) => res.json())
      .then((data) => setLanguages(data))
      .catch((err) => console.error("Failed to load languages", err));
  };

  const fetchAllProducts = () => {
    setLoading(true);
    fetch("http://localhost:8080/api/products")
      .then((res) => res.json())
      .then((data) => {
        setBooks(data);
        setLoading(false);
      })
      .catch(() => setLoading(false));
  };

  useEffect(() => {
    if (location.state?.reset) {
      setSearchText("");
      setSelectedBook(null);
      fetchAllProducts();
      window.history.replaceState({}, document.title);
    }
  }, [location.state]);

  const handleSearch = () => {
    const query = searchText.trim();

    if (query.length === 0) {
      fetchAllProducts();
      return;
    }

    setLoading(true);
    fetch(
      `http://localhost:8080/api/products/search?name=${encodeURIComponent(
        query,
      )}`,
    )
      .then((res) => res.json())
      .then((data) => {
        setBooks(data);
        setLoading(false);
      })
      .catch(() => setLoading(false));
  };

  const handleGenereClick = (genere) => {
    setSelectedGenere(genere);
    setSearchText("");
    fetchFilteredProducts(genere, selectedLanguage);
  };

  const handleLanguageClick = (language) => {
    setSelectedLanguage(language);
    setSearchText("");
    fetchFilteredProducts(selectedGenere, language);
  };

  if (loading && books.length === 0 && searchText === "") {
    // Initial load only
    //return <p className="text-center mt-20 text-slate-500">Loading books...</p>;
  }

  // Sidebar Button Component
  const FilterButton = ({ label, active, onClick }) => (
    <button
      onClick={onClick}
      className={`w-full text-left px-4 py-3 rounded-xl transition-all duration-300 font-serif mb-2 text-sm
        ${active
          ? "bg-[#C5A059] text-black font-bold shadow-lg transform scale-105"
          : "text-slate-300 hover:bg-white/10 hover:text-white"
        }`}
    >
      {label}
    </button>
  );

  return (
    <>
      <div className="flex min-h-screen bg-[#FDFCFB]">

        {/* 🌑 SIDEBAR FILTERS (Admin Style - Fixed Left) */}
        <aside className="w-72 bg-[#1C1B1A] text-white p-6 flex flex-col fixed inset-y-0 left-0 z-50 overflow-y-auto shadow-2xl">

          {/* Brand / Logo Area (Optional, mimics Admin) */}
          <div className="mb-10">
            <h1 className="text-2xl font-serif font-bold tracking-wider">
              BOOKWORM<span className="text-[#C5A059]">.</span>
            </h1>
          </div>

          {/* Genre Section */}
          <div className="mb-8">
            <h3 className="text-[#C5A059] font-bold text-sm uppercase tracking-widest mb-4 border-b border-white/10 pb-2">
              Genres
            </h3>
            <div className="space-y-1">
              <FilterButton
                label="All Genres"
                active={selectedGenere === ""}
                onClick={() => handleGenereClick("")}
              />
              {generes.map((g) => (
                <FilterButton
                  key={g.genereId}
                  label={g.genereDesc}
                  active={selectedGenere === g.genereDesc}
                  onClick={() => handleGenereClick(g.genereDesc)}
                />
              ))}
            </div>
          </div>

          {/* Language Section */}
          <div>
            <h3 className="text-[#C5A059] font-bold text-sm uppercase tracking-widest mb-4 border-b border-white/10 pb-2">
              Languages
            </h3>
            <div className="space-y-1">
              <FilterButton
                label="All Languages"
                active={selectedLanguage === ""}
                onClick={() => handleLanguageClick("")}
              />
              {languages.map((l) => (
                <FilterButton
                  key={l.languageId}
                  label={l.languageDesc}
                  active={selectedLanguage === l.languageDesc}
                  onClick={() => handleLanguageClick(l.languageDesc)}
                />
              ))}
            </div>
          </div>
        </aside>

        {/* 📚 MAIN CONTENT (Offset by Sidebar Width) */}
        <main className="flex-1 min-h-screen transition-all duration-300 bg-[#FDFCFB]">

          {/* 
              Container matches Navbar's max-w-7xl
              pl-80 ensures content clears the fixed sidebar (288px) + gap
              pr-8 matches navbar padding
          */}
          <div className="max-w-7xl mx-auto px-8 py-12">
            <div className="mb-10 flex flex-col md:flex-row md:items-end justify-between gap-4">
              <div>
                <h1 className="text-4xl font-serif font-bold text-slate-900 mb-2 italic">
                  The Stacks
                </h1>
                <p className="text-slate-500 font-serif text-lg">
                  Discover your next obsession.
                </p>
              </div>

              {/* Search Bar */}
              <div className="flex gap-4 w-full md:w-auto md:min-w-[400px]">
                <input
                  type="text"
                  placeholder="Search by book name, author, ISBN..."
                  value={searchText}
                  onChange={(e) => setSearchText(e.target.value)}
                  className="flex-1 px-6 py-3 rounded-xl border-2 border-slate-200 
                  focus:outline-none focus:border-[#C5A059] focus:ring-0
                  font-serif text-slate-700 shadow-sm transition-all bg-white"
                />
                <button
                  onClick={handleSearch}
                  className="px-8 py-3 rounded-xl bg-black text-white font-serif font-bold tracking-wide
                            hover:bg-[#C5A059] hover:text-white transition-all shadow-lg hover:shadow-xl active:scale-95"
                >
                  Search
                </button>
              </div>
            </div>

            {/* Product Grid - Strictly 4 columns on large screens */}
            {loading ? (
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-10 opacity-50 pointer-events-none">
                {[1, 2, 3, 4, 5, 6, 7, 8].map(i => (
                  <div key={i} className="aspect-[2/3] bg-slate-200 rounded-2xl animate-pulse"></div>
                ))}
              </div>
            ) : books.length === 0 ? (
              <div className="text-center py-24 bg-white rounded-3xl border border-slate-100 shadow-sm">
                <p className="text-3xl font-serif font-semibold text-slate-400 mb-4">
                  No books found
                </p>
                <div className="flex gap-2 justify-center">
                  {(selectedGenere || selectedLanguage || searchText) && (
                    <button
                      onClick={() => {
                        setSelectedGenere("");
                        setSelectedLanguage("");
                        setSearchText("");
                        fetchAllProducts();
                      }}
                      className="text-[#C5A059] font-bold hover:underline"
                    >
                      Clear all filters
                    </button>
                  )}
                </div>
              </div>
            ) : (
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-10">
                {books.map((b) => (
                  <BookCard
                    key={b.productId}
                    book={b}
                    onAddToCart={onAddToCart}
                    onOpen={() => setSelectedBook(b)}
                    isOwned={ownedBookIds.has(Number(b.productId))}
                  />
                ))}
              </div>
            )}
          </div>
        </main>
      </div>

      {selectedBook && (
        <ProductModal
          book={selectedBook}
          onClose={() => setSelectedBook(null)}
        />
      )}
    </>
  );
};

/*export const LibraryView = ({ books, onAddToCart }) => (
  <div className="animate-in fade-in duration-700">
    <div className="mb-10">
      <h1 className="text-4xl font-serif font-bold text-slate-900 mb-2 italic">The Stacks</h1>
      <p className="text-slate-500 font-serif text-lg">Discover your next obsession.</p>
    </div>
    <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-8">
      {books.map((b) => (
        <BookCard key={b.id} book={b} onAddToCart={onAddToCart} />
      ))}
    </div>
  </div>
);*/
