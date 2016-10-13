using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Service.Controllers
{
    public class FabricanteController : ApiController
    {
        public IEnumerable<Models.Fabricante> Get()
        {
            Models.LojaDataContext dc = new Models.LojaDataContext();
            var fabricantes = from f in dc.Fabricantes select f;
            return fabricantes.ToList();
        }

        // POST api/values
        public void Post([FromBody]Models.Fabricante fabricante)
        {
            Models.LojaDataContext dc = new Models.LojaDataContext();
            dc.Fabricantes.InsertOnSubmit(new Models.Fabricante() { Descricao = fabricante.Descricao });
            dc.SubmitChanges();
        }
    }
}
