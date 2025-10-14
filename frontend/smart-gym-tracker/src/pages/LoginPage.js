import { useState } from "react";

const LoginPage = ({ setCurrentPage }) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");


    
    const handleLogin = async () => {
      if (!username || !password) {
        setMessage("Please enter both username and password.");
        return;
      }
    
      // ----- MOCK LOGIN -----
      if (username === "admin" && password === "password") {
        setMessage("Login successful!");
        setCurrentPage("dashboard"); // redirect to dashboard
      } else {
        setMessage("Login failed: Invalid username or password");
      }
    };
    




    
    /*try {
      const response = await fetch("/api/User", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });
  
      if (response.ok) {
        setMessage("Login successful!");
        setCurrentPage("dashboard"); // redirect to dashboard for now
      } else {
        const text = await response.text();
        setMessage(`Login failed: ${text}`);
      }
    } catch (error) {
      console.error(error);
      setMessage("An error occurred while logging in.");
    }
  };
*/


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
  );
};

export default LoginPage;
