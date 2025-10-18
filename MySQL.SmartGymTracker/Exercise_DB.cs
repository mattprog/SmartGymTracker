using System.Data;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class Exercise_DB
    {
        private readonly DB db = new DB();

        public Exercise_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public List<Exercise> GetAll()
        {
            string sql = "SELECT * FROM exercise";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<Exercise> exercise = DataTableToList(dbreturn);
            return exercise;
        }

        public Exercise? Add(Exercise exercise)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(exercise);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO exercise ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, selparametersList) = BuildUpdateQueryList(exercise);
            string selectsql = $"SELECT exerciseId, muscleId, exerciseName, description FROM exercise WHERE {string.Join(" AND ", queries)};";
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

        public Exercise? Update(Exercise exercise)
        {
            // Return if no valid user id
            if (exercise.ExerciseId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(exercise);
            parametersList.Add(new MySqlParameter("@exerciseId", exercise.ExerciseId));
            string sql = $"UPDATE exercise SET {string.Join(", ", updateQueries)} WHERE exerciseId = @exerciseId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT exerciseId, muscleId, exerciseName, description FROM exercise WHERE exerciseId = @exerciseId";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@exerciseId", exercise.ExerciseId)
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

        public Exercise? Delete(int exerciseId)
        {
            if (exerciseId <= 0)
                return null;
            // Get updated record
            // Get updated record
            string selectSql = "SELECT exerciseId, muscleId, exerciseName, description FROM exercise WHERE exerciseId = @exerciseId";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@exerciseId", exerciseId)
            };
            var result = db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM exercise WHERE exerciseId = @exerciseId";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Exercise> DataTableToList(DataTable t)
        {
            List<Exercise> exercises = new List<Exercise>();
            foreach (DataRow row in t.Rows)
            {
                Exercise exercise = new Exercise
                {
                    ExerciseId = Convert.ToInt32(row["exerciseId"]),
                    MuscleId = Convert.ToInt32(row["muscleId"]),
                    ExerciseName = Convert.ToString(row["exerciseName"]) ?? "",
                    Description = Convert.ToString(row["description"]) ?? ""
                };
                exercises.Add(exercise);
            }
            return exercises;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Exercise exercise)
        {
            Exercise defaultExercise = new Exercise();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

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

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(Exercise exercise)
        {
            Exercise defaultExercise = new Exercise();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

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