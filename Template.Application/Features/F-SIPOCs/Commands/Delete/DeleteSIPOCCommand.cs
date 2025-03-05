using MediatR;

namespace Template.Application.Features.F_SIPOCs.Commands.Delete
{
    public class DeleteSIPOCCommand : BaseCommandQuery, IRequest<DeleteSIPOCCommandResponse>
    {
    }
}
