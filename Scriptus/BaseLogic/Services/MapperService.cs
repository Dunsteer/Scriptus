using AutoMapper;
using Commons.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseLogic.Services
{
    public class MapperService
    {
        private IMapper _mapper;
        private MapperConfiguration _configuration;

        public MapperService()
        {
            Configure();
        }

        public void Configure()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                AddDefaultConfiguration(cfg);
            });
        }

        public void AddDefaultConfiguration(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<DbServices.Models.User, UserLoginResponseModel>();
            cfg.CreateMap<DbServices.Models.User, UserViewModel>();
            cfg.CreateMap<DbServices.Models.User, UserMinModel>();
        }

        public void SetConfiguration(MapperConfiguration cfg)
        {
            _configuration = cfg;
            _mapper = _configuration.CreateMapper();
        }

        public IMapper Get()
        {
            return _mapper ?? (_mapper = _configuration.CreateMapper());
        }

        public List<TDestination> MapCollection<TDestination, TSource>(List<TSource> source)
        {
            List<TDestination> result = new List<TDestination>();
            var m = Get();

            foreach (var d in source)
            {
                result.Add(m.Map<TDestination>(d));
            }

            return result;
        }
    }
}
