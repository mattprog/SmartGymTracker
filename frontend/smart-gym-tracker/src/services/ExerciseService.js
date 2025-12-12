const CORE_API = process.env.REACT_APP_CORE_API ?? "http://localhost:5074/api";
const API_BASE = `${CORE_API}/exercises`; // adjust if needed

export async function fetchExercises(queryParams = {}) {
  // queryParams can be { q, muscle, equipment, category }
  const query = new URLSearchParams(queryParams).toString();
  const url = query ? `${API_BASE}?${query}` : API_BASE;

  try {
    const res = await fetch(url);
    if (!res.ok) throw new Error("Failed to fetch exercises");
    const data = await res.json();
    return data; // { count, data }
  } catch (err) {
    console.error(err);
    return null;
  }
}
