const API_BASE = "http://localhost:5000/api/biometrics";

export async function fetchBiometrics(userId) {
  try {
    const url = userId ? `${API_BASE}?userId=${userId}` : API_BASE;
    const res = await fetch(url);
    if (!res.ok) throw new Error("Failed to fetch biometrics");
    const data = await res.json();
    return data; // expected: array of biometrics
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
    if (!res.ok) throw new Error("Failed to post biometrics");
    const data = await res.json();
    return data;
  } catch (err) {
    console.error(err);
    return null;
  }
}
