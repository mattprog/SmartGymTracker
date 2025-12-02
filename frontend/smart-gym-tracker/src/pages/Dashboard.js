import { useState } from "react";
import { Bar, Line } from "react-chartjs-2";
import "chart.js/auto";
import { FaDumbbell, FaChartLine, FaLightbulb, FaEnvelope, FaWeight } from "react-icons/fa";

export default function Dashboard() {
  const [userName] = useState("User");




  // Mock data
  const totalWorkouts = 26;
  const goals =  [
    { id: 1, type: "weight", name: "Reach 125 lbs body weight", target: 125, current: 112 },
    { id: 2, type: "strength", name: "Bench Press 315 lbs", target: 315, current: 250 },
    { id: 3, type: "frequency", name: "Complete 5 workouts per week", target: 5, current: 3, freqUnit: "week" },
    { id: 4, type: "cardio", name: "Run 6 miles", target: 6, current: 3, durationUnit: "miles" },
  ];
  const workoutsPerWeek = [
    { week: "Week 1", count: 5 },
    { week: "Week 2", count: 7 },
    { week: "Week 3", count: 6 },
    { week: "Week 4", count: 8 },
  ];
  const tips = [
    "Stay hydrated!",
    "Consistently add 2.5 lbs to bench to reach your 315 lb goal!",
    "Eat protein rich foods to reach goal weight!",
  ];
  const messages = [
    "Welcome back!",
    "Don't forget to log your workouts today.",
  ];






  // Chart data
  const data = {
    labels: workoutsPerWeek.map((w) => w.week),
    datasets: [
      {
        label: "Workouts",
        data: workoutsPerWeek.map((w) => w.count),
        backgroundColor: "rgba(59, 130, 246, 0.7)",
        borderColor: "rgba(59, 130, 246, 1)",
        borderWidth: 1,
        borderRadius: 5,
      },
    ],
  };



  const options = {
    plugins: {
      legend: { display: false },
      tooltip: { enabled: true },
    },
    scales: {
      y: {
        beginAtZero: true,
        stepSize: 1,
        ticks: { precision: 0 },
      },
    },
    maintainAspectRatio: false,
  };



  // New mock weight trend (lbs)
  const weightTrend = [
    { week: "Week 1", weight: 106 },
    { week: "Week 2", weight: 110 },
    { week: "Week 3", weight: 108.2 },
    { week: "Week 4", weight: 112 },
  ];



  // Chart data for weight trend
  const weightData = {
    labels: weightTrend.map((w) => w.week),
    datasets: [
      {
        label: "Weight (lbs)",
        data: weightTrend.map((w) => w.weight),
        borderColor: "rgba(59, 130, 246, 1)",
        backgroundColor: "rgba(59, 130, 246, 0.3)",
        tension: 0.4,
        fill: true,
        pointBackgroundColor: "rgba(59,130,246,1)",
        pointBorderWidth: 2,
        pointRadius: 5,
        pointHoverRadius: 7,
      },
    ],
  };

  const weightOptions = {
    plugins: {
      legend: { display: false },
      tooltip: {
        enabled: true,
        callbacks: {
          label: function (context) {
            return `Weight: ${context.parsed.y} lbs`;
          },
        },
      },
    },
    scales: {
      y: {
        beginAtZero: false,
        ticks: {
          stepSize: 0.5,
          precision: 1,
        },
      },
    },
    maintainAspectRatio: false,
  };




  // Pick first incomplete goal for dashboard quick status
  const incompleteGoal = goals.find((g) => g.current < g.target);
  let goalStatusMessage = "All goals completed! 🎉";
  if (incompleteGoal) {
    let unit = "";
    if (incompleteGoal.type === "weight" || incompleteGoal.type === "strength") unit = "lbs";
    else if (incompleteGoal.type === "cardio") unit = incompleteGoal.durationUnit;
    else if (incompleteGoal.type === "frequency") unit = "workouts";

    const remaining = incompleteGoal.target - incompleteGoal.current;
    goalStatusMessage = `${remaining} ${unit} left until your ${incompleteGoal.type} goal is completed! Keep it up! 🎉`;
  }

  return (
    <div className="overflow-auto flex-1 p-4 space-y-6 pb-12">
      {/* Dashboard headings */}
      <h1 className="text-3xl font-extrabold text-gray-800">Dashboard</h1>
      <h2 className="text-xl font-semibold text-gray-600 mb-4 text-center">
        Welcome, {userName}!
      </h2>





      {/* Stats Cards */}
      <div className="flex gap-4 flex-wrap justify-center">
        <div className="bg-white shadow-md rounded-lg p-6 flex flex-col items-center justify-center w-52 hover:scale-105 transform transition duration-300 text-center">
          <FaDumbbell size={36} className="text-blue-600 mb-2" />
          <h2 className="text-xl font-bold">{totalWorkouts}</h2>
          <p>Total Workouts This Month</p>
        </div>
        <div className="bg-white shadow-md rounded-lg p-6 flex flex-col items-center justify-center w-72 hover:scale-105 transform transition duration-300 text-center">
          <FaChartLine size={36} className="text-green-600 mb-2" />
          <p className="text-lg font-semibold">{goalStatusMessage}</p>
        </div>
      </div>





      {/* Workouts Bar Chart */}
      <div className="bg-white shadow-md rounded-lg p-6 w-full max-w-2xl mx-auto hover:shadow-lg transition duration-300">
        <h3 className="text-lg font-bold mb-4">Workouts Per Week (November)</h3>
        <div style={{ height: "250px" }}>
          <Bar data={data} options={options} />
        </div>
      </div>




      {/* Weight Trend Chart */}
      <div className="bg-white shadow-md rounded-lg p-6 w-full max-w-2xl mx-auto hover:shadow-lg transition duration-300">
        <h3 className="text-lg font-bold mb-4 flex items-center gap-2">
          <FaWeight className="text-blue-600" /> Weight Trend (Last 4 Weeks)
        </h3>
        <div style={{ height: "250px" }}>
          <Line data={weightData} options={weightOptions} />
        </div>
      </div>





      {/* Smart Tips */}
      <div className="bg-white shadow-md rounded-lg p-6 w-full max-w-2xl mx-auto hover:shadow-lg transition duration-300">
        <h3 className="text-lg font-bold mb-2 flex items-center gap-2">
          <FaLightbulb className="text-yellow-500" /> Smart Tips
        </h3>
        <div className="flex flex-col gap-2">
          {tips.map((tip, i) => (
            <div
              key={i}
              className="bg-blue-50 p-3 rounded border-l-4 border-blue-500 shadow-sm hover:shadow-md transition duration-200"
            >
              {tip}
            </div>
          ))}
        </div>
      </div>





      {/* Messages */}
      <div className="bg-white shadow-md rounded-lg p-6 w-full max-w-2xl mx-auto hover:shadow-lg transition duration-300">
        <h3 className="text-lg font-bold mb-2 flex items-center gap-2">
          <FaEnvelope className="text-red-500" /> Messages
        </h3>
        <div className="flex flex-col gap-2">
          {messages.map((msg, i) => (
            <div
              key={i}
              className="bg-green-50 p-3 rounded border-l-4 border-green-500 shadow-sm hover:shadow-md transition duration-200"
            >
              {msg}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
