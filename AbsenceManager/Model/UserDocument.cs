using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Objects.DataClasses;

namespace AbsenceManager
{
    [MetadataType(typeof(UserDocumentMetadata))]
    [DisplayColumn("Name", "Name", false)]
    public partial class UserDocument : EntityObject
    {
        
    }

    [DisplayName("Documentos")]
    public class UserDocumentMetadata
    {
        [Display(Name = "Nome", Order = 1)]
        public object Name { get; set; }

        [UIHint("JQueryCalendarDate", null, "PresentationMode", "Date", "Field", "FlightDate", "FieldClass", "Flight")]
        [Display(Name = "Validade", Order = 2)]
        public object Validade { get; set; }
    }
}