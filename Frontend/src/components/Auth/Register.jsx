import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../api/axios";

export default function Register() {
  const navigate = useNavigate();

  const [user, setUser] = useState({
    name: "",
    email: "",
    phone: "",
    address: "",
    password: "",
    confirmPassword: "",
    admin: false,
  });

  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});

  const handleChange = (e) => {
  const { name, value, type, checked } = e.target;

  setUser((prev) => ({
    ...prev,
    [name]: type === "checkbox" ? checked : value,
  }));

  setTouched((prev) => ({
    ...prev,
    [name]: true,
  }));

  let message = "";

  // ✅ Email live validation
  if (name === "email") {
    if (value.length > 0 && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      message = "Enter a valid email";
    }
  }

  // ✅ Phone live validation
  if (name === "phone") {
    if (!/^[0-9]*$/.test(value)) {
      message = "Only numbers allowed";
    } else if (value.length > 0 && value.length !== 10) {
      message = "Phone must be exactly 10 digits";
    }
  }

  // ✅ Password live validation
  if (name === "password") {
    if (value.length > 0 && value.length < 6) {
      message = "Password must be at least 6 characters";
    }
  }

  // ✅ Confirm password live validation
  if (name === "confirmPassword") {
    if (value.length > 0 && value !== user.password) {
      message = "Passwords do not match";
    }
  }

  setErrors((prev) => ({
    ...prev,
    [name]: message,
  }));
};


  // form validation
  const validateForm = () => {
    const newErrors = {};

    // ✅ Email
    if (!user.email) {
      newErrors.email = "Email is required";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(user.email)) {
      newErrors.email = "Invalid email format";
    }

    // ✅ Phone (10 digits)
    if (!user.phone) {
      newErrors.phone = "Phone number is required";
    } else if (!/^[0-9]{10}$/.test(user.phone)) {
      newErrors.phone = "Phone must be 10 digits";
    }

    // ✅ Password
    if (!user.password) {
      newErrors.password = "Password is required";
    } else if (user.password.length < 6) {
      newErrors.password = "Password must be at least 6 characters";
    }

    // ✅ Confirm Password
    if (!user.confirmPassword) {
      newErrors.confirmPassword = "Please confirm your password";
    } else if (user.password !== user.confirmPassword) {
      newErrors.confirmPassword = "Passwords do not match";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    if (!validateForm()) return;
    setLoading(true);

    try {
      await api.post("/auth/register", {
        ...user,
        email: user.email.toLowerCase(),
      });

      alert("Registration successful");
      navigate("/login");
    } catch {
      alert("User already exists");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-[#FDFCFB] flex items-center justify-center px-6">
      <div className="w-full max-w-6xl grid grid-cols-1 md:grid-cols-2 bg-white rounded-3xl shadow-xl overflow-hidden">
        {/* ================= LEFT : REGISTER FORM ================= */}
        <div className="p-10 flex flex-col justify-center">
          <h1 className="text-3xl font-bold text-slate-900 mb-2">
            Create Account 🚀
          </h1>
          <p className="text-slate-500 mb-8">Join Bookworm Digital Sanctuary</p>

          <form onSubmit={handleRegister}>
            <input
              name="name"
              placeholder="Full Name"
              onChange={handleChange}
              className="w-full text-xl mb-4 px-4 py-3 border rounded-[25px] focus:outline-none focus:ring-2 focus:ring-indigo-500"
              required
            />

            <div className="flex gap-3 mb-4">
            <div className="w-full">
              <input
                name="email"
                type="email"
                placeholder="Email"
                onChange={handleChange}
                className="w-full text-xl px-4 py-3 border rounded-[25px] focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
              {touched.email && errors.email && (
                <p className="text-red-500 text-xl mt-2 ml-2">{errors.email}</p>
              )}
              </div>

              <div className="w-full">
              <input
                name="phone"
                placeholder="Phone"
                onChange={handleChange}
                className="w-full text-xl px-4 py-3 border rounded-[25px] focus:outline-none focus:ring-2 focus:ring-indigo-500"
              />
              {touched.phone && errors.phone && (
                <p className="text-red-500 text-xl mt-1 ml-2">{errors.phone}</p>
              )}
              </div>
              
            </div>

            <input
              name="address"
              placeholder="Address"
              onChange={handleChange}
              className="w-full text-xl mb-4 px-4 py-3 border rounded-[25px] focus:outline-none focus:ring-2 focus:ring-indigo-500"
              required
            />


            <input
              name="password"
              type="password"
              placeholder="Password"
              onChange={handleChange}
              className="w-full text-xl mb-1 px-4 py-3 border rounded-[25px] focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {touched.password && errors.password && (
              <p className="text-red-500 text-xl mb-3 ml-2">
                {errors.password}
              </p>
            )}

            <input
              name="confirmPassword"
              type="password"
              placeholder="Confirm Password"
              onChange={handleChange}
              className="w-full text-xl mb-1 px-4 py-3 border rounded-[25px] focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            {touched.confirmPassword && errors.confirmPassword && (
              <p className="text-red-500 text-xl mb-3 ml-2">
                {errors.confirmPassword}
              </p>
            )}

            <label className="flex items-center gap-2 mb-6 text-sm text-slate-600">
              <input type="checkbox" name="admin" onChange={handleChange} />
              Register as Admin
            </label>

            <button
              disabled={loading}
              className="w-full bg-indigo-600 hover:bg-indigo-700 transition text-white py-3 rounded-[25px] font-semibold disabled:opacity-60"
            >
              {loading ? "Creating account..." : "Register"}
            </button>
          </form>

          {/* OR */}
          <div className="my-6 flex items-center gap-3">
            <div className="flex-1 h-px bg-slate-200" />
            <span className="text-slate-400 text-sm">OR</span>
            <div className="flex-1 h-px bg-slate-200" />
          </div>

          {/* GOOGLE SSO */}
          <button
            type="button"
            onClick={() =>
              (window.location.href =
                "http://localhost:8080/oauth2/authorization/google")
            }
            className="w-full border hover:bg-slate-50 transition py-3 rounded-[25px] font-medium flex items-center justify-center gap-2"
          >
            <img
              src="https://www.svgrepo.com/show/475656/google-color.svg"
              alt="Google"
              className="w-5 h-5"
            />
            Continue with Google
          </button>

          <p className="text-center text-sm text-slate-500 mt-6">
            Already have an account?{" "}
            <span
              onClick={() => navigate("/login")}
              className="text-indigo-600 font-semibold cursor-pointer hover:underline"
            >
              Login
            </span>
          </p>
        </div>

        {/* ================= RIGHT : ILLUSTRATION ================= */}
        <div className="hidden md:flex items-center justify-center bg-gradient-to-br from-indigo-500 to-purple-600 p-10">
          <img
            src="/auth-illustration.png"
            alt="Register illustration"
            className="max-w-sm"
          />
        </div>
      </div>
    </div>
  );
}
