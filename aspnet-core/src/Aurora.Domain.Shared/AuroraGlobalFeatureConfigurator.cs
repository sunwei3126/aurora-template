using Volo.Abp.Threading;

namespace Aurora
{
    public static class AuroraGlobalFeatureConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
            });
        }
    }
}