
using JobCommon.Model;
using Portal.DAL;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class CronJobsController : Controller
    {
        JobRecordDAL dal = new JobRecordDAL();
        private string configPath = ConfigurationManager.AppSettings["Folder_Job_Config"];
        //
        // GET: /CronJobs/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            int ret = dal.Delete(id, false);
            return RedirectToAction("TaskList");
        }
        public ActionResult Edit(string id = "")
        {
            Job_Config record = new Job_Config();
            if (string.IsNullOrEmpty(id))
            {
                record.ID = -1;
                record.RepeatMode = 0;
                record.StartTime = DateTime.Now;

            }
            else
            {
                record = dal.Get(Convert.ToInt32(id));
            }
            if (record != null)
            {
                return View(record);
            }
            else
            {
                return View("_NoFound");
            }

        }

        [HttpPost]
        public ActionResult Edit(Job_Config record, string buttons = "")
        {
            int cnt = -1;
            record.GroupName = "testGroup";
            if (ModelState.IsValid)
            {
                if (buttons == "上传")
                {
                    string fullPath = UploadFiles();
                    if (!string.IsNullOrEmpty(fullPath))
                    {
                        record.FilePath = fullPath;
                    }
                }

                if (record.ID == -1)
                {
                    record.DataChange_CreateTime = DateTime.Now;

                    record.ID = dal.Add(record);

                }
                else
                {
                    record.Status = 1;//更新为更新状态
                    cnt = dal.Update(record);
                }



                //if (record.ID!=-1 && cnt > 0)
                //{
                //    return RedirectToAction("TaskList");
                //}
                return RedirectToAction("Edit", new { id = record.ID });
            }
            else
            {
                return RedirectToAction("Edit");
            }




            return View();

        }

        string UploadFiles()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                string dirPath = configPath;
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
            string sql = "select * from Job_Config  ";
            List<Job_Config> list = dal.GetList(sql);

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
