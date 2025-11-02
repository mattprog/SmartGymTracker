import React, { useState } from "react";

const initialUsers = [
  { id: 1, username: "janedoe", email: "jdoe@example.com", role: "User", status: "Active" },
  { id: 2, username: "austinsmith", email: "asmith@example.com", role: "Admin", status: "Active" },
  { id: 3, username: "martinjones", email: "mjones@example.com", role: "User", status: "Inactive" },
  { id: 4, username: "amandaorama", email: "amanda@example.com", role: "Admin", status: "Active", password: "mandy123" },
  { id: 5, username: "admin", email: "admin@example.com", role: "Admin", status: "Active", password: "password" },
  { id: 6, username: "rockwayne", email: "bwayne@example.com", role: "User", status: "Active" },
  { id: 7, username: "matthew", email: "matthew@example.com", role: "User", status: "Inactive" },
  { id: 8, username: "shannon", email: "pparker@example.com", role: "User", status: "Active" },
  { id: 9, username: "ashton", email: "tstark@example.com", role: "Admin", status: "Active" },
  { id: 10, username: "martinaberdnt", email: "srogers@example.com", role: "User", status: "Inactive" },
];

const UserManagement = () => {
  const [users, setUsers] = useState(initialUsers);
  const [editUserId, setEditUserId] = useState(null);
  const [deleteUserId, setDeleteUserId] = useState(null);
  const [resetUserId, setResetUserId] = useState(null);

  const handleEdit = (id) => setEditUserId(id);
  const handleSave = (id) => setEditUserId(null);
  const handleDelete = (id) => {
    setUsers(users.filter(u => u.id !== id));
    setDeleteUserId(null);
  };
  const handleResetPassword = (id) => {
    alert(`Reset password link sent to: ${users.find(u => u.id === id).email}`);
    setResetUserId(null);
  };
  const handleChange = (id, field, value) => {
    setUsers(users.map(u => u.id === id ? { ...u, [field]: value } : u));
  };

  return (
    <div className="bg-gray-100 p-6 min-h-full">
      <h2 className="text-2xl font-bold mb-4">User Management</h2>
      <div className="overflow-x-auto bg-white shadow-md rounded">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-blue-600 text-white">
            <tr>
              <th className="px-4 py-2 text-left">Username</th>
              <th className="px-4 py-2 text-left">Email</th>
              <th className="px-4 py-2 text-left">Role</th>
              <th className="px-4 py-2 text-left">Status</th>
              <th className="px-4 py-2 text-left">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {users.map(user => (
              <tr key={user.id}>
                <td className="px-4 py-2">
                  {editUserId === user.id ? (
                    <input
                      type="text"
                      value={user.username}
                      onChange={(e) => handleChange(user.id, "username", e.target.value)}
                      className="border rounded px-2 py-1 w-full"
                    />
                  ) : user.username}
                </td>
                <td className="px-4 py-2">
                  {editUserId === user.id ? (
                    <input
                      type="email"
                      value={user.email}
                      onChange={(e) => handleChange(user.id, "email", e.target.value)}
                      className="border rounded px-2 py-1 w-full"
                    />
                  ) : user.email}
                </td>
                <td className="px-4 py-2">
                  {editUserId === user.id ? (
                    <select
                      value={user.role}
                      onChange={(e) => handleChange(user.id, "role", e.target.value)}
                      className="border rounded px-2 py-1 w-full"
                    >
                      <option>User</option>
                      <option>Admin</option>
                    </select>
                  ) : user.role}
                </td>
                <td className="px-4 py-2">
                  {editUserId === user.id ? (
                    <select
                      value={user.status}
                      onChange={(e) => handleChange(user.id, "status", e.target.value)}
                      className="border rounded px-2 py-1 w-full"
                    >
                      <option>Active</option>
                      <option>Inactive</option>
                    </select>
                  ) : user.status}
                </td>
                <td className="px-4 py-2 flex gap-2">
                  {editUserId === user.id ? (
                    <button
                      onClick={() => handleSave(user.id)}
                      className="bg-green-600 text-white px-2 py-1 rounded"
                    >
                      Save
                    </button>
                  ) : (
                    <button
                      onClick={() => handleEdit(user.id)}
                      className="bg-yellow-500 text-white px-2 py-1 rounded"
                    >
                      Edit
                    </button>
                  )}
                  <button
                    onClick={() => setDeleteUserId(user.id)}
                    className="bg-red-600 text-white px-2 py-1 rounded"
                  >
                    Delete
                  </button>
                  <button
                    onClick={() => setResetUserId(user.id)}
                    className="bg-blue-600 text-white px-2 py-1 rounded"
                  >
                    Reset Password
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Delete Confirmation Modal */}
      {deleteUserId && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center">
          <div className="bg-white p-6 rounded shadow-md w-96">
            <h3 className="text-xl font-bold mb-4">Confirm Delete</h3>
            <p>Are you sure you want to delete {users.find(u => u.id === deleteUserId).username}?</p>
            <div className="flex justify-end gap-4 mt-4">
              <button
                onClick={() => setDeleteUserId(null)}
                className="px-4 py-2 rounded border"
              >
                Cancel
              </button>
              <button
                onClick={() => handleDelete(deleteUserId)}
                className="px-4 py-2 rounded bg-red-600 text-white"
              >
                Delete
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Reset Password Modal */}
      {resetUserId && (
        <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center">
          <div className="bg-white p-6 rounded shadow-md w-96">
            <h3 className="text-xl font-bold mb-4">Reset Password</h3>
            <p>Send a password reset link to {users.find(u => u.id === resetUserId).email}?</p>
            <div className="flex justify-end gap-4 mt-4">
              <button
                onClick={() => setResetUserId(null)}
                className="px-4 py-2 rounded border"
              >
                Cancel
              </button>
              <button
                onClick={() => handleResetPassword(resetUserId)}
                className="px-4 py-2 rounded bg-blue-600 text-white"
              >
                Send Link
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default UserManagement;

