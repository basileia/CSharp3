namespace ToDoList.Domain.Mapping;

using AutoMapper;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ToDoItemCreateRequestDto, ToDoItem>().ReverseMap();
        CreateMap<ToDoItemUpdateRequestDto, ToDoItem>().ReverseMap();
        CreateMap<ToDoItemGetResponseDto, ToDoItem>().ReverseMap();
    }
}
