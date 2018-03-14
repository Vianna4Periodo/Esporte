using Esporte.Model.DB;
using Esporte.Model.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Esporte.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var Sports = DbFactory.Instance.EsporteRepository.FindAll();
            return View(Sports);
        }

        public ActionResult Create()
        {
            var s = new Sport();
            return View(s);
        }

        public ActionResult Edit(Guid id)
        {
            var s = DbFactory.Instance.EsporteRepository.FindById(id);
            return View("Create", s);
        }

        public ActionResult PersistPearson(Sport p, Guid id)
        {
            Sport s;
            if(DbFactory.Instance.EsporteRepository.FindById(p.Id) == null)
            {
                s = new Sport()
                {
                    DataCadastro = DateTime.Now,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    IsIndividual = p.IsIndividual
                };                                
            }
            else
            {
                s = DbFactory.Instance.EsporteRepository.FindById(p.Id);
                s.Descricao = p.Descricao;
                s.Nome = p.Nome;
                s.IsIndividual = p.IsIndividual;
            }
            
            DbFactory.Instance.EsporteRepository.SaveOrupdate(s);
            return RedirectToAction("Index");
        }        

        public ActionResult Search(String edtSearch)
        {
            if (String.IsNullOrEmpty(edtSearch))
            {
                return View("Index");
            }
            //1 - Filtro com FOR
            //for(var i=0; i<Sport.Sports.Count; i++)
            //{
            //    if(Sport.Sports[i].Nome == edtSearch.Trim())
            //    {
            //        SportAux.Add(Sport.Sports[i]);
            //    }
            //}

            //2 - Filtro com FOREACH
            //foreach(var Sport in Sport.Sports)
            //{
            //    if(Sport.Nome == edtSearch.Trim())
            //    {
            //        SportAux.Add(Sport);
            //    }
            //}

            //3 - Filtro com LINQ
            //SportAux = (
            //    from p in Sport.Sports
            //    where p.Nome == edtSearch.Trim()
            //    select p
            //    ).ToList();

            //4 - Filtro com LAMBDA
            var Sports = DbFactory.Instance.EsporteRepository.GetAllByName(edtSearch);

            return View("Index", Sports);
        }

        public ActionResult Delete(Guid id)
        {
            var p = DbFactory.Instance.EsporteRepository.FindById(id);

            if (p != null)
            {
                DbFactory.Instance.EsporteRepository.Delete(p);
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdatePearson(Guid id)
        {
            var p = DbFactory.Instance.EsporteRepository.FindById(id);

            if (p != null)
            {
                return View(p);
            }
            return RedirectToAction("Index");
        }
    }
}