using System.Data;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class WorkoutType_DB
    {
        private readonly DB db = new DB();

        public WorkoutType_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public List<WorkoutType> GetAll()
        {
            string sql = "SELECT * FROM workoutType";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<WorkoutType> workoutType = DataTableToList(dbreturn);
            return workoutType;
        }

        public WorkoutType? Add(WorkoutType workoutType)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workoutType);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO workout_type ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, selectParametersList) = BuildUpdateQueryList(workoutType);
            string selectsql = $"SELECT workoutTypeId, name, description, difficulty FROM workoutType WHERE {string.Join(" AND ", queries)};";
            var result = db.ExecuteSelect(selectsql, selectParametersList);

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

        public WorkoutType? Update(WorkoutType workoutType)
        {
            // Return if no valid user id
            if (workoutType.WorkoutTypeId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(workoutType);
            parametersList.Add(new MySqlParameter("@workoutTypeId", workoutType.WorkoutTypeId));
            string sql = $"UPDATE workout_type SET {string.Join(", ", updateQueries)} WHERE workoutTypeId = @workoutTypeId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectsql = $"SELECT workoutTypeId, name, description, difficulty FROM workout_type WHERE workoutTypeId = @workoutTypeId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutTypeId", workoutType.WorkoutTypeId)
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

        public WorkoutType? Delete(int workoutTypeId)
        {
            if (workoutTypeId <= 0)
                return null;
            // Get updated record
            // Get updated record
            string selectsql = $"SELECT workoutTypeId, name, description, difficulty FROM workout_type WHERE workoutTypeId = @workoutTypeId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@workoutTypeId", workoutTypeId)
            };
            var result = db.ExecuteSelect(selectsql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM workout_type WHERE workoutTypeId = @workoutTypeId";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<WorkoutType> DataTableToList(DataTable t)
        {
            List<WorkoutType> workoutTypes = new List<WorkoutType>();
            foreach (DataRow row in t.Rows)
            {
                WorkoutType workoutType = new WorkoutType
                {
                    WorkoutTypeId = Convert.ToInt32(row["workoutTypeId"]),
                    Name = Convert.ToString(row["name"]) ?? "",
                    Description = Convert.ToString(row["description"]) ?? "",
                    Difficulty = Convert.ToString(row["difficulty"]) ?? ""
                };
                workoutTypes.Add(workoutType);
            }
            return workoutTypes;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(WorkoutType workoutType)
        {
            WorkoutType defaultWorkoutType = new WorkoutType();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            
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

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(WorkoutType workoutType)
        {
            WorkoutType defaultWorkoutType = new WorkoutType();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

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