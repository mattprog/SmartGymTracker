import React, { useState } from "react";

const ExerciseManager = () => {
  const [exercises, setExercises] = useState([]); // empty for now
  const [newExercise, setNewExercise] = useState("");

  const handleAddExercise = () => {
    if (!newExercise.trim()) return;
    setExercises([...exercises, newExercise.trim()]);
    setNewExercise("");
  };

  const handleDeleteExercise = (exerciseToDelete) => {
    setExercises(exercises.filter((e) => e !== exerciseToDelete));
  };

  return (
    <div className="bg-white shadow-md rounded p-6 mb-6">
      <h2 className="text-xl font-bold mb-4">Exercises</h2>
      <div className="flex gap-2 mb-4">
        <input
          type="text"
          value={newExercise}
          onChange={(e) => setNewExercise(e.target.value)}
          className="border rounded px-3 py-2 flex-1"
          placeholder="Add exercise"
          disabled // keep disabled until backend is ready
        />
        <button
          onClick={handleAddExercise}
          className="bg-blue-600 text-white px-4 py-2 rounded"
          disabled // keep disabled until backend is ready
        >
          Add
        </button>
      </div>
      <ul className="space-y-2">
        {exercises.map((exercise, idx) => (
          <li
            key={idx}
            className="flex justify-between items-center border-b pb-2"
          >
            <span>{exercise}</span>
            <button
              onClick={() => handleDeleteExercise(exercise)}
              className="text-red-500 hover:underline"
              disabled // keep disabled until backend is ready
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ExerciseManager;

