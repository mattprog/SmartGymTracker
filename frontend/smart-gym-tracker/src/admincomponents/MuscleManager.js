import React, { useState } from "react";

const MuscleManager = () => {
  const [muscles, setMuscles] = useState(["Chest", "Back", "Legs"]); // sample initial data
  const [newMuscle, setNewMuscle] = useState("");

  const handleAddMuscle = () => {
    if (!newMuscle.trim()) return;
    setMuscles([...muscles, newMuscle.trim()]);
    setNewMuscle("");
  };

  const handleDeleteMuscle = (muscleToDelete) => {
    setMuscles(muscles.filter((m) => m !== muscleToDelete));
  };

  return (
    <div className="bg-white shadow-md rounded p-6 mb-6">
      <h2 className="text-xl font-bold mb-4">Muscle Groups</h2>
      <div className="flex gap-2 mb-4">
        <input
          type="text"
          value={newMuscle}
          onChange={(e) => setNewMuscle(e.target.value)}
          className="border rounded px-3 py-2 flex-1"
          placeholder="Add muscle group"
        />
        <button
          onClick={handleAddMuscle}
          className="bg-blue-600 text-white px-4 py-2 rounded"
        >
          Add
        </button>
      </div>
      <ul className="space-y-2">
        {muscles.map((muscle, idx) => (
          <li
            key={idx}
            className="flex justify-between items-center border-b pb-2"
          >
            <span>{muscle}</span>
            <button
              onClick={() => handleDeleteMuscle(muscle)}
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

export default MuscleManager;
