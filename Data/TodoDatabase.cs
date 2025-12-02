using SQLite;

namespace CetTodoApp.Data;

public class TodoDatabase
{
    SQLiteAsyncConnection Database;

    public TodoDatabase()
    {
    }

    async Task Init()
    {
        if (Database is not null)
            return;

        // Veritabanı dosyasının yolunu belirliyoruz
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyTodo.db");

        Database = new SQLiteAsyncConnection(databasePath);
        
        // Tabloyu oluşturuyoruz (Eğer zaten varsa bir şey yapmaz)
        await Database.CreateTableAsync<TodoItem>();
    }

    public async Task<List<TodoItem>> GetItemsAsync()
    {
        await Init();
        // Tüm görevleri listeler
        return await Database.Table<TodoItem>().ToListAsync();
    }

    public async Task<int> SaveItemAsync(TodoItem item)
    {
        await Init();
        if (item.Id != 0) // ID sıfır değilse, bu kayıt zaten vardır, güncelle
            return await Database.UpdateAsync(item);
        else // ID sıfırsa, bu yeni bir kayıttır, ekle
            return await Database.InsertAsync(item);
    }

    public async Task<int> DeleteItemAsync(TodoItem item)
    {
        await Init();
        // Kaydı sil
        return await Database.DeleteAsync(item);
    }
}