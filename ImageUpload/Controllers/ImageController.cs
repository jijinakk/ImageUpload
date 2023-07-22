using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageUpload.Models;

namespace ImageUpload.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Image imageModel)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(imageModel.Imagefile.FileName);
                string extention = Path.GetExtension(imageModel.Imagefile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
                imageModel.ImagePath = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                imageModel.Imagefile.SaveAs(fileName);
                using (StoredProcedureEntities sp = new StoredProcedureEntities())
                {
                    sp.Images.Add(imageModel);
                    sp.SaveChanges();

                }
                ModelState.Clear();
            }
            catch(Exception ex)
            {
                ErrorEventArgs e = new ErrorEventArgs(ex);
            }
            return View();

        }
        [HttpGet]
        public ActionResult View(int id)
        {
            Image imageModel = new Image();
            using (StoredProcedureEntities sp = new StoredProcedureEntities())
            {
                imageModel = sp.Images.Where(x => x.ImageID == id).FirstOrDefault();

            }
            return View(imageModel);

        }

    }
}