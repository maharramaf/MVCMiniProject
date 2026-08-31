namespace MVCMiniProject.Models
{
    public class Teacher : BaseEntity
    {
        public  string  FullName { get; set; }
        public string Image { get; set; }
        public int PositionId { get; set; }
        public  Position Position { get; set; }
        public  ICollection<CourseInfo> CourseInfos { get; set; }

    }
}
