using BackEnd.Data;
using ConferenceDTO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Endpoints
{
    public static class SearchEndpoints
    {
        public static void MapSearchResults(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/Search/{term}", async (string term, ApplicationDbContext db) =>
            {
                var sessionResults = await db.Sessions.Include(s => s.Track)
                                        .Include(s => s.SessionSpeakers)
                                        .ThenInclude(ss => ss.Speaker)
                                        .Where(s =>
                                            s.Title!.Contains(term) ||
                                            s.Track!.Name!.Contains(term)
                                        )
                                        .ToListAsync();

                var speakerResults = await db.Speakers.Include(s => s.SessionSpeakers)
                                        .ThenInclude(ss => ss.Speaker)
                                        .Where(s =>
                                            s.Name!.Contains(term) ||
                                            s.Bio!.Contains(term) ||
                                            s.Website!.Contains(term)
                                            )
                                            .ToListAsync();

                var results = sessionResults.Select(s => new SearchResult
                {
                    Type = SearchResultType.Session,
                    Session = s.MapSessionResponse()
                })
                .Concat(speakerResults.Select(s => new SearchResult
                {
                    Type = SearchResultType.Speaker,
                    Speaker = s.MapSpeakerResponse()
                }));

                return results
                    is IEnumerable<SearchResult> model
                        ? Results.Ok(model)
                        : Results.NotFound();
            })
            .WithTags("Seach")
            .WithName("GetSearchResults")
            .Produces<IEnumerable<SearchResult>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
