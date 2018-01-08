﻿using System.Linq;
using AutoMapper;
using DatingAPI.Controllers.Response;
using DatingAPI.Extensions;
using DatingAPI.Models;

namespace DatingAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            DomainToResourceMapping();
        }

        private void DomainToResourceMapping()
        {
            CreateMap<User, UserListResponse>()
                .ForMember(destination => destination.PhotoUrl,
                    opts => { opts.MapFrom(source => source.Photos.FirstOrDefault(p => p.IsMain).Url); })
                .ForMember(destination => destination.Age,
                    opts => { opts.ResolveUsing(source => source.DateOfBirth.CalculateAge()); });

            CreateMap<User, UserDetailResponse>()
                .ForMember(destination => destination.PhotoUrl,
                    opts => { opts.MapFrom(source => source.Photos.FirstOrDefault(p => p.IsMain).Url); })
                .ForMember(destination => destination.Age,
                    opts => { opts.ResolveUsing(source => source.DateOfBirth.CalculateAge()); });

            CreateMap<Photo, PhotoResponse>();
            ;
        }
    }
}