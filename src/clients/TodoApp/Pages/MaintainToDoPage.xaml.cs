using ToDoApiServiceClient;
using TodoApp.ServiceClientFacade;

namespace TodoApp.Pages;

[QueryProperty(nameof(ToDoApiServiceClient.ToDo), nameof(ToDoApiServiceClient.ToDo))]
public partial class MaintainToDoPage : ContentPage
{
    private readonly IToDoApiServiceFacade _facade;
    private ToDo _toDo;
    private bool _isNew;

    public MaintainToDoPage(IToDoApiServiceFacade facade)
    {
        InitializeComponent();

        _facade = facade;

        BindingContext = this;
    }

    public ToDo ToDo
    {
        get => _toDo;
        set
        {
            _isNew = IsNew(value);
            _toDo = value;
            OnPropertyChanged();
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        deleteButton.IsVisible = !IsNew(_toDo);
    }

    protected async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var saveToDoResult = await _facade.SaveToDoAsync(ToDo);
        if (!saveToDoResult.Succeeded)
        {
            await DisplayAlert("Oops!", saveToDoResult.Message, "Close");

            return;
        }

        await RedirectToMainPage();
    }

    protected async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var promptResult = await DisplayPromptAsync("Confirmation", "Do you wish to delete the selected item?", initialValue: "YES");
        if (promptResult != "YES")
            return;

        var deleteToDoResult = await _facade.DeleteToDoAsync(ToDo.Id);
        if (!deleteToDoResult.Succeeded)
        {
            await DisplayAlert("Oops!", deleteToDoResult.Message, "Close");

            return;
        }

        await RedirectToMainPage();
    }

    protected async void OnCancelButtonClicked(object sender, EventArgs e)
        => await RedirectToMainPage();

    private async Task RedirectToMainPage()
        => await Shell.Current.GoToAsync("..");

    private bool IsNew(ToDo toDo)
        => toDo.Id == Guid.Empty;
}