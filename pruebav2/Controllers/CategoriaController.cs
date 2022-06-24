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

    public class CategoriaController : Controller
    {
        private readonly ICategotiaRepository _ctRepo;
        private readonly IMapper _mapper;
        public CategoriaController(ICategotiaRepository ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;

        }

        /// <summary>
        /// Obtener todas las categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoriaDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();

            foreach (var item in listaCategorias)
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(item));

            return Ok(listaCategoriasDto);
        }


        /// <summary>
        /// Obtener una categoria por su ID
        /// </summary>
        /// <param name="CategoriaId">Digite el ID de la categoria</param>
        /// <returns></returns>
        [HttpGet("{CategoriaId:int}",Name = "GetCategoria")]
        [ProducesResponseType(200, Type = typeof(CategoriaDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategoria(int CategoriaId)
        {
            var itemCategoria = _ctRepo.GetCategoria(CategoriaId);

            if (itemCategoria == null)
                return NotFound();

            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);

            return Ok(itemCategoriaDto);
            
        }


        /// <summary>
        /// Crear una categoria
        /// </summary>
        /// <param name="categoriaDto">Digite los datos solicitados para crear la categoria</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CrearCategoria([FromBody]CategoriaDto categoriaDto)
        {
            if (categoriaDto == null)
                return BadRequest(ModelState);

            if (_ctRepo.ExisteCategoria(categoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal con el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new {categoriaId = categoria.Id}, categoria);
        }


        /// <summary>
        /// Actualizar los datos de una categoria
        /// </summary>
        /// <param name="categoriaId">Digite el ID de la categoria que quiere modificar</param>
        /// <param name="categoriaDto">Cambie el dato que desea modificar</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (categoriaDto == null || categoriaId != categoriaDto.Id)  
                return BadRequest(ModelState);

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal al actualizar los datos de {categoria.Nombre}");
                return StatusCode(500, ModelState);                
            }

            return NoContent();
        }


        /// <summary>
        /// Borrar una categoria por su ID
        /// </summary>
        /// <param name="categoriaId">Digite el ID de la categoria a eliminar</param>
        /// <returns></returns>
        [HttpDelete("{categoriaId:int}", Name = "EliminarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EliminarCategoria(int categoriaId)
        {
            if (!_ctRepo.ExisteCategoria(categoriaId))
                return NotFound();

            var categoria = _ctRepo.GetCategoria(categoriaId);
            if (!_ctRepo.EliminarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Ups! Algo salio mal al eliminar el registro {categoria.Nombre}");
            }

            return NoContent();
        }




    }
}
