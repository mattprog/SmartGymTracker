import React from "react";

const WorkoutHistoryEntry = ({ workout, onDelete }) => {
  // Group sets by exercise name (local entries only)
  const grouped = {};
  (workout.sets || []).forEach((s) => {
    if (!grouped[s.exerciseName]) grouped[s.exerciseName] = [];
    grouped[s.exerciseName].push(s.setObj);
  });

  const displayDate = workout.WorkoutStart ?? workout.workoutStart;
  const duration = workout.Duration ?? workout.duration;
  const notes = workout.Notes ?? workout.notes;
  const workoutId = workout.WorkoutId ?? workout.workoutId;

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
      <div className="flex justify-between items-center">
        <strong>{displayDate ? new Date(displayDate).toLocaleString() : "Workout"}</strong>
        {onDelete && workoutId && (
          <button
            onClick={() => onDelete(workoutId)}
            className="text-sm text-red-600 hover:underline"
          >
            Delete
          </button>
        )}
      </div>
      <div>Duration: {duration ?? "-"} minutes</div>
      {notes && <div>Notes: {notes}</div>}
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

