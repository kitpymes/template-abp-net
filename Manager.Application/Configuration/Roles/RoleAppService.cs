using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Manager.Core.Configuration.Roles;
using Manager.Application.Configuration.Roles.Dto;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;

namespace Manager.Application.Configuration.Roles
{
    public class RoleAppService : ManagerAppServiceBase, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RoleAppService(RoleManager roleManager, IPermissionManager permissionManager)
        {
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        /// <summary>
        /// Obtiene los roles.
        /// Si el usuario tiene como rol MASTER: muestra todos los roles de todas las empresas, 
        ///     si tenantId tiene un valor, muestra los roles de esa empresa.
        /// Si es usuario tiene como rol ADMIN: muestra los roles de esa empresa.
        /// </summary>
        /// <param name="tenantId">Id de la empresa.</param>
        /// <returns>Lista de roles.</returns>
        public async Task<Tuple<ResultType, ListResultDto<RoleDto>>> GetRolesAsync(bool IsMaster, int? tenantId = null)
        {
            //try
            //{
                ListResultDto<RoleDto> result = null;

                if (IsMaster)
                {
                    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                    {
                        result = new ListResultDto<RoleDto>(
                               _roleManager.Roles
                               .WhereIf(tenantId.HasValue, o => o.TenantId.Equals(tenantId.Value))
                               .OrderBy(o => o.Name)
                               .ToList()
                               .MapTo<List<RoleDto>>());
                    }
                }
                else
                {
                    result = new ListResultDto<RoleDto>(
                            _roleManager.Roles
                            .OrderBy(o => o.Name)
                            .ToList()
                            .MapTo<List<RoleDto>>());
                }

                return Tuple.Create(ResultType.Success, result);
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(typeof(RoleAppService).ToString(), ex);
            //    return Tuple.Create(ResultType.Error, new ListResultDto<RoleDto>());
            //}
        }

        public async Task<ResultType> CreateRoleAsync(CreateRoleDtoInput input)
        {
            //try
            //{
                if (input.IsDefault)
                    await ResetIsDefaulRoletAsync();

                var obj = input.MapTo<Role>();

                CheckErrors(await _roleManager.CreateAsync(obj));

                return ResultType.Success;
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error(typeof(RoleAppService).ToString(), ex);
            //    return ResultType.Error;
            //}
        }

        public async Task ResetIsDefaulRoletAsync()
        {
            var role = _roleManager.Roles.Where(o => o.IsDefault).FirstOrDefault();
            if (role != null)
            {
                role.IsDefault = false;
                await _roleManager.UpdateAsync(role);
            }
        }

        public async Task<ResultType> UpdateRoleAsync(UpdateRoleDtoInput input)
        {
            try
            {
                if (input.IsDefault)
                    await ResetIsDefaulRoletAsync();

                var obj = await _roleManager.FindByIdAsync(input.Id);

                obj.Name = input.Name;
                obj.DisplayName = input.DisplayName;
                obj.IsDefault = input.IsDefault;
                obj.IsDeleted = input.IsDeleted;
                obj.IsStatic = input.IsStatic;

                CheckErrors(await _roleManager.UpdateAsync(obj));

                return ResultType.Success;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
         }

        public async Task<ResultType> DeleteRoleAsync(int id)
        {
            try
            {
                var obj = await _roleManager.FindByIdAsync(id);

                CheckErrors(await _roleManager.DeleteAsync(obj));

                return ResultType.Success;
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(RoleAppService).ToString(), ex);
                return ResultType.Error;
            }

        }

        public async Task UpdateRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }


        public async Task GetRolePermissions(UpdateRolePermissionsInput input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.RoleId);
            var grantedPermissions = _permissionManager
                .GetAllPermissions()
                .Where(p => input.GrantedPermissionNames.Contains(p.Name))
                .ToList();
        }
    }
}