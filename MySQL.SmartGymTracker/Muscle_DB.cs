using System.Data;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class Muscle_DB
    {
        private readonly DB db = new DB();

        public Muscle_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public List<Muscle> GetAll()
        {
            string sql = "SELECT * FROM exercise";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<Muscle> muscle = DataTableToList(dbreturn);
            return muscle;
        }

        public Muscle? Add(Muscle muscle)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(muscle);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO muscle ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, selparametersList) = BuildUpdateQueryList(muscle);
            string selectsql = $"SELECT muscleId, name, description FROM muscle WHERE {string.Join(" AND ", queries)};";
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

        public Muscle? Update(Muscle muscle)
        {
            // Return if no valid user id
            if (muscle.MuscleId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(muscle);
            parametersList.Add(new MySqlParameter("@muscleId", muscle.MuscleId));
            string sql = $"UPDATE muscle SET {string.Join(", ", updateQueries)} WHERE muscleId = @muscleId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectsql = $"SELECT muscleId, name, description FROM muscle WHERE muscleId = @ muscleId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@muscleId", muscle.MuscleId)
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

        public Muscle? Delete(int muscleId)
        {
            if (muscleId <= 0)
                return null;
            // Get updated record
            // Get updated record
            string selectsql = $"SELECT muscleId, name, description FROM muscle WHERE muscleId = @muscleId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@muscleId", muscleId)
            };
            var result = db.ExecuteSelect(selectsql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM muscle WHERE muscleId = @muscleId";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Muscle> DataTableToList(DataTable t)
        {
            List<Muscle> muscles = new List<Muscle>();
            foreach (DataRow row in t.Rows)
            {
                Muscle muscle = new Muscle
                {
                    MuscleId = Convert.ToInt32(row["muscleId"]),
                    Name = Convert.ToString(row["name"]) ?? "",
                    Description = Convert.ToString(row["description"]) ?? ""
                };
                muscles.Add(muscle);
            }
            return muscles;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Muscle muscle)
        {
            Muscle defaultMuscle = new Muscle();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(muscle.Name != defaultMuscle.Name)
            {
                querys.Add("name = @name");
                parameters.Add(new MySqlParameter("@name", muscle.Name));
            }
            if(muscle.Description != defaultMuscle.Description)
            {
                querys.Add("description = @description");
                parameters.Add(new MySqlParameter("@description", muscle.Description));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(Muscle muscle)
        {
            Muscle defaultMuscle = new Muscle();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(muscle.Name != defaultMuscle.Name)
            {
                cols.Add("name");
                vals.Add("@name");
                parameters.Add(new MySqlParameter("@name", muscle.Name));
            }
            if(muscle.Description != defaultMuscle.Description)
            {
                cols.Add("description");
                vals.Add("@description");
                parameters.Add(new MySqlParameter("@description", muscle.Description));
            }

            return (cols, vals, parameters);
        }
    }
}