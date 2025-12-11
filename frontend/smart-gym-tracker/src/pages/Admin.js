import WorkoutTypeManager from '../admincomponents/WorkoutTypeManager';
import MuscleManager from '../admincomponents/MuscleManager';
import ExerciseManager from '../admincomponents/ExerciseManager';


function Admin() {
  return (
    <div className="max-w-2xl mx-auto space-y-6 p-6">
    <h1 className="text-2xl font-bold mb-6">Workout Management</h1>
    <WorkoutTypeManager />
    <MuscleManager />
    <ExerciseManager/>
  </div>
  

  );
}

export default Admin;
