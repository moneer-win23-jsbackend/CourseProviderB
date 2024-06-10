using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CourseProviderB.Functions
{
    public class GraphQL(ILogger<GraphQL> logger, IGraphQLRequestExecutor graphQLRequestExecutor)
    {
        private readonly ILogger<GraphQL> _logger = logger;
        private readonly IGraphQLRequestExecutor graphQLRequestExecutor = graphQLRequestExecutor;

        [Function("GraphQL")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route ="graphql")] HttpRequest req)
        {
            return await graphQLRequestExecutor.ExecuteAsync(req);


        }
    }
}
