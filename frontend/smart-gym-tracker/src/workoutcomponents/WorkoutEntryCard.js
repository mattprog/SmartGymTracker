import React from "react";

const WorkoutHistoryEntry = ({ workout }) => {
  // Group sets by exercise name
  const grouped = {};
  (workout.sets || []).forEach((s) => {
    if (!grouped[s.exerciseName]) grouped[s.exerciseName] = [];
    grouped[s.exerciseName].push(s.setObj);
  });

  return (
    <div
      style={{
        border: "1px solid #ccc",
        padding: "10px",
        marginBottom: "10px",
        borderRadius: "5px",
        backgroundColor: "#fff",
      }}
    >
      <strong>{new Date(workout.WorkoutStart).toLocaleDateString()} Workout</strong> — Duration: {workout.Duration} minutes
      {Object.keys(grouped).map((ex) => (
        <div key={ex} style={{ marginLeft: "10px" }}>
          <strong>{ex}</strong>
          <ul>
            {grouped[ex].map((s, i) => (
              <li key={i}>
                {s.SetNumber !== undefined
                  ? `Set ${s.SetNumber}: ${s.Weight} lbs x ${s.Reps} reps`
                  : `Cardio: ${s.Duration} min, ${s.Distance} miles`}
              </li>
            ))}
          </ul>
        </div>
      ))}
    </div>
  );
};

export default WorkoutHistoryEntry;

