using System;
using System.Collections.Generic;

namespace demo2024.Model;

public partial class ResponsibleT
{
    public int Id { get; set; }

    public int? RequestId { get; set; }

    public int? UserId { get; set; }

    public virtual RequestT? Request { get; set; }

    public virtual UserT? User { get; set; }
}
