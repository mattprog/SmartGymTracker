import React, { useState } from "react";

const ExerciseManager = () => {
  const [muscles, setMuscles] = useState(["Chest", "Back", "Legs"]);
  const [selectedMuscle, setSelectedMuscle] = useState("Chest");
  const [exercisesByMuscle, setExercisesByMuscle] = useState({
    Chest: ["Bench Press", "Chest Flies"],
    Back: ["Pull-ups", "Lat Pulldown"],
    Legs: ["Squats", "Leg Press"],
  });

  const [newMuscle, setNewMuscle] = useState("");
  const [newExercise, setNewExercise] = useState("");

  const handleAddMuscle = () => {
    if (!newMuscle.trim() || muscles.includes(newMuscle.trim())) return;
    const muscleName = newMuscle.trim();
    setMuscles([...muscles, muscleName]);
    setExercisesByMuscle({ ...exercisesByMuscle, [muscleName]: [] });
    setNewMuscle("");
  };

  const handleDeleteMuscle = (muscleToDelete) => {
    const updatedMuscles = muscles.filter((m) => m !== muscleToDelete);
    const updatedExercises = { ...exercisesByMuscle };
    delete updatedExercises[muscleToDelete];
    setMuscles(updatedMuscles);
    setExercisesByMuscle(updatedExercises);
    if (selectedMuscle === muscleToDelete) setSelectedMuscle(updatedMuscles[0] || "");
  };

  const handleAddExercise = () => {
    if (!newExercise.trim()) return;
    setExercisesByMuscle({
      ...exercisesByMuscle,
      [selectedMuscle]: [...exercisesByMuscle[selectedMuscle], newExercise.trim()],
    });
    setNewExercise("");
  };

  const handleDeleteExercise = (exerciseToDelete) => {
    setExercisesByMuscle({
      ...exercisesByMuscle,
      [selectedMuscle]: exercisesByMuscle[selectedMuscle].filter((e) => e !== exerciseToDelete),
    });
  };

  return (
    <div className="bg-gray-100 p-6 min-h-full">

      <h2 className="text-2xl font-bold mb-6">Exercise & Muscle Manager</h2>

      {/* Muscle Groups Section */}
      <div className="bg-white shadow-md rounded p-6 mb-6 w-full">
        <h3 className="text-xl font-bold mb-4">Muscle Groups</h3>
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
          {muscles.map((muscle) => (
            <li key={muscle} className="flex justify-between items-center border-b pb-2">
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

      {/* Exercises Section */}
      <div className="bg-white shadow-md rounded p-6 w-full">
        <h3 className="text-xl font-bold mb-4">Exercises</h3>
        <div className="flex gap-2 mb-4 items-center">
          <select
            value={selectedMuscle}
            onChange={(e) => setSelectedMuscle(e.target.value)}
            className="border rounded px-3 py-2 w-32"
          >
            {muscles.map((muscle) => (
              <option key={muscle} value={muscle}>
                {muscle}
              </option>
            ))}
          </select>
          <input
            type="text"
            value={newExercise}
            onChange={(e) => setNewExercise(e.target.value)}
            className="border rounded px-3 py-2 flex-1"
            placeholder={`Add exercise for ${selectedMuscle}`}
          />
          <button
            onClick={handleAddExercise}
            className="bg-blue-600 text-white px-4 py-2 rounded"
          >
            Add
          </button>
        </div>

        <ul className="space-y-2">
          {exercisesByMuscle[selectedMuscle]?.map((exercise, idx) => (
            <li key={idx} className="flex justify-between items-center border-b pb-2">
              <span>{exercise}</span>
              <button
                onClick={() => handleDeleteExercise(exercise)}
                className="text-red-500 hover:underline"
              >
                Delete
              </button>
            </li>
          ))}
        </ul>
      </div>

    </div>
  );
};

export default ExerciseManager;

