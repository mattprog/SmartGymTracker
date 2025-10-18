using System.Data;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class Workout_DB
    {
        private readonly DB db = new DB();

        public Workout_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public List<Workout>? GetById(int id)
        {
            if (id <= 0)
                return null;
            string sql = "SELECT workoutId, userId, workoutStart, duration, notes FROM workout WHERE workoutId = @workoutId";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutId", id)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<Workout> workout = DataTableToList(dbreturn);
            if (workout.Count != 0)
                return workout;
            return null;
        }

        public List<Workout>? GetAll()
        {
            string sql = "SELECT workoutId, userId, workoutStart, duration, notes FROM workout;";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<Workout> workout = DataTableToList(dbreturn);
            if (workout.Count != 0)
                return workout;
            return null;
        }

        public Workout? Add(Workout workout)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workout);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO workout ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, selparametersList) = BuildUpdateQueryList(workout);
            string selectsql = $"SELECT workoutId, userId, workoutStart, duration, notes FROM workout WHERE {string.Join(" AND ", queries)};";
            var result = db.ExecuteSelect(selectsql, selparametersList);

            if (result.Rows.Count > 0)
            {
                var val = DataTableToList(result);
                if (val.Count > 0)
                {
                    return val.Last();
                }
            }
            return null;
        }

        public Workout? Update(Workout workout)
        {
            // Return if no valid user id
            if (workout.WorkoutId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(workout);
            parametersList.Add(new MySqlParameter("@workoutId", workout.WorkoutId));
            string sql = $"UPDATE workout SET {string.Join(", ", updateQueries)} WHERE workoutId = @workoutId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT workoutId, userId, workoutStart, duration, notes FROM workout WHERE workoutId = @workout;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutId", workout.WorkoutId)
            };
            var result = db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count > 0)
            {
                var val = DataTableToList(result);
                if (val.Count > 0)
                {
                    return val[0];
                }
            }
            return null;
        }

        public Workout? Delete(int workoutId)
        {
            if (workoutId <= 0)
                return null;
            // Get updated record
            string selectSql = "SELECT workoutId, userId, workoutStart, duration, notes FROM workout WHERE workoutId = @workout;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutId", workoutId)
            };
            var result = db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM workout WHERE workoutId = @workoutId;";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Workout> DataTableToList(DataTable t)
        {
            List<Workout> workouts = new List<Workout>();
            foreach (DataRow row in t.Rows)
            {
                Workout workout = new Workout
                {
                    WorkoutId = Convert.ToInt32(row["workoutId"]),
                    UserId = Convert.ToInt32(row["userId"]),
                    WorkoutStart = Convert.ToDateTime(row["workoutStart"]),
                    Duration = Convert.ToInt32(row["duration"]),
                    Notes = Convert.ToString(row["notes"]) ?? ""
                };
                workouts.Add(workout);
            }
            return workouts;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Workout workout)
        {
            Workout defaultWorkout = new Workout();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(workout.UserId != defaultWorkout.UserId)
            {
                querys.Add("userId = @userId");
                parameters.Add(new MySqlParameter("@userId", workout.UserId));
            }

            if(workout.WorkoutStart != defaultWorkout.WorkoutStart)
            {
                querys.Add("workoutStart = @workoutStart");
                parameters.Add(new MySqlParameter("@workoutStart", workout.WorkoutStart));
            }

            if(workout.Duration != defaultWorkout.Duration)
            {
                querys.Add("duration = @duration");
                parameters.Add(new MySqlParameter("@duration", workout.Duration));
            }
            
            if(workout.Notes != defaultWorkout.Notes)
            {
                querys.Add("notes = @notes");
                parameters.Add(new MySqlParameter("@notes", workout.Notes));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(Workout workout)
        {
            Workout defaultWorkout = new Workout();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(workout.UserId != defaultWorkout.UserId)
            {
                cols.Add("userId");
                vals.Add("@userId");
                parameters.Add(new MySqlParameter("@userId", workout.UserId));
            }

            if(workout.WorkoutStart != defaultWorkout.WorkoutStart)
            {
                cols.Add("workoutStart");
                vals.Add("@workoutStart");
                parameters.Add(new MySqlParameter("@workoutStart", workout.WorkoutStart));
            }

            if(workout.Duration != defaultWorkout.Duration)
            {
                cols.Add("duration");
                vals.Add("@duration");
                parameters.Add(new MySqlParameter("@duration", workout.Duration));
            }

            if(workout.Notes != defaultWorkout.Notes)
            {
                cols.Add("notes");
                vals.Add("@notes");
                parameters.Add(new MySqlParameter("@notes", workout.Notes));
            }

            return (cols, vals, parameters);
        }
    }
}