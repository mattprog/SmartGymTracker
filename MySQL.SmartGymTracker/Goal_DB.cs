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
    public class Goal_DB
    {
        private readonly DB db = new DB();

        public Goal_DB() { }

        public string getLastErrorMessage()
        {
            return db.ErrorMessage;
        }

        public Goal? GetById(int id)
        {
            if (id <= 0)
                return null;
            string sql = "SELECT goalId, userId, timeCreated, title, description, startDate, targetEndDate, status FROM goal WHERE goalId = @goalId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@goalId", id)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<Goal> goals = DataTableToList(dbreturn);
            if (goals.Count != 0)
            {
                return goals[0];
            }
            return null;
        }


        public List<Goal>? GetByUserId(int userId)
        {
            if (userId <= 0)
                return null;
            string sql = "SELECT goalId, userId, timeCreated, title, description, startDate, targetEndDate, status FROM goal WHERE userId = @userId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@userId", userId)
            };
            var dbreturn = db.ExecuteSelect(sql, parameters);
            List<Goal> goals = DataTableToList(dbreturn);
            if (goals.Count != 0)
            {
                return goals;
            }
            return null;
        }


        public List<Goal>? GetAll()
        {
            string sql = "SELECT goalId, userId, timeCreated, title, description, startDate, targetEndDate, status FROM goal;";
            var dbreturn = db.ExecuteSelect(sql, new List<MySqlParameter>());
            List<Goal> goals = DataTableToList(dbreturn);
            if (goals.Count != 0)
            {
                return goals;
            }
            return null;
        }

        public Goal? Add(Goal goal)
        {
            // Perform update query
            var (columnQueries, valQueries, parametersList) = BuildInsertQueryList(goal);
            if (columnQueries.Count == 0 || valQueries.Count == 0 || parametersList.Count == 0)
                return null;
            string sql = $"INSERT INTO goal ({string.Join(", ", columnQueries)}) VALUES ({string.Join(", ", valQueries)}); SELECT LAST_INSERT_ID();";
            var success = db.ExecuteScalar(sql, parametersList);
            var validId = Convert.ToInt64(success);

            // Get added record
            string selectsql = "SELECT goalId, userId, timeCreated, title, description, startDate, targetEndDate, status FROM goal WHERE goalId = @goalId;";
            var selparametersList = new List<MySqlParameter>
            {
                new MySqlParameter("@goalId", validId)
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

        public Goal? Update(Goal goal)
        {
            // Return if no valid user id
            if (goal.GoalId <= 0)
                return null;

            // Perform update query
            var (updateQueries, parametersList) = BuildUpdateQueryList(goal);
            parametersList.Add(new MySqlParameter("@goalId", goal.GoalId));
            string sql = $"UPDATE goal SET {string.Join(", ", updateQueries)} WHERE goalId = @goalId;";
            db.ExecuteNonQuery(sql, parametersList);

            // Get updated record
            string selectSql = "SELECT goalId, userId, timeCreated, title, description, startDate, targetEndDate, status FROM goal WHERE goalId = @goalId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@goalId", goal.GoalId)
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

        public Goal? Delete(int goalId)
        {
            if (goalId <= 0)
                return null;
            // Get updated record
            string selectSql = "SELECT goalId, userId, timeCreated, title, description, startDate, targetEndDate, status FROM goal WHERE goalId = @goalId;";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@goalId", goalId)
            };
            var result = db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count == 0)
            {
                // Goal not found
                return null;
            }

            string sql = "DELETE FROM goal WHERE goalId = @goalId;";
            db.ExecuteNonQuery(sql, parameters);

            var val = DataTableToList(result);
            if (val.Count > 0)
            {
                return val[0];
            }
            return null;
        }

        public List<Goal> DataTableToList(DataTable t)
        {
            List<Goal> goals = new List<Goal>();
            foreach (DataRow row in t.Rows)
            {
                Goal goal = new Goal
                {
                    GoalId = Convert.ToInt32(row["goalId"]),
                    UserId = Convert.ToInt32(row["userId"]),
                    TimeCreated = Convert.ToDateTime(row["timeCreated"]),
                    Title = Convert.ToString(row["title"]) ?? "",
                    Description = Convert.ToString(row["description"]) ?? "",
                    StartDate = DateOnly.FromDateTime(Convert.ToDateTime(row["startDate"])),
                    TargetEndDate = DateOnly.FromDateTime(Convert.ToDateTime(row["targetEndDate"])),
                    Status = Enum.TryParse<GoalStatus>(Convert.ToString(row["status"]), out var result) ? result : GoalStatus.Not_Started
                };
                goals.Add(goal);
            }
            return goals;
        }

        public (List<string> query, List<MySqlParameter> parameters) BuildUpdateQueryList(Goal goal)
        {
            Goal defaultGoal = new Goal();
            List<string> querys = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(goal.UserId != defaultGoal.UserId)
            {
                querys.Add("userId = @userId");
                parameters.Add(new MySqlParameter("@userId", goal.UserId));
            }

            if (goal.TimeCreated != defaultGoal.TimeCreated)
            {
                querys.Add("timeCreated = @timeCreated");
                parameters.Add(new MySqlParameter("@timeCreated", goal.TimeCreated.ToString("yyyy-MM-dd HH:mm:ss")));
            }

            if (goal.Title != defaultGoal.Title)
            {
                querys.Add("title = @title");
                parameters.Add(new MySqlParameter("@title", goal.Title));
            }

            if (goal.Description != defaultGoal.Description)
            {
                querys.Add("description = @description");
                parameters.Add(new MySqlParameter("@description", goal.Description));
            }

            if (goal.StartDate != defaultGoal.StartDate)
            {
                querys.Add("startDate = @startDate");
                parameters.Add(new MySqlParameter("@startDate", goal.StartDate.ToString("yyyy-MM-dd")));
            }

            if(goal.TargetEndDate != defaultGoal.TargetEndDate)
            {
                querys.Add("targetEndDate = @targetEndDate");
                parameters.Add(new MySqlParameter("@targetEndDate", goal.TargetEndDate.ToString("yyyy-MM-dd")));
            }

            querys.Add("status = @status");
            parameters.Add(new MySqlParameter("@status", goal.Status.ToString()));

            return (querys, parameters);
        }

        public (List<string> cols, List<string> vals, List<MySqlParameter> parameters) BuildInsertQueryList(Goal goal)
        {
            Goal defaultGoal = new Goal();
            List<string> cols = new List<string>();
            List<string> vals = new List<string>();
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if(goal.UserId != defaultGoal.UserId)
            {
                cols.Add("userId");
                vals.Add("@userId");
                parameters.Add(new MySqlParameter("@userId", goal.UserId));
            }

            if(goal.TimeCreated != defaultGoal.TimeCreated)
            {
                cols.Add("timeCreated");
                vals.Add("@timeCreated");
                parameters.Add(new MySqlParameter("@timeCreated", goal.TimeCreated.ToString("yyyy-MM-dd HH:mm:ss")));
            }

            if(goal.Title != defaultGoal.Title)
            {
                cols.Add("title");
                vals.Add("@title");
                parameters.Add(new MySqlParameter("@title", goal.Title));
            }

            if(goal.Description != defaultGoal.Description)
            {
                cols.Add("description");
                vals.Add("@description");
                parameters.Add(new MySqlParameter("@description", goal.Description));
            }

            if(goal.StartDate!= defaultGoal.StartDate)
            {
                cols.Add("startDate");
                vals.Add("@startDate");
                parameters.Add(new MySqlParameter("@startDate", goal.StartDate.ToString("yyyy-MM-dd")));
            }

            if(goal.TargetEndDate != defaultGoal.TargetEndDate)
            {
                cols.Add("targetEndDate");
                vals.Add("@targetEndDate");
                parameters.Add(new MySqlParameter("@targetEndDate", goal.TargetEndDate.ToString("yyyy-MM-dd")));
            }

            cols.Add("status");
            vals.Add("@status");
            parameters.Add(new MySqlParameter("@status", goal.Status.ToString()));

            return (cols, vals, parameters);
        }
    }
}
