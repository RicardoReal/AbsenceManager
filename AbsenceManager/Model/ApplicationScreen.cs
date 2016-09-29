using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(ApplicationScreenMetadata))]
    [DisplayColumn("ScreenName", "ScreenName", false)]
    public partial class ApplicationScreen : EntityObject
    {
        public override string ToString()
        {
            return this.ScreenName;
        }
    }

    [DisplayName("Application Screen")]
    public class ApplicationScreenMetadata
    {

    }
}