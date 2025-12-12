import React, { useEffect, useState } from "react";
import { FaDumbbell, FaArrowLeft } from "react-icons/fa";
import { fetchUserById, updateUserById } from "../services/UserService";

const emptyForm = {
  firstname: "",
  lastname: "",
  email: "",
  username: "",
  phone: "",
  dateofbirth: "",
  gender: "",
};

const ProfilePage = ({ setCurrentPage, user, onUserUpdate }) => {
  const userId = user?.UserId ?? user?.userId ?? null;
  const [formData, setFormData] = useState(emptyForm);
  const [isEditing, setIsEditing] = useState(false);
  const [showSuccess, setShowSuccess] = useState(false);
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!userId) return;
    setLoading(true);
    fetchUserById(userId).then((res) => {
      if (res?.error) {
        setError(res.error);
      } else if (res) {
        setFormData({
          firstname: res.firstName ?? res.FirstName ?? "",
          lastname: res.lastName ?? res.LastName ?? "",
          email: res.email ?? res.Email ?? "",
          username: res.username ?? res.Username ?? "",
          phone: res.phoneNumber ?? res.PhoneNumber ?? "",
          dateofbirth: (res.dateOfBirth ?? res.DateOfBirth ?? "").toString(),
          gender: res.gender ?? res.Gender ?? "",
        });
      }
      setLoading(false);
    });
  }, [userId]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSave = async () => {
    if (!userId) {
      setError("Please log in to edit your profile.");
      return;
    }
    setSaving(true);
    setError("");
    const payload = {
      username: formData.username,
      email: formData.email,
      firstName: formData.firstname,
      lastName: formData.lastname,
      phoneNumber: formData.phone,
      dateOfBirth: formData.dateofbirth,
      gender: formData.gender,
    };
    const res = await updateUserById(userId, payload);
    setSaving(false);
    if (res?.error) {
      setError(res.error);
      return;
    }
    setIsEditing(false);
    setShowSuccess(true);
    onUserUpdate?.(res);
  };

  const handleCloseSuccess = () => {
    setShowSuccess(false);
  };

  if (!userId) {
    return (
      <div className="bg-blue-600 min-h-screen flex flex-col items-center justify-center text-white">
        <p>Please log in to view your profile.</p>
        <button
          onClick={() => setCurrentPage("login")}
          className="mt-4 px-4 py-2 bg-white text-blue-600 rounded"
        >
          Go to Login
        </button>
      </div>
    );
  }

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
        {loading ? (
          <p className="text-center text-gray-500">Loading...</p>
        ) : (
          Object.entries(formData).map(([key, value]) => (
            <div key={key} className="flex flex-col">
              <label className="text-gray-700 font-semibold capitalize">
                {key.replace(/([A-Z])/g, " $1")}
              </label>
              <input
                type={key === "dateofbirth" ? "date" : "text"}
                name={key}
                value={value ?? ""}
                onChange={handleChange}
                className={`border rounded px-3 py-2 mt-1 ${
                  isEditing ? "bg-white" : "bg-gray-100 cursor-not-allowed"
                }`}
                disabled={!isEditing}
              />
            </div>
          ))
        )}

        {error && (
          <div className="bg-red-100 text-red-700 text-sm rounded px-3 py-2">{error}</div>
        )}

        <button
          onClick={isEditing ? handleSave : () => setIsEditing(true)}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 mt-2 disabled:opacity-60"
          disabled={loading || saving}
        >
          {isEditing ? (saving ? "Saving..." : "Save Changes") : "Edit Profile"}
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

  
