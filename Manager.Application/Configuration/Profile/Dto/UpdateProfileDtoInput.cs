using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Manager.Core.Configuration.Users;

namespace Manager.Application.Configuration.Profile.Dto
{
    public class UpdateProfileDtoInput : EntityDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}