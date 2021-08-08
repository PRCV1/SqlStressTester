using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using SqlStressTester.Models;
using SqlStressTester.Models.Interfaces;
using SqlStressTester.Utils.DataAccess;
using SqlStressTester.Utils.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlStressTester.ViewModels
{
    public class DataSourcesViewModel : ObservableObject, IBaseViewModel
    {
        private readonly IDialogService _dialogService;
        private DataSource _currentDataSource;
        private int _selectedTabIndex = 0;

        public string Title => "Datenquellen";

        public ObservableCollection<DataSource> DataSources { get; set; } = new();
        public DataSource CurrentDataSource
        {
            get => _currentDataSource;
            set
            {
                SetProperty(ref _currentDataSource, value);
                EditDataSourceCommand.NotifyCanExecuteChanged();
                DeleteDataSourceCommand.NotifyCanExecuteChanged();
            }
        }
        public int SelectedTabIndex { get => _selectedTabIndex; set => SetProperty(ref _selectedTabIndex, value); }

        public ObservableCollection<string> Databases { get; set; } = new();

        public RelayCommand AddNewDataSourceCommand { get; }
        public RelayCommand EditDataSourceCommand { get; }
        public RelayCommand<DataSource> DeleteDataSourceCommand { get; }
        public AsyncRelayCommand SaveDataSourceCommand { get; }
        public AsyncRelayCommand<DataSource> ConnectToDatabaseCommand { get; }

        public DataSourcesViewModel(IDialogService dialogService)
        {
            this._dialogService = dialogService;

            AddNewDataSourceCommand = new(OnAddNewDataSourceCommand);
            SaveDataSourceCommand = new(OnSaveDataSourceCommand, CanSaveDataSourceCommand);
            ConnectToDatabaseCommand = new(OnConnectoToDatabaseCommand);
            EditDataSourceCommand = new(OnEditDataSourceCommand, CanEditDataSourceCommand);
            DeleteDataSourceCommand = new(OnDeleteDataSourceCommand, CanDeleteDataSourceCommand);
        }

        private void OnAddNewDataSourceCommand()
        {
            DataSource dataSource = new();
            DataSources.Add(dataSource);
            CurrentDataSource = dataSource;

            SelectedTabIndex = 1;

            SaveDataSourceCommand.NotifyCanExecuteChanged();
        }

        private async Task OnSaveDataSourceCommand(CancellationToken cancellationToken)
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appdataProgramPath = Path.Combine(appdataPath, AppDomain.CurrentDomain.FriendlyName);
            string filePath = Path.Combine(appdataProgramPath, "datasources.json");
            Directory.CreateDirectory(appdataProgramPath);

            await Json.SaveToFileAsync<ObservableCollection<DataSource>>(DataSources, filePath, cancellationToken);
        }

        private bool CanSaveDataSourceCommand()
        {
            return DataSources != null && DataSources.Count >= 1;
        }

        private async Task OnConnectoToDatabaseCommand(DataSource dataSource, CancellationToken cancellationToken)
        {
            Database database = DatabaseFactory.Create(dataSource);
            Databases.Clear();
            foreach (string item in await database.GetDatabasesAsync(cancellationToken))
            {
                Databases.Add(item);
            }
        }

        private void OnEditDataSourceCommand()
        {
            SelectedTabIndex = 1;
        }

        private bool CanEditDataSourceCommand()
        {
            return CurrentDataSource != null;
        }

        private void OnDeleteDataSourceCommand(DataSource dataSource)
        {
            DataSources.Remove(dataSource);
            SaveDataSourceCommand.NotifyCanExecuteChanged();
        }

        private bool CanDeleteDataSourceCommand(DataSource dataSource)
        {
            return CurrentDataSource != null || dataSource != null;
        }

    }
}
