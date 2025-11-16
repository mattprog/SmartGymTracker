using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class StrengthSet_DB
    {
        private readonly DB db = new DB();
        
        public StrengthSet_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public StrengthSet? GetById(int id)
        {
            if (id <= 0)
                return null;
            string sql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, s.setNumber, s.reps, s.weight FROM strength_set s JOIN exercise_set e ON e.exerciseSetId = s.exerciseSetId WHERE e.exerciseSetId = @exerciseSetId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@exerciseSetId", id)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<StrengthSet> strengthSet = DataTableToList(dbreturn);
            if (strengthSet.Count != 0)
                return strengthSet[0];
            return null;
        }

        public List<StrengthSet>? GetByWorkoutId(int workoutId)
        {
            if (workoutId <= 0)
                return null;
            string sql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, s.setNumber, s.reps, s.weight FROM strength_set s JOIN exercise_set e ON e.exerciseSetId = s.exerciseSetId WHERE e.workoutId = @workoutId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutId", workoutId)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<StrengthSet> strengthSet = DataTableToList(dbreturn);
            if (strengthSet.Count != 0)
                return strengthSet;
            return null;
        }

        public List<StrengthSet>? GetByExerciseId(int exerciseId)
        {
            if (exerciseId <= 0)
                return null;
            string sql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, s.setNumber, s.reps, s.weight FROM strength_set s JOIN exercise_set e ON e.exerciseSetId = s.exerciseSetId WHERE e.exerciseId = @exerciseId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@exerciseId", exerciseId)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<StrengthSet> strengthSet = DataTableToList(dbreturn);
            if (strengthSet.Count != 0)
                return strengthSet;
            return null;
        }

        public List<StrengthSet>? GetAll()
        {
            string sql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, s.setNumber, s.reps, s.weight FROM strength_set s JOIN exercise_set e ON e.exerciseSetId = s.exerciseSetId;";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<StrengthSet> strengthSet = DataTableToList(dbreturn);
            if(strengthSet.Count != 0)
                return strengthSet;
            return null;
        }

        public StrengthSet? Add(StrengthSet strengthSet)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildBaseInsertQueryList(strengthSet);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO exercise_set ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)}); SELECT LAST_INSERT_ID();";
            if (strengthSet.ExerciseSetId <= 0)
            {
                var success = db.ExecuteScalar(sql, parametersList);
                var validId = Convert.ToInt64(success);
                strengthSet.ExerciseSetId = Convert.ToInt32(validId);
            }
            (columnQueries, valQueries, parametersList) = BuildInsertQueryList(strengthSet);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            columnQueries.Add("exerciseSetId");
            valQueries.Add("@exerciseSetId");
            parametersList.Add(new MySqlParameter("@exerciseSetId", strengthSet.ExerciseSetId));
            sql = $"INSERT INTO strength_set ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, selparametersList) = BuildBaseUpdateQueryList(strengthSet);
            var (queriestemp, selparametersListtemp) = BuildUpdateQueryList(strengthSet);
            queries.AddRange(queriestemp);
            selparametersList.AddRange(selparametersListtemp);
            string selectsql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, s.setNumber, s.reps, s.weight FROM strength_set s JOIN exercise_set e ON e.exerciseSetId = s.exerciseSetId;";
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

        public StrengthSet? Update(StrengthSet strengthSet)
        {
            // Return if no valid user id
            if (strengthSet.ExerciseSetId <= 0 || strengthSet.WorkoutId <= 0 || strengthSet.ExerciseId <= 0)
                return null;

            // Perform update query

            var (updateQueries, parametersList) = BuildBaseUpdateQueryList(strengthSet);
            parametersList.Add(new MySqlParameter("@exerciseSetId", strengthSet.ExerciseSetId));
            string sql = $"UPDATE exercise_set SET {string.Join(", ", updateQueries)} WHERE exerciseSetId = @exerciseSetId AND workoutId = @workoutId AND exerciseId = @exerciseId;";
            if (strengthSet.SetNumber == 1)
            {
                db.ExecuteNonQuery(sql, parametersList);
            }
            (updateQueries, parametersList) = BuildUpdateQueryList(strengthSet);
            parametersList.Add(new MySqlParameter("@exerciseSetId", strengthSet.ExerciseSetId));
            sql = $"UPDATE strength_set SET {string.Join(", ", updateQueries)} WHERE exerciseSetId = @exerciseSetId AND setNumber = @setNumber;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectsql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, s.setNumber, s.reps, s.weight FROM strength_set s JOIN exercise_set e ON e.exerciseSetId = s.exerciseSetId WHERE s.exerciseSetId = @exerciseSetId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@exerciseSetId", strengthSet.ExerciseSetId),
                new MySqlParameter("@workoutId", strengthSet.WorkoutId),
                new MySqlParameter("@exerciseId", strengthSet.ExerciseId)
            };
            var result = db.ExecuteSelect(selectsql, parameters);
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

        public StrengthSet? Delete(int exerciseSetId, int setId=-1)
        {
            if (exerciseSetId <= 0)
                return null;

            string selectsql = "SELECT e.exerciseSetId, e.workoutId, e.exerciseId, e.notes, s.setNumber, s.reps, s.weight FROM strength_set s JOIN exercise_set e ON e.exerciseSetId = s.exerciseSetId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@exerciseSetId", exerciseSetId)
            };
            var result = db.ExecuteSelect(selectsql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            // Delete head exercise_set if record is only one linked to that set
            if(result.Rows.Count == 1 && setId >= 1)
            {
                string sql = "DELETE FROM exercise_set WHERE exerciseSetId = @exerciseSetId";
                db.ExecuteNonQuery(sql, parameters);
            }
            else
            {
                string sql = "DELETE FROM strength_set WHERE exerciseSetId = @exerciseSetId AND setNumber = @setNumber";
                parameters.Add(new MySqlParameter("@setNumber", setId));
                db.ExecuteNonQuery(sql, parameters);
            }

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        private List<StrengthSet> DataTableToList(DataTable table)
        {
            var list = new List<StrengthSet>();
            foreach (DataRow row in table.Rows)
            {
                var strengthSet = new StrengthSet
                {
                    ExerciseSetId = Convert.ToInt32(row["exerciseSetId"]),
                    WorkoutId = Convert.ToInt32(row["workoutId"]),
                    ExerciseId = Convert.ToInt32(row["exerciseId"]),
                    Notes = Convert.ToString(row["notes"]) ?? "",
                    SetNumber = Convert.ToInt32(row["setNumber"]),
                    Reps = Convert.ToInt32(row["reps"]),
                    Weight = Convert.ToDouble(row["weight"])
                };
                list.Add(strengthSet);
            }
            return list;
        }

        private (List<string> query, List<MySqlParameter> parameters) BuildBaseUpdateQueryList(StrengthSet strengthSet)
        {
            StrengthSet defaultStrengthSet = new StrengthSet();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (strengthSet.ExerciseId != defaultStrengthSet.ExerciseId)
            {
                querys.Add("exerciseId = @exerciseId");
                parameters.Add(new MySqlParameter("@exerciseId", strengthSet.ExerciseId));
            }
            if (strengthSet.WorkoutId != defaultStrengthSet.WorkoutId)
            {
                querys.Add("workoutId = @workoutId");
                parameters.Add(new MySqlParameter("@workoutId", strengthSet.WorkoutId));
            }
            if (strengthSet.Notes != defaultStrengthSet.Notes)
            {
                querys.Add("notes = @notes");
                parameters.Add(new MySqlParameter("@notes", strengthSet.Notes));
            }

            querys.Add("setType = @setType");
            parameters.Add(new MySqlParameter("@setType", "Strength"));

            return (querys, parameters);
        }

        private (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(StrengthSet strengthSet)
        {
            StrengthSet defaultStrengthSet = new StrengthSet();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(strengthSet.SetNumber != defaultStrengthSet.SetNumber)
            {
                querys.Add("setNumber = @setNumber");
                parameters.Add(new MySqlParameter("@setNumber", strengthSet.SetNumber));
            }
            if (strengthSet.Reps != defaultStrengthSet.Reps)
            {
                querys.Add("reps = @reps");
                parameters.Add(new MySqlParameter("@reps", strengthSet.Reps));
            }
            if (strengthSet.Weight != defaultStrengthSet.Weight)
            {
                querys.Add("weight = @weight");
                parameters.Add(new MySqlParameter("@weight", strengthSet.Weight));
            }
            return (querys, parameters);
        }

        private (List<string>, List<string>, List<MySqlParameter>) BuildBaseInsertQueryList(StrengthSet strengthSet)
        {
            StrengthSet defaultStrengthSet = new StrengthSet();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (strengthSet.ExerciseId != defaultStrengthSet.ExerciseId)
            {
                cols.Add("exerciseId");
                vals.Add("@exerciseId");
                parameters.Add(new MySqlParameter("@exerciseId", strengthSet.ExerciseId));
            }
            if (strengthSet.WorkoutId != defaultStrengthSet.WorkoutId)
            {
                cols.Add("workoutId");
                vals.Add("@workoutId");
                parameters.Add(new MySqlParameter("@workoutId", strengthSet.WorkoutId));
            }
            if (strengthSet.Notes != defaultStrengthSet.Notes)
            {
                cols.Add("notes");
                vals.Add("@notes");
                parameters.Add(new MySqlParameter("@notes", strengthSet.Notes));
            }

            cols.Add("setType");
            vals.Add("@setType");
            parameters.Add(new MySqlParameter("@setType", "Strength"));

            return (cols, vals, parameters);
        }

        private (List<string>, List<string>, List<MySqlParameter>) BuildInsertQueryList(StrengthSet strengthSet)
        {
            StrengthSet defaultStrengthSet = new StrengthSet();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if(strengthSet.SetNumber != defaultStrengthSet.SetNumber)
            {
                cols.Add("setNumber");
                vals.Add("@setNumber");
                parameters.Add(new MySqlParameter("@setNumber", strengthSet.SetNumber));
            }
            if (strengthSet.Reps != defaultStrengthSet.Reps)
            {
                cols.Add("reps");
                vals.Add("@reps");
                parameters.Add(new MySqlParameter("@reps", strengthSet.Reps));
            }
            if (strengthSet.Weight != defaultStrengthSet.Weight)
            {
                cols.Add("weight");
                vals.Add("@weight");
                parameters.Add(new MySqlParameter("@weight", strengthSet.Weight));
            }
            return (cols, vals, parameters);
        }
    }
}
