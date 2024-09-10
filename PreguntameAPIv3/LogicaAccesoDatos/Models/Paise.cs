using System;
using System.Collections.Generic;

namespace PreguntameAPIv3.LogicaAccesoDatos.Models;

public partial class Paise
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Abr { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
