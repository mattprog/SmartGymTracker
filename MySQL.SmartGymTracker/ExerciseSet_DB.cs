using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class ExericseSet_DB
    {
        private readonly Database _db;

        public ExerciseSet_DB()
        {
            _db = new DB();
        }

        public List<Exercise> GetAll()
        {
            string sql = "SELECT * FROM exercise_set";
            var dbreturn = _db.ExecuteSelect(sql);
            List<Exercise> exercise = DataTableToList(dbreturn);
            return exercise;
        }

        public ExerciseSet? Add(ExerciseSet exercise)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workout);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return;
            string sql = $"INSERT INTO exercise_set ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, parametersList) = BuildUpdateQueryList(exercise);
            string sql = $"SELECT exerciseSetId, workoutId, exerciseId, notes, setType FROM exercise_set WHERE {string.Join(" AND ", queries)};";
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

        public ExerciseSet? Update(ExerciseSet exercise)
        {
            // Return if no valid user id
            if (exercise.ExerciseSetId <= 0 && exercise.workoutId <= 0 && exerciseId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(user);
            parametersList.Add(new MySqlParameter("@exerciseSetId", exercise.ExerciseSetId));
            parametersList.Add(new MySqlParameter("@workoutId", exercise.WorkoutId));
            parametersList.Add(new MySqlParameter("@exerciseId", exercise.ExerciseId));
            string sql = $"UPDATE exercise SET {string.Join(", ", updateQueries)} WHERE exerciseSetId = @exerciseId AND workoutId = @workoutId AND exerciseId = @exerciseId;";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT exerciseSetId, workoutId, exerciseId, notes, setType FROM exercise_set WHERE exerciseId = @exerciseId  AND workoutId = @workoutId AND exerciseId = @exerciseId;";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@exerciseId", exercise.ExerciseId),
                new MySqlParameter("@workoutId", exercise.WorkoutId),
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
            string selectSql = "SELECT exerciseSetId, workoutId, exerciseId, notes, setType FROM exercise_set WHERE exerciseId = @exerciseId  AND workoutId = @workoutId AND exerciseId = @exerciseId;";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@exerciseId", exercise.ExerciseId),
                new MySqlParameter("@workoutId", exercise.WorkoutId),
                new MySqlParameter("@exerciseId", exercise.ExerciseId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM exercise_set WHERE exerciseSetId = @exerciseSetId AND workoutId = @workoutId AND exerciseId = @exerciseId";
            _db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Rows.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<ExerciseSet> DataTableToList(Datatable t)
        {
            List<ExerciseSet> exercises = new List<ExerciseSet>();
            foreach (DataRow row in t.Rows)
            {
                ExerciseSet exercise = new ExerciseSet
                {
                    ExerciseSet = Convert.ToInt32(row["exerciseSetId"]),
                    WorkoutId = Convert.ToInt32(row["workoutId"]),
                    ExerciseId = Convert.ToInt32(row["exerciseId"]),
                    Notes = Convert.ToString(row["notes"]),
                    SetType = Convert.ToString(row["setType"])
                };
                exercises.Add(exercise);
            }
            return exercises;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(ExerciseSet exercise)
        {
            Exercise defaultExerciseSet = new ExerciseSet();
            List<string> querys = new List<string>;
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(exercise.Notes != defaultExercise.Notes)
            {
                querys.Add("notes = @notes");
                parameters.Add(new MySqlParameter("@notes", exercise.Notes));
            }
            if(exercise.SetType != defaultExercise.SetType)
            {
                querys.Add("setType = @setType");
                parameters.Add(new MySqlParameter("@setType", exercise.SetType));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildUpdateQueryList(ExerciseSet exercise)
        {
            Exercise defaultExerciseSet = new Exercise();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(exercise.Notes != defaultExercise.Notes)
            {
                cols.Add("notes");
                vals.Add("@notes");
                parameters.Add(new MySqlParameter("@notes", exercise.Notes));
            }
            if(exercise.SetType != defaultExercise.SetType)
            {
                cols.Add("setType");
                vals.Add("@setType");
                parameters.Add(new MySqlParameter("@setType", exercise.SetType));
            }

            return (cols, vals, parameters);
        }
    }
}