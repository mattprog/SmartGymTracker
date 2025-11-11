const API_BASE = "http://localhost:5074/api/auth";

export async function login({ username, password }) {
  try {
    const res = await fetch(`${API_BASE}/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password }),
    });
    if (!res.ok) {
      const errText = await res.text();
      throw new Error(errText || "Login failed");
    }
    return await res.json(); // { message, user }
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}

export async function register(userData) {
  // userData = { username, password, email, firstName, lastName?, phoneNumber?, dateOfBirth?, gender? }
  try {
    const res = await fetch(`${API_BASE}/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(userData),
    });
    if (!res.ok) {
      const errText = await res.text();
      throw new Error(errText || "Registration failed");
    }
    return await res.json(); // created user object
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}

export async function resetPassword({ email, password }) {
  try {
    const res = await fetch(`${API_BASE}/password-reset`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });
    if (!res.ok) {
      const errText = await res.text();
      throw new Error(errText || "Password reset failed");
    }
    return await res.json(); // { message }
  } catch (err) {
    console.error(err);
    return { error: err.message };
  }
}
