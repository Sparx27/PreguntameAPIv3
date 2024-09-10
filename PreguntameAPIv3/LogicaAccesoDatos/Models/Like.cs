using System;
using System.Collections.Generic;

namespace PreguntameAPIv3.LogicaAccesoDatos.Models;

public partial class Like
{
    public Guid IdRespuesta { get; set; }

    public Guid IdUsuario { get; set; }

    public bool? Existe { get; set; }

    public virtual Respuesta IdRespuestaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
