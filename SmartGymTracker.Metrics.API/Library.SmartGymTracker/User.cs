using Library.SmartGymTracker;

namespace Library.SmartGymTracker
{
    public class User
    {
        public string Username { get; set; }
    }
}

/*
TODO
    data validation




Priorities:
   User
     UserId, username, password, firstname, lastname, email, phonenumber, dateofbirth
     weight, height, gender
   Workout
     workoutid, userid, workouttypeid, workoutstart, workoutdurration, notes
   WorkoutType
     workouttypeid, name, description, difficutlylevel
   ExerciseSet
     exerciseset, setnumber, workoutid, exerciseid, weight, repts, notes
   Excercise
     exerciseid, exercisename, muscleid, description
   Muscle
     muscleid, name, size, description

   biometricdata
     biometricdataid, userid, date, weight, bodyfatpercentage, notes, weight, height, stepcount


Increments:
   1) Workout, Exercise, Set, and Rep input forms
       2) Biometric data collection (increment 1&2)
    admin screen



   ) Login Screen
   ) Admin login
   3) Milestone and Progress Tracking, Trend Data



   4) Smart tips and suggestions
   5) Notifications 
*/