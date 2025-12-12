import { useState, useEffect } from "react";
import GoalCard from "../goalcomponents/GoalCard";
import CompletedGoals from "../goalcomponents/CompletedGoals";
import GoalForm from "../goalcomponents/GoalForm";

export default function Progress() {


  // ----------------------
  // MOCK DATA (for demo purposes since backend is not connected)
  // ----------------------
  const [goals, setGoals] = useState([
    { id: 1, type: "weight", name: "Reach 125 lbs body weight", target: 125, current: 112 },
    { id: 2, type: "strength", name: "Bench Press 315 lbs", target: 315, current: 250 },
    { id: 3, type: "frequency", name: "Complete 5 workouts per week", target: 5, current: 3, freqUnit: "week" },
    { id: 4, type: "cardio", name: "Run 6 miles", target: 6, current: 3, durationUnit: "miles" },
  ]);

  const [completedGoals] = useState([
    { id: 101, type: "weight", name: "Reach 105 lbs body weight", target: 105, current: 105 },
    { id: 102, type: "strength", name: "Bench Press 250 lbs", target: 250, current: 250 },
    { id: 103, type: "cardio", name: "Run 5 miles", target: 5, current: 5, durationUnit: "miles" },
  ]);




  // ----------------------
  // Form state for creating new goals
  // ----------------------
  const [newGoalType, setNewGoalType] = useState("weight");
  const [newGoalName, setNewGoalName] = useState("");
  const [newGoalTarget, setNewGoalTarget] = useState("");
  const [newFreqUnit, setNewFreqUnit] = useState("week");
  const [newCardioUnit, setNewCardioUnit] = useState("miles");

  // ----------------------
  // Celebration state for completed goals
  // ----------------------
  const [celebration, setCelebration] = useState(null);




  // ----------------------
  // Tab state (current vs completed)
  // ----------------------
  const [tab, setTab] = useState("current"); // "current" or "completed"




  // ----------------------
  // Effect to detect goal completion
  // ----------------------
  useEffect(() => {
    goals.forEach((goal) => {
      if (goal.current >= goal.target && !goal.celebrated) {
        setCelebration(goal.name);
        setGoals((prev) =>
          prev.map((g) => (g.id === goal.id ? { ...g, celebrated: true } : g))
        );
        setTimeout(() => setCelebration(null), 3000);
      }
    });
  }, [goals]);




  // ----------------------
  // Function to add a new goal
  // ----------------------
  const addGoal = () => {
    let name = "";
    if (newGoalType === "weight") {
      if (!newGoalTarget) return;
      name = `Reach ${newGoalTarget} lbs body weight`;
    } else if (newGoalType === "strength") {
      if (!newGoalName || !newGoalTarget) return;
      name = `${newGoalName} ${newGoalTarget} lbs`;
    } else if (newGoalType === "frequency") {
      if (!newGoalTarget) return;
      name = `Complete ${newGoalTarget} workouts per ${newFreqUnit}`;
    } else if (newGoalType === "cardio") {
      if (!newGoalName || !newGoalTarget) return;
      name = `${newGoalName} ${newGoalTarget} ${newCardioUnit}`;
    }




    const newGoal = {
      id: Date.now(),
      type: newGoalType,
      name,
      target: Number(newGoalTarget),
      current: 0,
      freqUnit: newGoalType === "frequency" ? newFreqUnit : null,
      durationUnit: newGoalType === "cardio" ? newCardioUnit : null,
    };




    setGoals([...goals, newGoal]);
    setNewGoalName("");
    setNewGoalTarget("");
    setNewFreqUnit("week");
    setNewCardioUnit("miles");
  };




  return (
    <div className="overflow-auto p-6 space-y-6 pb-32">
      <h1 className="text-3xl font-bold text-center mb-4">Progress & Goals</h1>



      {/* Celebration popup for completed goals */}
      {celebration && (
        <div className="fixed top-20 left-1/2 transform -translate-x-1/2 bg-yellow-400 text-white px-4 py-2 rounded shadow-lg z-50 animate-bounce">
          🎉 Goal Completed: {celebration} 🎉
        </div>
      )}




      {/* Tabs */}
      <div className="flex justify-center gap-4 mb-6">
        <button
          className={`px-4 py-2 rounded ${
            tab === "current" ? "bg-blue-600 text-white" : "bg-gray-200"
          }`}
          onClick={() => setTab("current")}
        >
          Current Goals
        </button>
        <button
          className={`px-4 py-2 rounded ${
            tab === "completed" ? "bg-blue-600 text-white" : "bg-gray-200"
          }`}
          onClick={() => setTab("completed")}
        >
          Completed Goals
        </button>
      </div>




      {/* Current Goals */}
      {tab === "current" && (
        <>


          {/* Goal creation form */}
          <GoalForm
            newGoalType={newGoalType}
            setNewGoalType={setNewGoalType}
            newGoalName={newGoalName}
            setNewGoalName={setNewGoalName}
            newGoalTarget={newGoalTarget}
            setNewGoalTarget={setNewGoalTarget}
            newFreqUnit={newFreqUnit}
            setNewFreqUnit={setNewFreqUnit}
            newCardioUnit={newCardioUnit}
            setNewCardioUnit={setNewCardioUnit}
            addGoal={addGoal}
          />



          {/* Display current goals */}
          <div className="space-y-6 max-w-2xl mx-auto">
            {["weight", "strength", "frequency", "cardio"].map((type) => {
              const typeGoals = goals.filter((g) => g.type === type);
              if (!typeGoals.length) return null;
              return (
                <div key={type} className="space-y-3">
                  <h2 className="text-lg font-semibold text-gray-700 capitalize">{type} goals</h2>
                  <div className="flex flex-col gap-3">
                    {typeGoals.map((goal) => (
                      <GoalCard key={goal.id} goal={goal} setGoals={setGoals} />
                    ))}
                  </div>
                </div>
              );
            })}
          </div>
        </>
      )}

      {/* Completed Goals */}
      {tab === "completed" && <CompletedGoals completedGoals={completedGoals} />}
    </div>
  );
}
