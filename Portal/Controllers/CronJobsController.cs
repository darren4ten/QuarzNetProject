using JobCommon.Model;
using Portal.DAL;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class CronJobsController : Controller
    {
        JobRecordDAL dal = new JobRecordDAL();
        //
        // GET: /CronJobs/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id = "")
        {
            JobRecord record = new JobRecord();
            if (string.IsNullOrEmpty(id))
            {
                record.StartTime = DateTime.Now;

            }
            else
            {
                record = dal.Get(Guid.Parse(id));
            }
            return View(record);
        }

        [HttpPost]
        public ActionResult Edit(JobRecord record, string buttons = "")
        {
            int cnt = -1;
            if (ModelState.IsValid)
            {
                if (record.ID == Guid.Empty)
                {
                    record.ID = Guid.NewGuid();
                    cnt = dal.Add(record);
                }
                else
                {
                    if (buttons == "上传")
                    {
                        string fullPath = UploadFiles();
                        if (!string.IsNullOrEmpty(fullPath))
                        {
                            record.AssemblyPath = fullPath;
                        }
                    }
                    cnt = dal.Update(record);
                }

                if (cnt > 0)
                {
                    return RedirectToAction("Edit", new { id = record.ID });
                }
            }
            else
            {
                return RedirectToAction("Edit");
            }


            record.GroupName = "testGroup";

            return View();

        }

        string UploadFiles()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string dirPath = "D:/temp/";
                string fullPath = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), file.FileName);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                fullPath = Path.Combine(dirPath, fullPath);
                file.SaveAs(fullPath);
                return fullPath;
            }
            else
            {
                return "";
            }
        }

        public ActionResult TaskList()
        {
            string sql = "select * from JobRecord  ";
            List<JobRecord> list = dal.GetList(sql);

            return View(list);
        }

        public ActionResult Test()
        {
            var list = new List<TestEntitiy>();
            list.Add(new TestEntitiy()
            {
                Name = "aaaa",
                Age = 43

            });

            return View(list);
        }

    }
}
