// import { Navigate } from "react-router-dom";

// const getRoleFromToken = () => {
//   try {
//     const token = sessionStorage.getItem("token");
//     const payload = JSON.parse(atob(token.split(".")[1]));
//     return payload.role;
//   } catch {
//     return null;
//   }
// };

// export default function ProtectedRoute({ children, adminOnly = false }) {
//   const token = sessionStorage.getItem("token");

//   if (!token) {
//     return <Navigate to="/login" />;
//   }

//   if (adminOnly) {
//     const role = getRoleFromToken();
//     if (role !== "ROLE_ADMIN") {
//       return <Navigate to="/" />;
//     }
//   }

//   return children;
// }

import { Navigate, Outlet } from "react-router-dom";

// ✅ ROBUST JWT ROLE EXTRACTOR (Matches Login.jsx)
const getRoleFromToken = () => {
  try {
    const token = sessionStorage.getItem("token");
    if (!token) return null;

    // Decode payload safely
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
  } catch (error) {
    return null;
  }
};

export default function ProtectedRoute({ children, adminOnly = false }) {
  const token = sessionStorage.getItem("token");

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  if (adminOnly) {
    const role = getRoleFromToken();
    // Check for standard Spring Security "ROLE_ADMIN" or simple "ADMIN"
    const isAdmin = role === "ROLE_ADMIN" || role === "ADMIN";

    if (!isAdmin) {
      return <Navigate to="/" replace />;
    }
  }

  return children ? children : <Outlet />;
}