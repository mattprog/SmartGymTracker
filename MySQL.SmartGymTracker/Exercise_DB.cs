using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class Exericse_DB
    {
        private readonly Database _db;

        public Exercise_DB()
        {
            _db = new DB();
        }

        public List<Exercise> GetAll()
        {
            string sql = "SELECT * FROM exercise";
            var dbreturn = _db.ExecuteSelect(sql);
            List<Exercise> exercise = DataTableToList(dbreturn);
            return exercise;
        }

        public Exercise? Add(Exercise exercise)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workout);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return;
            string sql = $"INSERT INTO exercise ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, parametersList) = BuildUpdateQueryList(exercise);
            string sql = $"SELECT exerciseId, muscleId, exerciseName, description FROM exercise WHERE {string.Join(" AND ", queries)};";
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

        public Exercise? Update(Exercise exercise)
        {
            // Return if no valid user id
            if (exercise.ExerciseId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(user);
            parametersList.Add(new MySqlParameter("@exerciseId", exercise.ExerciseId));
            string sql = $"UPDATE exercise SET {string.Join(", ", updateQueries)} WHERE exerciseId = @exerciseId;";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT exerciseId, muscleId, exerciseName, description FROM exercise WHERE exerciseId = @exerciseId";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@exerciseId", exercise.ExerciseId)
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

        public Exercise? Delete(int exerciseId)
        {
            if (exerciseId <= 0)
                return;
            // Get updated record
            // Get updated record
            string selectSql = "SELECT exerciseId, muscleId, exerciseName, description FROM exercise WHERE exerciseId = @exerciseId";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@exerciseId", exercise.ExerciseId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM exercise WHERE exerciseId = @exerciseId";
            _db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Rows.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Exercise> DataTableToList(Datatable t)
        {
            List<Exercise> exercises = new List<Workout>();
            foreach (DataRow row in t.Rows)
            {
                Exercise exercise = new Exercise
                {
                    ExerciseId = Convert.ToInt32(row["exerciseId"]),
                    MuscleId = Convert.ToInt32(row["muscleId"]),
                    ExerciseName = Convert.ToString(row["exerciseName"]),
                    Description = Convert.ToString(row["description"])
                };
                exercises.Add(exercise);
            }
            return exercises;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Exercise exercise)
        {
            Exercise defaultExercise = new Exercise();
            List<string> querys = new List<string>;
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(exercise.MuscleId != defaultExercise.MuscleId)
            {
                querys.Add("muscleId = @muscleId");
                parameters.Add(new MySqlParameter("@muscleId", exercise.MuscleId));
            }
            if(exercise.ExerciseName != defaultExercise.ExerciseName)
            {
                querys.Add("exerciseName = @exerciseName");
                parameters.Add(new MySqlParameter("@exerciseName", exercise.ExerciseName));
            }
            if(exercise.Description != defaultExercise.Description)
            {
                querys.Add("description = @description");
                parameters.Add(new MySqlParameter("@description", exercise.Description));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildUpdateQueryList(Exercise exercise)
        {
            Exercise defaultExercise = new Exercise();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(exercise.MuscleId != defaultExercise.MuscleId)
            {
                cols.Add("muscleId");
                vals.Add("@muscleId");
                parameters.Add(new MySqlParameter("@muscleId", exercise.MuscleId));
            }
            if(exercise.ExerciseName != defaultExercise.ExerciseName)
            {
                cols.Add("exerciseName");
                vals.Add("@exerciseName");
                parameters.Add(new MySqlParameter("@exerciseName", exercise.ExerciseName));
            }
            if(exercise.Description != defaultExercise.Description)
            {
                cols.Add("description");
                vals.Add("@description");
                parameters.Add(new MySqlParameter("@description", exercise.Description));
            }

            return (cols, vals, parameters);
        }
    }
}