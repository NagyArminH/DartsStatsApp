using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DartsStatsApp.Models;
using DartsStatsApp.Services;
using DartsStatsApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DartsStatsApp.ViewModels
{
    public class OOMListViewModel : ObservableObject
    {
        #region fields
        private DbService _dbService;
        private string _searchText;
        private List<PlayerEntity> allPlayers = new List<PlayerEntity>();
        #endregion

        #region properties
        public ObservableCollection<PlayerEntity> PlayerList { get; } = new ObservableCollection<PlayerEntity>();

        public string SearchText
        {
            get => _searchText;
            set 
            {  
                SetProperty(ref _searchText, value);
                SearchFilter();
            }

        }
        #endregion
        public IRelayCommand SearchCommand { get; }
        public IRelayCommand<PlayerEntity> NavigateToPlayerProfile {  get; }

        public OOMListViewModel(DbService dbService)
        {
            _dbService = dbService;
            SearchCommand = new RelayCommand(SearchFilter);
            NavigateToPlayerProfile = new RelayCommand<PlayerEntity>(navigateToPlayerProfile);
            getAllOOMPlayers();
        }
        private async void getAllOOMPlayers()
        {
            var players = await _dbService.GetData<PlayerEntity>();

            var orderedList = (from p in players
                              where p.OOMPlacement > 0
                              orderby p.OOMPlacement
                              select p).ToList();
            allPlayers = orderedList;
            PlayerList.Clear();
            foreach (PlayerEntity player in orderedList)
            {
                PlayerList.Add(player);
            }
        }
        private void SearchFilter()
        {
            var search = allPlayers.Where(s =>
            string.IsNullOrWhiteSpace(SearchText) ||
            (!string.IsNullOrEmpty(s.Name) && s.Name.ToLowerInvariant().Contains(SearchText))).OrderBy(s => s.OOMPlacement);

            PlayerList.Clear();
            foreach (var player in search)
                PlayerList.Add(player);
        }

        private async void navigateToPlayerProfile(PlayerEntity player)
        {
            if (player == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(PlayerProfileView)}?Id={player.Id}");
        }
    }
}
