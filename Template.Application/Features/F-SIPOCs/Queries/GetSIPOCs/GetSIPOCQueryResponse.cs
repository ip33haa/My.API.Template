using Template.Application.DTOs;
using Template.Application.Responses;

namespace Template.Application.Features.F_SIPOCs.Queries.GetSIPOCs
{
    public class GetSIPOCQueryResponse : BaseResponse
    {
        public List<SIPOCDto> SIPOCs { get; set; }
    }
}
