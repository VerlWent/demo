using System;
using System.Collections.Generic;

namespace demo2024.Model;

public partial class RequestStatusT
{
    public int Id { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<StatusChangeT> StatusChangeTs { get; } = new List<StatusChangeT>();
}
