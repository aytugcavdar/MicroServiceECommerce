using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Core.Domain;

public interface IEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
}

public interface IEntity<TId>
{
    public TId Id { get; set; }
}