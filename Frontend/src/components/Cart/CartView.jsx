import React from 'react';
import { useNavigate } from 'react-router-dom'; // Required for routing
import { Trash2 } from 'lucide-react';
import { BackButton } from '../UI/Shared';
import { FALLBACK_IMAGE } from '../../data/libraryData';
import { removeFromCartAPI } from '../../services/cartservice';
export const CartView = ({ cart, setCart, calculateTotal }) => {
  const navigate = useNavigate(); // Initialize the navigation hook

  const handleImgError = (e) => { 
    e.target.src = FALLBACK_IMAGE; 
  };



  const handleRemove = async (cartId) => {
  try {
    await removeFromCartAPI(cartId);

    // update UI only AFTER backend success
    setCart((prev) => prev.filter(item => item.cartId !== cartId));
  } catch (err) {
    console.error("DELETE ERROR:", err);
    alert("Failed to remove item from cart");
  }
};


  return (
    <div className="max-w-4xl mx-auto animate-in fade-in slide-in-from-bottom-4 duration-700 pb-12 px-4">
      {/* Navigate back to the library route */}
      <BackButton onClick={() => navigate("/library")} label="Back to Library" />
      
      <h1 className="text-3xl font-serif font-bold text-slate-900 mb-8 pb-4 border-b border-slate-100 italic">
        Reading List
      </h1>

      <div className="space-y-4">
        {cart.map((item) => (
          <div key={item.id} className="group flex flex-col sm:flex-row gap-6 bg-white p-5 rounded-xl shadow-sm border border-slate-100 transition-all hover:shadow-md">
            
            {/* Standardized Book Image */}
            <div className="w-20 h-28 overflow-hidden rounded-lg shadow-sm flex-shrink-0">
              <img 
                src={item.image} 
                alt={item.name} 
                className="w-full h-full object-cover" 
                onError={handleImgError} 
              />
            </div>

            {/* Book Info */}
            <div className="flex-1 flex flex-col justify-center">
              <div className="flex justify-between items-start">
                <div>
                  <h3 className="text-lg font-serif text-slate-900 font-bold leading-tight">
                    {item.name}
                  </h3>
                  <p className="text-slate-500 italic text-sm font-serif">
                    {item.author}
                  </p>
                </div>
                
                {/* Remove Item Logic */}
                <button 
                  onClick={() => handleRemove(item.cartId)} 
                  className="text-slate-300 hover:text-rose-500 transition-colors p-2"
                >
                  <Trash2 size={18} />
                </button>
              </div>
            </div>

            {/* Price section formatted for India */}
            <div className="flex flex-col justify-center items-end border-t sm:border-t-0 sm:border-l border-slate-100 pt-3 sm:pt-0 sm:pl-8">
              <p className="text-xs text-slate-400 font-bold uppercase tracking-widest mb-1">Price</p>
              <p className="text-xl font-sans font-bold text-slate-900">
                ₹{item.price.toLocaleString('en-IN')}
              </p>
            </div>
          </div>
        ))}
      </div>



      {cart.length > 0 ? (
        <div className="mt-8 px-8 py-4 bg-slate-900 text-white rounded-2xl flex items-center justify-between shadow-lg">
          <div>
            <p className="text-[10px] uppercase tracking-widest text-slate-400">Total Amount</p>
            <p className="text-xl font-sans font-black">₹{calculateTotal()}</p>
          </div>
          {/* Navigate to the transaction selection route */}
          <button
            onClick={() => navigate('/transaction')}
            className="px-8 py-2.5 bg-[#C5A059] rounded-full text-xs font-bold uppercase tracking-widest hover:bg-[#b38f4d] transition-all active:scale-95"
          >
            Finalize Choice
          </button>


          
        </div>
      ) : (
        <div className="text-center py-20 bg-white/40 rounded-3xl border-2 border-dashed border-slate-200">
          <p className="text-slate-400 italic mb-6 text-lg font-serif">Your list is currently silent.</p>
          <button 
            onClick={() => navigate('/library')}
            className="text-slate-900 font-bold border-b-2 border-[#C5A059] pb-0.5 hover:text-[#C5A059] transition-all uppercase tracking-widest text-xs"
          >
            Discover More
          </button>
        </div>
      )}
    </div>
  );
};