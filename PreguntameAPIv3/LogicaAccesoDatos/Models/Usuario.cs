using System;
using System.Collections.Generic;

namespace PreguntameAPIv3.LogicaAccesoDatos.Models;

public partial class Usuario
{
    public Guid Id { get; set; }

    public bool Activo { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string UPassword { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public string? Bio { get; set; }

    public string? PreguntaBio { get; set; }

    public string? FotoPath { get; set; }

    public string? FondoPath { get; set; }

    public Guid? PaisId { get; set; }

    public int NLikes { get; set; }

    public int NSeguidores { get; set; }

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual Paise? Pais { get; set; }

    public virtual ICollection<Pregunta> PreguntaUsuarioEnviaNavigations { get; set; } = new List<Pregunta>();

    public virtual ICollection<Pregunta> PreguntaUsuarioRecibeNavigations { get; set; } = new List<Pregunta>();
}
