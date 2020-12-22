using PremierLeague.Core.Entities;
using PremierLeague.Persistence;
using PremierLeague.Wpf.Common;
using PremierLeague.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PremierLeague.Wpf.ViewModels
{
    public class AddGameModel : BaseViewModel
    {
        private int _round;
        private int _homeGoals;
        private int _guestGoals;
        private Team _selectedGuestTeam;
        private Team _selectedHomeTeam;
        private ObservableCollection<Team> _teams;

        public int Round
        {
            get { return _round; }
            set 
            { 
                _round = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public int HomeGoals
        {
            get { return _homeGoals; }
            set 
            { 
                _homeGoals = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public int GuestGoals
        {
            get { return _guestGoals; }
            set 
            { 
                _guestGoals = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public Team SelectedHomeTeam
        {
            get { return _selectedHomeTeam; }
            set 
            { 
                _selectedHomeTeam = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public Team SelectedGuestTeam
        {
            get { return _selectedGuestTeam; }
            set 
            { 
                _selectedGuestTeam = value;
                OnPropertyChanged();
                Validate();
            }
        }

        public ObservableCollection<Team> Teams
        {
            get { return _teams; }
            set 
            { 
                _teams = value;
                OnPropertyChanged();
            }
        }

        public ICommand CmdSave { get; set; }

        public AddGameModel(IWindowController controller) : base(controller)
        {
            LoadCommands();
        }

        private void LoadCommands()
        {
            CmdSave = new RelayCommand(async _ => await SaveAsync(), _ => IsValid);
        }

        private async Task SaveAsync()
        {
            using UnitOfWork uow = new UnitOfWork();

            var teamHome = await uow.Teams.GetById(SelectedHomeTeam.Id);
            var teamGuest = await uow.Teams.GetById(SelectedGuestTeam.Id);


            var game = new Game
            {
                Round = Round,
                HomeTeam = teamHome,
                GuestTeam = teamGuest,
                HomeGoals = HomeGoals,
                GuestGoals = GuestGoals
            };

            uow.Games.Add(game);

            try
            {
                await uow.SaveChangesAsync();
                Controller.CloseWindow(this);
            }
            catch (ValidationException validationException)
            {
                if (validationException.Value is IEnumerable<string> properties)
                {
                    foreach (var property in properties)
                    {
                        AddErrorsToProperty(property, new List<string> { validationException.ValidationResult.ErrorMessage });
                    }
                }
                else
                {
                    DbError = validationException.ValidationResult.ToString();
                }
            }
        }

        public static async Task<AddGameModel> CreateAsync(IWindowController windowController)
        {
            var viewModel = new AddGameModel(windowController);
            await viewModel.LoadDataAsync();
            viewModel.Validate();
            return viewModel;
        }

        public async Task LoadDataAsync()
        {
            using UnitOfWork uow = new UnitOfWork();
            var teams = await uow.Teams.GetAllAsync();
            Teams = new ObservableCollection<Team>(teams);
        }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int maxRound = (Teams.Count - 1) * 2;
            if (Round < 1 || Round > maxRound)
            {
                yield return new ValidationResult($"Round has to be beetween 1 and {maxRound}", new string[] { nameof(Round) });
            }

            if (SelectedHomeTeam == null)
            {
                yield return new ValidationResult($"Hometeam is not selected", new string[] { nameof(SelectedHomeTeam) });
            }

            if (SelectedGuestTeam == null)
            {
                yield return new ValidationResult($"Guestteam is not selected", new string[] { nameof(SelectedGuestTeam) });
            }

            if (SelectedGuestTeam != null && SelectedHomeTeam != null && SelectedGuestTeam == SelectedHomeTeam)
            {
                yield return new ValidationResult($"Hometeam is same as Guestteam", new string[] { nameof(Teams) });
            }

            if (HomeGoals < 0)
            {
                yield return new ValidationResult($"Homegoals are < 0", new string[] { nameof(HomeGoals) });
            }

            if (GuestGoals < 0)
            {
                yield return new ValidationResult($"Guestgoals are < 0", new string[] { nameof(GuestGoals) });
            }
        }
    }
}
