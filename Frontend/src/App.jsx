import React, { useState, useEffect } from "react";
import { Routes, Route, useNavigate } from "react-router-dom";

/* ================= ADMIN ================= */
import AdminDashboard from "./admin/AdminDashboard";

/* ================= LAYOUT ================= */
import Navbar from "./components/Layout/Navbar";
import { OwnershipModal } from "./components/UI/Shared";

/* ================= VIEWS ================= */
import { HomeView } from "./components/Views/HomeView";
import { ProductView } from "./components/Views/ProductView";
import { PaymentView } from "./components/Views/PaymentView";
import { SuccessView } from "./components/Views/SuccessView";
import { NotFoundView } from "./components/Views/NotFoundView";

/* ================= CART / LIBRARY ================= */
import { CartView } from "./components/Cart/CartView";
import { TransactionSelector } from "./components/Cart/TransactionSelector";
import { ShelfView } from "./components/Library/ShelfView";
import { LibraryView } from "./components/Views/IsLibrary";

/* ================= AUTH ================= */
import Login from "./components/Auth/Login";
import Register from "./components/Auth/Register";
import LoginSuccess from "./components/Auth/LoginSuccess";
import ProtectedRoute from "./components/Auth/ProtectedRoute";

/* ================= DATA & SERVICES ================= */
import { LIBRARY_BOOKS } from "./data/libraryData";
import { addToCartAPI, getCartAPI } from "./services/cartservice";
import { getShelfAPI } from "./services/shelfService";
import { checkoutAPI } from "./services/checkoutService";

/* ================= ORDER HISTORY ================= */

import { OrdersView } from "./components/Views/OrdersView";
import { LibraryPackageView } from "./components/Views/LibraryPackageView";
import { ConfirmBuy } from "./components/Library/ConfirmBuy";
import { MyLibrary } from "./components/Library/MyLibrary";
import Footer from "./components/Layout/Footer";


