using Volo.Abp.Threading;

namespace Aurora
{
    public static class AuroraModuleExtensionConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ConfigureExistingProperties();
                ConfigureExtraProperties();
            });
        }

        private static void ConfigureExistingProperties()
        {
        }

        private static void ConfigureExtraProperties()
        {
        }
    }
}