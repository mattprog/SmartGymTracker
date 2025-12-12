import { useEffect, useMemo, useState } from "react";
import { Bar, Line } from "react-chartjs-2";
import "chart.js/auto";
import { FaDumbbell, FaChartLine, FaLightbulb, FaEnvelope, FaWeight } from "react-icons/fa";
import { fetchWorkouts } from "../services/WorkoutsService";
import { fetchBiometrics } from "../services/BiometricsService";

const getWeekStart = (date) => {
  const d = new Date(date);
  const day = d.getDay();
  const diff = d.getDate() - day + (day === 0 ? -6 : 1);
  return new Date(d.setDate(diff));
};

export default function Dashboard({ user }) {
  const userName = user?.FirstName ?? user?.firstName ?? user?.Username ?? user?.username ?? "User";
  const userId = user?.UserId ?? user?.userId ?? null;
  const [workouts, setWorkouts] = useState([]);
  const [biometrics, setBiometrics] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!userId) return;
    setLoading(true);
    Promise.all([fetchWorkouts(userId), fetchBiometrics(userId)])
      .then(([workoutData, bioData]) => {
        setWorkouts(workoutData ?? []);
        setBiometrics(bioData ?? []);
      })
      .finally(() => setLoading(false));
  }, [userId]);

  const workoutsPerWeek = useMemo(() => {
    const grouped = new Map();
    workouts.forEach((w) => {
      const rawDate = w.workoutStart ?? w.WorkoutStart;
      if (!rawDate) return;
      const start = getWeekStart(rawDate);
      const key = start.toISOString();
      const entry = grouped.get(key) || {
        label: `Week of ${start.toLocaleDateString(undefined, { month: "short", day: "numeric" })}`,
        count: 0,
        date: start,
      };
      entry.count += 1;
      grouped.set(key, entry);
    });
    const sorted = Array.from(grouped.values()).sort((a, b) => a.date - b.date);
    if (sorted.length === 0) {
      const now = new Date();
      return Array.from({ length: 4 }).map((_, idx) => {
        const start = new Date(now);
        start.setDate(start.getDate() - (3 - idx) * 7);
        return {
          label: `Week of ${start.toLocaleDateString(undefined, { month: "short", day: "numeric" })}`,
          count: 0,
        };
      });
    }
    return sorted.slice(-4);
  }, [workouts]);

  const weightTrend = useMemo(() => {
    const sorted = [...biometrics]
      .filter((b) => b.dateEntered || b.DateEntered)
      .sort((a, b) => new Date(a.dateEntered ?? a.DateEntered) - new Date(b.dateEntered ?? b.DateEntered));
    return sorted.slice(-4).map((entry) => ({
      week: new Date(entry.dateEntered ?? entry.DateEntered).toLocaleDateString(undefined, {
        month: "short",
        day: "numeric",
      }),
      weight: entry.weight ?? entry.Weight ?? 0,
    }));
  }, [biometrics]);

  const totalWorkouts = workouts.length;
  const latestWeight = biometrics.length ? biometrics[biometrics.length - 1].weight ?? biometrics[biometrics.length - 1].Weight : null;
  const earliestWeight = biometrics.length ? biometrics[0].weight ?? biometrics[0].Weight : null;
  const goalStatusMessage = latestWeight && earliestWeight
    ? `${(latestWeight - earliestWeight).toFixed(1)} lbs gained since your first entry.`
    : "Log biometrics to track progress!";

  const tips = weightTrend.length > 0
    ? [
        `Your latest recorded weight is ${weightTrend[weightTrend.length - 1].weight} lbs. Keep up the consistency!`,
        "Schedule your next workout now to keep the streak alive.",
      ]
    : ["Once you add biometrics, personalized tips will appear here."];

  const messages = [
    loading ? "Loading your stats..." : "Data synced from the server.",
    "Remember to log every workout to keep charts accurate.",
  ];

  if (!userId) {
    return (
      <div className="flex-1 p-6">
        <h1 className="text-3xl font-extrabold text-gray-800 mb-4">Dashboard</h1>
        <p className="text-gray-700">Log in to view personalized stats.</p>
      </div>
    );
  }






  // Chart data
  const data = {
    labels: workoutsPerWeek.map((w) => w.label),
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
          <p>Total Logged Workouts</p>
        </div>
        <div className="bg-white shadow-md rounded-lg p-6 flex flex-col items-center justify-center w-72 hover:scale-105 transform transition duration-300 text-center">
          <FaChartLine size={36} className="text-green-600 mb-2" />
          <p className="text-lg font-semibold">{goalStatusMessage}</p>
        </div>
      </div>





      {/* Workouts Bar Chart */}
      <div className="bg-white shadow-md rounded-lg p-6 w-full max-w-2xl mx-auto hover:shadow-lg transition duration-300">
        <h3 className="text-lg font-bold mb-4">Workouts Per Week</h3>
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
