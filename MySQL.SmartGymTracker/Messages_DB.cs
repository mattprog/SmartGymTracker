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
    public class Messages_DB
    {
        private readonly DB db = new DB();

        public Messages_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public Messages? GetById(int id)
        {
            if (id <= 0)
                return null;
            string sql = "SELECT messageId, title, details, timeCreated, type FROM messages WHERE messageId = @messageId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@messageId", id)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<Messages> messages = DataTableToList(dbreturn);
            if (messages.Count > 0)
            {
                return messages[0];
            }
            return null;
        }

        public List<Messages>? GetAll()
        {
            string sql = "SELECT messageId, title, details, timeCreated, type FROM messages;";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<Messages> messages = DataTableToList(dbreturn);
            if (messages.Count != 0)
            {
                return messages;
            }
            return null;
        }

        public Messages? Add(Messages messages)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(messages);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO messages ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)}); SELECT LAST_INSERT_ID();";
            var success = db.ExecuteScalar(sql, parametersList);
            var validId = Convert.ToInt64(success);

            // Get added record
            string selectsql = "SELECT messageId, title, details, timeCreated, type FROM messages WHERE messageId = @messageId;";
            var selectParametersList = new List<MySqlParameter>
            {
                new MySqlParameter("@messageId", validId)
            };
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

        public Messages? Update(Messages messages)
        {
            // Return if no valid user id
            if (messages.MessageId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(messages);
            parametersList.Add(new MySqlParameter("@messageId", messages.MessageId));
            string sql = $"UPDATE messages SET {string.Join(", ", updateQueries)} WHERE messageId = @messageId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectsql = "SELECT messageId, title, details, timeCreated, type FROM messages WHERE messageId = @messageId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@messageId", messages.MessageId)
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

        public Messages? Delete(int messageId)
        {
            if (messageId <= 0)
                return null;
            // Get updated record
            string selectsql = "SELECT messageId, title, details, timeCreated, type FROM messages WHERE messageId = @messageId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@messageId", messageId)
            };
            var result = db.ExecuteSelect(selectsql, parameters);

            if (result.Rows.Count == 0)
            {
                // Message not found
                return null;
            }

            string sql = "DELETE FROM messages WHERE messageId = @messageId;";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Messages> DataTableToList(DataTable t)
        {
            List<Messages> messages = new List<Messages>();
            foreach (DataRow row in t.Rows)
            {
                Messages message = new Messages
                {
                    MessageId = Convert.ToInt32(row["messageId"]),
                    Title = Convert.ToString(row["title"]) ?? "",
                    Details = Convert.ToString(row["details"]) ?? "",
                    TimeCreated = Convert.ToDateTime(row["timeCreated"]),
                    Type = Enum.TryParse<MessageType>(Convert.ToString(row["type"]), out var result) ? result : MessageType.System
                };
                messages.Add(message);
            }
            return messages;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Messages messages)
        {
            Messages defaultMessage = new Messages();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(messages.Title != defaultMessage.Title)
            {
                querys.Add("title = @title");
                parameters.Add(new MySqlParameter("@title", messages.Title));
            }

            if(messages.Details != defaultMessage.Details)
            {
                querys.Add("details = @details");
                parameters.Add(new MySqlParameter("@details", messages.Details));
            }

            if(messages.TimeCreated != defaultMessage.TimeCreated)
            {
                querys.Add("timeCreated = @timeCreated");
                parameters.Add(new MySqlParameter("@timeCreated", messages.TimeCreated.ToString("yyyy-MM-dd HH:mm:ss")));
            }

            querys.Add("type = @type");
            parameters.Add(new MySqlParameter("@type", messages.Type.ToString()));

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(Messages messages)
        {
            Messages defaultMessage = new Messages();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(messages.Title != defaultMessage.Title)
            {
                cols.Add("title");
                vals.Add("@title");
                parameters.Add(new MySqlParameter("@title", messages.Title));
            }

            if(messages.Details != defaultMessage.Details)
            {
                cols.Add("details");
                vals.Add("@details");
                parameters.Add(new MySqlParameter("@details", messages.Details));
            }

            if(messages.TimeCreated != defaultMessage.TimeCreated)
            {
                cols.Add("timeCreated");
                vals.Add("@timeCreated");
                parameters.Add(new MySqlParameter("@timeCreated", messages.TimeCreated.ToString("yyyy-MM-dd HH:mm:ss")));
            }

            cols.Add("type");
            vals.Add("@type");
            parameters.Add(new MySqlParameter("@type", messages.Type.ToString()));

            return (cols, vals, parameters);
        }
    }
}
