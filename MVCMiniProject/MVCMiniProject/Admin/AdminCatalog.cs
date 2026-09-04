using MVCMiniProject.Models;

namespace MVCMiniProject.Admin
{
    public static class AdminCatalog
    {
        public static IReadOnlyList<AdminEntityDescriptor> Entities { get; } = Build();

        public static AdminEntityDescriptor? Find(string? slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return null;
            }

            return Entities.FirstOrDefault(e => string.Equals(e.Slug, slug, StringComparison.OrdinalIgnoreCase));
        }

        private static IReadOnlyList<AdminEntityDescriptor> Build()
        {
            return new List<AdminEntityDescriptor>
            {
                new()
                {
                    Slug = "courses",
                    PluralName = "Courses",
                    SingularName = "Course",
                    Icon = "fa-solid fa-book",
                    ClrType = typeof(CourseInfo),
                    Includes = new[] { "Teacher", "CourseImages" },
                    Fields = new[]
                    {
                        Text("Title", required: true, searchable: true),
                        Area("Description", required: true, list: false),
                        Number("Price"),
                        Number("SalesCount", "Sales"),
                        Flag("IsFeature", "Featured", filterable: true),
                        Flag("IsNew", "New", filterable: true),
                        Rel("TeacherId", "Teacher", "teachers", "FullName", "Teacher", filterable: true),
                        Image("CoverImage", "Cover image", isVirtual: true)
                    }
                },
                new()
                {
                    Slug = "course-images",
                    PluralName = "Course Images",
                    SingularName = "Course Image",
                    Icon = "fa-solid fa-image",
                    ClrType = typeof(CourseImage),
                    Includes = new[] { "CourseInfo" },
                    Fields = new[]
                    {
                        Image("Name", "Image", required: true),
                        Flag("IsMain", "Main image", filterable: true),
                        Rel("CourseInfoId", "Course", "courses", "Title", "CourseInfo", required: true, filterable: true)
                    }
                },
                new()
                {
                    Slug = "teachers",
                    PluralName = "Teachers",
                    SingularName = "Teacher",
                    Icon = "fa-solid fa-chalkboard-teacher",
                    ClrType = typeof(Teacher),
                    Includes = new[] { "Position" },
                    Fields = new[]
                    {
                        Text("FullName", "Full name", required: true, searchable: true),
                        Image("Image", required: true),
                        Rel("PositionId", "Position", "positions", "Name", "Position", required: true, filterable: true)
                    }
                },
                new()
                {
                    Slug = "positions",
                    PluralName = "Positions",
                    SingularName = "Position",
                    Icon = "fa-solid fa-id-badge",
                    ClrType = typeof(Position),
                    Fields = new[]
                    {
                        Text("Name", required: true, searchable: true)
                    }
                },
                new()
                {
                    Slug = "news",
                    PluralName = "News",
                    SingularName = "News",
                    Icon = "fa-solid fa-newspaper",
                    ClrType = typeof(News),
                    Includes = new[] { "Author" },
                    Fields = new[]
                    {
                        Text("Date", searchable: true),
                        Area("Description", required: true),
                        Image("Image", required: true),
                        Rel("AuthorId", "Author", "authors", "FullName", "Author", required: true, filterable: true)
                    }
                },
                new()
                {
                    Slug = "authors",
                    PluralName = "Authors",
                    SingularName = "Author",
                    Icon = "fa-solid fa-pen-nib",
                    ClrType = typeof(Author),
                    Fields = new[]
                    {
                        Text("FullName", "Full name", required: true, searchable: true)
                    }
                },
                new()
                {
                    Slug = "events",
                    PluralName = "Events",
                    SingularName = "Event",
                    Icon = "fa-solid fa-calendar",
                    ClrType = typeof(Event),
                    Fields = new[]
                    {
                        Number("Date", "Day"),
                        Text("Month", required: true, searchable: true),
                        Text("Title", required: true, searchable: true),
                        Area("Description", required: true, list: false)
                    }
                },
                new()
                {
                    Slug = "sliders",
                    PluralName = "Sliders",
                    SingularName = "Slider",
                    Icon = "fa-solid fa-images",
                    ClrType = typeof(Slider),
                    Fields = new[]
                    {
                        Text("Name", required: true, searchable: true),
                        Area("Description", list: false),
                        Image("Logo"),
                        Image("Image", required: true)
                    }
                },
                new()
                {
                    Slug = "icons",
                    PluralName = "Icons",
                    SingularName = "Icon",
                    Icon = "fa-solid fa-icons",
                    ClrType = typeof(Icon),
                    Fields = new[]
                    {
                        Image("Name", "Icon image", required: true)
                    }
                },
                new()
                {
                    Slug = "videos",
                    PluralName = "Videos",
                    SingularName = "Video",
                    Icon = "fa-solid fa-video",
                    ClrType = typeof(Video),
                    Fields = new[]
                    {
                        Text("VideoName", "YouTube URL", required: true, searchable: true)
                    }
                },
                new()
                {
                    Slug = "settings",
                    PluralName = "Settings",
                    SingularName = "Setting",
                    Icon = "fa-solid fa-gear",
                    ClrType = typeof(Setting),
                    Fields = new[]
                    {
                        Text("Key", required: true, searchable: true),
                        Area("Value", required: true)
                    }
                },
                new()
                {
                    Slug = "platform-abouts",
                    PluralName = "Platform About",
                    SingularName = "Platform About",
                    Icon = "fa-solid fa-building",
                    ClrType = typeof(PlatformAbout),
                    Fields = new[]
                    {
                        Text("Title", required: true, searchable: true),
                        Area("Description", required: true, list: false),
                        Image("Image", required: true)
                    }
                },
                new()
                {
                    Slug = "vision-abouts",
                    PluralName = "Vision About",
                    SingularName = "Vision About",
                    Icon = "fa-solid fa-eye",
                    ClrType = typeof(VisionAbout),
                    Fields = new[]
                    {
                        Text("Title", required: true, searchable: true),
                        Area("Description", required: true, list: false),
                        Image("Image", required: true)
                    }
                },
                new()
                {
                    Slug = "users",
                    PluralName = "Users",
                    SingularName = "User",
                    Icon = "fa-solid fa-users",
                    ClrType = typeof(AppUser),
                    Fields = new[]
                    {
                        Text("Username", required: true, searchable: true),
                        Password("Password", required: true),
                        Text("FullName", "Full name", required: true, searchable: true),
                        Text("Email", required: true, searchable: true),
                        Text("Role", required: true, filterable: true),
                        Flag("IsEmailVerified", "Email verified", filterable: true),
                        Text("VerificationCode", "Verification code", list: false, form: true, detail: false, searchable: false, required: false)
                    }
                }
            };
        }

