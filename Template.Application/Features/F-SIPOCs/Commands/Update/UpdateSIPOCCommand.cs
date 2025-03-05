using MediatR;
using Template.Application.DTOs;

namespace Template.Application.Features.F_SIPOCs.Commands.Update
{
    public class UpdateSIPOCCommand : BaseCommandQuery, IRequest<UpdateSIPOCCommandResponse>
    {
        public SIPOCDto SIPOC { get; set; }
    }
}
