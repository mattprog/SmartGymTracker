export class Workout {
    constructor(
        { WorkoutId = -1,
            UserId = -1,
             WorkoutTypeId = -1,
              WorkoutStart = '', 
              Duration = '', 
              Notes = '' 
        } = {}) 
    {
      this.WorkoutId = WorkoutId;
      this.UserId = UserId;
      this.WorkoutTypeId = WorkoutTypeId;
      this.WorkoutStart = WorkoutStart;
      this.Duration = Duration;
      this.Notes = Notes;
    }
  }
  