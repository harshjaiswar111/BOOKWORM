export default function Royalties() {
  return (
    <div className="bg-white rounded-2xl shadow p-6">
      <h2 className="text-xl font-bold mb-4">Royalties</h2>

      <p className="text-slate-600">
        Royalty calculated per author based on sales.
      </p>

      <div className="mt-4 border p-4 rounded-xl">
        <p><b>Author:</b> James Clear</p>
        <p><b>Royalty:</b> ₹12,500</p>
      </div>
    </div>
  );
}
