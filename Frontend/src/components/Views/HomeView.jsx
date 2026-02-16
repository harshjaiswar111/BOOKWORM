import React, { useState, useEffect, useRef } from "react";
import { Link } from "react-router-dom";
import { Star, Heart, ArrowRight, BookOpen, Award, Users, ChevronRight } from "lucide-react";
import { LIBRARY_BOOKS } from "../../data/libraryData";

const RevealOnScroll = ({ children, className = "" }) => {
  const [isVisible, setIsVisible] = useState(false);
  const ref = useRef(null);

  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setIsVisible(true);
          observer.disconnect();
        }
      },
      { threshold: 0.1 }
    );

    if (ref.current) observer.observe(ref.current);

    return () => observer.disconnect();
  }, []);

  return (
    <div
      ref={ref}
      className={`transition-all duration-1000 ease-out transform ${isVisible ? "opacity-100 translate-y-0" : "opacity-0 translate-y-12"
        } ${className}`}
    >
      {children}
    </div>
  );
};

const FeatureCard = ({ icon, title, desc }) => (
  <div className="bg-white p-6 md:p-8 rounded-2xl border border-slate-100 shadow-sm hover:shadow-xl transition-all duration-300 hover:-translate-y-2 group cursor-default">
    <div className="mb-4 bg-amber-50 w-14 h-14 flex items-center justify-center rounded-xl text-[#C5A059] shadow-inner group-hover:scale-110 transition-transform duration-300">
      {icon}
    </div>
    <h3 className="font-serif font-bold text-xl mb-3 text-slate-800 group-hover:text-[#C5A059] transition-colors">{title}</h3>
    <p className="text-slate-500 text-base leading-relaxed">{desc}</p>
  </div>
);

const BookCard = ({ book }) => (
  <div className="group relative bg-white rounded-2xl p-4 shadow-sm hover:shadow-2xl transition-all duration-500 border border-slate-100 flex flex-col h-full hover:-translate-y-2">
    <div className="aspect-[2/3] overflow-hidden rounded-xl mb-4 bg-slate-100 relative shadow-inner">
      <img
        src={book.image}
        alt={book.name}
        className="w-full h-full object-cover transform group-hover:scale-110 transition-transform duration-700 ease-out"
        onError={(e) =>
        (e.target.src =
          "https://images.unsplash.com/photo-1541963463532-d68292c34b19?w=400&auto=format&fit=crop&q=60")
        }
      />
      <div className="absolute inset-0 bg-slate-900/40 opacity-0 group-hover:opacity-100 transition-opacity duration-300 flex items-center justify-center backdrop-blur-[2px]">
        <Link
          to={`/products`}
          className="bg-white text-slate-900 px-6 py-3 rounded-full font-bold text-sm transform translate-y-8 group-hover:translate-y-0 transition-all duration-500 shadow-xl hover:bg-[#C5A059] hover:text-white"
        >
          View Details
        </Link>
      </div>
    </div>
    <div className="space-y-2 mt-auto">
      <p className="text-xs font-bold text-[#C5A059] uppercase tracking-widest">
        {book.genre}
      </p>
      <h3 className="font-serif font-bold text-lg text-slate-900 leading-tight group-hover:text-[#C5A059] transition-colors line-clamp-2">
        {book.name}
      </h3>
      <p className="text-sm text-slate-500 font-medium">{book.author}</p>
      <div className="flex items-center gap-1 pt-2">
        {[...Array(5)].map((_, i) => (
          <Star key={i} size={14} className="fill-amber-400 text-amber-400" />
        ))}
        <span className="text-xs text-slate-400 ml-1 font-semibold">
          (4.8)
        </span>
      </div>
    </div>
  </div>
);

