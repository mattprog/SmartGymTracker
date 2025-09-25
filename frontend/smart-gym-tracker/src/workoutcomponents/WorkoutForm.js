import React from "react";

const WorkoutForm = ({
  workout, setWorkout, exerciseType, setExerciseType,
  muscle, setMuscle, exerciseName, setExerciseName,
  setNumber, setSetNumber, weight, setWeight,
  reps, setReps, cardioDuration, setCardioDuration,
  cardioDistance, setCardioDistance, sampleMuscles,
  handleAddSet, handleAddWorkout
}) => {
  const inputStyle = { padding: "6px 10px", margin: "5px 0", borderRadius: "5px", border: "1px solid #ccc", width: "150px" };
  const buttonStyle = { padding: "8px 16px", margin: "10px 5px 10px 0", borderRadius: "5px", backgroundColor: "#007bff", color: "white", border: "none", cursor: "pointer" };
  const sectionStyle = { border: "1px solid #ddd", padding: "15px", borderRadius: "8px", marginTop: "15px", backgroundColor: "#f9f9f9" };

  return (
    <>
      {/* Date & Duration Section */}
      <div style={sectionStyle}>
        <div>
          <label>Date:</label>
          <input
            type="date"
            value={workout.WorkoutStart ? workout.WorkoutStart.split('T')[0] : ''}
            onChange={(e) => setWorkout({ ...workout, WorkoutStart: e.target.value })}
            style={inputStyle}
          />
        </div>
        <div>
          <label>Duration (minutes):</label>
          <input
            type="number"
            min={0}
            value={workout.Duration || ""}
            onChange={(e) => setWorkout({ ...workout, Duration: parseInt(e.target.value) })}
            style={inputStyle}
          />
        </div>

        {/* Strength/Cardio selection */}
        <div style={{ marginTop: "10px" }}>
          <label>
            <input type="radio" checked={exerciseType === "strength"} onChange={() => setExerciseType("strength")} /> Strength
          </label>
          <label style={{ marginLeft: "20px" }}>
            <input type="radio" checked={exerciseType === "cardio"} onChange={() => setExerciseType("cardio")} /> Cardio
          </label>
        </div>
      </div>

      {/* Exercise Fields Section (new box) */}
      {exerciseType && (
        <div style={sectionStyle}>
          <div>
            <label>Muscle Group:</label>
            <select value={muscle} onChange={(e) => setMuscle(e.target.value)} style={inputStyle}>
              <option value="">Select Muscle</option>
              {sampleMuscles.map((m) => <option key={m.MuscleId} value={m.MuscleId}>{m.Name}</option>)}
            </select>
          </div>

          <div>
            <label>Exercise Name:</label>
            <input type="text" value={exerciseName} onChange={(e) => setExerciseName(e.target.value)} style={inputStyle} />
          </div>

          {exerciseType === "strength" ? (
            <div style={{ marginTop: "10px" }}>
              <label>Set Number:</label>
              <input type="number" min={1} value={setNumber} onChange={(e) => setSetNumber(parseInt(e.target.value))} style={inputStyle} />
              <br />
              <label>Weight (lbs):</label>
              <input type="number" min={0} value={weight} onChange={(e) => setWeight(e.target.value)} style={inputStyle} />
              <br />
              <label>Reps:</label>
              <input type="number" min={0} value={reps} onChange={(e) => setReps(e.target.value)} style={inputStyle} />
            </div>
          ) : (
            <div style={{ marginTop: "10px" }}>
              <label>Duration (minutes):</label>
              <input type="number" min={0} value={cardioDuration} onChange={(e) => setCardioDuration(e.target.value)} style={inputStyle} />
              <br />
              <label>Distance (miles):</label>
              <input type="number" min={0} value={cardioDistance} onChange={(e) => setCardioDistance(e.target.value)} style={inputStyle} />
            </div>
          )}

          <div>
            <button onClick={handleAddSet} style={buttonStyle}>Add Set</button>
            <button onClick={handleAddWorkout} style={buttonStyle}>Add Workout</button>
          </div>
        </div>
      )}
    </>
  );
};

export default WorkoutForm;

