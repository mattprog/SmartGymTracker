using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class Muscle_DB
    {
        private readonly Database _db;

        public Muscle_DB()
        {
            _db = new DB();
        }

        public List<Muscle> GetAll()
        {
            string sql = "SELECT * FROM exercise";
            var dbreturn = _db.ExecuteSelect(sql);
            List<Muscle> muscle = DataTableToList(dbreturn);
            return muscle;
        }

        public Muscle? Add(Muscle muscle)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workout);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return;
            string sql = $"INSERT INTO muscle ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, parametersList) = BuildUpdateQueryList(exercise);
            string sql = $"SELECT muscleId, name, description FROM muscle WHERE {string.Join(" AND ", queries)};";
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

        public Muscle? Update(Muscle muscle)
        {
            // Return if no valid user id
            if (muscle.MuscleId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(muscle);
            parametersList.Add(new MySqlParameter("@muscleId", muscle.MuscleId));
            string sql = $"UPDATE muscle SET {string.Join(", ", updateQueries)} WHERE muscleId = @muscleId;";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectsql = $"SELECT muscleId, name, description FROM muscle WHERE {string.Join(" AND ", queries)};";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@muscleId", muscle.MuscleId)
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

        public Exercise? Delete(int muscleId)
        {
            if (exerciseId <= 0)
                return;
            // Get updated record
            // Get updated record
            string selectsql = $"SELECT muscleId, name, description FROM muscle WHERE {string.Join(" AND ", queries)};";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@muscleId", muscle.MuscleId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM muscle WHERE muscleId = @muscleId";
            _db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Rows.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Muscle> DataTableToList(Datatable t)
        {
            List<Muscle> muscles = new List<Muscle>();
            foreach (DataRow row in t.Rows)
            {
                Muscle muscle = new Muscle
                {
                    MuscleId = Convert.ToInt32(row["muscleId"]),
                    Name = Convert.ToString(row["name"]),
                    Description = Convert.ToString(row["description"])
                };
                muscles.Add(muscle);
            }
            return muscles;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Muscle muscle)
        {
            Muscle defaultMuscle = new Muscle();
            List<string> querys = new List<string>;
            List<MySqlParameter> parameters = new List<MySqlParameters>();

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

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildUpdateQueryList(Muscle muscle)
        {
            Muscle defaultMuscle = new Muscle();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameters>();

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