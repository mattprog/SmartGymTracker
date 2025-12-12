using System.Data;
using Library.SmartGymTracker.Models;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    public class User_DB
    {
        private readonly DB db = new DB();

        public User_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public User? GetById(int id)
        {
            if (id <= 0)
                return null;
            string sql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender, privilegeLevel, active FROM users WHERE userId = @userId";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", id)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<User> user = DataTableToList(dbreturn);
            if (user.Count != 0)
                return user[0];
            return null;
        }

        public List<User>? GetAll()
        {
            string sql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender, privilegeLevel, active FROM users";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<User> users = DataTableToList(dbreturn);
            if (users.Count != 0)
                return users;
            return null;
        }

        public User? Add(User user)
        {
            // Get updated records include all values
            // Return if no valid user id
            if (string.IsNullOrEmpty(user.Username))
                return null;

            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(user);
            if(columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO users ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)});";
            db.ExecuteNonQuery(sql, parametersList);

            // Get added record
            string selectSql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender, privilegeLevel, active FROM users WHERE username = @username;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@username", user.Username)
            };
            var result = db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count > 0)
            {
                var val = DataTableToList(result);
                if(val.Count > 0)
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
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender, privilegeLevel, active FROM users WHERE userId = @userId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", user.UserId)
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

        public User? Delete(int userId)
        {
            if(userId <= 0)
                return null;
            string selectSql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender, privilegeLevel, active FROM users WHERE userId = @userId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", userId)
            };
            var result = db.ExecuteSelect(selectSql, parameters);
            
            if(result.Rows.Count == 0)
            {
                // User not found
                return null;
            }

            string sql = "DELETE FROM users WHERE userId = @userId;";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public User? Login(string username, string password)
        {
            if(username == "" || password == "")
                return null;
            string selectSql = "SELECT userId, username, firstName, lastName, email, phoneNumber, dateOfBirth, gender, privilegeLevel, active FROM users WHERE username = @username AND password = @password AND active = TRUE;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@username", username),
                new MySqlParameter("@password", password)
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

        public List<User> DataTableToList(DataTable t)
        {
            List<User> users = new List<User>();
            foreach (DataRow row in t.Rows)
            {
                User user = new User
                {
                    UserId = Convert.ToInt32(row["userId"]),
                    Username = Convert.ToString(row["username"]) ?? "",
                    FirstName = Convert.ToString(row["firstName"]) ?? "",
                    LastName = Convert.ToString(row["lastName"]) ?? "",
                    Email = Convert.ToString(row["email"]) ?? "",
                    PhoneNumber = Convert.ToString(row["phoneNumber"]) ?? "",
                    DateOfBirth = DateOnly.FromDateTime(Convert.ToDateTime(row["dateOfBirth"])),
                    Gender = Convert.ToString(row["gender"]) ?? "",
                    Privilege = Enum.TryParse<PrivilegeLevel>(Convert.ToString(row["privilegeLevel"]), out var result) ? result : PrivilegeLevel.User,
                    Active = Convert.ToBoolean(row["active"])
                };
                users.Add(user);
            }
            return users;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(User user)
        {
            User defaultUser = new User();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (user.Username != defaultUser.Username)
            {
                querys.Add("username = @username");
                parameters.Add(new MySqlParameter("@username", user.Username));
            }

            if(user.Password != defaultUser.Password)
            {
                querys.Add("password = @password");
                parameters.Add(new MySqlParameter("@password", user.Password));
            }

            if (user.FirstName != defaultUser.FirstName)
            {
                querys.Add("firstName = @firstName");
                parameters.Add(new MySqlParameter("@firstName", user.FirstName));
            }

            if(user.LastName != defaultUser.LastName)
            {
                querys.Add("lastName = @lastName");
                parameters.Add(new MySqlParameter("@lastName", user.LastName));
            }

            if(user.Email != defaultUser.Email)
            {
                querys.Add("email = @email");
                parameters.Add(new MySqlParameter("@email", user.Email));
            }

            if(user.PhoneNumber != defaultUser.PhoneNumber)
            {
                querys.Add("phoneNumber = @phoneNumber");
                parameters.Add(new MySqlParameter("@phoneNumber", user.PhoneNumber));
            }

            if(user.DateOfBirth != defaultUser.DateOfBirth)
            {
                querys.Add("dateOfBirth = @dateOfBirth");
                parameters.Add(new MySqlParameter("@dateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd")));
            }

            if(user.Gender != defaultUser.Gender)
            {
                querys.Add("gender = @gender");
                parameters.Add(new MySqlParameter("@gender", user.Gender));
            }

            querys.Add("privilegeLevel = @privilegeLevel");
            parameters.Add(new MySqlParameter("@privilegeLevel", user.Privilege.ToString()));

            querys.Add("active = @active");
            parameters.Add(new MySqlParameter("@active", user.Active));

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(User user)
        {
            User defaultUser = new User();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (user.Username != defaultUser.Username)
            {
                cols.Add("username");
                vals.Add("@username");
                parameters.Add(new MySqlParameter("@username", user.Username));
            }
            
            if(user.Password != defaultUser.Password)
            {
                cols.Add("password");
                vals.Add("@password");
                parameters.Add(new MySqlParameter("@password", user.Password));
            }

            if (user.FirstName != defaultUser.FirstName)
            {
                cols.Add("firstName");
                vals.Add("@firstName");
                parameters.Add(new MySqlParameter("@firstName", user.FirstName));
            }

            if (user.LastName != defaultUser.LastName)
            {
                cols.Add("lastName");
                vals.Add("@lastName");
                parameters.Add(new MySqlParameter("@lastName", user.LastName));
            }

            if (user.Email != defaultUser.Email)
            {
                cols.Add("email");
                vals.Add("@email");
                parameters.Add(new MySqlParameter("@email", user.Email));
            }

            if (user.PhoneNumber != defaultUser.PhoneNumber)
            {
                cols.Add("phoneNumber");
                vals.Add("@phoneNumber");
                parameters.Add(new MySqlParameter("@phoneNumber", user.PhoneNumber));
            }

            if (user.DateOfBirth != defaultUser.DateOfBirth)
            {
                cols.Add("dateOfBirth");
                vals.Add("@dateOfBirth");
                parameters.Add(new MySqlParameter("@dateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd")));
            }

            if (user.Gender != defaultUser.Gender)
            {
                cols.Add("gender");
                vals.Add("@gender");
                parameters.Add(new MySqlParameter("@gender", user.Gender));
            }

            cols.Add("privilegeLevel");
            vals.Add("@privilegeLevel");
            parameters.Add(new MySqlParameter("@privilegeLevel", user.Privilege.ToString()));

            cols.Add("active");
            vals.Add("@active");
            parameters.Add(new MySqlParameter("@active", user.Active));

            return (cols, vals, parameters);
        }
    }
}