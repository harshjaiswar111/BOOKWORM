export default function Transactions() {
  return (
    <div className="bg-white rounded-2xl shadow p-6">
      <h2 className="text-xl font-bold mb-6">Transactions</h2>

      <table className="w-full text-sm">
        <thead>
          <tr className="border-b text-left text-slate-500">
            <th>ID</th>
            <th>User</th>
            <th>Amount</th>
            <th>Status</th>
          </tr>
        </thead>

        <tbody>
          <tr className="border-b">
            <td>#1023</td>
            <td>adarsh@gmail.com</td>
            <td>₹499</td>
            <td className="text-green-600 font-semibold">SUCCESS</td>
          </tr>
        </tbody>
      </table>
    </div>
  );
}
