import { Outlet, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

export default function Layout() {
  const navigate = useNavigate();
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  // 🔑 Page load pe token check
  useEffect(() => {
    const token = localStorage.getItem("token");
    setIsLoggedIn(!!token);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    setIsLoggedIn(false);
    navigate("/");
  };

  return (
    <>
      {/* HEADER */}
      <header className="bg-white shadow-md px-10 py-4 flex justify-between items-center">
        <h1
          className="text-2xl font-bold cursor-pointer"
          onClick={() => navigate("/")}
        >
          BOOKWORM.
        </h1>

        <nav className="flex gap-6 items-center text-sm font-semibold">
          <span
            className="cursor-pointer hover:text-blue-600"
            onClick={() => navigate("/library")}
          >
            LIBRARY
          </span>

          <span
            className="cursor-pointer hover:text-blue-600"
            onClick={() => navigate("/shelf")}
          >
            MY SHELF
          </span>

          {!isLoggedIn ? (
            <button
              onClick={() => navigate("/login")}
              className="hover:text-blue-600"
            >
              LOGIN
            </button>
          ) : (
            <button
              onClick={handleLogout}
              className="text-red-600 hover:underline"
            >
              LOGOUT
            </button>
          )}
        </nav>
      </header>

      {/* PAGE CONTENT */}
      <main className="min-h-[80vh] ">
        <Outlet />
      </main>

      {/* FOOTER (optional) */}
      <footer className="text-center py-6 text-sm text-gray-500 border-t">
        © 2026 BOOKWORM DIGITAL SANCTUARY
      </footer>
    </>
  );
}
