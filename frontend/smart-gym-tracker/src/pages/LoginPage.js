import React, { useState } from "react";
import { FaDumbbell } from "react-icons/fa";
import { login } from "../services/AuthService";

const LoginPage = ({ setCurrentPage, onLoginSuccess }) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    if (!username || !password) {
      setMessage("Please enter both username and password.");
      return;
    }

    setMessage("");
    setLoading(true);

    try {
      const response = await login({ username, password });
      if (response?.error) {
        setMessage(response.error);
        return;
      }
      if (response?.user) {
        onLoginSuccess?.(response.user);
      } else {
        setMessage("Unexpected response from server.");
      }
    } catch (err) {
      console.error(err);
      setMessage("Unable to login. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-blue-600 px-4">

      
      {/* Heading inside the blue background */}
      <div className="text-3xl font-bold text-white mb-8 flex items-center gap-2">
        SmartGymTracker <FaDumbbell size={32} />
        {loading && (
          <div className="ml-2 w-6 h-6 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
        )}
      </div>




      {/* Form Card */}
      <div className="bg-white shadow-md rounded p-6 max-w-md w-full">
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
            onClick={() => setCurrentPage("forgotPassword")}
            className="text-sm text-gray-500 underline"
          >
            Forgot Password?
          </button>
        </div>
        <p className="text-sm mt-2">
          Don't have an account?{" "}
          <span
            onClick={() => setCurrentPage("registration")}
            className="text-blue-600 underline cursor-pointer"
          >
            Sign up
          </span>
        </p>
      </div>
    </div>
  );
};

export default LoginPage;


