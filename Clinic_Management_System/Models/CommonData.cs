using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinic_Management_System.Models
{
    public class CommonData
    {
        public static List<SelectListItem> GetRoles()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (KIITEntities db = new KIITEntities())
            {

                var getRoles = db.RoleDetails.Where(a => a.RoleName != "Admin").ToList();
                foreach (var item in getRoles)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.RoleId.ToString()
                    });
                }
            }
            return lst;
        }

        public static List<SelectListItem> GetDocs()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (KIITEntities db = new KIITEntities())
            {

                var getDocs = db.Doctors.ToList();
                foreach (var item in getDocs)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = item.FirstName,
                        Value = item.DoctorId.ToString()
                    });
                }
            }
            return lst;
        }
        public static List<SelectListItem> GetDocsId()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (KIITEntities db = new KIITEntities())
            {

                var getDocs = db.Doctors.ToList();
                foreach (var item in getDocs)
                {
                    string id = db.MemberLogins.Where(a => a.MemberId == item.MemberId).Select(a => a.EmailId).FirstOrDefault();
                    lst.Add(new SelectListItem
                    {
                        Text = item.FirstName,
                        Value = id
                    });
                }
            }
            return lst;
        }

        public static List<SelectListItem> GetDrugs()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (KIITEntities db = new KIITEntities())
            {

                var getDrugs = db.Drugs.ToList();
                foreach (var item in getDrugs)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = item.DrugName,
                        Value = item.DrugId.ToString()
                    });
                }
            }
            return lst;
        }

        public static List<SelectListItem> GetSpecs()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (KIITEntities db = new KIITEntities())
            {

                var getSpecs = db.SpecializedDatas.ToList();
                foreach (var item in getSpecs)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = item.SpecializedName,
                        Value = item.SpecializedId.ToString()
                    });
                }
            }
            return lst;
        }

        public static List<SelectListItem> GetSuppliers()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            using (KIITEntities db = new KIITEntities())
            {

                var getSplr = db.Suppliers.ToList();
                foreach (var item in getSplr)
                {
                    lst.Add(new SelectListItem
                    {
                        Text = item.FirstName,
                        Value = item.SupplierId.ToString()
                    });
                }
            }
            return lst;
        }
    }
}