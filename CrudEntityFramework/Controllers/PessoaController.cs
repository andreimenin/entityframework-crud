using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesteCrud.Models;
namespace TesteCrud.Controllers
{
    public class PessoaController : Controller
    {
        // GET: Pessoa  
        public ActionResult Index()
        {
            return View();
        }

        //Fetch Data From Database to show in Datatable  
        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())

            {
                //filtrando pessoas ativas no sistema
                var pessoasAtivas = db.Pessoas.Where(a => a.Ativo == true);
                List<Pessoa> pessoaList = pessoasAtivas.ToList<Pessoa>();

                return Json(new { data = pessoaList }, JsonRequestBehavior.AllowGet);
            }
        }

        //Create Method for Insert and Update  

        [HttpGet]
        public ActionResult StoreOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Pessoa());
            else
            {
                using (DBModel db = new DBModel())
                {


                    return View(db.Pessoas.Where(x => x.Codigo == id).FirstOrDefault<Pessoa>());

                }
            }
        }

        [HttpPost]
        public ActionResult StoreOrEdit(Pessoa pessoaObj)
        {
            using (DBModel db = new DBModel())
            {
                if (pessoaObj.Codigo == 0)
                {
                    pessoaObj.Ativo = true;
                    db.Pessoas.Add(pessoaObj);                    
                    db.SaveChanges();
                    return Json(new { success = true, message = "Salvo com sucesso!", JsonRequestBehavior.AllowGet });
                }
                else
                {
                    pessoaObj.Ativo = true;
                    db.Entry(pessoaObj).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = pessoaObj.Codigo + " atualizado com sucesso!", JsonRequestBehavior.AllowGet });
                }
            }

        }

        [HttpPost]
        public ActionResult Delete(int id)        
        {
            using (DBModel db = new DBModel())
            {
                Pessoa pessoaObj = db.Pessoas.Where(x => x.Codigo == id).FirstOrDefault<Pessoa>();
                //db.Pessoas.Remove(emp);
                pessoaObj.Ativo = false;
                db.Entry(pessoaObj).State = EntityState.Modified;
                db.SaveChanges();                
                return Json(new { success = true, message = pessoaObj.Codigo+" excluído com sucesso", JsonRequestBehavior.AllowGet });
            }
        }

    }
}