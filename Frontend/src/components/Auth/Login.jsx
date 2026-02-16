// import { useState } from "react";
// import { useNavigate } from "react-router-dom";
// import api from "../../api/axios";

// // ✅ JWT ROLE EXTRACTOR (SAFE)
// const getRoleFromToken = (token) => {
//   try {
//     const payload = JSON.parse(atob(token.split(".")[1]));
//     return payload.role; // ROLE_ADMIN | ROLE_USER
//   } catch {
//     return null;
//   }
// };

// export default function Login({ setIsLoggedIn, setUser }) {
//   const navigate = useNavigate();
//   const [email, setEmail] = useState("");
//   const [password, setPassword] = useState("");
//   const [loading, setLoading] = useState(false);

//   const handleLogin = async (e) => {
//     e.preventDefault();
//     setLoading(true);

//     try {
//       const res = await api.post("/auth/login", {
//         email: email.trim().toLowerCase(),
//         password: password.trim(),
//       });

//       // ✅ NORMAL LOGIN STORAGE
//       sessionStorage.setItem("token", res.data.token);
//       sessionStorage.setItem("authType", "NORMAL");
//       sessionStorage.setItem(
//         "user",
//         JSON.stringify({ userId: res.data.userId })
//       );

//       setIsLoggedIn(true);
//       setUser({ userId: res.data.userId });

//       // ✅ ROLE BASED REDIRECT (FIXED PATHS)
//       const role = getRoleFromToken(res.data.token);

//       if (role === "ADMIN" || role === "ROLE_ADMIN") {
//         navigate("/admin/dashboard");   // ✅ ADMIN
//       } else {
//         navigate("/");                  // ✅ NORMAL USER → HomeView
//       }
//     } catch (err) {
//       alert("Invalid credentials");
//     } finally {
//       setLoading(false);
//     }
//   };

//   return (
//   <div className="relative min-h-screen overflow-hidden mt-0 pt-0">

//     {/* FLOATING BOOKS BACKGROUND */}
// {/* <div className="absolute inset-0 overflow-hidden pointer-events-none">
//   <span className="book-float book-1" />
//   <span className="book-float book-2" />
//   <span className="book-float book-3" />
//   <span className="book-float book-5" />
//   <span className="book-float book-6" />
//   <span className="book-float book-7" />
//   <span className="book-float book-8" />
// </div> */}

// <div className="absolute inset-0 pointer-events-none">
//   <div className="light-orb orb-1"></div>
//   <div className="light-orb orb-2"></div>
// </div>

//     {/* BACKGROUND LIBRARY */}
//    <div className="absolute inset-0 flex justify-center">
//     <div className="bg-image-container" />
//     </div>
//     <div className="absolute inset-0 bg-white/20" />

//     {/* LIGHT GRADIENT */}
//     <div className="absolute -top-40 -left-40 w-[500px] h-[500px] bg-white-300/10 rounded-full blur-[120px]" />
//     <div className="absolute bottom-0 right-0 w-[400px] h-[400px] bg-white-500/10 rounded-full blur-[120px]" />

//     {/* CONTENT */}
//     <div className="relative z-10 min-h-screen flex items-center justify-center px-6">
//       <div className="login-card grid grid-cols-1 md:grid-cols-2">

//         {/* LEFT TEXT */}
//         <div className="hidden md:flex flex-col justify-center p-12 text-white">
//           <h1 className="text-4xl font-bold leading-tight">
//             Your personal <br /> reading space
//           </h1>
//           <p className="mt-6 text-slate-300 max-w-sm">
//             Track books, save progress, and build your own digital library with BookWorm.
//           </p>
//           <span className="mt-10 text-xs tracking-widest opacity-60">
//             READ • REMEMBER • RETURN
//           </span>
//         </div>

//         {/* LOGIN */}
//         <div className="p-12 bg-white/90 backdrop-blur-xl">
//           <h2 className="text-2xl font-semibold mb-2 ">Welcome back</h2>
//           <p className="text-slate-500 mb-8">
//             Sign in to continue reading
//           </p>

//           <form onSubmit={handleLogin} className="space-y-5">
//             <input
//               type="email"
//               placeholder="Email address"
//               value={email}
//               onChange={(e) => setEmail(e.target.value)}
//               className="input-real"
//               required
//             />

//             <input
//               type="password"
//               placeholder="Password"
//               value={password}
//               onChange={(e) => setPassword(e.target.value)}
//               className="input-real"
//               required
//             />

//             <button disabled={loading} className="btn-real">
//               {loading ? "Signing in..." : "Sign in"}
//             </button>
//           </form>

//           <div className="divider-real">OR</div>

//           <button
//             onClick={() =>
//               (window.location.href =
//                 "http://localhost:8080/oauth2/authorization/google")
//             }
//             className="google-real"
//           >
//             <img
//               src="https://www.svgrepo.com/show/475656/google-color.svg"
//               className="w-5 h-5"
//             />
//             Continue with Google
//           </button>

//           <p className="text-sm text-center mt-8 text-slate-500">
//             New here?{" "}
//             <span
//               onClick={() => navigate("/register")}
//               className="text-indigo-600 font-medium cursor-pointer"
//             >
//               Create account
//             </span>
//           </p>
//         </div>
//       </div>
//     </div>
//   </div>
// );


// }

