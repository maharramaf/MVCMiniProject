namespace MVCMiniProject.Admin
{
    public class AdminFieldDescriptor
    {
        public string Name { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public AdminFieldKind Kind { get; set; } = AdminFieldKind.Text;
        public bool Required { get; set; }
        public bool ShowInList { get; set; } = true;
        public bool ShowInForm { get; set; } = true;
        public bool ShowInDetail { get; set; } = true;
        public bool Searchable { get; set; }
        public bool Filterable { get; set; }
        public bool Sortable { get; set; } = true;
        public bool IsVirtual { get; set; }
        public string? RelationEntity { get; set; }
        public string? RelationDisplay { get; set; }
        public string? NavigationName { get; set; }
    }
}
