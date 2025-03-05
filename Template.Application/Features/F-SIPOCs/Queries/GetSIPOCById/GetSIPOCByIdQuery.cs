using MediatR;

namespace Template.Application.Features.F_SIPOCs.Queries.GetSIPOCById
{
    public class GetSIPOCByIdQuery : BaseCommandQuery, IRequest<GetSIPOCByIdQueryResponse>
    {
    }
}
