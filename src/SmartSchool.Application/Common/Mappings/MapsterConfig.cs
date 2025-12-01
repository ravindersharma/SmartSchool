using Mapster;
using System.Reflection;

namespace SmartSchool.Application.Common.Mappings
{
    public static class MapsterConfig
    {
        public static TypeAdapterConfig CreateConfig()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            //global settings 
            config.Default.IgnoreNullValues(true)
                  .PreserveReference(true)
                  .NameMatchingStrategy(NameMatchingStrategy.Flexible)
                  .ShallowCopyForSameType(true);

            config.Scan(Assembly.GetExecutingAssembly());

            return config;
        }
    }
}
