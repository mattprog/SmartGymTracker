import React, { useState, useEffect } from "react";
import { FaArrowLeft } from "react-icons/fa";

const ForgotPassword = ({ setCurrentPage }) => {
  const [email, setEmail] = useState("");
  const [sent, setSent] = useState(false);
  const [countdown, setCountdown] = useState(60);

  useEffect(() => {
    let timer;
    if (sent && countdown > 0) {
      timer = setInterval(() => setCountdown(prev => prev - 1), 1000);
    }
    return () => clearInterval(timer);
  }, [sent, countdown]);

  const handleSend = () => {
    if (!email) return alert("Please enter your email");
    // Simulate sending email
    alert(`Password reset link sent to ${email}`);
    setSent(true);
    setCountdown(60);
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
           Enter the email associated with your account, and we’ll send a password reset link.
        </p>

        <input
          type="email"
          placeholder="Enter your email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="border rounded px-4 py-2 w-full mb-4"
        />

        <button
          onClick={handleSend}
          className="bg-blue-600 text-white px-4 py-2 rounded w-full hover:bg-blue-700 mb-4"
        >
          Send Reset Link
        </button>

        {sent && (
          <p className="text-gray-700 text-sm">
            {countdown > 0
              ? `Didn't receive email? Resend in ${countdown}s`
              : <button onClick={handleResend} className="text-blue-600 underline">Resend</button>}
          </p>
        )}
      </div>
    </div>
  );
};

export default ForgotPassword;
