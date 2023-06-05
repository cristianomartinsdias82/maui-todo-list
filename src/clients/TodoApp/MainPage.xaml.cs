using ToDoApiServiceClient;
using TodoApp.Pages;
using TodoApp.ServiceClientFacade;

namespace TodoApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IToDoApiServiceFacade _facade;

        public MainPage(IToDoApiServiceFacade facade)
        {
            InitializeComponent();

            _facade = facade;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadToDoList();
        }

        private async void OnAddToDoClicked(object sender, EventArgs e)
        {
            //await DisplayAlert("Way to go!", "You did it!", "Close"); It works!

            await RedirectToMaintainPage(new ToDo());
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //await DisplayAlert("Way to go!", "You did it again!", "Close"); It works!

            await RedirectToMaintainPage(e.CurrentSelection.FirstOrDefault() as ToDo);
        }

        private async void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            await LoadToDoList();
        }

        private async Task LoadToDoList()
        {
            var getToDosResult = await _facade.GetToDoListAsync();

            if (!getToDosResult.Succeeded)
            {
                await DisplayAlert("Oops!", getToDosResult.Message, "Close");
                return;
            }

            collectionView.ItemsSource = getToDosResult.Value!.ToDos;
        }

        private async Task RedirectToMaintainPage(ToDo toDo)
            => await Shell.Current.GoToAsync(nameof(MaintainToDoPage), new Dictionary<string, object> { { nameof(ToDo), toDo } });
    }
}