import WorkoutTypeManager from '../admincomponents/WorkoutTypeManager';
import MuscleManager from '../admincomponents/MuscleManager';
import ExerciseManager from '../admincomponents/ExerciseManager';


function Admin() {
  return (
    <div className="max-w-lg mx-auto space-y-6 p-6">
      <h1 className="text-2xl font-bold mb-6">Admin Panel</h1>
      <WorkoutTypeManager />
      <MuscleManager />
      <ExerciseManager/>
    </div>
  );
}

export default Admin;
