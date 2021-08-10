using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MvvmDialogs;
using SqlStressTester.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlStressTester.ViewModels
{
    public class MainWindowViewModel : ObservableObject, IBaseViewModel
    {
        private readonly IDialogService dialogService;
        private readonly IServiceProvider serviceProvider;
        private IBaseViewModel currentViewModel;

        public string Title => "SqlStressTester";

        public IBaseViewModel CurrentViewModel { get => currentViewModel; set => SetProperty(ref currentViewModel, value); }
        public RelayCommand<int?> NavigateRelayCommand { get; }

        public MainWindowViewModel(IDialogService dialogService, IServiceProvider serviceProvider)
        {
            this.dialogService = dialogService;
            this.serviceProvider = serviceProvider;
            NavigateRelayCommand = new(OnNavigate);
        }

        void OnNavigate(int? t) => CurrentViewModel = t switch
        {
            0 => (IBaseViewModel)serviceProvider.GetService(typeof(StressTesterViewModel)),
            1 => (IBaseViewModel)serviceProvider.GetService(typeof(SettingsViewModel)),
            2 => (IBaseViewModel)serviceProvider.GetService(typeof(DataSourcesViewModel)),
            _ => null
        };

    }
}
