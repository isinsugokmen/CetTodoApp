using CetTodoApp.Data;

namespace CetTodoApp;

public partial class MainPage : ContentPage
{
    // YENİ: Veritabanı bağlantı nesnesi
    TodoDatabase database;

    public MainPage()
    {
        InitializeComponent();
        
        // YENİ: Veritabanını başlatıyoruz
        database = new TodoDatabase();
        
        // NOT: FakeDb ile eklediğin test verilerini sildik çünkü veritabanı boş başlayacak
        // veya daha önce kaydettiklerini getirecek.
    }

    // YENİ: Uygulama açıldığında verileri yüklemek için bu metodu ekliyoruz
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshListView();
    }

    private async void AddButton_OnClicked(object? sender, EventArgs e)
    {
        // --- SENİN YAZDIĞIN VALIDASYON KODLARI (AYNEN KORUNDU) ---
        bool isValid = true;
        
        TitleErrorLabel.IsVisible = false;
        DueDateErrorLabel.IsVisible = false;

        // 1. Title Validasyonu
        if (string.IsNullOrWhiteSpace(Title.Text))
        {
            TitleErrorLabel.IsVisible = true;
            isValid = false;
        }

        // 2. Due Date Validasyonu
        if (DueDate.Date < DateTime.Now.Date)
        {
            DueDateErrorLabel.IsVisible = true;
            isValid = false;
        }

        if (!isValid)
        {
            return; 
        }
        // --- VALIDASYON BİTİŞ ---

        // DEĞİŞEN KISIM: FakeDb yerine gerçek veritabanına kayıt
        var newItem = new TodoItem
        {
            Title = Title.Text,
            DueDate = DueDate.Date,
            IsComplete = false
        };

        // Veritabanına kaydet (Async olduğu için 'await' kullanıyoruz)
        await database.SaveItemAsync(newItem);

        // Ekranı temizle
        Title.Text = string.Empty;
        DueDate.Date = DateTime.Now;

        // Listeyi güncelle
        await RefreshListView();
    }

    // DEĞİŞEN KISIM: Verileri veritabanından çekme
    private async Task RefreshListView()
    {
        // FakeDb yerine veritabanından listeyi alıyoruz
        var items = await database.GetItemsAsync();
        
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = items; 
    }

    private async void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;

        var item = e.SelectedItem as TodoItem;
        
        // Durumu tersine çevir (Tamamlandı/Devam Ediyor)
        item.IsComplete = !item.IsComplete;

        // DEĞİŞEN KISIM: Güncellemeyi veritabanına kaydet
        await database.SaveItemAsync(item);

        // Seçimi kaldır
        TasksListView.SelectedItem = null;
        
        // Listeyi yenile
        await RefreshListView();
    }
}