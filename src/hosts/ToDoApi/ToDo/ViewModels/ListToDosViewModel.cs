namespace ToDoApi.ToDo.ViewModels
{
    public record struct ListToDosViewModel
    {
        public IEnumerable<ToDoViewModelItem> ToDos { get; init; }
    }
}
