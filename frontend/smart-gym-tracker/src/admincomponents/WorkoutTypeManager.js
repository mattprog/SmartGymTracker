import React, { useState } from "react";

const WorkoutTypeManager = () => {
  const [types, setTypes] = useState(["Strength", "Cardio"]); // sample initial data
  const [newType, setNewType] = useState("");

  const handleAddType = () => {
    if (!newType.trim()) return;
    setTypes([...types, newType.trim()]);
    setNewType("");
  };

  const handleDeleteType = (typeToDelete) => {
    setTypes(types.filter((t) => t !== typeToDelete));
  };

  return (
    <div className="bg-white shadow-md rounded p-6 mb-6">
      <h2 className="text-xl font-bold mb-4">Workout Types</h2>
      <div className="flex gap-2 mb-4">
        <input
          type="text"
          value={newType}
          onChange={(e) => setNewType(e.target.value)}
          className="border rounded px-3 py-2 flex-1"
          placeholder="Add workout type"
        />
        <button
          onClick={handleAddType}
          className="bg-blue-600 text-white px-4 py-2 rounded"
        >
          Add
        </button>
      </div>
      <ul className="space-y-2">
        {types.map((type, idx) => (
          <li
            key={idx}
            className="flex justify-between items-center border-b pb-2"
          >
            <span>{type}</span>
            <button
              onClick={() => handleDeleteType(type)}
              className="text-red-500 hover:underline"
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default WorkoutTypeManager;
