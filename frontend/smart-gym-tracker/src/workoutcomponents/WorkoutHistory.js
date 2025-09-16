import React from "react";

const WorkoutHistory = ({ completedWorkouts }) => {
  const sectionStyle = { border: "1px solid #ddd", padding: "15px", borderRadius: "8px", marginTop: "15px", backgroundColor: "#f9f9f9" };

  return (
    <div style={sectionStyle}>
      <h3>Workout History:</h3>
      <div style={{ maxHeight: "300px", overflowY: "auto" }}>
        {completedWorkouts.map((w, idx) => {
          const grouped = {};
          (w.sets || []).forEach((s) => {
            if (!grouped[s.exerciseName]) grouped[s.exerciseName] = [];
            grouped[s.exerciseName].push(s.setObj);
          });

          return (
            <div key={idx} style={{ border: "1px solid #ccc", padding: "10px", marginBottom: "10px", borderRadius: "5px", backgroundColor: "#fff" }}>
              <strong>{new Date(w.WorkoutStart).toLocaleDateString()} Workout</strong> — Duration: {w.Duration} minutes
              {Object.keys(grouped).map((ex) => (
                <div key={ex} style={{ marginLeft: "10px" }}>
                  <strong>{ex}</strong>
                  <ul>
                    {grouped[ex].map((s, i) => (
                      <li key={i}>
                        {s.SetNumber !== undefined ? `Set ${s.SetNumber}: ${s.Weight} lbs x ${s.Reps} reps` : `Cardio: ${s.Duration} min, ${s.Distance} miles`}
                      </li>
                    ))}
                  </ul>
                </div>
              ))}
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default WorkoutHistory;
