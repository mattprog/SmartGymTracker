import React, { useEffect, useState } from "react";
import { fetchMuscles, createMuscle, deleteMuscle } from "../services/MuscleService";

const MuscleManager = () => {
  const [muscles, setMuscles] = useState([]);
  const [newMuscle, setNewMuscle] = useState("");
  const [description, setDescription] = useState("");
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    void loadMuscles();
  }, []);

  const loadMuscles = async () => {
    setLoading(true);
    const data = await fetchMuscles();
    setMuscles(data ?? []);
    setLoading(false);
  };

  const handleAddMuscle = async () => {
    if (!newMuscle.trim()) return;
    setSubmitting(true);
    setError("");
    const res = await createMuscle({
      name: newMuscle.trim(),
      description: description.trim(),
    });
    setSubmitting(false);
    if (res?.error) {
      setError(res.error);
      return;
    }
    setNewMuscle("");
    setDescription("");
    await loadMuscles();
  };

  const handleDeleteMuscle = async (id) => {
    if (!id) return;
    await deleteMuscle(id);
    await loadMuscles();
  };

  return (
    <div className="bg-white shadow-md rounded p-6 mb-6">
      <h2 className="text-xl font-bold mb-4">Muscle Groups</h2>
      {error && (
        <div className="bg-red-100 text-red-700 rounded px-3 py-2 text-sm mb-4">
          {error}
        </div>
      )}
      <div className="flex flex-col gap-2 mb-4">
        <input
          type="text"
          value={newMuscle}
          onChange={(e) => setNewMuscle(e.target.value)}
          className="border rounded px-3 py-2"
          placeholder="Muscle name"
        />
        <textarea
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="border rounded px-3 py-2"
          placeholder="Description (optional)"
        />
        <button
          onClick={handleAddMuscle}
          className="bg-blue-600 text-white px-4 py-2 rounded disabled:opacity-60"
          disabled={submitting}
        >
          {submitting ? "Adding..." : "Add"}
        </button>
      </div>
      <ul className="space-y-2 max-h-64 overflow-y-auto">
        {loading ? (
          <li className="text-gray-500">Loading...</li>
        ) : muscles.length === 0 ? (
          <li className="text-gray-500">No muscles defined yet.</li>
        ) : (
          muscles.map((muscle) => (
            <li
              key={muscle.muscleId ?? muscle.MuscleId}
              className="flex justify-between items-center border-b pb-2 gap-4"
            >
              <div>
                <span className="font-semibold block">{muscle.name ?? muscle.Name}</span>
                {muscle.description && (
                  <span className="text-sm text-gray-600">{muscle.description}</span>
                )}
              </div>
              <button
                onClick={() => handleDeleteMuscle(muscle.muscleId ?? muscle.MuscleId)}
                className="text-red-500 hover:underline"
              >
                Delete
              </button>
            </li>
          ))
        )}
      </ul>
    </div>
  );
};

export default MuscleManager;