const CategoryCard = ({ name, count, image }) => (
  <Link
    to="/products"
    className="group relative overflow-hidden rounded-3xl aspect-[4/3] md:aspect-[3/4] lg:aspect-[4/3] flex items-end p-6 md:p-8 cursor-pointer shadow-md hover:shadow-2xl transition-all duration-500 hover:-translate-y-1"
  >
    <img
      src={image}
      alt={name}
      className="absolute inset-0 w-full h-full object-cover transition-transform duration-700 group-hover:scale-110"
    />
    <div className="absolute inset-0 bg-gradient-to-t from-black/90 via-black/40 to-transparent opacity-90 group-hover:opacity-100 transition-opacity" />
    <div className="relative z-10 text-white transform group-hover:-translate-y-2 transition-transform duration-300">
      <h3 className="font-serif font-bold text-2xl md:text-3xl mb-2">
        {name}
      </h3>
      <p className="text-sm font-medium text-white/90 group-hover:text-amber-400 transition-colors flex items-center gap-2">
        Explore {count} Books{" "}
        <ChevronRight
          size={16}
          className="group-hover:translate-x-2 transition-transform duration-300"
        />
      </p>
    </div>
  </Link>
);

const TestimonialCard = ({ name, role, text, avatar }) => (
  <div className="bg-slate-50 p-8 rounded-3xl relative border border-slate-100 shadow-sm hover:shadow-lg transition-all duration-300 hover:bg-white">
    <div className="text-[#C5A059] text-7xl font-serif absolute -top-2 left-6 opacity-20 selection:bg-none pointer-events-none">
      "
    </div>
    <p className="text-slate-700 italic mb-8 relative z-10 leading-loose text-lg font-medium font-serif">
      "{text}"
    </p>
    <div className="flex items-center gap-4">
      <img
        src={avatar}
        alt={name}
        className="w-14 h-14 rounded-full object-cover border-2 border-white shadow-md"
      />
      <div>
        <h4 className="font-bold text-slate-900 text-base">{name}</h4>
        <p className="text-[#C5A059] text-xs font-bold tracking-wider uppercase">
          {role}
        </p>
      </div>
    </div>
  </div>
);

