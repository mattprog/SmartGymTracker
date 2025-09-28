import React, { useState } from "react";
import { Workout } from "../models/Workout";
import { Muscle } from "../models/Muscle";
import { StrengthSet } from "../models/StrengthSet";
import { CardioSet } from "../models/CardioSet";
import WorkoutForm from "../workoutcomponents/WorkoutForm";
import CurrentSets from "../workoutcomponents/CurrentSets";
import WorkoutHistory from "../workoutcomponents/WorkoutHistory";

const sampleMuscles = [
  new Muscle({ MuscleId: 1, Name: "Chest" }),
  new Muscle({ MuscleId: 2, Name: "Back" }),
  new Muscle({ MuscleId: 3, Name: "Legs" }),
  new Muscle({ MuscleId: 4, Name: "Glutes" }),
];

const WorkoutPage = () => {
  const [workout, setWorkout] = useState(new Workout());
  const [exerciseType, setExerciseType] = useState("");
  const [muscle, setMuscle] = useState("");
  const [exerciseName, setExerciseName] = useState("");
  const [setNumber, setSetNumber] = useState(1);
  const [weight, setWeight] = useState("");
  const [reps, setReps] = useState("");
  const [cardioDuration, setCardioDuration] = useState("");
  const [cardioDistance, setCardioDistance] = useState("");
  const [completedWorkouts, setCompletedWorkouts] = useState([]);

  const handleAddSet = () => {
    if (!exerciseName) return alert("Please enter exercise name");

    const newSet = exerciseType === "strength"
      ? new StrengthSet({ SetNumber: setNumber, Weight: parseFloat(weight), Reps: parseInt(reps), ExerciseId: exerciseName })
      : new CardioSet({ Duration: parseFloat(cardioDuration), Distance: parseFloat(cardioDistance), ExerciseId: exerciseName });

    setWorkout({ ...workout, sets: [...(workout.sets || []), { exerciseName, setObj: newSet }] });
    setSetNumber(setNumber + 1);
    setWeight(""); setReps(""); setCardioDuration(""); setCardioDistance("");
  };

  const handleAddWorkout = () => {
    if (!workout.WorkoutStart || !workout.Duration) return alert("Please select date and duration");
    setCompletedWorkouts([workout, ...completedWorkouts]);
    setWorkout(new Workout());
    setMuscle(""); setExerciseName(""); setSetNumber(1); setExerciseType("");
  };

  return (
    <div style={{ padding: "20px", maxWidth: "600px", margin: "0 auto" }}>
      <h1 style={{ fontSize: "28px", fontWeight: "bold", marginBottom: "20px" }}>Time to log your workout!</h1>
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
        sampleMuscles={sampleMuscles}
        handleAddSet={handleAddSet}
        handleAddWorkout={handleAddWorkout}
      />
      {workout.sets && workout.sets.length > 0 && <CurrentSets workout={workout} />}
      <WorkoutHistory completedWorkouts={completedWorkouts} />
    </div>
  );
};

export default WorkoutPage;



