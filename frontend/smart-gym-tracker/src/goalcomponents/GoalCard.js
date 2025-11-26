import { FaTrophy } from "react-icons/fa";

export default function GoalCard({ goal, setGoals, isCompleted = false }) {
  const percent = isCompleted ? 100 : Math.floor((goal.current / goal.target) * 100);
  const barColor = isCompleted ? "bg-yellow-400" : "bg-green-500";

  return (
    <div
      key={goal.id}
      className="bg-white shadow-md rounded-lg p-4 hover:shadow-lg transition relative"
    >
      {isCompleted && (
        <div className="absolute top-2 right-2 text-yellow-500">
          <FaTrophy />
        </div>
      )}
      {!isCompleted && setGoals && (
        <div
          className="absolute top-2 right-2 cursor-pointer text-red-600 font-bold"
          onClick={() => setGoals((prev) => prev.filter((g) => g.id !== goal.id))}
        >
          ×
        </div>
      )}
      <h3 className="font-semibold mb-2">{goal.name}</h3>
      <div className="w-full bg-gray-200 rounded h-4 mb-2">
        <div
          className={`h-4 rounded ${barColor}`}
          style={{ width: `${percent}%` }}
        />
      </div>
      <span className="text-sm">
        {goal.current}{" "}
        {goal.type === "weight" || goal.type === "strength"
          ? "lbs"
          : goal.type === "cardio"
          ? goal.durationUnit
          : ""}
        {" / "}
        {goal.target}{" "}
        {goal.type === "weight" || goal.type === "strength"
          ? "lbs"
          : goal.type === "cardio"
          ? goal.durationUnit
          : goal.type === "frequency"
          ? "workouts"
          : ""}
        {isCompleted ? " — Completed" : ""}
      </span>
    </div>
  );
}
