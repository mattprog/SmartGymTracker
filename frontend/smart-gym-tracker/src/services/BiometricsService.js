const METRICS_API = process.env.REACT_APP_METRICS_API ?? "http://localhost:5153/api";
const API_BASE = `${METRICS_API}/biometrics`;

const jsonOrThrow = async (res) => {
  if (!res.ok) {
    const errText = await res.text();
    throw new Error(errText || "Biometrics request failed");
  }
  return res.json();
};

export async function fetchBiometrics(userId) {
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

export async function postBiometrics(bioData) {
  try {
    const res = await fetch(API_BASE, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(bioData),
    });
    return await jsonOrThrow(res);
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}

export async function deleteBiometric(id) {
  try {
    const res = await fetch(`${API_BASE}/${id}`, { method: "DELETE" });
    return await jsonOrThrow(res);
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}
