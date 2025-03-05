using MediatR;
using Template.Application.DTOs;

namespace Template.Application.Features.F_SIPOCs.Commands.Create
{
    public class CreateSIPOCCommand : IRequest<CreateSIPOCCommandResponse>
    {
        public SIPOCDto SIPOC { get; set; }
    }
}
