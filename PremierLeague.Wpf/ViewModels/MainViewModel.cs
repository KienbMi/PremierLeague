using PremierLeague.Core.DataTransferObjects;
using PremierLeague.Persistence;
using PremierLeague.Wpf.Common;
using PremierLeague.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PremierLeague.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private ObservableCollection<TeamTableRowDto> teamTable;

        public ObservableCollection<TeamTableRowDto> TeamTable
        {
            get { return teamTable; }
            set 
            { 
                teamTable = value;
                OnPropertyChanged();
            }
        }

        public ICommand CmdAddGame { get; set; }


        public MainViewModel(IWindowController windowController) : base(windowController)
        {
            LoadCommands();
        }

        /// <summary>
        /// Erstellt die notwendigen Commands.
        /// </summary>
        private void LoadCommands()
        {
            CmdAddGame = new RelayCommand(async _ => await AddGame(), _ => true);
        }

        private async Task AddGame()
        {
            var model = await AddGameModel.CreateAsync(Controller);
            Controller.ShowWindow(model, true);
            _ = LoadDataAsync();
        }

        /// <summary>
        /// Asynchrones Laden von Daten für das ViewModel.
        /// Wird in CreateAsync(..) aufgerufen.
        /// </summary>
        /// <returns></returns>
        private async Task LoadDataAsync()
        {
            using UnitOfWork uow = new UnitOfWork();
            var teamTable = await uow.Teams.GetTeamTableAsync();
            TeamTable = new ObservableCollection<TeamTableRowDto>(teamTable);
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public static async Task<MainViewModel> CreateAsync(IWindowController windowController)
        {
            var viewModel = new MainViewModel(windowController);
            await viewModel.LoadDataAsync();
            return viewModel;
        }
    }
}
