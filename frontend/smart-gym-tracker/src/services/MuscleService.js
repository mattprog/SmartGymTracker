const CORE_API = process.env.REACT_APP_CORE_API ?? "http://localhost:5074/api";
const API_BASE = `${CORE_API}/muscles`;

const jsonOrThrow = async (res) => {
  if (!res.ok) {
    const errText = await res.text();
    throw new Error(errText || "Muscle request failed");
  }
  return res.json();
};

export async function fetchMuscles() {
  try {
    const res = await fetch(API_BASE);
    const data = await jsonOrThrow(res);
    if (Array.isArray(data)) return data;
    if (Array.isArray(data.data)) return data.data;
    return [];
  } catch (err) {
    console.error(err);
    return [];
  }
}

export async function createMuscle(payload) {
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

export async function deleteMuscle(id) {
  try {
    const res = await fetch(`${API_BASE}/${id}`, { method: "DELETE" });
    return await jsonOrThrow(res);
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}
