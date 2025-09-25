const API_BASE = "http://localhost:5000/api/workouts";

export async function fetchWorkouts(userId) {
  try {
    const url = userId ? `${API_BASE}?userId=${userId}` : API_BASE;
    const res = await fetch(url);
    if (!res.ok) throw new Error("Failed to fetch workouts");
    const data = await res.json();
    return data; // expected: array of workouts
  } catch (err) {
    console.error(err);
    return [];
  }
}
