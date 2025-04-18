using JobMagnet.Controllers.Base;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using JobMagnet.Mappers;
using JobMagnet.Models.Queries.Profile;
using JobMagnet.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V1;

public class ProfileController(
    ILogger<ProfileController> logger,
    IProfileQueryRepository queryRepository) : BaseController<ProfileController>(logger)
{
    [HttpGet]
    [ProducesResponseType(typeof(ProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetProfileAsync([FromQuery] ProfileQueryParameters queryParameters)
    {
        var entity = await queryRepository
            .IncludeTalents()
            .IncludeResume()
            .IncludeTestimonials()
            .IncludeSkill()
            .GetFirstByExpressionWithIncludesAsync(x => x.FirstName == queryParameters.Name)
            .ConfigureAwait(false);

        if (entity is null)
            return Results.NotFound();

        var responseModel = ProfileMapper.ToModel(entity);

        return Results.Ok(responseModel);
    }
}