using AutoMapper;
using JsonDataTool.DLEntities;
using JsonDataTool.Entities;

namespace JsonDataTool.Providers
{
    internal static class MapperProvider
    {
        public static IMapper Mapper { get; }


        static MapperProvider()
        {
            Mapper = CreateMapper();
        }

        private static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                //UI -> DL
                cfg.CreateMap<JsonItem, JsonItemDL>();

                //DL -> UI
                cfg.CreateMap<JsonItemDL, JsonItem>();
            });

            return configuration.CreateMapper();
        }
    }
}
