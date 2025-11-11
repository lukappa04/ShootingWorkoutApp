namespace swbackend.Db.Models.Log;

public class UserActionLog
{
    public int Id { get; set; } //Primary key
    
    public string UserId { get; set; } = string.Empty; //dall'idenity
    public string UserName { get; set; } = string.Empty; //Maggiore chiarezza nel sapere di chi è il log
    
    public DateTime RecordDateTime { get; set; } = DateTime.UtcNow; 
    
    public string HttpMethod { get; set; } = string.Empty; //GET, POST, ecc
    public string Path { get; set; } = string.Empty; // api/product/5
    public string QueryString { get; set; } = string.Empty; //?page=...
    
    public string ActionName { get; set; } = string.Empty; // nome dell'endpoint
    
}