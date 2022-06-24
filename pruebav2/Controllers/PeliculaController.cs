using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pruebav2.Models;
using pruebav2.Models.Dto;
using pruebav2.Repository.IRepository;

namespace pruebav2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Categoria peliculas")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PeliculaController : Controller
    {
        private readonly IPeliculaRepository _pelRepo;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;
        public PeliculaController(IPeliculaRepository pelRepo, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _pelRepo = pelRepo;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;

        }

        [HttpGet]
        public IActionResult GetPeliculas()
        {
            var listaPelicula = _pelRepo.GetPeliculas();
            var listaPeliculaDto = new List<PeliculaDto>();

            foreach (var item in listaPelicula)
                listaPeliculaDto.Add(_mapper.Map<PeliculaDto>(item));

            return Ok(listaPeliculaDto);
        }

        [HttpGet("{PeliculaId:int}",Name = "GetPelicula")]
        public IActionResult GetPelicula(int PeliculaId)
        {
            var itemPelicula = _pelRepo.GetPelicula(PeliculaId);

            if (itemPelicula == null)
                return NotFound();

            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);

            return Ok(itemPeliculaDto);
            
        }


        [HttpPost]
        public IActionResult CrearPelicula([FromForm]PeliculaCreateDto peliculaDto) 
        {
            if (peliculaDto == null)
                return BadRequest(ModelState);

            if (_pelRepo.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            //subida de fotos
            var archivo = peliculaDto.Foto;
            string rutaPrincipal = _hostEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length > 0)
            {
                //Nueva imagen
                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                peliculaDto.RutaImagen = @"\fotos\" + nombreFoto + extension;


            }

            

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal con el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new {peliculaId = pelicula.Id}, pelicula);
        }


        [HttpPatch]
        public IActionResult ActualizarPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (peliculaDto == null || peliculaId != peliculaDto.Id)  
                return BadRequest(ModelState);

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal al actualizar los datos de {pelicula.Nombre}");
                return StatusCode(500, ModelState);                
            }

            return NoContent();
        }



        [HttpDelete("{peliculaId:int}", Name = "EliminarPelicula")]
        public IActionResult EliminarPelicula(int peliculaId)
        {
            if (!_pelRepo.ExistePelicula(peliculaId))
                return NotFound();

            var pelicula = _pelRepo.GetPelicula(peliculaId);
            if (!_pelRepo.EliminarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Ups! Algo salio mal al eliminar el registro {pelicula.Nombre}");
            }

            return NoContent();
        }

        [HttpGet("GetPeliculasPorCategoria/{categoriaId:int}")]
        public IActionResult GetPeliculasPorCategoria(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasPorCategoria(categoriaId);

            if (listaPeliculas == null)
                return NotFound();

            var itemPelicula = new List<PeliculaDto>();
            foreach (var item in listaPeliculas)
                itemPelicula.Add(_mapper.Map<PeliculaDto>(item));

            return Ok(itemPelicula);
            
        }

        [HttpGet("Buscar")]
        public IActionResult BuscarPelicula(string nombre)
        {
            try
            {
                var res = _pelRepo.BuscarPelicula(nombre);
                if (res.Any())
                    return Ok(res);

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error en la recuperación de los datos");
            }
        }




    }
}
