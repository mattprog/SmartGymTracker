import React, { useState, useEffect } from "react";
import { FaArrowLeft } from "react-icons/fa";
import { resetPassword } from "../services/AuthService";

const ForgotPassword = ({ setCurrentPage }) => {
  const [email, setEmail] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [sent, setSent] = useState(false);
  const [countdown, setCountdown] = useState(60);
  const [error, setError] = useState("");

  useEffect(() => {
    let timer;
    if (sent && countdown > 0) {
      timer = setInterval(() => setCountdown(prev => prev - 1), 1000);
    }
    return () => clearInterval(timer);
  }, [sent, countdown]);

  const handleSend = async () => {
    if (!email || !newPassword) {
      setError("Please enter both email and a new password.");
      return;
    }

    setError("");
    setLoading(true);
    try {
      const res = await resetPassword({ email, password: newPassword });
      if (res?.error) {
        setError(res.error);
        return;
      }
      setSent(true);
      setCountdown(60);
    } catch (err) {
      console.error(err);
      setError("Failed to reset password.");
    } finally {
      setLoading(false);
    }
  };

  const handleResend = () => {
    setSent(false);
    setCountdown(60);
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-blue-600 p-4">
      <div className="bg-white rounded shadow-md p-8 w-full max-w-md text-center">
        <button
          onClick={() => setCurrentPage("login")}
          className="flex items-center gap-2 text-blue-600 font-bold mb-6 text-left"
        >
          <FaArrowLeft /> Back to Login
        </button>

        <h2 className="text-2xl font-bold mb-4">Forgot Password</h2>
        <p className="text-gray-700 mb-6">
          Enter the email associated with your account and your new password.
        </p>

        {error && (
          <div className="bg-red-100 text-red-700 text-sm rounded p-3 mb-4">
            {error}
          </div>
        )}

        <input
          type="email"
          placeholder="Enter your email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="border rounded px-4 py-2 w-full mb-4"
        />

        <input
          type="password"
          placeholder="Enter new password"
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          className="border rounded px-4 py-2 w-full mb-4"
        />

        <button
          onClick={handleSend}
          className="bg-blue-600 text-white px-4 py-2 rounded w-full hover:bg-blue-700 mb-4 flex items-center justify-center"
        >
          {loading && (
            <div className="mr-2 w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
          )}
          Send Reset Link
        </button>

        {sent && (
          <p className="text-gray-700 text-sm">
            {countdown > 0
              ? `Password reset sent. Resend in ${countdown}s`
              : <button onClick={handleResend} className="text-blue-600 underline">Resend</button>}
          </p>
        )}
      </div>
    </div>
  );
};

export default ForgotPassword;

