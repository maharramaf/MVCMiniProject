using MVCMiniProject.Admin;
using MVCMiniProject.Helpers;
using MVCMiniProject.Models;
using MVCMiniProject.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using MVCMiniProject.Data;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IAdminCrudService
    {
        Task<AdminDashboardVM> GetDashboardAsync();
        Task<AdminListVM> GetListAsync(string entity, string? search, string? sort, string? dir, int page, IQueryCollection filters);
        Task<AdminDetailVM?> GetDetailAsync(string entity, int id);
        Task<AdminFormVM?> GetFormAsync(string entity, int? id);
        Task<(bool Ok, string Message, int? Id)> CreateAsync(string entity, IFormCollection form, IFormFileCollection files);
        Task<(bool Ok, string Message)> UpdateAsync(string entity, int id, IFormCollection form, IFormFileCollection files);
        Task<(bool Ok, string Message)> DeleteAsync(string entity, int id);
        Task<List<AdminSelectOption>> GetLookupAsync(string entitySlug);
    }
}

namespace MVCMiniProject.Services
{
    public class AdminCrudService : MVCMiniProject.Services.Interfaces.IAdminCrudService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminCrudService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<AdminDashboardVM> GetDashboardAsync()
        {
            var counts = new List<AdminEntityCountVM>();
            var recent = new List<AdminRecentItemVM>();

            foreach (var entity in AdminCatalog.Entities)
            {
                var query = AdminQueryHelper.GetSet(_context, entity.ClrType);
                var count = await AdminQueryHelper.CountAsync(query);
                counts.Add(new AdminEntityCountVM
                {
                    Slug = entity.Slug,
                    Name = entity.PluralName,
                    Icon = entity.Icon,
                    Count = count
                });

                var latestQuery = AdminQueryHelper.OrderByProperty(query, entity.ClrType, "Id", true);
                latestQuery = AdminQueryHelper.SkipTake(latestQuery, entity.ClrType, 0, 3);
                var items = await AdminQueryHelper.ToListAsync(latestQuery);
                foreach (var item in items)
                {
                    var id = (int)entity.ClrType.GetProperty("Id")!.GetValue(item)!;
                    recent.Add(new AdminRecentItemVM
                    {
                        EntitySlug = entity.Slug,
                        EntityName = entity.SingularName,
                        Id = id,
                        Title = GetDisplayTitle(entity, item)
                    });
                }
            }

            return new AdminDashboardVM
            {
                TotalRecords = counts.Sum(c => c.Count),
                EntityCounts = counts,
                RecentItems = recent.OrderByDescending(r => r.Id).Take(12).ToList(),
                FeaturedCourses = await _context.CourseInfos.CountAsync(c => c.IsFeature),
                NewCourses = await _context.CourseInfos.CountAsync(c => c.IsNew)
            };
        }

        public async Task<AdminListVM> GetListAsync(string entitySlug, string? search, string? sort, string? dir, int page, IQueryCollection filters)
        {
            var entity = AdminCatalog.Find(entitySlug) ?? throw new InvalidOperationException("Unknown entity.");
            if (page < 1) page = 1;
            var sortField = entity.Fields.Any(f => f.Sortable && f.Name.Equals(sort, StringComparison.OrdinalIgnoreCase))
                ? sort!
                : entity.DefaultSort;
            var descending = !string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);

