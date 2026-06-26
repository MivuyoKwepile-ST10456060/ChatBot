using System;
using System.Collections.Generic;
using System.Linq;

namespace Chatbot
{
    public class ActivityLogService
    {
        private readonly List<string> _logs = new();

        public void AddLog(string action)
        {
            _logs.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm}] {action}");
        }

        public List<string> GetRecentLogs()
        {
            return _logs
                .AsEnumerable()
                .Reverse()
                .Take(10)
                .ToList();
        }
    }
}