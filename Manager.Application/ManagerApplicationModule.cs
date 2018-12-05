using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Manager.Core;

namespace Manager.Application
{
    [DependsOn(typeof(ManagerCoreModule), typeof(AbpAutoMapperModule))]
    public class ManagerApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //Add your custom AutoMapper mappings here...
                //mapper.CreateMap<,>()
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
