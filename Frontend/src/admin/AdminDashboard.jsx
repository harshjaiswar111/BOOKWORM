import { useState } from "react";
import AdminSidebar from "./AdminSidebar";
import AdminTopbar from "./AdminTopbar";
import Transactions from "./pages/Transactions";
import Users from "./pages/Users";
import Royalties from "./pages/Royalties";
import Beneficiaries from "./pages/Beneficiaries";
import AddBook from "./pages/AddBook";

export default function AdminDashboard() {
  const [active, setActive] = useState("DASHBOARD");

  return (
    <div className="flex min-h-screen bg-[#F8F6F3]">
      <AdminSidebar active={active} setActive={setActive} />

      <div className="flex-1 flex flex-col">
        <AdminTopbar />

        <div className="p-8">
          {active === "DASHBOARD" && <DashboardStats />}
          {active === "TRANSACTIONS" && <Transactions />}
          {active === "USERS" && <Users />}
          {active === "ROYALTIES" && <Royalties />}
          {active === "BENEFICIARIES" && <Beneficiaries />}
          {active === "ADD_BOOK" && <AddBook />}
        </div>
      </div>
    </div>
  );
}

function DashboardStats() {
  const cards = [
    { title: "Total Revenue", value: "₹1,24,560" },
    { title: "Total Royalties", value: "₹34,200" },
    { title: "Active Users", value: "268" },
    { title: "Books Listed", value: "142" },
  ];

  return (
    <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
      {cards.map((c, i) => (
        <div
          key={i}
          className="bg-white rounded-2xl shadow-md p-6 hover:shadow-xl transition"
        >
          <p className="text-sm text-slate-500">{c.title}</p>
          <h2 className="text-2xl font-bold mt-2">{c.value}</h2>
        </div>
      ))}
    </div>
  );
}
