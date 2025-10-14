import React, { useState } from "react";

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

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  
  const handleRegister = () => {
    // Mock validation
    if (!formData.username || !formData.password || !formData.email) {
      alert("Please fill in username, email, and password.");
      return;
    }
  
    // ----- MOCK REGISTRATION -----
    alert(`User "${formData.username}" created successfully!`);
    
    // Redirect to login page
    setCurrentPage("login");
  
    // Reset form
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
  
 /* const handleRegister = async () => {
    // validate required fields
    if (!formData.username || !formData.password || !formData.email) {
      alert("Please fill in username, email, and password.");
      return;
    }

    try {
      const response = await fetch("/api/Register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          username: formData.username,
          password: formData.password,
          email: formData.email,
          firstname: formData.firstname,
          lastname: formData.lastname,
          phone_number: formData.phone,
          dateofbirth: formData.dateofbirth,
          weight: parseInt(formData.weight),
          height: parseInt(formData.height),
        }),
      });

      if (response.ok) {
        alert("Registration successful!");
        // Redirect to login page
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
      } else {
        const text = await response.text();
        alert(`Registration failed: ${text}`);
      }
    } catch (error) {
      console.error(error);
      alert("An error occurred while registering.");
    }
  };

*/




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
