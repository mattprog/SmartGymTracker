using System.Data;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Mozilla;

namespace MySQL.SmartGymTracker
{
    public class WorkoutTypesPerWorkout_DB
    {
        private readonly DB db = new DB();

        public WorkoutTypesPerWorkout_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public List<List<int>>? GetByWorkoutId(int workoutId)
        {
            if (workoutId <= 0)
                return null;
            string sql = "SELECT workoutId, workoutTypeId FROM workout_types_per_workout WHERE workoutId = @workoutId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutId", workoutId)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<List<int>>? workout = DataTableToList(dbreturn);
            if (workout.Count != 0)
                return workout;
            return null;
        }

        public List<List<int>>? GetByWorkoutTypeId(int workoutTypeId)
        {
            if (workoutTypeId <= 0)
                return null;
            string sql = "SELECT workoutId, workoutTypeId FROM workout_types_per_workout WHERE workoutTypeId = @workoutTypeId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutTypeId", workoutTypeId)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<List<int>>? workout = DataTableToList(dbreturn);
            if (workout.Count != 0)
                return workout;
            return null;
        }

        public List<List<int>>? GetAll()
        {
            string sql = "SELECT workoutId, workoutTypeId FROM workout_types_per_workout;";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<List<int>>? workout = DataTableToList(dbreturn);
            if (workout.Count != 0)
                return workout;
            return null;
        }

        public bool Add(int workoutId, int workoutTypeId)
        {
            if (workoutId <= 0 || workoutTypeId <= 0)
                return false;
            string sql = "INSERT INTO workout_types_per_workout (workoutId, workoutTypeId) VALUES (@workoutId, @workoutTypeId);";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutId", workoutId),
                new MySqlParameter("@workoutTypeId", workoutTypeId)
            };
            db.ExecuteNonQuery(sql, parameters);
            return true;
        }

        public bool Update(int oldWorkoutId, int newWorkoutId, int oldWorkoutTypeId, int newWorkoutTypeId)
        {
            if(oldWorkoutId <= 0 || newWorkoutId <= 0 || oldWorkoutTypeId <= 0 || newWorkoutTypeId <= 0)
                return false;
            string sql = "UPDATE workout_types_per_workout SET workoutId = @newWorkoutId, workoutTypeId = @newWorkoutTypeId WHERE workoutId = @oldWorkoutId AND workoutTypeId = @oldWorkoutTypeId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@newWorkoutId", newWorkoutId),
                new MySqlParameter("@newWorkoutTypeId", newWorkoutTypeId),
                new MySqlParameter("@oldWorkoutId", oldWorkoutId),
                new MySqlParameter("@oldWorkoutTypeId", oldWorkoutTypeId)
            };
            db.ExecuteNonQuery(sql, parameters);
            return true;
        }

        public bool Delete(int workoutId, int workoutTypeId)
        {
            if (workoutId <= 0 || workoutTypeId <= 0)
                return false;
            string sql = "DELETE FROM workout_types_per_workout WHERE workoutId = @workoutId AND workoutTypeId = @workoutTypeId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutId", workoutId),
                new MySqlParameter("@workoutTypeId", workoutTypeId)
            };
            db.ExecuteNonQuery(sql, parameters);
            return true;
        }

        private List<List<int>> DataTableToList(DataTable table)
        {
            var list = new List<List<int>>();
            foreach (DataRow row in table.Rows)
            {
                var c = new List<int>()
                {
                    Convert.ToInt32(row["workoutId"]),
                    Convert.ToInt32(row["workoutTypeId"])
                }
                ;
                list.Add(c);
            }
            return list;
        }
    }
}