// import { useEffect } from "react";
// import { useNavigate, useSearchParams } from "react-router-dom";

// // ✅ SAME JWT ROLE EXTRACTOR
// const getRoleFromToken = (token) => {
//   try {
//     const payload = JSON.parse(atob(token.split(".")[1]));
//     return payload.role;
//   } catch {
//     return null;
//   }
// };

// export default function LoginSuccess({ setIsLoggedIn, setUser }) {
//   const navigate = useNavigate();
//   const [params] = useSearchParams();

//   useEffect(() => {
//     const token = params.get("token");
//     const userId = params.get("userId");

//     if (!token) {
//       navigate("/login");
//       return;
//     }

//     // ✅ SSO LOGIN STORAGE
//     sessionStorage.setItem("token", token);
//     sessionStorage.setItem("authType", "SSO");
//     sessionStorage.setItem("user", JSON.stringify({ userId: Number(userId) }));

//     setIsLoggedIn(true);
//     setUser({ userId: Number(userId) });

//     // ✅ ROLE BASED REDIRECT
//     const role = getRoleFromToken(token);

//     if (role === "ROLE_ADMIN") {
//       navigate("/admin/dashboard");
//     } else {
//       navigate("/");
//     }
//   }, []);

//   return null;
// }

import { useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";

// ✅ SAME JWT ROLE EXTRACTOR
// ✅ ROBUST JWT ROLE EXTRACTOR
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
    const roleKey = Object.keys(payload).find(key =>
      key.toLowerCase().endsWith("/role") || key.toLowerCase().endsWith("/claims/role")
    );
    if (roleKey) return payload[roleKey];

    // 3. Fallback check for manual flags
    if (payload.is_admin || payload.isAdmin) return "ADMIN";

    return null;
  } catch {
    return null;
  }
};

export default function LoginSuccess({ setIsLoggedIn, setUser }) {
  const navigate = useNavigate();
  const [params] = useSearchParams();

  useEffect(() => {
    const token = params.get("token");
    const userId = params.get("userId");

    if (!token) {
      navigate("/login");
      return;
    }

    // ✅ SSO LOGIN STORAGE
    sessionStorage.setItem("token", token);
    sessionStorage.setItem("authType", "SSO");
    sessionStorage.setItem("user", JSON.stringify({ userId: Number(userId) }));

    setIsLoggedIn(true);
    setUser({ userId: Number(userId) });

    // ✅ ROLE BASED REDIRECT
    const role = getRoleFromToken(token);

    if (role === "ROLE_ADMIN" || role === "ADMIN") {
      navigate("/admin/dashboard");
    } else {
      navigate("/");
    }
  }, []);

  return null;
}
