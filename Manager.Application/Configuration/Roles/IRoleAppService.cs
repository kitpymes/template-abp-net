using System.Threading.Tasks;
using Abp.Application.Services;
using Manager.Application.Configuration.Roles.Dto;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Manager.Core.Configuration.Roles;
using Microsoft.AspNet.Identity;
using System;

namespace Manager.Application.Configuration.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task<Tuple<ResultType, ListResultDto<RoleDto>>> GetRolesAsync(bool IsMaster, int? tenantId = null);

        Task<ResultType> CreateRoleAsync(CreateRoleDtoInput input);

        Task<ResultType> UpdateRoleAsync(UpdateRoleDtoInput input);

        Task<ResultType> DeleteRoleAsync(int id);

        Task ResetIsDefaulRoletAsync();

        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
