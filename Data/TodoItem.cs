using SQLite; // 1. EKLENDİ: SQLite kütüphanesi

namespace CetTodoApp.Data;

public class TodoItem
{
    [PrimaryKey, AutoIncrement] // 2. EKLENDİ: Veritabanı için otomatik artan ID
    public int Id {get; set;}
    
    public string? Title {get; set;}
    public DateTime DueDate { get; set; }
    public DateTime CreatedDate {get; set;}
    public bool IsComplete { get; set; }

    // YENİ EKLENEN KOD: Süresi geçmiş ve tamamlanmamış öğeleri tespit etmek için
    public bool IsOverdue 
    {
        get
        {
            // Görev tamamlanmadıysa (IsComplete == false) VE Son Tarih bugünden öncesiyse
            return !IsComplete && DueDate.Date < DateTime.Now.Date;
        }
    }

    public TodoItem()
    {
        CreatedDate = DateTime.Now;
        IsComplete = false;
        Title = "";
    }
    
}