using System;
using System.Collections.Generic;

namespace demo2024.Model;

public partial class TypeFailT
{
    public int Id { get; set; }

    public string? FailName { get; set; }

    public virtual ICollection<RequestT> RequestTs { get; } = new List<RequestT>();
}
