using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MySQL.SmartGymTracker;

namespace MySQL.SmartGymTracker
{
    public class DB
    {
        public const int DEFAULT_LIMIT = 4;
        public string ErrorMessage { get; private set; } = string.Empty;

        private MySqlConnection? _connection;
        private MySqlCommand? _command;

        private static readonly string ConnectionString = $"Server={DB_HOST};Database={DB_NAME};User ID={DB_USER};Password={DB_PASSWORD};Convert Zero Datetime=True;Allow Zero Datetime=True;";
        private const string DB_HOST = "localhost";
        private const string DB_NAME = "smart_gym_tracker";
        private const string DB_USER = "root";
        private const string DB_PASSWORD = "";
        //private const long DB_PORT = 3306;

        public DB()
        {
            try
            {
                _connection = new MySqlConnection(ConnectionString);
                _connection.Open();
            }
            catch (MySqlException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        // For SELECT statements
        public DataTable ExecuteSelect(string query, List<MySqlParameter> parameters)
        {
            _command = new MySqlCommand(query, _connection);
            if(parameters != null && parameters.Count > 0)
            {
                _command.Parameters.AddRange(parameters.ToArray());
            }
            var adapter = new MySqlDataAdapter(_command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // For INSERT, UPDATE, DELETE statements
        public int ExecuteNonQuery(string query, List<MySqlParameter> parameters)
        {
            _command = new MySqlCommand(query, _connection);
            if (parameters != null && parameters.Count > 0)
            {
                _command.Parameters.AddRange(parameters.ToArray());
            }
            return _command.ExecuteNonQuery();
        }

        // For potential aggregate function calls/Counts
        public object ExecuteScalar(string query, List<MySqlParameter> parameters)
        {
            _command = new MySqlCommand(query, _connection);
            if (parameters != null && parameters.Count > 0)
            {
                _command.Parameters.AddRange(parameters.ToArray());
            }
            return _command.ExecuteScalar();
        }
    }
}