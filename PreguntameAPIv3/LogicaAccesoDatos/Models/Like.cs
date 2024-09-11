using System;
using System.Collections.Generic;

namespace PreguntameAPIv3.LogicaAccesoDatos.Models;

public partial class Like
{
    public Guid IdRespuesta { get; set; }

    public Guid IdUsuarioEnvia { get; set; }

    public string UsernameUsuarioRecibe { get; set; } = null!;

    public bool? Existe { get; set; }

    public virtual Respuesta IdRespuestaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioEnviaNavigation { get; set; } = null!;

    public virtual Usuario UsernameUsuarioRecibeNavigation { get; set; } = null!;
}
