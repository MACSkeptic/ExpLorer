using MACSkeptic.ExpLorer.Utils.Extensions;

namespace MACSkeptic.ExpLorer.Parameters
{
    public class ConfigurationParameters
    {
        internal ConfigurationParameters(Configuration configuration)
        {
            Configuration = configuration;
        }

        public Configuration Configuration { get; private set; }

        public ConfigurationParameters BelongingTo(Configuration configuration)
        {
            if (configuration != null)
            {
                configuration.Add(Configuration);
            }
            return this;
        }

        public ConfigurationParameters With(params Configuration[] configurations)
        {
            configurations.ExecuteForEach(configuration => Configuration.Add(configuration));
            return this;
        }
    }
}