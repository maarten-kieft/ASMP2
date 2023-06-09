﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Asmp2.Server.Persistence.Entities;

public class MeterEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    public string? Name { get; set; }

    public List<MeasurementEntity> Measurements { get; set; } = new List<MeasurementEntity>();
}
