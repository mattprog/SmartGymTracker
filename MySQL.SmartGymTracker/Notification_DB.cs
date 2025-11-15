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
    public class Notification_DB
    {
        private readonly DB db = new DB();

        public Notification_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public List<Notification>? GetByMessageId(int id)
        {
            if (id <= 0)
                return null;
            string sql = "SELECT userId, messageId, timeSent, read FROM notification WHERE messageId = @messageId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@messageId", id)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<Notification> n = DataTableToList(dbreturn);
            if (n.Count != 0)
                return n;
            return null;
        }

        public List<Notification>? GetByUserId(int id)
        {
            if (id <= 0)
                return null;
            string sql = "SELECT userId, messageId, timeSent, read FROM notification WHERE userId = @userId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", id)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<Notification> n = DataTableToList(dbreturn);
            if (n.Count != 0)
                return n;
            return null;
        }

        public List<Notification>? GetAll()
        {
            string sql = "SELECT userId, messageId, timeSent, read FROM notification;";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<Notification> n = DataTableToList(dbreturn);
            if (n.Count != 0)
                return n;
            return null;
        }

        public Notification? Add(Notification n)
        {
            if(n.UserId <= 0 || n.MessageId <= 0)
                return null;

            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(n);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO notification ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)}); SELECT LAST_INSERT_ID();";
            var success = db.ExecuteScalar(sql, parametersList);
            var validId = Convert.ToInt64(success);

            // Get added record
            string selectsql = $"SELECT userId, messageId, timeSent, read FROM notification WHERE userId = @userId AND messageId = @messageId;";
            var selparametersList = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", n.UserId),
                new MySqlParameter("@messageId", n.MessageId)
            };
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

        public Notification? Update(Notification n)
        {
            // Return if no valid user id
            if (n.UserId <= 0 || n.MessageId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(n);
            parametersList.Add(new MySqlParameter("@userId", n.UserId));
            parametersList.Add(new MySqlParameter("@messageId", n.MessageId));
            string sql = $"UPDATE notification SET {string.Join(", ", updateQueries)} WHERE userId = @userId AND messageId = @messageId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectsql = $"SELECT userId, messageId, timeSent, read FROM notification WHERE userId = @userId AND messageId = @messageId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", n.UserId),
                new MySqlParameter("@messageId", n.MessageId)
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

        public Notification? Delete(int userId, int messageId)
        {
            if (userId <= 0 || messageId <= 0)
                return null;
            string selectSql = $"SELECT userId, messageId, timeSent, read FROM notification WHERE userId = @userId AND messageId = @messageId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", userId),
                new MySqlParameter("@messageId", messageId)
            };
            var result = db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // Notification not found
                return null;
            }

            string sql = "DELETE FROM notification WHERE userId = @userId AND messageId = @messageId;";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Notification> DataTableToList(DataTable t)
        {
            List<Notification> n = new List<Notification>();
            foreach (DataRow row in t.Rows)
            {
                Notification notification = new Notification
                {
                    UserId = Convert.ToInt32(row["userId"]),
                    MessageId = Convert.ToInt32(row["messageId"]),
                    TimeSent = Convert.ToDateTime(row["timeSent"]),
                    Read = Convert.ToBoolean(row["read"])
                };
                n.Add(notification);
            }
            return n;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Notification n)
        {
            Notification defaultNotification = new Notification();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(n.TimeSent != defaultNotification.TimeSent)
            {
                querys.Add("timeSent = @timeSent");
                parameters.Add(new MySqlParameter("@timeSent", n.TimeSent.ToString("yyyy-MM-dd HH:mm:ss")));
            }

            if(n.Read != defaultNotification.Read)
            {
                querys.Add("read = @read");
                parameters.Add(new MySqlParameter("@read", n.Read));
            }
            
            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(Notification n)
        {
            Notification defaultNotification = new Notification();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(n.TimeSent != defaultNotification.TimeSent)
            {
                cols.Add("timeSent");
                vals.Add("@timeSent");
                parameters.Add(new MySqlParameter("@timeSent", n.TimeSent.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            
            if(n.Read != defaultNotification.Read)
            {
                cols.Add("read");
                vals.Add("@read");
                parameters.Add(new MySqlParameter("@read", n.Read));
            }

            return (cols, vals, parameters);
        }
    }
}