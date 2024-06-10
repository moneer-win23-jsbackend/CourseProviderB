using CourseProviderB.Infrastructure.Models;
using CourseProviderB.Infrastructure.Services;

namespace CourseProviderB.Infrastructure.GraphQL.Queries;

public class Query(ICourseService courseService)
{
    private readonly ICourseService _courseService = courseService;

    [GraphQLName("getCourses")]

    public async Task<IEnumerable<Course>> GetCoursesAsync()
    {
        return await _courseService.GetCourseAsync();
    }

    [GraphQLName("getCourseById")]

    public async Task<Course> GetCourseByIdAsync(string id)
    {
        return await _courseService.GetCourseByIdAsync(id);
    }
}
