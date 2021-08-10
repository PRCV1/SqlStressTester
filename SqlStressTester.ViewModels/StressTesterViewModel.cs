using LiveCharts.Wpf;
using LiveCharts;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using SqlStressTester.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlStressTester.ViewModels
{
    public class StressTesterViewModel : ObservableObject, IBaseViewModel
    {
        public string Title => "StressTester";

        private string _sql;
        private readonly IDialogService dialogService;

        public string SQL
        {
            get { return _sql; }
            set
            {
                if (SetProperty(ref _sql, value))
                {
                    SaveSqlFileCommand.NotifyCanExecuteChanged();
                    RunStressTestCommand.NotifyCanExecuteChanged();
                };
            }
        }

        public AsyncRelayCommand LoadSqlFileCommand { get; }
        public AsyncRelayCommand SaveSqlFileCommand { get; }
        public AsyncRelayCommand RunStressTestCommand { get; }
        public RelayCommand StopStressTestCommand { get; }

        public StressTesterViewModel(IDialogService dialogService)
        {
            LoadSqlFileCommand = new(OnLoadSqlFileCommand);
            SaveSqlFileCommand = new(OnSaveSqlFileCommand, CanSaveSqlFile);
            RunStressTestCommand = new(OnRunStressTestCommand, CanStressTest);
            StopStressTestCommand = new(OnStopStressTestCommand, CanStopStressTest);

            this.dialogService = dialogService;
        }

        private async Task OnLoadSqlFileCommand(CancellationToken cancellationToken)
        {
            OpenFileDialogSettings fileDialogSettings = new()
            {
                Title = "SQL-Datei auswählen",
                Filter = "SQL-Dateien (*.sql)|*.sql|Textdateien (*.txt)|*.txt|alle Dateien (*.*)|*.*",
                Multiselect = false
            };

            bool? success = dialogService.ShowOpenFileDialog(this, fileDialogSettings);

            if (!success ?? false)
            {
                return;
            }

            using Stream stream = File.OpenRead(fileDialogSettings.FileName);
            using StreamReader reader = new(stream);
            SQL = await reader.ReadToEndAsync();
        }

        private async Task OnSaveSqlFileCommand(CancellationToken cancellationToken)
        {
            SaveFileDialogSettings fileDialogSettings = new()
            {
                InitialDirectory = Environment.CurrentDirectory,
                Title = "SQL-Datei speichern",
                Filter = "SQL-Dateien (*.sql)|*.sql|Textdateien (*.txt)|*.txt"
            };

            bool? success = dialogService.ShowSaveFileDialog(this, fileDialogSettings);

            if (!success ?? false)
            {
                return;
            }

            using Stream stream = File.OpenWrite(fileDialogSettings.FileName);
            using StreamWriter writer = new(stream);
            await writer.WriteAsync(SQL);
        }

        private bool CanSaveSqlFile()
        {
            return !string.IsNullOrWhiteSpace(SQL);
        }

        private async Task OnRunStressTestCommand(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(5000, cancellationToken);
            }
        }

        private bool CanStressTest()
        {
            return !string.IsNullOrWhiteSpace(SQL) && !RunStressTestCommand.IsRunning;
        }

        private void OnStopStressTestCommand()
        {
            if (!RunStressTestCommand.CanBeCanceled)
            {
                return;
            }

            RunStressTestCommand.Cancel();
        }

        private bool CanStopStressTest()
        {
            return RunStressTestCommand.CanBeCanceled;
        }

    }
}
