const CORE_API = process.env.REACT_APP_CORE_API ?? "http://localhost:5074/api";
const API_BASE = `${CORE_API}/workouts`;

const jsonOrThrow = async (res) => {
  if (!res.ok) {
    const errText = await res.text();
    throw new Error(errText || "Workout request failed");
  }
  return res.json();
};

export async function fetchWorkouts(userId) {
  try {
    const url = userId ? `${API_BASE}?userId=${userId}` : API_BASE;
    const res = await fetch(url);
    const data = await jsonOrThrow(res);
    if (Array.isArray(data)) return data;
    if (Array.isArray(data.data)) return data.data;
    return [];
  } catch (err) {
    console.error(err);
    return [];
  }
}

export async function createWorkout(payload) {
  try {
    const res = await fetch(API_BASE, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });
    return await jsonOrThrow(res);
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}

export async function updateWorkout(id, payload) {
  try {
    const res = await fetch(`${API_BASE}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });
    return await jsonOrThrow(res);
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}

export async function deleteWorkout(id) {
  try {
    const res = await fetch(`${API_BASE}/${id}`, { method: "DELETE" });
    return await jsonOrThrow(res);
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}
