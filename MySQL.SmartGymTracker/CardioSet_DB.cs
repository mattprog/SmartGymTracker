using System.Data;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace MySQL.SmartGymTracker
{
    public class CardioSet_DB
    {
        private readonly DB db = new DB();
       
        public CardioSet_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public List<CardioSet> GetAll()
        {
            string sql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, c.duration, c.distance FROM exercise_set e JOIN cardio_set c WHERE e.exerciseSetId = c.exerciseSetId;";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<CardioSet> biometrics = DataTableToList(dbreturn);
            return biometrics;
        }

        public CardioSet? Add(CardioSet cardioSet)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildBaseInsertQueryList(cardioSet);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO exercise_set ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            db.ExecuteNonQuery(sql, parametersList);
            (columnQueries, valQueries, parametersList) = BuildInsertQueryList(cardioSet);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            sql = $"INSERT INTO cardio_set ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, selparametersList) = BuildBaseUpdateQueryList(cardioSet);
            var (queriestemp, selparametersListtemp) = BuildUpdateQueryList(cardioSet);
            queries.AddRange(queriestemp);
            selparametersList.AddRange(selparametersListtemp);
            string selectsql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, c.duration, c.distance FROM exercise_set e JOIN cardio_set c WHERE e.exerciseSetId = c.exerciseSetId AND c.exerciseSetId = @exerciseSetId;";
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

        public CardioSet? Update(CardioSet cardioSet)
        {
            // Return if no valid user id
            if (cardioSet.ExerciseSetId <= 0 || cardioSet.WorkoutId <= 0 || cardioSet.ExerciseId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildBaseUpdateQueryList(cardioSet);
            parametersList.Add(new MySqlParameter("@exerciseSetId", cardioSet.ExerciseSetId));
            parametersList.Add(new MySqlParameter("@workoutId", cardioSet.WorkoutId));
            parametersList.Add(new MySqlParameter("@exerciseId", cardioSet.ExerciseId));
            string sql = $"UPDATE exercise_set SET {string.Join(", ", updateQueries)} WHERE exerciseSetId = @exerciseSetId AND workoutId = @workoutId AND exerciseId = @exerciseId;";
            (updateQueries, parametersList) = BuildUpdateQueryList(cardioSet);
            parametersList.Add(new MySqlParameter("@exerciseSetId", cardioSet.ExerciseSetId));
            sql = $"UPDATE cardio_set SET {string.Join(", ", updateQueries)} WHERE exerciseSetId = @exerciseSetId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, c.duration, c.distance FROM exercise_set e JOIN cardio_set c WHERE e.exerciseSetId = c.exerciseSetId AND c.exerciseSetId = @exerciseSetId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@exerciseSetId", cardioSet.ExerciseSetId),
                new MySqlParameter("@workoutId", cardioSet.WorkoutId),
                new MySqlParameter("@exerciseId", cardioSet.ExerciseId)
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

        public CardioSet? Delete(int exerciseSetId)
        {
            if (exerciseSetId <= 0)
                return null;

            string selectSql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, c.duration, c.distance FROM exercise_set e JOIN cardio_set c WHERE e.exerciseSetId = c.exerciseSetId AND c.exerciseSetId = @exerciseSetId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@exerciseSetId", exerciseSetId)
            };
            var result = db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM exercise_set WHERE exerciseSetId = @exerciseSetId";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        private List<CardioSet> DataTableToList(DataTable table)
        {
            var list = new List<CardioSet>();
            foreach (DataRow row in table.Rows)
            {
                var cardioSet = new CardioSet
                {
                    ExerciseSetId = Convert.ToInt32(row["exerciseSetId"]),
                    WorkoutId = Convert.ToInt32(row["workoutId"]),
                    ExerciseId = Convert.ToInt32(row["exerciseId"]),
                    Notes = Convert.ToString(row["notes"]) ?? "",
                    Duration = Convert.ToInt32(row["duration"]),
                    Distance = Convert.ToDouble(row["distance"])
                };
                list.Add(cardioSet);
            }
            return list;
        }

        private (List<string> query, List<MySqlParameter> parameters) BuildBaseUpdateQueryList(CardioSet cardioSet)
        {
            CardioSet defaultCardioSet = new CardioSet();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cardioSet.ExerciseId != defaultCardioSet.ExerciseSetId)
            {
                querys.Add("exerciseId = @exerciseId");
                parameters.Add(new MySqlParameter("@exerciseId", cardioSet.ExerciseId));
            }
            if(cardioSet.WorkoutId != defaultCardioSet.WorkoutId)
            {
                querys.Add("workoutId = @workoutId");
                parameters.Add(new MySqlParameter("@workoutId", cardioSet.WorkoutId));
            }
            if (cardioSet.Notes != defaultCardioSet.Notes)
            {
                querys.Add("notes = @notes");
                parameters.Add(new MySqlParameter("@notes", cardioSet.Notes));
            }
            return (querys, parameters);
        }

        private (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(CardioSet cardioSet)
        {
            CardioSet defaultCardioSet = new CardioSet();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cardioSet.Duration != defaultCardioSet.Duration)
            {
                querys.Add("duration = @duration");
                parameters.Add(new MySqlParameter("@duration", cardioSet.Duration));
            }
            if (cardioSet.Distance != defaultCardioSet.Distance)
            {
                querys.Add("distance = @distance");
                parameters.Add(new MySqlParameter("@distance", cardioSet.Distance));
            }
            return (querys, parameters);
        }

        private (List<string>, List<string>, List<MySqlParameter>) BuildBaseInsertQueryList(CardioSet cardioSet)
        {
            CardioSet defaultCardioSet = new CardioSet();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if(cardioSet.ExerciseId != defaultCardioSet.ExerciseId)
            {
                cols.Add("exerciseId");
                vals.Add("@exerciseId");
                parameters.Add(new MySqlParameter("@exerciseId", cardioSet.ExerciseId));
            }
            if(cardioSet.WorkoutId != defaultCardioSet.WorkoutId)
            {
                cols.Add("workoutId");
                vals.Add("@workoutId");
                parameters.Add(new MySqlParameter("@workoutId", cardioSet.WorkoutId));
            }
            if(cardioSet.Notes != defaultCardioSet.Notes)
            {
                cols.Add("notes");
                vals.Add("@notes");
                parameters.Add(new MySqlParameter("@notes", cardioSet.Notes));
            }
            return (cols, vals, parameters);
        }

        private (List<string>, List<string>, List<MySqlParameter>) BuildInsertQueryList(CardioSet cardioSet)
        {
            CardioSet defaultCardioSet = new CardioSet();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (cardioSet.Duration != defaultCardioSet.Duration)
            {
                cols.Add("duration");
                vals.Add("@duration");
                parameters.Add(new MySqlParameter("@duration", cardioSet.Duration));
            }
            if (cardioSet.Distance != defaultCardioSet.Distance)
            {
                cols.Add("distance");
                vals.Add("@distance");
                parameters.Add(new MySqlParameter("@distance", cardioSet.Distance));
            }
            return (cols, vals, parameters);
        }
    }
}