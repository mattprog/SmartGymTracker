import React from "react";

const CurrentSets = ({ workout }) => {
  const sectionStyle = { border: "1px solid #ddd", padding: "15px", borderRadius: "8px", marginTop: "15px", backgroundColor: "#f9f9f9" };

  const groupedSets = {};
  (workout.sets || []).forEach((s) => {
    if (!groupedSets[s.exerciseName]) groupedSets[s.exerciseName] = [];
    groupedSets[s.exerciseName].push(s.setObj);
  });

  return (
    <div style={sectionStyle}>
      <h3>Current Sets:</h3>
      {Object.keys(groupedSets).map((ex) => (
        <div key={ex} style={{ marginBottom: "10px" }}>
          <strong>{ex}</strong>
          <ul>
            {groupedSets[ex].map((s, idx) => (
              <li key={idx}>
                {s.SetNumber !== undefined ? `Set ${s.SetNumber}: ${s.Weight} lbs x ${s.Reps} reps` : `Cardio: ${s.Duration} min, ${s.Distance} miles`}
              </li>
            ))}
          </ul>
        </div>
      ))}
    </div>
  );
};

export default CurrentSets;
