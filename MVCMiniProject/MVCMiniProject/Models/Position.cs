namespace MVCMiniProject.Models
{
    public class Position : BaseEntity
    {
        public string  Name  { get; set; }
        public  ICollection<Teacher> Teachers { get; set; }
        
    }
}
