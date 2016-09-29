using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ComponentModel;

namespace AbsenceManager
{
    [MetadataType(typeof(RoleApplicationScreenMetadata))]
    [DisplayColumn("Description", "Description", false)]
    public partial class RoleApplicationScreen : EntityObject
    {
        public override string ToString()
        {
            return this.Role.Description + " - " + this.ApplicationScreen.ScreenName;
        }
    }

    [DisplayName("Application Screen Permissions")]
    public class RoleApplicationScreenMetadata
    {
        [Display(Name = "Application Screen", Order = 1)]
        public object ApplicationScreen { get; set; }

        [Display(Name = "Read Permission", Order = 2)]
        public object ReadPermission { get; set; }

        [Display(Name = "Write Permission", Order = 3)]
        public object WritePermission { get; set; }

        [HideColumnIn(PageTemplate.Edit, PageTemplate.Insert)]
        public object Role { get; set; }

        [ScaffoldColumn(false)]
        public object CanTrackChanges { get; set; }


    }
}