using System;
using System.Collections.Generic;

namespace PreguntameAPIv3.LogicaAccesoDatos.Models;

public partial class Respuesta
{
    public Guid Id { get; set; }

    public Guid PreguntaId { get; set; }

    public string Dsc { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public int NLikes { get; set; }

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual Pregunta Pregunta { get; set; } = null!;
}
