import React, { useState } from "react";
import { FaDumbbell, FaArrowLeft } from "react-icons/fa";

const ProfilePage = ({ setCurrentPage }) => {
  // Mock user data for demo purposes
  const [formData, setFormData] = useState({
    firstname: "User",
    lastname: "User",
    email: "user@example.com",
    username: "username",
    phone: "123-456-7890",
    dateofbirth: "1990-01-01",
    weight: "60",
    height: "165",
  });

  const [isEditing, setIsEditing] = useState(false);
  const [showSuccess, setShowSuccess] = useState(false);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSave = () => {
    // Normally here i;d call an API to save changes
    setIsEditing(false);
    setShowSuccess(true);
  };

  const handleCloseSuccess = () => {
    setShowSuccess(false);
  };

  return (



    <div className="bg-blue-600 min-h-screen flex flex-col items-center justify-start py-12 px-4">
      {/* Back Arrow */}
      <div
        className="self-start mb-4 cursor-pointer text-white text-xl flex items-center gap-1"
        onClick={() => setCurrentPage("dashboard")}
      >
        <FaArrowLeft /> Back
      </div>



      {/* Top Title */}
      <div className="text-3xl font-bold text-white mb-8 flex justify-center items-center gap-2">
        Profile <FaDumbbell size={28} className="text-white" />
      </div>




      {/* Profile Box */}
      <div className="bg-white shadow-md rounded p-6 max-w-md w-full flex flex-col gap-4">
        {Object.entries(formData).map(([key, value]) => (
          <div key={key} className="flex flex-col">
            <label className="text-gray-700 font-semibold capitalize">
              {key.replace(/([A-Z])/g, " $1")}
            </label>
            <input
              type={key === "password" ? "password" : key === "dateofbirth" ? "date" : "text"}
              name={key}
              value={value}
              onChange={handleChange}
              className={`border rounded px-3 py-2 mt-1 ${
                isEditing ? "bg-white" : "bg-gray-100 cursor-not-allowed"
              }`}
              disabled={!isEditing}
            />
          </div>
        ))}

        <button
          onClick={isEditing ? handleSave : () => setIsEditing(true)}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 mt-2"
        >
          {isEditing ? "Save Changes" : "Edit Profile"}
        </button>
      </div>




      {/* Success Modal */}
      {showSuccess && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-40 z-50">
          <div className="bg-white rounded shadow-lg p-6 max-w-sm w-full text-center">
            <h3 className="text-xl font-bold mb-4">Profile Updated!</h3>
            <button
              onClick={handleCloseSuccess}
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Close
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfilePage;

  