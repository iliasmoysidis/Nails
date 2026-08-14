using Application.Professionals.Common.Queries;
using Application.Common.Exceptions;

namespace Application.Professionals.GetDetails;

public sealed class GetProfessionalDetailsHandler
{
    private readonly IProfessionalQueries _queries;

    public GetProfessionalDetailsHandler(IProfessionalQueries queries)
    {
        _queries = queries;
    }

    public async Task<ProfessionalDTO> Handle(GetProfessionalDetailsQuery query, CancellationToken ct)
    {
        return await _queries.GetProfessionalDetailsAsync(
            professionalId: query.ProfessionalId,
            ct: ct
        )
            ?? throw new ApplicationLayerNotFoundException("Professional not found.");
    }
}