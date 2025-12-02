import GoalCard from "./GoalCard";

// Component for displaying completed goals
export default function CompletedGoals({ completedGoals }) {
  return (
    <div className="space-y-6 max-w-2xl mx-auto">
      {["weight", "strength", "frequency", "cardio"].map((type) => {
        const typeGoals = completedGoals.filter((g) => g.type === type);
        if (!typeGoals.length) return null;

        return (
          <div key={type} className="space-y-3">
            <h2 className="text-lg font-semibold text-gray-700 capitalize">{type} goals</h2>
            <div className="flex flex-col gap-3">
              {typeGoals.map((goal) => (
                <GoalCard key={goal.id} goal={goal} isCompleted />
              ))}
            </div>
          </div>
        );
      })}
    </div>
  );
}
