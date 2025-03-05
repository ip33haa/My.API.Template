using Template.Application.DTOs;
using Template.Application.Responses;

namespace Template.Application.Features.F_SIPOCs.Queries.GetSIPOCById
{
    public class GetSIPOCByIdQueryResponse : BaseResponse
    {
        public SIPOCDto SIPOC { get; set; }
    }
}
