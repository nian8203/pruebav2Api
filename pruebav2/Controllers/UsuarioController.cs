using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using pruebav2.Models;
using pruebav2.Models.Dto;
using pruebav2.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pruebav2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _userRepo;
        public readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UsuarioController(IUsuarioRepository userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _userRepo.GetUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var item in listaUsuarios)
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(item));

            return Ok(listaUsuariosDto);
        }

        [HttpGet("{usuarioId:int}",Name = "GetUsuario")]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario = _userRepo.GetUsuario(usuarioId);

            if (itemUsuario == null)
                return NotFound();

            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);

            return Ok(itemUsuarioDto);
            
        }

        [HttpPost("Registro")]
        public IActionResult Registro(UsuarioAuthDto usuarioAuthDto)
        {
            usuarioAuthDto.Usuario = usuarioAuthDto.Usuario.ToLower();

            if (_userRepo.ExisteUsuario(usuarioAuthDto.Usuario))
                return BadRequest("El usuario ya se encuentra registrado");

            var usuarioACrear = new Usuario
            {
                UsuarioA = usuarioAuthDto.Usuario,
            };

            var usuarioCreado = _userRepo.RegistroUsuario(usuarioACrear, usuarioAuthDto.Password);
            return Ok(usuarioCreado);

        }

        [HttpPost("Login")]
        public IActionResult Login(UsuarioAuthLoginDto usuarioAuthLoginDto)
        {
            var usuarioDesdeRepo = _userRepo.LoginUsuario(usuarioAuthLoginDto.Usuario, usuarioAuthLoginDto.Password);

            if (usuarioDesdeRepo == null)
                return Unauthorized();

            var claims = new[]
            { 
                new Claim(ClaimTypes.NameIdentifier, usuarioDesdeRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, usuarioDesdeRepo.UsuarioA.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
            });

        
        }

       


        




    }
}
