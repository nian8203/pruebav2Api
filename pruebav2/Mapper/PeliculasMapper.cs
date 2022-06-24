using AutoMapper;
using pruebav2.Models;
using pruebav2.Models.Dto;

namespace pruebav2.Mapper;  

public class PeliculasMapper : Profile
{
    public PeliculasMapper()
    {
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        CreateMap<Pelicula, PeliculaDto>().ReverseMap();
        CreateMap<Pelicula, PeliculaCreateDto>().ReverseMap();
        CreateMap<Pelicula, PeliculaUpdateDto>().ReverseMap();
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
    }
}
