using MyWebAPI.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models
{
    public class Communication : BazaEntityBase
    {
        [Required, StringLength(BazaConstants.Validations.GuidStringLength), VarcharConvention]
        public string OwnerId { get; set; }
        
        [Required, StringLength(BazaConstants.Validations.GuidStringLength), VarcharConvention]
        public string CandidateId { get; set; }
        
        [Required, StringLength(BazaConstants.Validations.GuidStringLength), VarcharConvention]
        public string JobId { get; set; }
        
        [DefaultValue(CommunicationStatus.Unprocessed)]
        public CommunicationStatus Status { get; set; }
    }
}
