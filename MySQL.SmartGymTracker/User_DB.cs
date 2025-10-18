using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class User_DB
    {
        private readonly Database _db;

        public User_DB()
        {
            _db = new DB();
        }

        public List<User> GetAll()
        {
            string sql = "SELECT * FROM users";
            var dbreturn = _db.ExecuteSelect(sql);
            List<User> users = DataTableToList(dbreturn);
            return users;
        }

        public User? Add(User user)
        {
            // Get updated records include all values
            // Return if no valid user id
            if (string.IsNullOrEmpty(user.username))
                return null;

            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(user);
            if(columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO users ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            string selectSql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender FROM users WHERE username = @username";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", user.Username)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count > 0)
            {
                var val = DataTableToList(result);
                if(val.Rows.Count > 0)
                {
                    return val[0];
                }
            }
            return null;
        }

        public User? Update(User user)
        {
            // Return if no valid user id
            if (user.UserId <= 0)
                return null;
            
            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(user);
            parametersList.Add(new MySqlParameter("@userId", user.UserId));
            string sql = $"UPDATE users SET {string.Join(", ", updateQueries)} WHERE userId = @userId;";
            _db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender FROM users WHERE userId = @userId";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", user.UserId)
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

        public User? Delete(int userId)
        {
            if(userId <= 0)
                return;
            string selectSql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender FROM users WHERE userId = @userId";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", userId)
            };
            var result = _db.ExecuteSelect(selectSql, parameters);
            
            if(result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM users WHERE userId = @userId";
            _db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Rows.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public User? Login(string username, string password)
        {
            if (userId <= 0)
                return null;
            string selectSql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender FROM users WHERE username = @username AND password = @password";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", username),
                new MySqlParameter("@password", password)
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

        public List<User> DataTableToList(Datatable t)
        {
            List<User> users = new List<User>();
            foreach (DataRow row in t.Rows)
            {
                User user = new User
                {
                    UserId = Convert.ToInt32(row["userId"]),
                    Username = Convert.ToString(row["username"]),
                    FirstName = Convert.ToString(row["firstName"]),
                    LastName = Convert.ToString(row["lastName"]),
                    Email = Convert.ToString(row["email"]),
                    PhoneNumber = Convert.ToString(row["phoneNumber"]),
                    DateOfBirth = Convert.ToDateTime(row["dateOfBirth"]),
                    Gender = Convert.ToString(row["gender"])
                };
                users.Add(user);
            }
            return users;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(User user)
        {
            User defaultUser = new User();
            List<string> querys = new List<string>;
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if (user.Username != defaultUser.Username)
            {
                updates.Add("username = @username");
                parameters.Add(new MySqlParameter("@username", user.Username));
            }

            if(user.FirstName != defaultUser.FirstName)
            {
                updates.Add("firstName = @firstName");
                parameters.Add(new MySqlParameter("@firstName", user.FirstName));
            }

            if(user.LastName != defaultUser.LastName)
            {
                updates.Add("lastName = @lastName");
                parameters.Add(new MySqlParameter("@lastName", user.LastName));
            }

            if(user.email != defaultUser.Email)
            {
                updates.Add("email = @email");
                parameters.Add(new MySqlParameter("@email", user.Email));
            }

            if(user.phoneNumber != defaultUser.PhoneNumber)
            {
                updates.Add("phoneNumber = @phoneNumber");
                parameters.Add(new MySqlParameter("@phoneNumber", user.PhoneNumber));
            }

            if(user.dateOfBirth != defaultUser.DateOfBirth)
            {
                updates.Add("dateOfBirth = @dateOfBirth");
                parameters.Add(new MySqlParameter("@dateOfBirth", user.DateOfBirth));
            }

            if(user.gender != defaultUser.Gender)
            {
                updates.Add("gender = @gender");
                parameters.Add(new MySqlParameter("@gender", user.Gender));
            }

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(User user)
        {
            User defaultUser = new User();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameters>();

            if (user.Username != defaultUser.Username)
            {
                cols.Add("username");
                vals.Add("@username");
                parameters.Add(new MySqlParameter("@username", user.Username));
            }

            if (user.FirstName != defaultUser.FirstName)
            {
                cols.add("firstName");
                vals.add("@firstName");
                parameters.Add(new MySqlParameter("@firstName", user.FirstName));
            }

            if (user.LastName != defaultUser.LastName)
            {
                cols.Add("lastName");
                vals.Add("@lastName");
                parameters.Add(new MySqlParameter("@lastName", user.LastName));
            }

            if (user.email != defaultUser.Email)
            {
                cols.Add("email");
                vals.Add("@email");
                parameters.Add(new MySqlParameter("@email", user.Email));
            }

            if (user.phoneNumber != defaultUser.PhoneNumber)
            {
                cols.Add("phoneNumber");
                vals.Add("@phoneNumber");
                parameters.Add(new MySqlParameter("@phoneNumber", user.PhoneNumber));
            }

            if (user.dateOfBirth != defaultUser.DateOfBirth)
            {
                cols.Add("dateOfBirth");
                vals.Add("@dateOfBirth");
                parameters.Add(new MySqlParameter("@dateOfBirth", user.DateOfBirth));
            }

            if (user.gender != defaultUser.Gender)
            {
                cols.Add("gender");
                vals.Add("@gender");
                parameters.Add(new MySqlParameter("@gender", user.Gender));
            }

            return (cols, vals, parameters);
        }
    }
}