export default function App() {
  const navigate = useNavigate();

  /* ================= STATE ================= */
  const [cart, setCart] = useState([]);
  const [shelf, setShelf] = useState([]);
  const [ownershipWarning, setOwnershipWarning] = useState(null);
  const [lastPurchaseType, setLastPurchaseType] = useState("buy");
  const [upiId, setUpiId] = useState("");
  const [cartMessage, setCartMessage] = useState(null);

  const [isLoggedIn, setIsLoggedIn] = useState(
    !!sessionStorage.getItem("token")
  );

  const [user, setUser] = useState(() => {
    const stored = sessionStorage.getItem("user");
    return stored ? JSON.parse(stored) : null;
  });

  /* ===== SESSION RESTORE (ORIGINAL LOGIC) ===== */
  useEffect(() => {
    const token = sessionStorage.getItem("token");
    if (!token) return;

    const authType = sessionStorage.getItem("authType");
    if (authType === "NORMAL") {
      const saved = sessionStorage.getItem("user");
      if (saved) setUser(JSON.parse(saved));
    }

    setIsLoggedIn(true);
  }, []);

  /* ================= SHELF ================= */
  useEffect(() => {
    if (user?.userId) {
      fetchShelf(user.userId);
    } else {
      setShelf([]);
    }
  }, []);

  const fetchShelf = async (userId) => {
    try {
      if (!userId) return;

      const res = await getShelfAPI(userId);
      console.log("Shelf API raw data:", res.data);

      const formattedShelf = res.data.map((item) => {
        const imagePath =
          item.product?.cartImage ||
          item.product?.productImage ||
          item.product?.image ||
          item.productImage;

        return {
          id: item.product?.productId || item.productId,
          name: item.product?.productName || item.productName,
          author: item.product?.author?.name || item.authorName,
          image: imagePath
            ? `${import.meta.env.VITE_PUBLIC_URL || ""}${imagePath}`
            : "/fallback-book.png",
          purchaseType: item.type ?? "buy",
          productExpiryDate: item.productExpiryDate ?? null,
        };
      });

      setShelf(formattedShelf);
    } catch (err) {
      console.error("FAILED TO FETCH SHELF:", err);
    }
  };

  /* ================= CART ================= */
  useEffect(() => {
    if (user?.userId) {
      fetchCart();
    } else {
      setCart([]);
    }
  }, [user]);

  const fetchCart = async () => {
    try {
      if (!user?.userId) return;

      const res = await getCartAPI(user.userId);
      console.log("Cart Fetched:", res.data);

      // const formattedCart = res.data.map((item) => ({

      //   cartId: item.cartId,
      //   id: item.product.productId,
      //   name: item.product.productName,
      //   author: item.product.author?.name,
      //   price:
      //     item.product.productOfferprice ??
      //     item.product.productBaseprice,
      //   image: item.product.cartImage,

      //   rentable: item.product.rentable,
      //   rentPerDay: item.product.rentPerDay,
      //   minRentDays: item.product.minRentDays,
      // }));


      const formattedCart = res.data.map((item) => {
        const rawImage =
          item.product.cartImage || item.product.productImage || item.product.image;

        return {
          cartId: item.cartId,
          id: item.product.productId,
          name: item.product.productName,
          author: item.product.author?.name,
          price:
            item.product.productOfferprice ?? item.product.productBaseprice,
          image: rawImage
            ? `${import.meta.env.VITE_PUBLIC_URL || ""}${rawImage}`
            : "/fallback-book.png",
          rentable: item.product.rentable,
          rentPerDay: item.product.rentPerDay,
          minRentDays: item.product.minRentDays,
        };
      });


      setCart(formattedCart);
    } catch (err) {
      console.error("FAILED TO FETCH CART:", err);
    }
  };

  /* ================= ADD TO CART ================= */
  const addToCart = async (book) => {
    const user = JSON.parse(sessionStorage.getItem("user"));

    if (!user?.userId) {
      alert("User not logged in");
      return;
    }

    // ✅ CHECK: already in cart
    const alreadyInCart = cart.some(
      (item) => item.id === book.productId
    );

    if (alreadyInCart) {
      setCartMessage("Book already present in cart");
      return;
    }

    const alreadyOwned = shelf.some(
      (shelfBook) => shelfBook.id === book.productId
    );

    if (alreadyOwned) {
      setOwnershipWarning({
        name: book.productName,
      });
      return;
    }

    try {
      const res = await addToCartAPI(
        user.userId,
        book.productId,
        1
      );

      const cartItem = res.data;



      const rawImage = book.cartImage || book.productImage || book.image;

      setCart((prev) => [
        ...prev,
        {
          cartId: cartItem.cartId,
          id: book.productId,
          name: book.productName,
          author: book.author?.name,
          price: book.productOfferprice ?? book.productBaseprice,
          image: rawImage
            ? `${import.meta.env.VITE_PUBLIC_URL || ""}${rawImage}`
            : "/fallback-book.png",
          rentable: book.rentable,
          rentPerDay: book.rentPerDay,
          minRentDays: book.minRentDays,
          selectedDays: book.minRentDays || 7,
        },
      ]);


      // ✅ SUCCESS POPUP
      setCartMessage("Book added to cart");


      // setCart((prev) => [
      //   ...prev,
      //   {
      //     cartId: cartItem.cartId,
      //     id: book.productId,
      //     name: book.productName,
      //     author: book.author?.name,
      //     price:
      //       book.productOfferprice ??
      //       book.productBaseprice,
      //     image: book.cartImage,

      //     rentable: book.rentable,
      //     rentPerDay: book.rentPerDay,
      //     minRentDays: book.minRentDays,
      //     selectedDays: book.minRentDays || 7,
      //   },
      // ]);



    } catch (err) {
      console.error("ADD TO CART ERROR:", err);
      setCartMessage("Failed to add item to cart");
    }
  };

  /* ================= TOTALS ================= */
  const calculateTotal = () =>
    cart.reduce((sum, b) => sum + b.price, 0)
      .toLocaleString("en-IN");

  const calculateRentalTotal = () =>
    cart.reduce((sum, item) => {
      if (!item.rentable) return sum;
      const days =
        item.selectedDays || item.minRentDays || 7;
      return sum + (item.rentPerDay || 0) * days;
    }, 0).toLocaleString("en-IN");

  /* ================= PAYMENT ================= */
  const startPaymentFlow = (type) => {
    setLastPurchaseType(type);
    navigate("/payment");
  };

  const completeTransaction = async (rentPayload) => {
    const storedUser = JSON.parse(sessionStorage.getItem("user"));

    if (!storedUser?.userId) {
      alert("User session expired. Please login again.");
      return;
    }

    if (cart.length === 0) {
      alert("Cart is empty");
      return;
    }

    try {
      const productIds = cart.map((item) => item.id);


      const rentedItem = rentPayload?.find(
        (item) => item.mode === "rent"
      );

      await checkoutAPI({
        userId: storedUser.userId,
        productIds,
        rentDays: rentedItem?.rentDays ?? null,
      });

      setCart([]);
      await fetchShelf(storedUser.userId);

      navigate("/success");
    } catch (error) {
      console.error("CHECKOUT ERROR:", error);
      alert("Transaction failed! Please try again.");
    }
  };

  /* ================= RENDER ================= */
  return (
    <div className="min-h-screen bg-[#FDFCFB]">
      {ownershipWarning && (
        <OwnershipModal
          bookName={ownershipWarning.name}
          onClose={() => setOwnershipWarning(null)}
          onGoToShelf={() => {
            setOwnershipWarning(null);
            navigate("/shelf");
          }}
        />
      )}


      {cartMessage && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40">
          <div className="bg-white px-10 py-6 rounded-2xl shadow-2xl text-center w-[380px]">
            <h2 className="text-xl font-bold text-slate-800">
              {cartMessage}
            </h2>

            <button
              onClick={() => setCartMessage(null)}
              className="mt-5 px-6 py-2 rounded-full bg-indigo-600 text-white font-semibold"
            >
              OK
            </button>
          </div>
        </div>
      )}

      <Navbar
        cartCount={cart.length}
        isLoggedIn={isLoggedIn}
        setIsLoggedIn={setIsLoggedIn}
        user={user}
      />

      <Routes>
        <Route path="/" element={<HomeView />} />

        <Route
          path="/library"
          element={
            <LibraryPackageView
              books={LIBRARY_BOOKS}
              onAddToCart={addToCart}
            />
          }
        />
        <Route
          path="/library/:packageId"
          element={
            <LibraryView />
          }
        />

        <Route
          path="/confirm-buy"
          element={
            <ConfirmBuy />
          }
        />

        <Route path="/my-library" element={<MyLibrary />} />



        <Route
          path="/products"
          element={
            <ProductView
              books={LIBRARY_BOOKS}
              onAddToCart={addToCart}
            />
          }
        />

        <Route
          path="/cart"
          element={
            <ProtectedRoute>
              <CartView
                cart={cart}
                setCart={setCart}
                calculateTotal={calculateTotal}
              />
            </ProtectedRoute>
          }
        />

        <Route
          path="/transaction"
          element={
            <ProtectedRoute>
              <TransactionSelector
                cart={cart}
                setCart={setCart}
                calculateTotal={calculateTotal}
                calculateRentalTotal={calculateRentalTotal}
                startPaymentFlow={startPaymentFlow}
              />
            </ProtectedRoute>
          }
        />

        <Route
          path="/payment"
          element={
            <ProtectedRoute>
              <PaymentView
                amount={
                  lastPurchaseType === "buy"
                    ? calculateTotal()
                    : calculateRentalTotal()
                }
                upiId={upiId}
                setUpiId={setUpiId}
                onComplete={completeTransaction}
              />
            </ProtectedRoute>
          }
        />

        <Route
          path="/shelf"
          element={
            <ProtectedRoute>
              <ShelfView shelf={shelf} />
            </ProtectedRoute>
          }
        />


        <Route
          path="/orders"
          element={
            <ProtectedRoute>
              <OrdersView />
            </ProtectedRoute>
          }
        />


        {/* 🔐 ADMIN (ONLY ADDITION) */}
        <Route
          path="/admin/dashboard"
          element={
            <ProtectedRoute adminOnly>
              <AdminDashboard />
            </ProtectedRoute>
          }
        />

        {/* 🔥 LOGIN / SSO */}
        <Route
          path="/login-success"
          element={
            <LoginSuccess
              setIsLoggedIn={setIsLoggedIn}
              setUser={setUser}
            />
          }
        />

        <Route
          path="/sso-success"
          element={
            <LoginSuccess
              setIsLoggedIn={setIsLoggedIn}
              setUser={setUser}
            />
          }
        />

        <Route path="/success" element={<SuccessView />} />
        <Route
          path="/login"
          element={
            <Login
              setIsLoggedIn={setIsLoggedIn}
              setUser={setUser}
            />
          }
        />
        <Route path="/register" element={<Register />} />

        <Route path="*" element={<NotFoundView />} />
      </Routes>
      <Footer />


    </div>
  );
}
