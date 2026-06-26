using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Chatbot
{
    public class TaskService
    {
        private readonly string _connectionString =
            "server=localhost;database=cybersecurity_chatbot;user=root;password=YOUR_PASSWORD;";

        public void AddTask(string title, string description, DateTime? reminderDate)
        {
            using MySqlConnection conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = @"INSERT INTO Tasks 
                            (Title, Description, ReminderDate, IsCompleted) 
                            VALUES (@title, @description, @reminderDate, false)";

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@reminderDate", reminderDate.HasValue ? reminderDate.Value : DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public List<CyberTask> GetTasks()
        {
            List<CyberTask> tasks = new();

            using MySqlConnection conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "SELECT TaskID, Title, Description, ReminderDate, IsCompleted FROM Tasks";

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            using MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    TaskID = reader.GetInt32("TaskID"),
                    Title = reader.GetString("Title"),
                    Description = reader.GetString("Description"),
                    ReminderDate = reader["ReminderDate"] == DBNull.Value ? null : reader.GetDateTime("ReminderDate"),
                    IsCompleted = reader.GetBoolean("IsCompleted")
                });
            }

            return tasks;
        }

        public void CompleteTask(int taskId)
        {
            using MySqlConnection conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "UPDATE Tasks SET IsCompleted = true WHERE TaskID = @taskId";

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@taskId", taskId);
            cmd.ExecuteNonQuery();
        }

        public void DeleteTask(int taskId)
        {
            using MySqlConnection conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = "DELETE FROM Tasks WHERE TaskID = @taskId";

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@taskId", taskId);
            cmd.ExecuteNonQuery();
        }
    }
}