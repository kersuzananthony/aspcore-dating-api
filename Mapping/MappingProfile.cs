﻿using System.Linq;
using AutoMapper;
using DatingAPI.Controllers.Requests;
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
            ResourceToDomainMapping();
        }

        private void DomainToResourceMapping()
        {
            CreateMap<User, UserListResponse>()
                .ForMember(destination => destination.PhotoUrl,
                    opts => opts.MapFrom(source => source.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(destination => destination.Age,
                    opts => opts.ResolveUsing(source => source.DateOfBirth.CalculateAge()));

            CreateMap<User, UserDetailResponse>()
                .ForMember(destination => destination.PhotoUrl,
                    opts => opts.MapFrom(source => source.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(destination => destination.Age,
                    opts => opts.ResolveUsing(source => source.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoResponse>();

            CreateMap<Photo, PhotoDetailResponse>();

            CreateMap(typeof(QueryResult<>), typeof(QueryResultResponse<>));

            CreateMap<Message, MessageResponse>()
                .ForMember(destination => destination.RecipientKnownAs,
                    opts => opts.MapFrom(source => source.Recipient.KnownAs))
                .ForMember(destination => destination.RecipientPhotoUrl,
                    opts => opts.MapFrom(source => source.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(destination => destination.SenderKnownAs,
                    opts => opts.MapFrom(source => source.Sender.KnownAs))
                .ForMember(destination => destination.SenderPhotoUrl, 
                    opts => opts.MapFrom(source => source.Sender.Photos.FirstOrDefault(p => p.IsMain).Url));
        }

        private void ResourceToDomainMapping()
        {
            CreateMap<UserUpdateRequest, User>();

            CreateMap<PhotoCreationRequest, Photo>();

            CreateMap<RegisterRequest, User>();

            CreateMap<MessageRequest, Message>().ReverseMap();
        }
    }
}