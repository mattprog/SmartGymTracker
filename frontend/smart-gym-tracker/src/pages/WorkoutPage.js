import React, { useCallback, useEffect, useState } from "react";
import { Workout } from "../models/Workout";
import { StrengthSet } from "../models/StrengthSet";
import { CardioSet } from "../models/CardioSet";
import WorkoutForm from "../workoutcomponents/WorkoutForm";
import CurrentSets from "../workoutcomponents/CurrentSets";
import WorkoutHistory from "../workoutcomponents/WorkoutHistory";
import { fetchWorkouts, createWorkout, deleteWorkout } from "../services/WorkoutsService";
import { fetchMuscles } from "../services/MuscleService";

const WorkoutPage = ({ user }) => {
  const userId = user?.UserId ?? user?.userId ?? null;
  const [workout, setWorkout] = useState(new Workout());
  const [exerciseType, setExerciseType] = useState("");
  const [muscle, setMuscle] = useState("");
  const [exerciseName, setExerciseName] = useState("");
  const [setNumber, setSetNumber] = useState(1);
  const [weight, setWeight] = useState("");
  const [reps, setReps] = useState("");
  const [cardioDuration, setCardioDuration] = useState("");
  const [cardioDistance, setCardioDistance] = useState("");
  const [notes, setNotes] = useState("");
  const [completedWorkouts, setCompletedWorkouts] = useState([]);
  const [loadingHistory, setLoadingHistory] = useState(false);
  const [muscleOptions, setMuscleOptions] = useState([]);
  const [loadingMuscles, setLoadingMuscles] = useState(false);
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  const loadMuscles = useCallback(async () => {
    setLoadingMuscles(true);
    const data = await fetchMuscles();
    setMuscleOptions(data ?? []);
    setLoadingMuscles(false);
  }, []);

  const loadHistory = useCallback(async () => {
    if (!userId) return;
    setLoadingHistory(true);
    const data = await fetchWorkouts(userId);
    setCompletedWorkouts(data ?? []);
    setLoadingHistory(false);
  }, [userId]);

  useEffect(() => {
    void loadMuscles();
  }, [loadMuscles]);

  useEffect(() => {
    if (!userId) return;
    void loadHistory();
  }, [userId, loadHistory]);

  const handleAddSet = () => {
    if (!exerciseName) {
      setError("Please enter exercise name");
      return;
    }
    setError("");

    const newSet =
      exerciseType === "strength"
        ? new StrengthSet({
            SetNumber: setNumber,
            Weight: weight ? parseFloat(weight) : 0,
            Reps: reps ? parseInt(reps, 10) : 0,
            ExerciseId: exerciseName,
          })
        : new CardioSet({
            Duration: cardioDuration ? parseFloat(cardioDuration) : 0,
            Distance: cardioDistance ? parseFloat(cardioDistance) : 0,
            ExerciseId: exerciseName,
          });

    setWorkout({
      ...workout,
      sets: [...(workout.sets || []), { exerciseName, setObj: newSet }],
    });
    setSetNumber(setNumber + 1);
    setWeight("");
    setReps("");
    setCardioDuration("");
    setCardioDistance("");
  };

  const buildNotesFromSets = () => {
    if (!workout.sets || workout.sets.length === 0) return "";
    return workout.sets
      .map((entry) => {
        const set = entry.setObj;
        if (set.SetNumber !== undefined) {
          return `${entry.exerciseName}: ${set.Weight} lbs x ${set.Reps} reps`;
        }
        return `${entry.exerciseName}: ${set.Duration} min, ${set.Distance} miles`;
      })
      .join(" | ");
  };

  const handleAddWorkout = async () => {
    if (!userId) {
      setError("Please log in to save workouts.");
      return;
    }
    if (!workout.WorkoutStart || !workout.Duration) {
      setError("Please select a date and duration.");
      return;
    }

    setError("");
    setSaving(true);

    const payload = {
      userId,
      workoutStart: new Date(workout.WorkoutStart).toISOString(),
      duration: Number(workout.Duration),
      notes: notes || buildNotesFromSets(),
    };

    const result = await createWorkout(payload);
    setSaving(false);
    if (result?.error) {
      setError(result.error);
      return;
    }

    setWorkout(new Workout());
    setNotes("");
    setMuscle("");
    setExerciseName("");
    setSetNumber(1);
    setExerciseType("");
    await loadHistory();
  };

  const handleDeleteWorkout = async (id) => {
    if (!id) return;
    await deleteWorkout(id);
    await loadHistory();
  };

  if (!userId) {
    return (
      <div className="max-w-md mx-auto bg-white shadow-md rounded p-6 space-y-4 text-center">
        <p className="text-gray-700">Please log in to log your workouts.</p>
      </div>
    );
  }

  return (
    <div style={{ padding: "20px", maxWidth: "800px", margin: "0 auto" }}>
      <h1 style={{ fontSize: "28px", fontWeight: "bold", marginBottom: "20px" }}>
        Time to log your workout!
      </h1>
      {error && (
        <div className="bg-red-100 text-red-700 rounded px-3 py-2 mb-4 text-sm">
          {error}
        </div>
      )}
      <WorkoutForm
        workout={workout}
        setWorkout={setWorkout}
        exerciseType={exerciseType}
        setExerciseType={setExerciseType}
        muscle={muscle}
        setMuscle={setMuscle}
        exerciseName={exerciseName}
        setExerciseName={setExerciseName}
        setNumber={setNumber}
        setSetNumber={setSetNumber}
        weight={weight}
        setWeight={setWeight}
        reps={reps}
        setReps={setReps}
        cardioDuration={cardioDuration}
        setCardioDuration={setCardioDuration}
        cardioDistance={cardioDistance}
        setCardioDistance={setCardioDistance}
        notes={notes}
        setNotes={setNotes}
        muscleOptions={muscleOptions}
        musclesLoading={loadingMuscles}
        handleAddSet={handleAddSet}
        handleAddWorkout={handleAddWorkout}
      />
      {workout.sets && workout.sets.length > 0 && <CurrentSets workout={workout} />}
      <div style={{ marginTop: "20px" }}>
        <WorkoutHistory
          completedWorkouts={completedWorkouts}
          loading={loadingHistory || saving}
          onDelete={handleDeleteWorkout}
        />
      </div>
    </div>
  );
};

export default WorkoutPage;



