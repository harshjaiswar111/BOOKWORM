export default function AdminTopbar() {
  return (
    <div className="bg-white px-8 py-4 shadow-sm flex justify-between items-center">
      <h2 className="text-xl font-serif font-bold">
        Admin Panel
      </h2>

      <div className="text-sm text-slate-600">
        Logged in as <span className="font-semibold">ADMIN</span>
      </div>
    </div>
  );
}
