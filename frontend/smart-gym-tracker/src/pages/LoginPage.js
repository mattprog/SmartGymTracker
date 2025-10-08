import { useState } from "react";

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const handleLogin = () => {
    if (!username || !password) {
      setMessage("Please enter both username and password.");
      return;
    }
    // For now, just log to console
    console.log("Login attempt:", { username, password });
    setMessage("Login attempted! (Check console for details)");
  };

  return (
    <div className="bg-white shadow-md rounded p-6 max-w-md mx-auto mt-10">
      <h2 className="text-xl font-bold mb-4">Login</h2>
      {message && <p className="mb-2 text-red-500">{message}</p>}
      <input
        type="text"
        placeholder="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        className="border rounded px-3 py-2 w-full mb-3"
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        className="border rounded px-3 py-2 w-full mb-3"
      />
      <div className="flex justify-between items-center mb-3">
        <button
          onClick={handleLogin}
          className="bg-blue-600 text-white px-4 py-2 rounded"
        >
          Login
        </button>
        <button
          onClick={() => alert("Forgot Password clicked!")}
          className="text-sm text-gray-500 underline"
        >
          Forgot Password?
        </button>
      </div>
    </div>
  );
};

export default LoginPage;

