using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class Workout_DB
    {
        private readonly Database _db;

        public Workout_DB()
        {
            _db = new DB();
        }

        public List<Workout> GetAll()
        {
            string sql = "SELECT * FROM workout";
            var dbreturn = _db.ExecuteSelect(sql);
            List<Workout> workout = DataTableToList(dbreturn);
            return workout;
        }

        public Workout? Add(Workout workout)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workout);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return;
            string sql = $"INSERT INTO workout ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, parametersList) = BuildUpdateQueryList(user);
            string sql = $"SELECT workoutId, userId, workoutStart, duration, notes FROM workout WHERE {string.Join(" AND ", queries)};";
            _db.ExecuteNonQuery(sql, parametersList);

            if (result.Rows.Count > 0)
            {
                var val = DataTableToList(result);
                if (val.Rows.Count > 0)
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
            var (updateQueries, parametersList) = BuildUpdateQueryList(user);
            parametersList.Add(new MySqlParameter("@workoutId", workout.WorkoutId));
            string sql = $"UPDATE workout SET {string.Join(", ", updateQueries)} WHERE workoutId = @workoutId;";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT workoutId, userId, workoutStart, duration, notes FROM workout WHERE workoutId = @workout";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@workoutId", workout.WorkoutId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count > 0)
            {
                var val = DataTableToList(result);
                if (val.Rows.Count > 0)
                {
                    return val[0];
                }
            }
            return null;
        }

        public Workout? Delete(int workoutId)
        {
            if (workoutId <= 0)
                return;
            // Get updated record
            string selectSql = "SELECT workoutId, userId, workoutStart, duration, notes FROM workout WHERE workoutId = @workout";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@workoutId", workout.WorkoutId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM workout WHERE workoutId = @workoutId";
            _db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Rows.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Workout> DataTableToList(Datatable t)
        {
            List<Workout> workouts = new List<Workout>();
            foreach (DataRow row in t.Rows)
            {
                Workout workout = new Workout
                {
                    WorkoutId = Convert.ToInt32(row["workoutId"]),
                    UserId = Convert.ToString(row["userId"]),
                    WorkoutStart = Convert.ToDateTime(row["workoutStart"]),
                    Duration = Convert.ToInt32(row["duration"])
                    Notes = Convert.ToString(row["notes"])
                };
                workouts.Add(workout);
            }
            return workouts;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Workout workout)
        {
            Workout defaultWorkout = new Workout();
            List<string> querys = new List<string>;
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(workout.UserId != defaultUser.UserId)
            {
                updates.Add("userId = @userId");
                parameters.Add(new MySqlParameter("@userId", workout.UserId));
            }

            if(workout.WorkoutStart != defaultUser.WorkoutStart)
            {
                updates.Add("workoutStart = @workoutStart");
                parameters.Add(new MySqlParameter("@workoutStart", workout.WorkoutStart));
            }

            if(workout.Duration != defaultUser.Duration)
            {
                updates.Add("duration = @duration");
                parameters.Add(new MySqlParameter("@duration", workout.Duration));
            }
            
            if(workout.Notes != defaultUser.Notes)
            {
                updates.Add("notes = @notes");
                parameters.Add(new MySqlParameter("@notes", workout.Notes));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildUpdateQueryList(Workout workout)
        {
            Workout defaultWorkout = new Workout();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(workout.UserId != defaultUser.UserId)
            {
                cols.Add("userId");
                vals.Add("@userId");
                parameters.Add(new MySqlParameter("@userId", workout.UserId));
            }

            if(workout.WorkoutStart != defaultUser.WorkoutStart)
            {
                cols.Add("workoutStart");
                vals.Add("@workoutStart");
                parameters.Add(new MySqlParameter("@workoutStart", workout.WorkoutStart));
            }

            if(workout.Duration != defaultUser.Duration)
            {
                cols.Add("duration");
                vals.Add("@duration");
                parameters.Add(new MySqlParameter("@duration", workout.Duration));
            }

            if(workout.Notes != defaultUser.Notes)
            {
                cols.Add("notes");
                vals.Add("@notes");
                parameters.Add(new MySqlParameter("@notes", workout.Notes));
            }

            return (cols, vals, parameters);
        }
    }
}