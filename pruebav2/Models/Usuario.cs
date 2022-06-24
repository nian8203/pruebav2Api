﻿using System.ComponentModel.DataAnnotations;

namespace pruebav2.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }
    public string UsuarioA { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
}
