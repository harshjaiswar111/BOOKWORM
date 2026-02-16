export default function AdminSidebar({ active, setActive }) {
  const menus = [
    { key: "DASHBOARD", label: "Dashboard" },
    { key: "TRANSACTIONS", label: "Transactions" },
    { key: "ROYALTIES", label: "Royalties" },
    { key: "BENEFICIARIES", label: "Beneficiaries" },
    { key: "USERS", label: "Users" },
    { key: "ADD_BOOK", label: "Add Book" },
  ];

  return (
    <aside className="w-64 bg-[#1C1B1A] text-white p-6">
      <h1 className="text-2xl font-serif mb-10">
        BOOKWORM<span className="text-[#C5A059]">.</span>
      </h1>

      <nav className="space-y-2">
        {menus.map(m => (
          <button
            key={m.key}
            onClick={() => setActive(m.key)}
            className={`w-full text-left px-4 py-3 rounded-xl transition
              ${active === m.key
                ? "bg-[#C5A059] text-black"
                : "hover:bg-white/10"}`}
          >
            {m.label}
          </button>
        ))}
      </nav>
    </aside>
  );
}
