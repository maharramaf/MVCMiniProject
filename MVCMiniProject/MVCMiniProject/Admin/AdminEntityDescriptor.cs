namespace MVCMiniProject.Admin
{
    public class AdminEntityDescriptor
    {
        public string Slug { get; set; } = string.Empty;
        public string PluralName { get; set; } = string.Empty;
        public string SingularName { get; set; } = string.Empty;
        public string Icon { get; set; } = "fa-solid fa-database";
        public Type ClrType { get; set; } = typeof(object);
        public IReadOnlyList<AdminFieldDescriptor> Fields { get; set; } = Array.Empty<AdminFieldDescriptor>();
        public string[] Includes { get; set; } = Array.Empty<string>();
        public string DefaultSort { get; set; } = "Id";
        public string DefaultSortDir { get; set; } = "desc";
    }
}
