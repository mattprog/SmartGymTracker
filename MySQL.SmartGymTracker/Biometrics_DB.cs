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
            string sql = "SELECT * FROM biometrics";
            var dbreturn = _db.ExecuteSelect(sql);
            List<Biometrics> biometrics = DataTableToList(dbreturn);
            return biometrics;
        }

        public Biometric? Add(Biometrics biometrics)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(workout);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return;
            string sql = $"INSERT INTO biometrics ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            var (queries, parametersList) = BuildUpdateQueryList(biometrics);
            string sql = $"SELECT biometricsId, userId, dateEntered, weight, height, bodyFatPercentage, bmi, restingHeartRate FROM biometrcs WHERE {string.Join(" AND ", queries)};";
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
            string selectSql = "SELECT biometricsId, userId, dateEntered, weight, height, bodyFatPercentage, bmi, restingHeartRate WHERE biometricsId = @biometricsId";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@biometrics", biometrics.BiometricsId)
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
            string selectSql = "SELECT biometricsId, userId, dateEntered, weight, height, bodyFatPercentage, bmi, restingHeartRate WHERE biometricsId = @biometricsId";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@biometricsId", biometrics.BiometricsId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM biometrics WHERE biometricsId = @biometricsId";
            _db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Rows.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Biometrics> DataTableToList(Datatable t)
        {
            List<Biometrics> biometrics = new List<Biometrics>();
            foreach (DataRow row in t.Rows)
            {
                Biometrics biometric = new Biometrics
                {
                    BiometricId = Convert.ToInt32(row["biometricId"]),
                    UserId = Convert.ToInt32(row["userId"]),
                    DateEntered = Convert.ToDateTime(row["dateEntered"]),
                    Weight = Convert.ToDecimal(row["weight"]),
                    Height = Convert.ToDecimal(row["height"]),
                    BodyFatPercentage = Convert.ToDecimal(row["bodyFatPercentage"]),
                    BMI = Convert.ToDecimal(row["bmi"]),
                    RestingHeartRate = Convert.ToInt32(row["restingHeartRate"])
                };
                biometrics.Add(biometric);
            }
            return biometrics;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Biometrics biometrics)
        {
            Biometrics defaultBiometrics = new Biometrics();
            List<string> querys = new List<string>;
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if (biometrics.UserId != defaultBiometrics.UserId)
            {
                querys.Add("userId = @userId");
                parameters.Add(new MySqlParameter("@userId", biometrics.UserId));
            }
            if(biometrics.DateEntered != defaultBiometrics.DateEntered)
            {
                querys.Add("dateEntered = @dateEntered");
                parameters.Add(new MySqlParameter("@dateEntered", biometrics.DateEntered));
            }
            if(biometrics.Weight != defaultBiometrics.Weight)
            {
                querys.Add("weight = @weight");
                parameters.Add(new MySqlParameter("@weight", biometrics.Weight));
            }
            if(biometrics.Height != defaultBiometrics.Height)
            {
                querys.Add("height = @height");
                parameters.Add(new MySqlParameter("@height", biometrics.Height));
            }
            if(biometrics.BodyFatPercentage != defaultBiometrics.BodyFatPercentage)
            {
                querys.Add("bodyFatPercentage = @bodyFatPercentage");
                parameters.Add(new MySqlParameter("@bodyFatPercentage", biometrics.BodyFatPercentage));
            }
            if(biometrics.BMI != defaultBiometrics.BMI)
            {
                querys.Add("bmi = @bmi");
                parameters.Add(new MySqlParameter("@bmi", biometrics.BMI));
            }
            if(biometrics.RestingHeartRate != defaultBiometrics.RestingHeartRate)
            {
                querys.Add("restingHeartRate = @restingHeartRate");
                parameters.Add(new MySqlParameter("@restingHeartRate", biometrics.RestingHeartRate));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildUpdateQueryList(Biometrics biometrics)
        {
            Biometrics defaultBiometrics = new Biometrics();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if(biometrics.UserId != defaultBiometrics.UserId)
            {
                cols.Add("userId");
                vals.Add("@userId");
                parameters.Add(new MySqlParameter("@userId", biometrics.UserId));
            }
            if(biometrics.DateEntered != defaultBiometrics.DateEntered)
            {
                cols.Add("dateEntered");
                vals.Add("@dateEntered");
                parameters.Add(new MySqlParameter("@dateEntered", biometrics.DateEntered));
            }
            if(biometrics.Weight != defaultBiometrics.Weight)
            {
                cols.Add("weight");
                vals.Add("@weight");
                parameters.Add(new MySqlParameter("@weight", biometrics.Weight));
            }
            if(biometrics.Height != defaultBiometrics.Height)
            {
                cols.Add("height");
                vals.Add("@height");
                parameters.Add(new MySqlParameter("@height", biometrics.Height));
            }
            if(biometrics.BodyFatPercentage != defaultBiometrics.BodyFatPercentage)
            {
                cols.Add("bodyFatPercentage");
                vals.Add("@bodyFatPercentage");
                parameters.Add(new MySqlParameter("@bodyFatPercentage", biometrics.BodyFatPercentage));
            }
            if(biometrics.BMI != defaultBiometrics.BMI)
            {
                cols.Add("bmi");
                vals.Add("@bmi");
                parameters.Add(new MySqlParameter("@bmi", biometrics.BMI));
            }
            if(biometrics.RestingHeartRate != defaultBiometrics.RestingHeartRate)
            {
                cols.Add("restingHeartRate");
                vals.Add("@restingHeartRate");
                parameters.Add(new MySqlParameter("@restingHeartRate", biometrics.RestingHeartRate));
            }

            return (cols, vals, parameters);
        }
    }
}