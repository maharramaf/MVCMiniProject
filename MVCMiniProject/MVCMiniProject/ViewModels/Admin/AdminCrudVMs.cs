using MVCMiniProject.Admin;

namespace MVCMiniProject.ViewModels.Admin
{
    public class AdminListVM
    {
        public AdminEntityDescriptor Entity { get; set; } = null!;
        public List<AdminListRow> Rows { get; set; } = new();
        public string? Search { get; set; }
        public string Sort { get; set; } = "Id";
        public string Dir { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int TotalPages => PageSize <= 0 ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public Dictionary<string, string> Filters { get; set; } = new();
        public Dictionary<string, List<AdminSelectOption>> FilterOptions { get; set; } = new();
    }

    public class AdminListRow
    {
        public int Id { get; set; }
        public Dictionary<string, string> Values { get; set; } = new();
        public string Title { get; set; } = string.Empty;
    }

    public class AdminSelectOption
    {
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }

    public class AdminFormVM
    {
        public AdminEntityDescriptor Entity { get; set; } = null!;
        public int? Id { get; set; }
        public bool IsEdit { get; set; }
        public Dictionary<string, string?> Values { get; set; } = new();
        public Dictionary<string, List<AdminSelectOption>> Lookups { get; set; } = new();
        public Dictionary<string, string> Errors { get; set; } = new();
    }

    public class AdminDetailVM
    {
        public AdminEntityDescriptor Entity { get; set; } = null!;
        public int Id { get; set; }
        public List<(string Label, string Value, bool IsImage)> Fields { get; set; } = new();
        public string Title { get; set; } = string.Empty;
    }

    public class AdminEntityCountVM
    {
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class AdminRecentItemVM
    {
        public string EntitySlug { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class AdminDashboardVM
    {
        public int TotalRecords { get; set; }
        public List<AdminEntityCountVM> EntityCounts { get; set; } = new();
        public List<AdminRecentItemVM> RecentItems { get; set; } = new();
        public int FeaturedCourses { get; set; }
        public int NewCourses { get; set; }
    }
}
