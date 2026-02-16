import React, { useEffect, useState } from "react";

export const LanguageFilter = ({ selectedLanguage, onLanguageChange }) => {
  const [languages, setLanguages] = useState([]);

  useEffect(() => {
    fetch("http://localhost:8080/api/languages")
      .then(res => res.json())
      .then(data => setLanguages(data))
      .catch(err => console.error("Failed to load languages", err));
  }, []);

  return (
    <select
      value={selectedLanguage}
      onChange={(e) => onLanguageChange(e.target.value)}
      className="
        w-full px-4 py-3 rounded-xl
        bg-white text-black
        border border-black
        focus:outline-none focus:ring-2 focus:ring-[#C5A059]
        font-serif cursor-pointer
      "
    >
      <option value="">All Languages</option>

      {languages.map(l => (
        <option key={l.languageId} value={l.languageDesc}>
          {l.languageDesc}
        </option>
      ))}
    </select>
  );
};
