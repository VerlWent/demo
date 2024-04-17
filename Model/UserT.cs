using System;
using System.Collections.Generic;

namespace demo2024.Model;

public partial class UserT
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronomic { get; set; }

    public int? Role { get; set; }

    public virtual ICollection<ResponsibleT> ResponsibleTs { get; } = new List<ResponsibleT>();

    public virtual RoleT? RoleNavigation { get; set; }
}
