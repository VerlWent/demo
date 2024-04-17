using System;
using System.Collections.Generic;

namespace demo2024.Model;

public partial class RequestT
{
    public int Number { get; set; }

    public DateTime? DateAdded { get; set; }

    public string? DeviceName { get; set; }

    public string? DescriptionFail { get; set; }

    public string? ClientFio { get; set; }

    public int? TypeFail { get; set; }

    public virtual ICollection<ResponsibleT> ResponsibleTs { get; } = new List<ResponsibleT>();

    public virtual ICollection<StatusChangeT> StatusChangeTs { get; } = new List<StatusChangeT>();

    public virtual TypeFailT? TypeFailNavigation { get; set; }
}
