using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Data.Common;
using System.Text;

namespace SqlStressTester.Models
{
    public class DataSource : ObservableObject
    {
        private string _displayName;
        private string _server;
        private bool _isMySql;
        private string _user;
        private string _password;
        private string _database;
        private int _port = 3306;
        private bool _windowsAuthentication;

        public string DisplayName { get => _displayName; set => SetProperty(ref _displayName, value); }
        public string Server { get => _server; set => SetProperty(ref _server, value); }
        public bool IsMySql { get => _isMySql; set => SetProperty(ref _isMySql, value); }
        public string User { get => _user; set => SetProperty(ref _user, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string Database { get => _database; set => SetProperty(ref _database, value); }
        public int Port { get => _port; set => SetProperty(ref _port, value); }
        public bool WindowsAuthentication { get => _windowsAuthentication; set => SetProperty(ref _windowsAuthentication, value); }
        public string ConnectionString { get => GetConnectionstring(); }

        private string GetConnectionstring()
        {
            StringBuilder stringBuilder = new();

            if (IsMySql)
            {
                stringBuilder.AppendFormat("Server={0};Uid={1};Pwd={2};", Server, User, Password);
                if (!string.IsNullOrWhiteSpace(Database))
                {
                    stringBuilder.AppendFormat("Database={0};", Database);
                }
                stringBuilder.AppendFormat("Port={0};", Port);
            }
            else
            {
                stringBuilder.AppendFormat("Server={0};", Server);
                if (WindowsAuthentication)
                {
                    stringBuilder.Append("Integrated Security=true;");
                }
                else
                {
                    stringBuilder.AppendFormat("User Id={0};Password={1};", User, Password);
                }
                if (!string.IsNullOrWhiteSpace(Database))
                {
                    stringBuilder.AppendFormat("Database={0};", Database);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
