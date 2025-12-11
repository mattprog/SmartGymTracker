const CORE_API = process.env.REACT_APP_CORE_API ?? "http://localhost:5074/api";

const jsonOrThrow = async (res) => {
  if (!res.ok) {
    const errText = await res.text();
    throw new Error(errText || "Request failed");
  }
  return res.json();
};

export async function fetchUserById(userId) {
  if (!userId) return null;
  try {
    const res = await fetch(`${CORE_API}/user/${userId}`);
    return await jsonOrThrow(res);
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}

export async function updateUserById(userId, payload) {
  if (!userId) return { error: "Missing user id" };
  try {
    const res = await fetch(`${CORE_API}/user/${userId}`, {
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
