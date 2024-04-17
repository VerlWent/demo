using System;
using System.Collections.Generic;

namespace demo2024.Model;

public partial class StatusChangeT
{
    public int Id { get; set; }

    public int? RequestId { get; set; }

    public int? RequestStatus { get; set; }

    public DateTime? ChangeDate { get; set; }

    public string? Comment { get; set; }

    public virtual RequestT? Request { get; set; }

    public virtual RequestStatusT? RequestStatusNavigation { get; set; }
}
