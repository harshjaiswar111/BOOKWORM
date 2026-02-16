export default function Footer() {
  return (
    <footer className="bg-black border-t border-white/10 mt-24">
      <div className="max-w-7xl mx-auto px-6 py-12 grid grid-cols-1 md:grid-cols-4 gap-10">

        {/* BRAND */}
        <div>
          <h2 className="text-xl font-bold tracking-wide text-white">
            BOOKWORM<span className="text-indigo-500">.</span>
          </h2>
          <p className="text-slate-400 mt-4 text-sm leading-relaxed">
            Your personal reading space. Track books, save progress, and build
            your digital library.
          </p>
        </div>

        {/* PRODUCT */}
        <div>
          <h3 className="font-semibold mb-4 text-white">Product</h3>
          <ul className="space-y-2 text-sm text-slate-400">
            <li className="hover:text-white cursor-pointer transition">Library</li>
            <li className="hover:text-white cursor-pointer transition">My Shelf</li>
            <li className="hover:text-white cursor-pointer transition">My Library</li>
            <li className="hover:text-white cursor-pointer transition">Explore</li>
          </ul>
        </div>

        {/* COMPANY */}
        <div>
          <h3 className="font-semibold mb-4 text-white">Company</h3>
          <ul className="space-y-2 text-sm text-slate-400">
            <li className="hover:text-white cursor-pointer transition">About</li>
            <li className="hover:text-white cursor-pointer transition">Careers</li>
            <li className="hover:text-white cursor-pointer transition">Blog</li>
            <li className="hover:text-white cursor-pointer transition">Contact</li>
          </ul>
        </div>

        {/* SUPPORT */}
        <div>
          <h3 className="font-semibold mb-4 text-white">Support</h3>
          <ul className="space-y-2 text-sm text-slate-400">
            <li className="hover:text-white cursor-pointer transition">Help Center</li>
            <li className="hover:text-white cursor-pointer transition">Privacy Policy</li>
            <li className="hover:text-white cursor-pointer transition">Terms of Service</li>
            <li className="hover:text-white cursor-pointer transition">Feedback</li>
          </ul>
        </div>
      </div>

      {/* BOTTOM */}
      <div className="border-t border-white/10 py-4 text-center text-sm text-slate-500">
        © {new Date().getFullYear()} BookWorm. All rights reserved.
      </div>
    </footer>
  );
}
