using AutoMapper;
using CompositeKey.AutoMapper.Profiles;

namespace CompositeKey.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Configure()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AreasProfile>();
            });
        }
    }
}
