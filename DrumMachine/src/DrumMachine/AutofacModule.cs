using Autofac;
using Microsoft.Extensions.Configuration;

namespace DrumMachine
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration configuration;
        public AutofacModule(IConfiguration config) => configuration = config;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PatternProjections>();
        }
    }
}
