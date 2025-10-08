import React, { useState } from "react";

const RegistrationPage = () => {
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

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleRegister = () => {
    // placeholder: will connect to backend later
    alert(`Registering user: ${formData.username}`);
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
    <div className="bg-white shadow-md rounded p-6 mb-6 max-w-md mx-auto mt-10">
      <h2 className="text-xl font-bold mb-4">Create New Account</h2>
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
        <input
          type="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          className="border rounded px-3 py-2"
          placeholder="Email"
        />
        <input
          type="text"
          name="username"
          value={formData.username}
          onChange={handleChange}
          className="border rounded px-3 py-2"
          placeholder="Username"
        />
        <input
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
          className="border rounded px-3 py-2"
          placeholder="Password"
        />
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
        className="bg-blue-600 text-white px-4 py-2 rounded w-full"
      >
        Register
      </button>
    </div>
  );
};

export default RegistrationPage;
