using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class WorkoutType_DB
    {
        private readonly Database _db;

        public WorkoutType_DB()
        {
            _db = new DB();
        }

        public List<WorkoutType> GetAll()
        {
            string sql = "SELECT * FROM workoutType";
            var dbreturn = _db.ExecuteSelect(sql);
            List<WorkoutType> workoutType = DataTableToList(dbreturn);
            return workoutType;
        }

        public WorkoutType? Add(WorkoutType workoutType)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workout);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return;
            string sql = $"INSERT INTO workout_type ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, parametersList) = BuildUpdateQueryList(exercise);
            string sql = $"SELECT workoutTypeId, name, description, difficulty FROM workoutType WHERE {string.Join(" AND ", queries)};";
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

        public WorkoutType? Update(WorkoutType workoutType)
        {
            // Return if no valid user id
            if (workoutType.WorkoutTypeId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(muscle);
            parametersList.Add(new MySqlParameter("@workoutTypeId", workoutType.WorkoutTypeId));
            string sql = $"UPDATE workout_type SET {string.Join(", ", updateQueries)} WHERE workoutTypeId = @workoutTypeId;";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectsql = $"SELECT workoutTypeId, name, description, difficulty FROM workout_type WHERE {string.Join(" AND ", queries)};";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@workoutTypeId", workoutType.WorkoutTypeId)
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

        public WorkoutType? Delete(int workoutTypeId)
        {
            if (workoutTypeId <= 0)
                return;
            // Get updated record
            // Get updated record
            string selectsql = $"SELECT workoutTypeId, name, description, difficulty FROM workout_type WHERE {string.Join(" AND ", queries)};";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@workoutTypeId", workoutTypeId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM workout_type WHERE workoutTypeId = @workoutTypeId";
            _db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Rows.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<WorkoutType> DataTableToList(Datatable t)
        {
            List<WorkoutType> workoutTypes = new List<WorkoutType>();
            foreach (DataRow row in t.Rows)
            {
                WorkoutType workoutType = new WorkoutType
                {
                    WorkoutTypeId = Convert.ToInt32(row["workoutTypeId"]),
                    Name = Convert.ToString(row["name"]),
                    Description = Convert.ToString(row["description"]),
                    Difficulty = Convert.ToString(row["difficulty"])
                };
                workoutTypes.Add(workoutType);
            }
            return workoutTypes;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(WorkoutType workoutType)
        {
            WorkoutType defaultWorkoutType = new WorkoutType();
            List<string> querys = new List<string>;
            List<MySqlParameter> parameters = new List<MySqlParameters>();
            
            if (workoutType.Name != defaultWorkoutType.Name)
            {
                querys.Add("name = @name");
                parameters.Add(new MySqlParameter("@name", workoutType.Name));
            }
            if( workoutType.Description != defaultWorkoutType.Description)
            {
                querys.Add("description = @description");
                parameters.Add(new MySqlParameter("@description", workoutType.Description));
            }
            if(workoutType.Difficulty != defaultWorkoutType.Difficulty)
            {
                querys.Add("difficulty = @difficulty");
                parameters.Add(new MySqlParameter("@difficulty", workoutType.Difficulty));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildUpdateQueryList(WorkoutType workoutType)
        {
            WorkoutType defaultWorkoutType = new WorkoutType();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(workoutType.Name != defaultWorkoutType.Name)
            {
                cols.Add("name");
                vals.Add("@name");
                parameters.Add(new MySqlParameter("@name", workoutType.Name));
            }
            if(workoutType.Description != defaultWorkoutType.Description)
            {
                cols.Add("description");
                vals.Add("@description");
                parameters.Add(new MySqlParameter("@description", workoutType.Description));
            }
            if(workoutType.Difficulty != defaultWorkoutType.Difficulty)
            {
                cols.Add("difficulty");
                vals.Add("@difficulty");
                parameters.Add(new MySqlParameter("@difficulty", workoutType.Difficulty));
            }

            return (cols, vals, parameters);
        }
    }
}