import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../api/axios";

// ✅ JWT ROLE EXTRACTOR (SAFE)
// ✅ ROBUST JWT ROLE EXTRACTOR (Including .NET Claims)
const getRoleFromToken = (token) => {
  try {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    );

    const payload = JSON.parse(jsonPayload);

    // 1. Check standard keys
    if (payload.role) return payload.role;
    if (payload.roles) return payload.roles;
    if (payload.authority) return payload.authority;

    // 2. Check for .NET/Microsoft specific long claim keys
    // e.g. "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    const roleKey = Object.keys(payload).find(key =>
      key.toLowerCase().endsWith("/role") || key.toLowerCase().endsWith("/claims/role")
    );
    if (roleKey) return payload[roleKey];

    // 3. Check specific admin flags mentioned by user
    if (payload.is_admin || payload.isAdmin) return "ADMIN";

    return null;
  } catch {
    return null;
  }
};

export default function Login({ setIsLoggedIn, setUser }) {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const res = await api.post("/auth/login", {
        email: email.trim().toLowerCase(),
        password: password.trim(),
      });

      // ✅ NORMAL LOGIN STORAGE
      sessionStorage.setItem("token", res.data.token);
      sessionStorage.setItem("authType", "NORMAL");
      sessionStorage.setItem(
        "user",
        JSON.stringify({ userId: res.data.userId })
      );

      setIsLoggedIn(true);
      setUser({ userId: res.data.userId });

      // ✅ ROLE BASED REDIRECT (FIXED PATHS)
      // 1. Try checking token
      let role = getRoleFromToken(res.data.token);

      // 2. Fallback: Check response body directly if token parsing failed or empty
      if (!role) {
        if (res.data.role) role = res.data.role;
        else if (res.data.is_admin || res.data.isAdmin) role = "ADMIN";
        else if (res.data.user?.role) role = res.data.user.role;
        else if (res.data.user?.is_admin) role = "ADMIN";
      }

      console.log("LOGIN DETECTED ROLE:", role);

      if (role === "ADMIN" || role === "ROLE_ADMIN") {
        navigate("/admin/dashboard");   // ✅ ADMIN
      } else {
        navigate("/");                  // ✅ NORMAL USER → HomeView
      }
    } catch (err) {
      console.error(err);
      alert("Invalid credentials");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="relative min-h-screen overflow-hidden mt-0 pt-0">

      {/* FLOATING BOOKS BACKGROUND */}
      {/* <div className="absolute inset-0 overflow-hidden pointer-events-none">
  <span className="book-float book-1" />
  <span className="book-float book-2" />
  <span className="book-float book-3" />
  <span className="book-float book-5" />
  <span className="book-float book-6" />
  <span className="book-float book-7" />
  <span className="book-float book-8" />
</div> */}

      <div className="absolute inset-0 pointer-events-none">
        <div className="light-orb orb-1"></div>
        <div className="light-orb orb-2"></div>
      </div>

      {/* BACKGROUND LIBRARY */}
      <div className="absolute inset-0 flex justify-center">
        <div className="bg-image-container" />
      </div>
      <div className="absolute inset-0 bg-white/20" />

      {/* LIGHT GRADIENT */}
      <div className="absolute -top-40 -left-40 w-[500px] h-[500px] bg-white-300/10 rounded-full blur-[120px]" />
      <div className="absolute bottom-0 right-0 w-[400px] h-[400px] bg-white-500/10 rounded-full blur-[120px]" />

      {/* CONTENT */}
      <div className="relative z-10 min-h-screen flex items-center justify-center px-6">
        <div className="login-card grid grid-cols-1 md:grid-cols-2">

          {/* LEFT TEXT */}
          <div className="hidden md:flex flex-col justify-center p-12 text-white">
            <h1 className="text-4xl font-bold leading-tight">
              Your personal <br /> reading space
            </h1>
            <p className="mt-6 text-slate-300 max-w-sm">
              Track books, save progress, and build your own digital library with BookWorm.
            </p>
            <span className="mt-10 text-xs tracking-widest opacity-60">
              READ • REMEMBER • RETURN
            </span>
          </div>

          {/* LOGIN */}
          <div className="p-12 bg-white/90 backdrop-blur-xl">
            <h2 className="text-2xl font-semibold mb-2 ">Welcome back</h2>
            <p className="text-slate-500 mb-8">
              Sign in to continue reading
            </p>

            <form onSubmit={handleLogin} className="space-y-5">
              <input
                type="email"
                placeholder="Email address"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="input-real"
                required
              />

              <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="input-real"
                required
              />

              <button disabled={loading} className="btn-real">
                {loading ? "Signing in..." : "Sign in"}
              </button>
            </form>

            <div className="divider-real">OR</div>

            <button
              onClick={() =>
              (window.location.href =
                "http://localhost:8080/oauth2/authorization/google")
              }
              className="google-real"
            >
              <img
                src="https://www.svgrepo.com/show/475656/google-color.svg"
                className="w-5 h-5"
              />
              Continue with Google
            </button>

            <p className="text-sm text-center mt-8 text-slate-500">
              New here?{" "}
              <span
                onClick={() => navigate("/register")}
                className="text-indigo-600 font-medium cursor-pointer"
              >
                Create account
              </span>
            </p>
          </div>
        </div>
      </div>
    </div>
  );


}
