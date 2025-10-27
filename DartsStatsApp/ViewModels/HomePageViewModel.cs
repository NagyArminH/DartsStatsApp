using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DartsStatsApp.ViewModels
{
    public class HomePageViewModel
    {
        public ICommand NavigateToOOM { get; set; }
        public ICommand NavigateToTournaments { get; set; }
        public ICommand NavigateToComparison { get; set; }
        public HomePageViewModel()
        {
            NavigateToOOM = new Command(navigateToOOM);
            NavigateToTournaments = new Command(navigateToTournaments);
            NavigateToComparison = new Command(navigateToComparison);
        }

        private async void navigateToOOM()
        {
            await Shell.Current.GoToAsync("//OOM");
        }
        private async void navigateToTournaments()
        {
            await Shell.Current.GoToAsync("//Tournaments");
        }
        private async void navigateToComparison()
        {
            await Shell.Current.GoToAsync("//Compare");
        }
    }
}
