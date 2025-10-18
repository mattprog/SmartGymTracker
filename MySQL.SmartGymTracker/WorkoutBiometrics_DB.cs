using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class Biometrics_DB
    {
        private readonly Database _db;

        public Biometrics_DB()
        {
            _db = new DB();
        }

        public List<Biometrics> GetAll()
        {
            string sql = "SELECT * FROM workoutbiometrics";
            var dbreturn = _db.ExecuteSelect(sql);
            List<WorkoutBiometrics> biometrics = DataTableToList(dbreturn);
            return biometrics;
        }

        public WorkoutBiometrics? Add(WorkoutBiometrics biometrics)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workout);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return;
            string sql = $"INSERT INTO workoutbiometrics ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, parametersList) = BuildUpdateQueryList(biometrics);
            string sql = $"SELECT workoutId, steps, averageHeartRate, maxHeartRate, caloriesBurned, feeling, sleepScore FROM workoutBiometrics WHERE {string.Join(" AND ", queries)};";
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

        public Biometrics? Update(Biometrics biometrics)
        {
            // Return if no valid user id
            if (biometrics.Biometrics <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(user);
            parametersList.Add(new MySqlParameter("@biometricsId", biometrics.BiometricsId));
            string sql = $"UPDATE biometrics SET {string.Join(", ", updateQueries)} WHERE biometricsId = @biometricsId;";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT workoutId, steps, averageHeartRate, maxHeartRate, caloriesBurned, feeling, sleepScore FROM workoutBiometrics WHERE workoutId = @workoutId";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@workoutId", biometrics.WorkoutId)
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

        public Biometrics? Delete(int biometricsId)
        {
            if (biometricsId <= 0)
                return;
            // Get updated record
            // Get updated record
            string selectSql = "SELECT workoutId, steps, averageHeartRate, maxHeartRate, caloriesBurned, feeling, sleepScore FROM workoutBiometrics WHERE workoutId = @workoutId";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@workoutId", biometrics.WorkoutId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM workoutBiometrics WHERE workoutId = @workoutId";
            _db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Rows.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<WorkoutBiometrics> DataTableToList(Datatable t)
        {
            List<WorkoutBiometrics> biometrics = new List<WorkoutBiometrics>();
            foreach (DataRow row in t.Rows)
            {
                WorkoutBiometrics biometric = new WorkoutBiometrics
                {
                    WorkoutId = Convert.ToInt32(row["workoutId"]),
                    Steps = Convert.ToInt32(row["steps"]),
                    AverageHeartRate = Convert.ToInt32(row["averageHeartRate"]),
                    MaxHeartRate = Convert.ToInt32(row["maxHeartRate"]),
                    CaloriesBurned = Convert.ToInt32(row["caloriesBurned"]),
                    Feeling = Convert.ToString(row["feeling"]),
                    SleepScore = Convert.ToInt32(row["sleepScore"])
                };
                biometrics.Add(biometric);
            }
            return biometrics;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(WorkoutBiometrics biometrics)
        {
            WorkoutBiometrics defaultBiometrics = new WorkoutBiometrics();
            List<string> querys = new List<string>;
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(biometrics.WorkoutId != defaultBiometrics.WorkoutId)
            {
                querys.Add("workoutId = @workoutId");
                parameters.Add(new MySqlParameter("@workoutId", biometrics.WorkoutId));
            }
            if(biometrics.Steps != defaultBiometrics.Steps)
            {
                querys.Add("steps = @steps");
                parameters.Add(new MySqlParameter("@steps", biometrics.Steps));
            }
            if(biometrics.AverageHeartRate != defaultBiometrics.AverageHeartRate)
            {
                querys.Add("averageHeartRate = @averageHeartRate");
                parameters.Add(new MySqlParameter("@averageHeartRate", biometrics.AverageHeartRate));
            }
            if(biometrics.MaxHeartRate != defaultBiometrics.MaxHeartRate)
            {
                querys.Add("maxHeartRate = @maxHeartRate");
                parameters.Add(new MySqlParameter("@maxHeartRate", biometrics.MaxHeartRate));
            }
            if(biometrics.CaloriesBurned != defaultBiometrics.CaloriesBurned)
            {
                querys.Add("caloriesBurned = @caloriesBurned");
                parameters.Add(new MySqlParameter("@caloriesBurned", biometrics.CaloriesBurned));
            }
            if(biometrics.Feeling != defaultBiometrics.Feeling)
            {
                querys.Add("feeling = @feeling");
                parameters.Add(new MySqlParameter("@feeling", biometrics.Feeling));
            }
            if(biometrics.SleepScore != defaultBiometrics.SleepScore)
            {
                querys.Add("sleepScore = @sleepScore");
                parameters.Add(new MySqlParameter("@sleepScore", biometrics.SleepScore));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildUpdateQueryList(WorkoutBiometrics biometrics)
        {
            WorkoutBiometrics defaultBiometrics = new WorkoutBiometrics();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(biometrics.WorkoutId != defaultBiometrics.WorkoutId)
            {
                cols.Add("workoutId");
                vals.Add("@workoutId");
                parameters.Add(new MySqlParameter("@workoutId", biometrics.WorkoutId));
            }
            if(biometrics.Steps != defaultBiometrics.Steps)
            {
                cols.Add("steps");
                vals.Add("@steps");
                parameters.Add(new MySqlParameter("@steps", biometrics.Steps));
            }
            if(biometrics.AverageHeartRate != defaultBiometrics.AverageHeartRate)
            {
                cols.Add("averageHeartRate");
                vals.Add("@averageHeartRate");
                parameters.Add(new MySqlParameter("@averageHeartRate", biometrics.AverageHeartRate));
            }
            if(biometrics.MaxHeartRate != defaultBiometrics.MaxHeartRate)
            {
                cols.Add("maxHeartRate");
                vals.Add("@maxHeartRate");
                parameters.Add(new MySqlParameter("@maxHeartRate", biometrics.MaxHeartRate));
            }
            if(biometrics.CaloriesBurned != defaultBiometrics.CaloriesBurned)
            {
                cols.Add("caloriesBurned");
                vals.Add("@caloriesBurned");
                parameters.Add(new MySqlParameter("@caloriesBurned", biometrics.CaloriesBurned));
            }
            if(biometrics.Feeling != defaultBiometrics.Feeling)
            {
                cols.Add("feeling");
                vals.Add("@feeling");
                parameters.Add(new MySqlParameter("@feeling", biometrics.Feeling));
            }
            if(biometrics.SleepScore != defaultBiometrics.SleepScore)
            {
                cols.Add("sleepScore");
                vals.Add("@sleepScore");
                parameters.Add(new MySqlParameter("@sleepScore", biometrics.SleepScore));
            }

            return (cols, vals, parameters);
        }
    }
}