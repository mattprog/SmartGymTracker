export class Exercise {
    constructor({ ExerciseId = -1, MuscleId = -1, ExerciseName = '', Description = '' } = {}) {
      this.ExerciseId = ExerciseId;
      this.MuscleId = MuscleId;
      this.ExerciseName = ExerciseName;
      this.Description = Description;
    }
  }
  