            var query = AdminQueryHelper.GetSet(_context, entity.ClrType);
            query = AdminQueryHelper.Include(query, entity.ClrType, entity.Includes);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchable = entity.Fields.Where(f => f.Searchable && !f.IsVirtual).Select(f => f.Name);
                query = AdminQueryHelper.WhereSearch(query, entity.ClrType, search.Trim(), searchable);
            }

            var appliedFilters = new Dictionary<string, string>();
            foreach (var field in entity.Fields.Where(f => f.Filterable))
            {
                if (!filters.TryGetValue(field.Name, out var raw) || string.IsNullOrWhiteSpace(raw))
                {
                    continue;
                }

                appliedFilters[field.Name] = raw.ToString();
                if (field.Kind == AdminFieldKind.Boolean && bool.TryParse(raw, out var flag))
                {
                    query = AdminQueryHelper.WhereEquals(query, entity.ClrType, field.Name, flag);
                }
                else if (int.TryParse(raw, out var number))
                {
                    query = AdminQueryHelper.WhereEquals(query, entity.ClrType, field.Name, number);
                }
                else
                {
                    query = AdminQueryHelper.WhereEquals(query, entity.ClrType, field.Name, raw.ToString());
                }
            }

            var total = await AdminQueryHelper.CountAsync(query);
            query = AdminQueryHelper.OrderByProperty(query, entity.ClrType, sortField, descending);
            query = AdminQueryHelper.SkipTake(query, entity.ClrType, (page - 1) * 10, 10);
            var items = await AdminQueryHelper.ToListAsync(query);

            var rows = items.Select(item => new AdminListRow
            {
                Id = (int)entity.ClrType.GetProperty("Id")!.GetValue(item)!,
                Title = GetDisplayTitle(entity, item),
                Values = entity.Fields.Where(f => f.ShowInList).ToDictionary(f => f.Name, f => FormatField(entity, item, f))
            }).ToList();

            var filterOptions = new Dictionary<string, List<AdminSelectOption>>();
            foreach (var field in entity.Fields.Where(f => f.Filterable && f.Kind == AdminFieldKind.Relation && f.RelationEntity != null))
            {
                filterOptions[field.Name] = await GetLookupAsync(field.RelationEntity!);
            }

            return new AdminListVM
            {
                Entity = entity,
                Rows = rows,
                Search = search,
                Sort = sortField,
                Dir = descending ? "desc" : "asc",
                Page = page,
                TotalCount = total,
                Filters = appliedFilters,
                FilterOptions = filterOptions
            };
        }

        public async Task<AdminDetailVM?> GetDetailAsync(string entitySlug, int id)
        {
            var entity = AdminCatalog.Find(entitySlug);
            if (entity == null)
            {
                return null;
            }

            var item = await FindTrackedAsync(entity, id, true);
            if (item == null)
            {
                return null;
            }

            return new AdminDetailVM
            {
                Entity = entity,
                Id = id,
                Title = GetDisplayTitle(entity, item),
                Fields = entity.Fields.Where(f => f.ShowInDetail).Select(f =>
                {
                    var value = FormatField(entity, item, f);
                    return (f.Label, value, f.Kind == AdminFieldKind.Image && !string.IsNullOrWhiteSpace(value) && value != "-");
                }).ToList()
            };
        }

        public async Task<AdminFormVM?> GetFormAsync(string entitySlug, int? id)
        {
            var entity = AdminCatalog.Find(entitySlug);
            if (entity == null)
            {
                return null;
            }

            var values = new Dictionary<string, string?>();
            object? item = null;
            if (id.HasValue)
            {
                item = await FindTrackedAsync(entity, id.Value, true);
                if (item == null)
                {
                    return null;
                }
            }

            foreach (var field in entity.Fields.Where(f => f.ShowInForm))
            {
                if (item == null)
                {
                    values[field.Name] = field.Kind == AdminFieldKind.Boolean ? "false" : string.Empty;
                    continue;
                }

                if (field.Kind == AdminFieldKind.Password)
                {
                    values[field.Name] = string.Empty;
                    continue;
                }

                values[field.Name] = ReadRaw(entity, item, field);
            }

            var lookups = new Dictionary<string, List<AdminSelectOption>>();
            foreach (var field in entity.Fields.Where(f => f.Kind == AdminFieldKind.Relation && f.RelationEntity != null))
            {
                lookups[field.Name] = await GetLookupAsync(field.RelationEntity!);
            }

            return new AdminFormVM
            {
                Entity = entity,
                Id = id,
                IsEdit = id.HasValue,
                Values = values,
                Lookups = lookups
            };
        }

        public async Task<(bool Ok, string Message, int? Id)> CreateAsync(string entitySlug, IFormCollection form, IFormFileCollection files)
        {
            var entity = AdminCatalog.Find(entitySlug);
            if (entity == null)
            {
                return (false, "Unknown entity.", null);
            }

            var errors = Validate(entity, form, files, false);
            if (errors.Count > 0)
            {
                return (false, string.Join(" ", errors.Values), null);
            }

            try
            {
                var instance = Activator.CreateInstance(entity.ClrType)!;
                await ApplyFormAsync(entity, instance, form, files, false);
                _context.Add(instance);
                await _context.SaveChangesAsync();
                var id = (int)entity.ClrType.GetProperty("Id")!.GetValue(instance)!;
                await ApplyVirtualImagesAsync(entity, instance, files);
                await _context.SaveChangesAsync();
                return (true, $"{entity.SingularName} created successfully.", id);
            }
            catch (DbUpdateException)
            {
                return (false, "Could not create this record because of a related data constraint.", null);
            }
            catch
            {
                return (false, $"{entity.SingularName} could not be created.", null);
            }
        }

        public async Task<(bool Ok, string Message)> UpdateAsync(string entitySlug, int id, IFormCollection form, IFormFileCollection files)
        {
            var entity = AdminCatalog.Find(entitySlug);
            if (entity == null)
            {
                return (false, "Unknown entity.");
            }

            var errors = Validate(entity, form, files, true);
            if (errors.Count > 0)
            {
                return (false, string.Join(" ", errors.Values));
            }

            var instance = await FindTrackedAsync(entity, id, true);
            if (instance == null)
            {
                return (false, $"{entity.SingularName} not found.");
            }

            try
            {
                await ApplyFormAsync(entity, instance, form, files, true);
                await ApplyVirtualImagesAsync(entity, instance, files);
                await _context.SaveChangesAsync();
                return (true, $"{entity.SingularName} updated successfully.");
            }
            catch (DbUpdateException)
            {
                return (false, "Could not update this record because of a related data constraint.");
            }
            catch
            {
                return (false, $"{entity.SingularName} could not be updated.");
            }
        }

        public async Task<(bool Ok, string Message)> DeleteAsync(string entitySlug, int id)
        {
            var entity = AdminCatalog.Find(entitySlug);
            if (entity == null)
            {
                return (false, "Unknown entity.");
            }

            var instance = await FindTrackedAsync(entity, id, true);
            if (instance == null)
            {
                return (false, $"{entity.SingularName} not found.");
            }

            try
            {
                DeleteImageFiles(entity, instance);
                if (instance is CourseInfo course && course.CourseImages != null)
                {
                    _context.CourseImages.RemoveRange(course.CourseImages);
                }

                _context.Remove(instance);
                await _context.SaveChangesAsync();
                return (true, $"{entity.SingularName} deleted successfully.");
            }
            catch (DbUpdateException)
            {
                return (false, "This record cannot be deleted while related data still exists.");
            }
            catch
            {
                return (false, $"{entity.SingularName} could not be deleted.");
            }
        }

        public async Task<List<AdminSelectOption>> GetLookupAsync(string entitySlug)
        {
            var entity = AdminCatalog.Find(entitySlug);
            if (entity == null)
            {
                return new List<AdminSelectOption>();
            }

            var query = AdminQueryHelper.OrderByProperty(AdminQueryHelper.GetSet(_context, entity.ClrType), entity.ClrType, "Id", false);
            var items = await AdminQueryHelper.ToListAsync(query);
            return items.Select(item => new AdminSelectOption
            {
                Value = entity.ClrType.GetProperty("Id")!.GetValue(item)!.ToString() ?? string.Empty,
                Text = GetDisplayTitle(entity, item)
            }).ToList();
        }

        private async Task<object?> FindTrackedAsync(AdminEntityDescriptor entity, int id, bool include)
        {
            var query = AdminQueryHelper.GetSet(_context, entity.ClrType);
            if (include)
            {
                query = AdminQueryHelper.Include(query, entity.ClrType, entity.Includes);
            }

            query = AdminQueryHelper.WhereEquals(query, entity.ClrType, "Id", id);
            var items = await AdminQueryHelper.ToListAsync(query);
            return items.FirstOrDefault();
        }

        private Dictionary<string, string> Validate(AdminEntityDescriptor entity, IFormCollection form, IFormFileCollection files, bool isEdit)
        {
            var errors = new Dictionary<string, string>();
            foreach (var field in entity.Fields.Where(f => f.ShowInForm))
            {
                if (field.Kind == AdminFieldKind.Image)
                {
                    var file = files[field.Name];
                    var required = field.Required && !isEdit;
                    if (!ImageFileHelper.TryValidate(file, required, out var imageError))
                    {
                        errors[field.Name] = imageError ?? "Invalid image.";
                    }

                    if (required && (file == null || file.Length == 0) && field.IsVirtual == false)
                    {
                        var existing = form[$"Existing_{field.Name}"].ToString();
                        if (string.IsNullOrWhiteSpace(existing))
                        {
                            errors[field.Name] = $"{field.Label} is required.";
                        }
                    }

                    continue;
                }

                if (field.Kind == AdminFieldKind.Password)
                {
                    var password = form[field.Name].ToString();
                    if (field.Required && !isEdit && string.IsNullOrWhiteSpace(password))
                    {
                        errors[field.Name] = $"{field.Label} is required.";
                    }
                    continue;
                }

                if (!field.Required || field.Kind == AdminFieldKind.Boolean)
                {
                    continue;
                }

                var value = form[field.Name].ToString();
                if (string.IsNullOrWhiteSpace(value) || (field.Kind == AdminFieldKind.Relation && value == "0"))
                {
                    errors[field.Name] = $"{field.Label} is required.";
                }
            }

            return errors;
        }

        private async Task ApplyFormAsync(AdminEntityDescriptor entity, object instance, IFormCollection form, IFormFileCollection files, bool isEdit)
        {
            foreach (var field in entity.Fields.Where(f => f.ShowInForm && !f.IsVirtual))
            {
                var property = entity.ClrType.GetProperty(field.Name);
                if (property == null || !property.CanWrite)
                {
                    continue;
                }

                if (field.Kind == AdminFieldKind.Image)
                {
                    var file = files[field.Name];
                    if (file != null && file.Length > 0)
                    {
                        var oldName = property.GetValue(instance) as string;
                        var saved = await ImageFileHelper.SaveAsync(file, _env.WebRootPath);
                        ImageFileHelper.DeleteIfExists(_env.WebRootPath, oldName);
                        property.SetValue(instance, saved);
                    }
                    continue;
                }

                if (field.Kind == AdminFieldKind.Password)
                {
                    var password = form[field.Name].ToString();
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        property.SetValue(instance, PasswordHelper.Hash(password));
                    }
                    continue;
                }

                if (field.Kind == AdminFieldKind.Boolean)
                {
                    var raw = form[field.Name].ToString();
                    property.SetValue(instance, raw.Contains("true", StringComparison.OrdinalIgnoreCase));
                    continue;
                }

                var value = form[field.Name].ToString();
                SetConverted(property, instance, value);
            }
        }

        private async Task ApplyVirtualImagesAsync(AdminEntityDescriptor entity, object instance, IFormFileCollection files)
        {
            if (entity.ClrType != typeof(CourseInfo))
            {
                return;
            }

            var file = files["CoverImage"];
            if (file == null || file.Length == 0)
            {
                return;
            }

            var course = (CourseInfo)instance;
            var saved = await ImageFileHelper.SaveAsync(file, _env.WebRootPath);
            course.CourseImages ??= new List<CourseImage>();
            var main = course.CourseImages.FirstOrDefault(i => i.IsMain);
            if (main != null)
            {
                ImageFileHelper.DeleteIfExists(_env.WebRootPath, main.Name);
                main.Name = saved;
            }
            else
            {
                var image = new CourseImage
                {
                    Name = saved,
                    IsMain = true,
                    CourseInfoId = course.Id
                };
                course.CourseImages.Add(image);
                if (image.Id == 0 && course.Id > 0)
                {
                    _context.CourseImages.Add(image);
                }
            }
        }

        private void DeleteImageFiles(AdminEntityDescriptor entity, object instance)
        {
            foreach (var field in entity.Fields.Where(f => f.Kind == AdminFieldKind.Image && !f.IsVirtual))
            {
                var property = entity.ClrType.GetProperty(field.Name);
                ImageFileHelper.DeleteIfExists(_env.WebRootPath, property?.GetValue(instance) as string);
            }

            if (instance is CourseInfo course)
            {
                foreach (var image in course.CourseImages ?? Enumerable.Empty<CourseImage>())
                {
                    ImageFileHelper.DeleteIfExists(_env.WebRootPath, image.Name);
                }
            }
        }

        private static void SetConverted(PropertyInfo property, object instance, string? value)
        {
            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (type == typeof(string))
            {
                property.SetValue(instance, value);
                return;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                if (property.PropertyType.IsValueType)
                {
                    property.SetValue(instance, Activator.CreateInstance(property.PropertyType));
                }
                else
                {
                    property.SetValue(instance, null);
                }
                return;
            }

            object converted = type == typeof(int)
                ? int.Parse(value, CultureInfo.InvariantCulture)
                : Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            property.SetValue(instance, converted);
        }

        private static string GetDisplayTitle(AdminEntityDescriptor entity, object item)
        {
            foreach (var name in new[] { "Title", "Name", "FullName", "Username", "Key", "VideoName" })
            {
                var property = entity.ClrType.GetProperty(name);
                var value = property?.GetValue(item)?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            if (item is CourseImage image)
            {
                return string.IsNullOrWhiteSpace(image.Name) ? $"Image #{image.Id}" : image.Name;
            }

            return $"{entity.SingularName} #{entity.ClrType.GetProperty("Id")?.GetValue(item)}";
        }

        private static string? ReadRaw(AdminEntityDescriptor entity, object item, AdminFieldDescriptor field)
        {
            if (field.IsVirtual && item is CourseInfo course)
            {
                return course.CourseImages?.FirstOrDefault(i => i.IsMain)?.Name;
            }

            return entity.ClrType.GetProperty(field.Name)?.GetValue(item)?.ToString();
        }

        private static string FormatField(AdminEntityDescriptor entity, object item, AdminFieldDescriptor field)
        {
            if (field.IsVirtual && item is CourseInfo course)
            {
                return course.CourseImages?.FirstOrDefault(i => i.IsMain)?.Name ?? "-";
            }

            if (field.Kind == AdminFieldKind.Relation && !string.IsNullOrWhiteSpace(field.NavigationName))
            {
                var navigation = entity.ClrType.GetProperty(field.NavigationName)?.GetValue(item);
                if (navigation != null && !string.IsNullOrWhiteSpace(field.RelationDisplay))
                {
                    var text = navigation.GetType().GetProperty(field.RelationDisplay)?.GetValue(navigation)?.ToString();
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        return text;
                    }
                }
            }

            var raw = entity.ClrType.GetProperty(field.Name)?.GetValue(item);
            if (raw is bool flag)
            {
                return flag ? "Yes" : "No";
            }

            if (raw is IEnumerable enumerable and not string)
            {
                return string.Join(", ", enumerable.Cast<object>().Select(o => o.ToString()));
            }

            return raw?.ToString() ?? "-";
        }
    }
}