        private static AdminFieldDescriptor Text(string name, string? label = null, bool required = false, bool searchable = false, bool list = true, bool form = true, bool detail = true, bool filterable = false)
            => new()
            {
                Name = name,
                Label = label ?? name,
                Kind = AdminFieldKind.Text,
                Required = required,
                Searchable = searchable,
                Filterable = filterable,
                ShowInList = list,
                ShowInForm = form,
                ShowInDetail = detail
            };

        private static AdminFieldDescriptor Area(string name, string? label = null, bool required = false, bool list = true)
            => new()
            {
                Name = name,
                Label = label ?? name,
                Kind = AdminFieldKind.TextArea,
                Required = required,
                Searchable = true,
                ShowInList = list
            };

        private static AdminFieldDescriptor Number(string name, string? label = null)
            => new()
            {
                Name = name,
                Label = label ?? name,
                Kind = AdminFieldKind.Number
            };

        private static AdminFieldDescriptor Flag(string name, string? label = null, bool filterable = false)
            => new()
            {
                Name = name,
                Label = label ?? name,
                Kind = AdminFieldKind.Boolean,
                Filterable = filterable
            };

        private static AdminFieldDescriptor Image(string name, string? label = null, bool required = false, bool isVirtual = false)
            => new()
            {
                Name = name,
                Label = label ?? name,
                Kind = AdminFieldKind.Image,
                Required = required,
                IsVirtual = isVirtual,
                Sortable = false
            };

        private static AdminFieldDescriptor Password(string name, bool required = false)
            => new()
            {
                Name = name,
                Label = name,
                Kind = AdminFieldKind.Password,
                Required = required,
                ShowInList = false,
                ShowInDetail = false
            };

        private static AdminFieldDescriptor Rel(string name, string label, string entity, string display, string navigation, bool required = false, bool filterable = false)
            => new()
            {
                Name = name,
                Label = label,
                Kind = AdminFieldKind.Relation,
                Required = required,
                Filterable = filterable,
                RelationEntity = entity,
                RelationDisplay = display,
                NavigationName = navigation
            };
    }
}
