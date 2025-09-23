const API_BASE = "http://localhost:5000/api/muscles"; // placeholder endpoint

export async function fetchMuscles() {
  try {
    const res = await fetch(API_BASE);
    if (!res.ok) throw new Error("Failed to fetch muscles");
    const data = await res.json();
    return data; // expected: array of muscles
  } catch (err) {
    console.error(err);
    return [];
  }
}
