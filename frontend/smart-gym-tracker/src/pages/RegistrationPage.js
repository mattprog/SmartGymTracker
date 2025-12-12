import React, { useState } from "react";
import { FaDumbbell, FaArrowLeft } from "react-icons/fa";
import { register } from "../services/AuthService";

const RegistrationPage = ({ setCurrentPage }) => {
  const [formData, setFormData] = useState({
    firstname: "",
    lastname: "",
    email: "",
    username: "",
    password: "",
    phone: "",
    dateofbirth: "",
    weight: "",
    height: "",
  });

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [showSuccess, setShowSuccess] = useState(false);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleRegister = async () => {
    if (!formData.username || !formData.password || !formData.email) {
      setError("Please fill in Username, Email, and Password.");
      return;
    }

    setError("");
    setLoading(true);
    try {
      const res = await register({
        username: formData.username,
        password: formData.password,
        email: formData.email,
        firstName: formData.firstname,
        lastName: formData.lastname,
        phoneNumber: formData.phone,
        dateOfBirth: formData.dateofbirth,
        gender: "",
      });

      if (res?.error) {
        setError(res.error);
      } else if (res) {
        setShowSuccess(true);
      } else {
        setError("Registration failed.");
      }
    } catch (err) {
      setError(err.message || "Registration failed.");
    } finally {
      setLoading(false);
    }
  };

  const handleCloseSuccess = () => {
    setShowSuccess(false);
    setCurrentPage("login");
    setFormData({
      firstname: "",
      lastname: "",
      email: "",
      username: "",
      password: "",
      phone: "",
      dateofbirth: "",
      weight: "",
      height: "",
    });
  };

  return (
    <div className="bg-blue-600 min-h-screen flex flex-col items-center justify-start py-12">
      {/* Back Arrow */}
      <div
        className="self-start ml-6 mb-4 cursor-pointer text-white text-xl flex items-center gap-1"
        onClick={() => setCurrentPage("login")}
      >
        <FaArrowLeft /> Back
      </div>




      {/* Top Title */}
      <div className="text-3xl font-bold text-white mb-8 flex justify-center items-center gap-2">
        SmartGymTracker <FaDumbbell size={32} className="text-white" />
      </div>




      {/* Registration Box */}
      <div className="bg-white shadow-md rounded p-6 max-w-md w-full">
        <h2 className="text-xl font-bold mb-2">Create New Account</h2>
        <p className="text-sm text-gray-600 mb-4">* indicates required fields</p>



        {/* Error Box */}
        {error && (
          <div className="bg-red-600 text-white rounded p-3 mb-4 flex justify-between items-center">
            <span className="text-sm">{error}</span>
            <button className="font-bold" onClick={() => setError("")}>
              X
            </button>
          </div>
        )}




        <div className="flex flex-col gap-3 mb-4">
          <input
            type="text"
            name="firstname"
            value={formData.firstname}
            onChange={handleChange}
            className="border rounded px-3 py-2"
            placeholder="First Name"
          />
          <input
            type="text"
            name="lastname"
            value={formData.lastname}
            onChange={handleChange}
            className="border rounded px-3 py-2"
            placeholder="Last Name"
          />



          {/* Email (Required) */}
          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">
              Email <span className="text-red-600">*</span>
            </label>
            <input
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              className="border rounded px-3 py-2"
              placeholder="Email"
            />
          </div>




          {/* Username (Required) */}
          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">
              Username <span className="text-red-600">*</span>
            </label>
            <input
              type="text"
              name="username"
              value={formData.username}
              onChange={handleChange}
              className="border rounded px-3 py-2"
              placeholder="Username"
            />
          </div>




          {/* Password (Required) */}
          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1">
              Password <span className="text-red-600">*</span>
            </label>
            <input
              type="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              className="border rounded px-3 py-2"
              placeholder="Password"
            />
          </div>

          <input
            type="text"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            className="border rounded px-3 py-2"
            placeholder="Phone Number"
          />
          <input
            type="date"
            name="dateofbirth"
            value={formData.dateofbirth}
            onChange={handleChange}
            className="border rounded px-3 py-2"
            placeholder="Date of Birth"
          />
          <input
            type="number"
            name="weight"
            value={formData.weight}
            onChange={handleChange}
            className="border rounded px-3 py-2"
            placeholder="Weight (kg)"
          />
          <input
            type="number"
            name="height"
            value={formData.height}
            onChange={handleChange}
            className="border rounded px-3 py-2"
            placeholder="Height (cm)"
          />
        </div>

        <button
          onClick={handleRegister}
          className="bg-blue-600 text-white px-4 py-2 rounded w-full disabled:opacity-60"
          disabled={loading}
        >
          {loading ? "Registering..." : "Register"}
        </button>
      </div>




      {/* Success popup */}
      {showSuccess && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-40 z-50">
          <div className="bg-white rounded shadow-lg p-6 max-w-sm w-full text-center">
            <h3 className="text-xl font-bold mb-4">Registration Successful!</h3>
            <p className="mb-4">
              User "{formData.username}" has been created successfully.
            </p>
            <button
              onClick={handleCloseSuccess}
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Return to Login
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default RegistrationPage;





