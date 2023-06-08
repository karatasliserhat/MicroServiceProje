using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Order.Application.Mappings
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new CustomMapperProfile());
            });

            return config.CreateMapper();

        });

        public static IMapper mapper => lazy.Value;
    }
}
