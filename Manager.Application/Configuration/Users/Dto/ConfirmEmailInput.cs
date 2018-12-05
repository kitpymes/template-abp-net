using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Manager.Application.Configuration.Users.Dto
{
    public class ConfirmEmailInput : EntityDto
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        [Required]
        public string ConfirmationCode { get; set; }
    }
}
