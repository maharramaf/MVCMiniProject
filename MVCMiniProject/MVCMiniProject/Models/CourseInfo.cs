namespace MVCMiniProject.Models
{
    public class CourseInfo : BaseEntity
    {
        public string Title { get; set; }
        public string  Description  { get; set; }
        public  int  Price { get; set; }
        public int SalesCount { get; set; }
        public bool IsFeature { get; set; }
        public bool IsNew { get; set; }
        public  int  TeacherId { get; set; }
        public Teacher  Teacher { get; set; }
        public ICollection<CourseImage> CourseImages { get; set; }

    }
}
