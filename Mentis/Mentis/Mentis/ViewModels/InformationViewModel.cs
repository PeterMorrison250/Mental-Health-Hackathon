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
    public class InformationViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public INavigation Navigation;

        private ICommand goToDepressionInformationCommand;

        public ICommand GoToDepressionInformationCommand
        {
            get => goToDepressionInformationCommand;
            set => SetProperty(ref goToDepressionInformationCommand, value);
        }

        private ICommand goToAnxietyInformationCommand;

        public ICommand GoToAnxietyInformationCommand
        {
            get => goToAnxietyInformationCommand;
            set => SetProperty(ref goToAnxietyInformationCommand, value);
        }

        public InformationViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            Title = "Conditions";
            Items = new ObservableCollection<Item>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            GoToDepressionInformationCommand = new Command(async () => await ExecuteGoToDepressionInformationCommand());
            GoToAnxietyInformationCommand = new Command(async () => await ExecuteGoToAnxietyInformationCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteGoToDepressionInformationCommand()
        {
            await Navigation.PushAsync(new DepressionInformationPage());
        }

        async Task ExecuteGoToAnxietyInformationCommand()
        {
            await Navigation.PushAsync(new AnxietyInformationPage());
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
