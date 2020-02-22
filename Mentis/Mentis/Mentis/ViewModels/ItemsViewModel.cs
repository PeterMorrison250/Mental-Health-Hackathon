using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Mentis.Models;
using Mentis.Views;
using System.Windows.Input;

namespace Mentis.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public INavigation Navigation;

        private ICommand goToInformationCommand;

        public ICommand GoToInformationCommand {
            get => goToInformationCommand;
            set => SetProperty(ref goToInformationCommand, value); 
        }

        private ICommand goToResourcesCommand;

        public ICommand GoToResourcesCommand
        {
            get => goToResourcesCommand;
            set => SetProperty(ref goToResourcesCommand, value);
        }

        private ICommand goToBreathingCommand;

        public ICommand GoToBreathingCommand
        {
            get => goToBreathingCommand;
            set => SetProperty(ref goToBreathingCommand, value);
        }

        public ItemsViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            Title = "Browse";
            Items = new ObservableCollection<Item>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            GoToInformationCommand = new Command(async () => await ExecuteGoToInformationCommand());
            GoToResourcesCommand = new Command(async () => await ExecuteGoToResourcesCommand());
            GoToBreathingCommand = new Command(async () => await ExecuteGoToBreathingCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteGoToBreathingCommand()
        {
            await Navigation.PushAsync(new BreathingPage());
        }

        async Task ExecuteGoToResourcesCommand()
        {
            await Navigation.PushAsync(new ResourcesPage());
        }

        async Task ExecuteGoToInformationCommand()
        {
            await Navigation.PushAsync(new Views.InformationPage());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}