using System;
using System.Collections.Generic;

namespace PreguntameAPIv3.LogicaAccesoDatos.Models;

public partial class Pregunta
{
    public Guid Id { get; set; }

    public string UsuarioRecibe { get; set; } = null!;

    public string? UsuarioEnvia { get; set; }

    public bool Anonima { get; set; }

    public string Dsc { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public bool Respondida { get; set; }

    public virtual Respuesta? Respuesta { get; set; }

    public virtual Usuario? UsuarioEnviaNavigation { get; set; }

    public virtual Usuario UsuarioRecibeNavigation { get; set; } = null!;
}
