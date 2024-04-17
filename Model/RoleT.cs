using System;
using System.Collections.Generic;

namespace demo2024.Model;

public partial class RoleT
{
    public int Id { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<UserT> UserTs { get; } = new List<UserT>();
}
