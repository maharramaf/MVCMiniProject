namespace MVCMiniProject.Models
{
    public class CourseImage : BaseEntity
    {
        public string  Name { get; set; }
        public  bool IsMain { get; set; }
        public  CourseInfo CourseInfo { get; set; }
        public  int  CourseInfoId { get; set; }
    }
}
