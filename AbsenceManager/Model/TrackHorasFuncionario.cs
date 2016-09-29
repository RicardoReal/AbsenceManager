﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Objects.DataClasses;

namespace AbsenceManager
{
    [MetadataType(typeof(TrackHorasFuncionarioMetadata))]
    public partial class TrackHorasFuncionario : EntityObject
    {
        public TrackHorasFuncionario() { }

        public TrackHorasFuncionario(long HorasFuncionarioID, string FieldName, string OldValue, string NewValue, string ChangedBy, DateTime ChangedDateTime, string MachineToken)
        {
            this.HorasFuncionarioID = HorasFuncionarioID;
            this.FieldName = FieldName;
            this.OldValue = OldValue;
            this.NewValue = NewValue;
            this.ChangedBy = ChangedBy;
            this.ChangedDateTime = ChangedDateTime;
            this.MachineToken = MachineToken;
        }
    }

    [DisplayName("Changes Registo de Horas ")]
    public class TrackHorasFuncionarioMetadata
    {
        [Display(Name = "Campo", Order = 1)]
        public object FieldName { get; set; }

        [Display(Name = "Valor Antigo", Order = 2)]
        public object OldValue { get; set; }

        [Display(Name = "Valor Novo", Order = 3)]
        public object NewValue { get; set; }

        [Display(Name = "Alterado Por", Order = 4)]
        public object ChangedBy { get; set; }

        [Display(Name = "Data Alteração", Order = 5)]
        [FilterUIHint("DateGreaterThan", null, "Mode", "FlightsMinutesFlightsAfter")]
        [UIHint("TextRO")]
        public object ChangedDateTime { get; set; }

        [Display(Name = "Machine Token", Order = 6)]
        public object MachineToken { get; set; }

        [ScaffoldColumn(false)]
        public object HorasFuncionarioID { get; set; }
    }
}