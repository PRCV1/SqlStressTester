using MvvmDialogs;
using SqlStressTester.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlStressTester.ViewModels
{
    public class SettingsViewModel : IBaseViewModel
    {
        private readonly IDialogService dialogService;

        public string Title => "Einstellungen";

        public SettingsViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }
            
    }
}
