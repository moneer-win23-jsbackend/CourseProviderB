using CourseProviderB.Infrastructure.Data.Contexts;
using CourseProviderB.Infrastructure.Factories;
using CourseProviderB.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProviderB.Infrastructure.Services;

public interface ICourseService
{
    Task<Course> CreateCourseAsync(CourseCreateRequest request);
    Task<Course> GetCourseByIdAsync(string id);
    Task<IEnumerable<Course>> GetCourseAsync();
    Task<Course> UpdateCourseAsync(CourseUpdateRequest request);
    Task<bool> DeleteCourseAsync(string id);
}

public class CourseService(IDbContextFactory<DataContext> contextFactory) : ICourseService
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<Course> CreateCourseAsync(CourseCreateRequest request)
    {
        await using var context = _contextFactory.CreateDbContext();

        var courseEntity = CourseFactory.Create(request);
        context.Course.Add(courseEntity);
        await context.SaveChangesAsync();

        return CourseFactory.Create(courseEntity);

    }

    public async Task<bool> DeleteCourseAsync(string id)
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntity = await context.Course.FirstOrDefaultAsync(c => c.Id == id);
        if (courseEntity != null) return false;

        context.Course.Remove(courseEntity);
        await context.SaveChangesAsync();
        return true;

    }

    public async Task<Course> GetCourseByIdAsync(string id)
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntity = await context.Course.FirstOrDefaultAsync(c => c.Id == id);

        return courseEntity == null ? null! : CourseFactory.Create(courseEntity);
    }


    public async Task<IEnumerable<Course>> GetCourseAsync()
    {
        await using var context = _contextFactory.CreateDbContext();
        var courseEntities = await context.Course.ToListAsync();

        return courseEntities.Select(CourseFactory.Create);
    }


    public async Task<Course> UpdateCourseAsync(CourseUpdateRequest request)
    {
        await using var context = _contextFactory.CreateDbContext();
        var existingCourse = await context.Course.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (existingCourse == null) return null!;

        var updatedCourseEntity = CourseFactory.Create(request);
        updatedCourseEntity.Id = existingCourse.Id;
        context.Entry(existingCourse).CurrentValues.SetValues(updatedCourseEntity);

        await context.SaveChangesAsync();
        return CourseFactory.Create(existingCourse);
    }
}
