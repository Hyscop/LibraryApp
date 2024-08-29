using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;
using AutoMapper;

namespace api.Mapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            // Book to BookDto and vice versa
            CreateMap<Book, BookDto>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ReverseMap();

            CreateMap<BookForCreationDto, Book>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<BookForUpdateDto, Book>();

            // Category to CategoryDto and vice versa
            CreateMap<Category, CategoryForCreationDto>()
                .ReverseMap();

            CreateMap<Category, CategoryForUpdateDto>()
            .ReverseMap();

            CreateMap<UserLoginDto, User>();

            CreateMap<UserForCreationDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());



            CreateMap<UserForUpdateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());



            // User to UserDto and vice versa
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserStats, opt => opt.MapFrom(src => src.UserStats))
                .ForMember(dest => dest.UserBookProgresses, opt => opt.MapFrom(src => src.UserBookProgresses));

            // UserStats to UserStatsDto and vice versa
            CreateMap<UserStats, UserStatsDto>()
                .ReverseMap();

            // UserBookProgress to UserBookProgressDto and vice versa
            CreateMap<UserBookProgress, UserBookProgressDto>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book));






        }
    }
}