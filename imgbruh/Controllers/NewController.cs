using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using imgbruh.Models;

public class TestController : Controller
{
    public void Index()
    {
        Department _department = new Department { DepartmentID = new Guid("aed99956-c3e1-44a7-b09a-00169f64bdff"), Status = true, SortOrder = 320, Image = null, Approved = true, ApprovedBy = "Import", ApprovedDate = Convert.ToDateTime("11/22/2016 3:40:50PM"), LastUpdated = Convert.ToDateTime("11/22/2016 3:40:50PM"), UpdatedBy = "Import" };
        DepartmentDescription _description = new DepartmentDescription { DepartmentID = new Guid("aed99956-c3e1-44a7-b09a-00169f64bdff"), LocaleID = 1033, Description = "Department Description" };
        _department.DepartmentDescriptions.Add(_description);
        imgbruhContext db = new imgbruhContext();
        db.Depts.Add(_department);
        db.SaveChanges();
    }
}

public class Department
{
    [Key]
    public Guid DepartmentID { get; set; }
    public Int32? SortOrder { get; set; }
    public Byte[] Image { get; set; }
    public Boolean Status { get; set; }
    public DateTime LastUpdated { get; set; }
    public String UpdatedBy { get; set; }
    public Boolean Approved { get; set; }
    public String ApprovedBy { get; set; }
    public DateTime ApprovedDate { get; set; }
    public Guid? ParentDepartment { get; set; }

    // Navigation Properties
    public virtual ICollection<DepartmentDescription> DepartmentDescriptions { get; set; }

    public Department()
    {
        DepartmentDescriptions = new HashSet<DepartmentDescription>();
    }

}


public class DepartmentDescription
{
    [Key]
    [Column(Order = 1)]
    public Guid DepartmentID { get; set; }
    [Key]
    [Column(Order = 2)]
    public Int32 LocaleID { get; set; }
    public String Description { get; set; }

    // Navigation Properties
    [ForeignKey("DepartmentID"), Required]
    public virtual Department Department { get; set; }

    [ForeignKey("LocaleID"), Required]
    public virtual Locale Locale { get; set; }

}

public class Locale
{
    [Key]
    public Int32 LocaleID { get; set; }
    public String ShortString { get; set; }
    public String Description { get; set; }
    public Boolean Status { get; set; }

    //Navigation Properties
   [InverseProperty("Locale")]
    public virtual ICollection<DepartmentDescription> DepartmentDescriptions { get; set; }

    public Locale()
    {
        DepartmentDescriptions = new HashSet<DepartmentDescription>();
    }
}