export const HomeView = () => {
  const featuredBooks = LIBRARY_BOOKS.slice(0, 4);
  const bestSellers = LIBRARY_BOOKS.slice(2, 6);

  const categories = [
    {
      name: "Fiction",
      count: "120+",
      image:
        "https://images.unsplash.com/photo-1474932430478-367dbb6832c1?auto=format&fit=crop&q=80&w=400",
    },
    {
      name: "Self-Help",
      count: "85+",
      image:
        "https://images.unsplash.com/photo-1544947950-fa07a98d237f?auto=format&fit=crop&q=80&w=400",
    },
    {
      name: "Business",
      count: "60+",
      image:
        "https://images.unsplash.com/photo-1460925895917-afdab827c52f?auto=format&fit=crop&q=80&w=400",
    },
    {
      name: "Biography",
      count: "40+",
      image:
        "https://images.unsplash.com/photo-1532012197267-da84d127e765?auto=format&fit=crop&q=80&w=400",
    },
  ];

  return (
    <div className="animate-in fade-in duration-700 pb-20 bg-[#FDFCFB] overflow-x-hidden">
      {/* 1. HERO SECTION */}
      <section className="relative overflow-hidden bg-slate-900 text-white min-h-[60vh] md:min-h-[500px] flex items-center rounded-b-[40px] md:rounded-br-[80px] shadow-2xl">
        {/* Background Blobs/Effects */}
        <div className="absolute top-0 right-0 w-full md:w-2/3 h-full bg-[#C5A059]/10 skew-x-12 translate-x-20 md:translate-x-0" />
        <div className="absolute bottom-0 left-0 w-[500px] h-[500px] bg-blue-900/20 rounded-full blur-[100px] -translate-x-1/3 translate-y-1/3" />

        <div className="container mx-auto px-6 md:px-12 relative z-10 grid md:grid-cols-2 gap-8 lg:gap-16 items-center py-12 md:py-0">
          <div className="space-y-6 md:space-y-8 text-center md:text-left animate-slide-down">
            <div className="inline-flex items-center gap-2 px-4 py-2 rounded-full bg-white/10 text-amber-300 text-xs font-semibold border border-white/10 backdrop-blur-md shadow-lg mx-auto md:mx-0 animate-bounce-slow">
              <Star size={14} className="fill-amber-300" />
              <span className="tracking-wide">#1 Rated Online Bookstore</span>
            </div>

            <h1 className="text-3xl md:text-5xl lg:text-6xl font-serif font-bold leading-[1.1] tracking-tight">
              Discover Your <br className="hidden md:block" />
              Next{" "}
              <span className="text-transparent bg-clip-text bg-gradient-to-r from-[#C5A059] to-amber-300 italic pr-2">
                Great Adventure
              </span>
            </h1>

            <p className="text-base md:text-lg text-slate-300 max-w-lg leading-relaxed mx-auto md:mx-0 font-light">
              Explore a curated collection of bestsellers, classics, and hidden
              gems.
              <span className="text-white font-medium"> Read, rent, or own</span>{" "}
              - the choice is yours.
            </p>

            <div className="flex flex-col sm:flex-row gap-4 justify-center md:justify-start pt-2">
              <Link
                to="/products"
                className="px-8 py-3 bg-[#C5A059] hover:bg-[#b08d4b] text-white rounded-full font-bold text-base transition-all transform hover:scale-105 shadow-[0_10px_40px_-10px_rgba(197,160,89,0.5)] flex items-center justify-center gap-2 group hover-glow"
              >
                Browse Books{" "}
                <ArrowRight
                  size={18}
                  className="group-hover:translate-x-1 transition-transform"
                />
              </Link>
              <Link
                to="/library"
                className="px-8 py-3 bg-white/10 hover:bg-white/20 text-white rounded-full font-bold text-base transition-all backdrop-blur-md border border-white/10 flex items-center justify-center hover:border-white/30"
              >
                View Packages
              </Link>
            </div>

            <div className="flex items-center justify-center md:justify-start gap-6 pt-4 border-t border-white/10 mt-6">
              <div className="flex -space-x-3">
                {[1, 2, 3, 4].map((i) => (
                  <img
                    key={i}
                    src={`https://randomuser.me/api/portraits/thumb/men/${i + 10
                      }.jpg`}
                    className="w-10 h-10 rounded-full border-2 border-slate-900 shadow-lg"
                    alt="user"
                  />
                ))}
              </div>
              <div className="text-left">
                <div className="flex gap-1 mb-0.5">
                  {[1, 2, 3, 4, 5].map((i) => (
                    <Star
                      key={i}
                      size={14}
                      className="fill-amber-400 text-amber-400"
                    />
                  ))}
                </div>
                <p className="text-xs font-medium text-slate-400">
                  Join <span className="text-white font-bold">10,000+</span>{" "}
                  Happy Readers
                </p>
              </div>
            </div>
          </div>

          <div className="hidden md:flex justify-end relative perspective-1000 animate-fade-up">
            {/* BACKGROUND BOOK (2nd Item) */}
            <div className="absolute top-4 right-36 z-0 w-[300px] aspect-[2/3] bg-slate-800 rounded-2xl shadow-xl rotate-y-12 -rotate-z-6 border border-white/10 overflow-hidden transform transition-all duration-700 hover:rotate-y-0 hover:rotate-z-0 hover:z-20 hover:scale-105 origin-bottom-right opacity-60 hover:opacity-100">
              <img
                src={LIBRARY_BOOKS[4].image}
                className="w-full h-full object-cover opacity-80"
                alt="Second Hero Book"
              />
            </div>
            <div className="relative z-10 w-[300px] aspect-[2/3] bg-slate-800 rounded-2xl shadow-2xl rotate-y-12 rotate-z-3 border border-white/10 overflow-hidden transform hover:rotate-y-0 hover:rotate-z-0 transition-all duration-700 cursor-pointer group hover:shadow-[0_20px_60px_-15px_rgba(197,160,89,0.3)]">
              <img
                src={LIBRARY_BOOKS[0].image}
                className="w-full h-full object-cover transform scale-105 group-hover:scale-100 transition-transform duration-700"
                alt="Hero Book"
              />
              <div className="absolute bottom-0 left-0 right-0 p-8 bg-gradient-to-t from-black via-black/60 to-transparent">
                <p className="text-[#C5A059] font-bold tracking-[0.2em] text-xs mb-2 uppercase">
                  Bestseller of the Month
                </p>
                <h3 className="font-serif text-3xl font-bold text-white shadow-black drop-shadow-lg">
                  {LIBRARY_BOOKS[0].name}
                </h3>
              </div>
            </div>
            {/* Decorative elements behind book */}
            <div className="absolute top-12 right-12 w-full h-full border-2 border-[#C5A059]/30 rounded-3xl -z-10 translate-x-8 translate-y-8" />
            <div className="absolute -bottom-10 -right-10 w-40 h-40 bg-[#C5A059] rounded-full blur-[80px] opacity-40 animate-pulse" />
          </div>
        </div>
      </section>

      <div className="container mx-auto px-6 md:px-12 space-y-36 mt-32">
        {/* 2. FEATURES */}
        <RevealOnScroll>
          <div className="grid md:grid-cols-3 gap-8 lg:gap-12">
            <FeatureCard
              icon={<Award size={28} strokeWidth={1.5} />}
              title="Curated Selection"
              desc="Handpicked titles ensuring you only spend time on books that truly matter. Quality over quantity, always."
            />
            <FeatureCard
              icon={<BookOpen size={28} strokeWidth={1.5} />}
              title="Read or Rent"
              desc="Flexible options to buy books for your permanent shelf or rent them for a quick weekend read."
            />
            <FeatureCard
              icon={<Users size={28} strokeWidth={1.5} />}
              title="Community Driven"
              desc="Join a vibrant community of readers sharing authentic reviews, thoughts, and monthly recommendations."
            />
          </div>
        </RevealOnScroll>

        {/* 3. FEATURED BOOKS */}
        <section>
          <RevealOnScroll>
            <div className="flex flex-col md:flex-row justify-between items-end mb-16 gap-6">
              <div className="text-center md:text-left w-full md:w-auto">
                <h2 className="text-4xl md:text-5xl font-serif font-bold text-slate-900 mb-4">
                  Featured Collection
                </h2>
                <p className="text-slate-500 text-lg max-w-xl mx-auto md:mx-0">
                  Top picks for this week. Dive into stories that have captured
                  the hearts of readers worldwide.
                </p>
              </div>
              <Link
                to="/products"
                className="hidden md:flex items-center gap-2 text-[#C5A059] font-bold text-lg hover:underline underline-offset-8 decoration-2 px-4 py-2 hover:bg-[#C5A059]/10 rounded-lg transition-all"
              >
                View All Books <ChevronRight size={20} />
              </Link>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8 md:gap-10">
              {featuredBooks.map((book) => (
                <BookCard key={book.id} book={book} />
              ))}
            </div>

            <Link
              to="/products"
              className="md:hidden mt-12 w-full py-4 rounded-xl bg-slate-100 flex justify-center items-center gap-2 text-slate-900 font-bold hover:bg-slate-200 transition-colors"
            >
              View All Collection <ChevronRight size={20} />
            </Link>
          </RevealOnScroll>
        </section>

        {/* 4. CATEGORIES */}
        <section>
          <RevealOnScroll>
            <h2 className="text-4xl md:text-5xl font-serif font-bold text-slate-900 mb-16 text-center">
              Explore by Genre
            </h2>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 md:gap-8">
              {categories.map((cat, idx) => (
                <CategoryCard key={idx} {...cat} />
              ))}
            </div>
          </RevealOnScroll>
        </section>




      </div>
    </div>
  );
};