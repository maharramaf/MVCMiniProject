namespace MVCMiniProject.ViewModels.Courses
{
    public class CourseDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SalesCount { get; set; }
        public bool IsFeature { get; set; }
        public bool IsNew { get; set; }
        public string TeacherName { get; set; }
        public string MainImage { get; set; }
        public string TeacherImage { get; set; }
        public List<string> CourseImages { get; set; }
    }
}
