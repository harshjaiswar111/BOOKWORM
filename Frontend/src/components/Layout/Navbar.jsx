import React, { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { ShoppingBag, User } from "lucide-react";

/**
 * Navbar Component
 * @param {number} cartCount - Number of items in cart
 * @param {boolean} isLoggedIn - User login status
 * @param {function} setIsLoggedIn - Setter for login state
 * @param {object} user - Logged-in user object
 */
const Navbar = ({
  cartCount = 0,
  isLoggedIn = false,
  setIsLoggedIn = () => { },
  user = null,
}) => {
  const navigate = useNavigate();
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [scrolled, setScrolled] = useState(false);
  const dropdownRef = useRef(null);

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setIsDropdownOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  // Scroll detection for sticky effect
  useEffect(() => {
    const handleScroll = () => {
      if (window.scrollY > 20) {
        setScrolled(true);
      } else {
        setScrolled(false);
      }
    };

    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const authType = sessionStorage.getItem("authType");

  const displayName =
    authType === "NORMAL" && user?.user_name ? user.user_name : "User";

  const handleLogout = () => {
    sessionStorage.clear();
    setIsLoggedIn(false);
    navigate("/login");
  };

  return (
    <>
      {/* Spacer to prevent content jump if we made it fixed, but for now we'll keep it absolute/fixed over hero or sticky */}
      <header
        className={`sticky top-0 left-0 right-0 z-50 transition-all duration-300 ease-in-out px-4 md:px-8
          ${scrolled
            ? "py-3 bg-white/90 backdrop-blur-md shadow-md"
            : "py-6 bg-[#FDFCFB]"
          }`}
      >
        <div className={`max-w-7xl mx-auto flex justify-between items-center rounded-2xl transition-all duration-300
           ${scrolled ? "px-0" : "bg-white/70 backdrop-blur-sm p-4 px-6 border border-white/40 shadow-lg"}`}
        >
          {/* Brand Logo */}
          <div
            onClick={() => navigate("/")}
            className="text-2xl sm:text-3xl font-serif font-black tracking-tight cursor-pointer hover:text-[#C5A059] transition-colors"
          >
            BOOKWORM<span className="text-[#C5A059]">.</span>
          </div>

          {/* Navigation Links */}
          <nav className="hidden md:flex gap-16 items-center">
            {["Library", "Products", "Shelf", "My Library"].map((item) => (
              <button
                key={item}
                onClick={() => {
                  const path = item === "Shelf" ? "/shelf" : item === "My Library" ? "/my-library" : `/${item.toLowerCase()}`;
                  navigate(path, { state: { reset: Date.now() } });
                }}
                className="relative group text-sm uppercase font-bold tracking-widest text-slate-600 hover:text-[#C5A059] transition-colors py-2"
              >
                {item}
                <span className="absolute bottom-0 left-0 w-0 h-0.5 bg-[#C5A059] transition-all duration-300 group-hover:w-full" />
              </button>
            ))}
          </nav>

          {/* Auth Section */}
          <div className="hidden md:flex items-center gap-6">
            {!isLoggedIn ? (
              <button
                onClick={() => navigate("/login")}
                className="px-6 py-2 rounded-full border border-slate-300 text-xs uppercase font-bold tracking-widest text-slate-600 hover:border-[#C5A059] hover:text-[#C5A059] hover:bg-[#C5A059]/5 transition-all"
              >
                Login
              </button>
            ) : (
              <div className="flex items-center gap-4">
                {/* Greeting and Logout removed - moved to dropdown */}
              </div>
            )}
          </div>

          <div className="flex items-center gap-4 ml-6">
            {/* Cart Icon */}
            <div
              onClick={() => navigate("/cart")}
              className="relative w-10 h-10 md:w-12 md:h-12 bg-white shadow-sm rounded-xl flex items-center justify-center cursor-pointer border border-slate-100 hover:shadow-md hover:scale-105 active:scale-95 transition-all"
            >
              <ShoppingBag size={20} className="text-slate-700" />
              {cartCount > 0 && (
                <span className="absolute -top-2 -right-2 bg-[#C5A059] text-white w-5 h-5 rounded-full text-[10px] font-bold flex items-center justify-center animate-bounce shadow-sm">
                  {cartCount}
                </span>
              )}
            </div>

            {/* Profile Dropdown */}
            <div className="relative" ref={dropdownRef}>
              <div
                onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                className="w-10 h-10 md:w-12 md:h-12 bg-[#F8F9FA] rounded-full flex items-center justify-center cursor-pointer border border-slate-200 hover:bg-slate-100 hover:scale-105 active:scale-95 transition-all"
              >
                <User size={20} className="text-slate-700" />
              </div>

              {isDropdownOpen && (
                <div className="absolute right-0 top-14 w-48 bg-white rounded-xl shadow-xl border border-slate-100 overflow-hidden py-2 z-50 animate-in fade-in zoom-in-95 duration-200">
                  <button
                    onClick={() => {
                      navigate("/orders");
                      setIsDropdownOpen(false);
                    }}
                    className="w-full text-left px-4 py-3 text-sm font-serif font-bold text-slate-600 hover:bg-[#F8F9FA] hover:text-[#C5A059] transition-colors"
                  >
                    My Orders
                  </button>

                  <button
                    onClick={handleLogout}
                    className="w-full text-left px-4 py-3 text-sm font-serif font-bold text-slate-600 hover:bg-red-50 hover:text-red-500 transition-colors border-t border-slate-50"
                  >
                    Logout
                  </button>
                </div>
              )}
            </div>
          </div>
        </div>
      </header>
    </>
  );
};

export default Navbar;
