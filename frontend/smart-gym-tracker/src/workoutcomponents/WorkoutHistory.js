import React from "react";
import WorkoutEntryCard from "./WorkoutEntryCard";

const WorkoutHistory = ({ completedWorkouts, loading, onDelete }) => {
  const sectionStyle = {
    border: "1px solid #ddd",
    padding: "15px",
    borderRadius: "8px",
    marginTop: "15px",
    backgroundColor: "#f9f9f9",
  };

  return (
    <div style={sectionStyle}>
      <h3>Workout History:</h3>
      <div style={{ maxHeight: "300px", overflowY: "auto" }}>
        {loading ? (
          <p>Loading...</p>
        ) : completedWorkouts.length === 0 ? (
          <p>No workouts logged yet.</p>
        ) : (
          completedWorkouts.map((workout, idx) => (
            <WorkoutEntryCard
              key={workout.WorkoutId ?? workout.workoutId ?? idx}
              workout={workout}
              onDelete={onDelete}
            />
          ))
        )}
      </div>
    </div>
  );
};

export default WorkoutHistory;

