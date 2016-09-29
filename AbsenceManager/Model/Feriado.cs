using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(FeriadoMetadata))]
    public partial class Feriado : EntityObject
    {
    }

    [DisplayName("Feriados")]
    public class FeriadoMetadata
    {
        [Display(Name = "Descrição", Order = 1)]
        public object Descricao { get; set; }

        [ScaffoldColumn(false)]
        public object Mes { get; set; }

        [ScaffoldColumn(false)]
        public object Dia { get; set; }

        [Display(Name = "Data", Order = 2)]
        [UIHint("JQueryCalendarDate", null, "PresentationMode", "Date", "Field", "FlightDate", "FieldClass", "Flight")]
        public object FeriadoData { get; set; }
    }
}