export default function GoalForm({
    newGoalType,
    setNewGoalType,
    newGoalName,
    setNewGoalName,
    newGoalTarget,
    setNewGoalTarget,
    newFreqUnit,
    setNewFreqUnit,
    newCardioUnit,
    setNewCardioUnit,
    addGoal,
  }) {
    return (
      <div className="bg-white shadow-md rounded-lg p-6 w-full max-w-2xl mx-auto flex flex-col gap-3 hover:shadow-lg transition">
        <h2 className="text-xl font-semibold">Create New Goal</h2>
        <select
          value={newGoalType}
          onChange={(e) => setNewGoalType(e.target.value)}
          className="border p-2 rounded w-full"
        >
          <option value="weight">Weight Goal</option>
          <option value="strength">Strength Goal</option>
          <option value="frequency">Workout Frequency Goal</option>
          <option value="cardio">Cardio Goal</option>
        </select>
  
        {newGoalType === "weight" && (
          <input
            type="number"
            placeholder="Target Weight (lbs)"
            value={newGoalTarget}
            onChange={(e) => setNewGoalTarget(e.target.value)}
            className="border p-2 rounded w-full"
          />
        )}
  
        {newGoalType === "strength" && (
          <>
            <input
              type="text"
              placeholder="Workout Name (e.g., Bench Press)"
              value={newGoalName}
              onChange={(e) => setNewGoalName(e.target.value)}
              className="border p-2 rounded w-full"
            />
            <input
              type="number"
              placeholder="Target Weight (lbs)"
              value={newGoalTarget}
              onChange={(e) => setNewGoalTarget(e.target.value)}
              className="border p-2 rounded w-full"
            />
          </>
        )}
  
        {newGoalType === "frequency" && (
          <>
            <select
              value={newFreqUnit}
              onChange={(e) => setNewFreqUnit(e.target.value)}
              className="border p-2 rounded w-full"
            >
              <option value="week">Per Week</option>
              <option value="month">Per Month</option>
            </select>
            <input
              type="number"
              placeholder="Number of Workouts"
              value={newGoalTarget}
              onChange={(e) => setNewGoalTarget(e.target.value)}
              className="border p-2 rounded w-full"
            />
          </>
        )}
  
        {newGoalType === "cardio" && (
          <>
            <input
              type="text"
              placeholder="Workout Name (e.g., Running)"
              value={newGoalName}
              onChange={(e) => setNewGoalName(e.target.value)}
              className="border p-2 rounded w-full"
            />
            <input
              type="number"
              placeholder={`Target (${newCardioUnit})`}
              value={newGoalTarget}
              onChange={(e) => setNewGoalTarget(e.target.value)}
              className="border p-2 rounded w-full"
            />
            <select
              value={newCardioUnit}
              onChange={(e) => setNewCardioUnit(e.target.value)}
              className="border p-2 rounded w-full"
            >
              <option value="miles">Miles</option>
              <option value="minutes">Minutes</option>
            </select>
          </>
        )}
  
        <button
          onClick={addGoal}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 w-1/2 self-center"
        >
          Add Goal
        </button>
      </div>
    );
  }
  
  