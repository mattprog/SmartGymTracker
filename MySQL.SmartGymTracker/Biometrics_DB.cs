using System.Data;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class Biometrics_DB
    {
        private readonly DB db = new DB();

        public Biometrics_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public List<Biometrics> GetAll()
        {
            string sql = "SELECT * FROM biometrics";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<Biometrics> biometrics = DataTableToList(dbreturn);
            return biometrics;
        }

        public Biometrics? Add(Biometrics biometrics)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(biometrics);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO biometrics ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            var success = db.ExecuteNonQuery(sql, parametersList);
            if (success <= 0)
                return null;

            // Get added record
            var (queries, selParametersList) = BuildUpdateQueryList(biometrics);
            string selectSql = $"SELECT biometricsId, userId, dateEntered, weight, height, bodyFatPercentage, bmi, restingHeartRate FROM biometrcs WHERE {string.Join(" AND ", queries)};";
            var result = db.ExecuteSelect(selectSql, selParametersList);

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

        public Biometrics? Update(Biometrics biometrics)
        {
            // Return if no valid user id
            if (biometrics.BiometricsId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(biometrics);
            parametersList.Add(new MySqlParameter("@biometricsId", biometrics.BiometricsId));
            string sql = $"UPDATE biometrics SET {string.Join(", ", updateQueries)} WHERE biometricsId = @biometricsId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT biometricsId, userId, dateEntered, weight, height, bodyFatPercentage, bmi, restingHeartRate WHERE biometricsId = @biometricsId";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@biometrics", biometrics.BiometricsId)
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

        public Biometrics? Delete(int biometricsId)
        {
            if (biometricsId <= 0)
                return null;
            // Get updated record
            string selectSql = "SELECT biometricsId, userId, dateEntered, weight, height, bodyFatPercentage, bmi, restingHeartRate WHERE biometricsId = @biometricsId";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@biometricsId", biometricsId)
            };
            var result = db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM biometrics WHERE biometricsId = @biometricsId";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Biometrics> DataTableToList(DataTable t)
        {
            List<Biometrics> biometrics = new List<Biometrics>();
            foreach (DataRow row in t.Rows)
            {
                Biometrics biometric = new Biometrics
                {
                    BiometricsId = Convert.ToInt32(row["biometricId"]),
                    UserId = Convert.ToInt32(row["userId"]),
                    DateEntered = DateOnly.FromDateTime(Convert.ToDateTime(row["dateEntered"])),
                    Weight = Convert.ToDouble(row["weight"]),
                    Height = Convert.ToDouble(row["height"]),
                    BodyFatPercentage = Convert.ToDouble(row["bodyFatPercentage"]),
                    BMI = Convert.ToDouble(row["bmi"]),
                    RestingHeartRate = Convert.ToInt32(row["restingHeartRate"])
                };
                biometrics.Add(biometric);
            }
            return biometrics;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Biometrics biometrics)
        {
            Biometrics defaultBiometrics = new Biometrics();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

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

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(Biometrics biometrics)
        {
            Biometrics defaultBiometrics = new Biometrics();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

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