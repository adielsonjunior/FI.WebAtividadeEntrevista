using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.AtividadeEntrevista.DAL;


namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        // GET: Beneficiario
        public ActionResult Index()
        {
            return PartialView("_PartialBeneficiarios");
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(Beneficiario model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {

                model.Id = bo.Incluir(new Beneficiario()
                {
                    
                    Nome = model.Nome,                 
                    CPF = model.CPF,
                    IdCliente = model.IdCliente
                });


                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(Beneficiario model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Beneficiario()
                {
                    Id = model.Id,                   
                    Nome = model.Nome,                
                    CPF = model.CPF,
                    IdCliente = model.IdCliente
                });

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoBeneficiario bo = new BoBeneficiario();
            Beneficiario beneficiario = bo.Consultar(id);
            Beneficiario model = null;

            if (beneficiario != null)
            {
                model = new Beneficiario()
                {
                    Id = beneficiario.Id,                  
                    Nome = beneficiario.Nome,                   
                    CPF = beneficiario.CPF,
                    IdCliente = beneficiario.IdCliente
                };


            }

            return View(model);
        }
                

        [HttpPost]
        public JsonResult VerificarCPFDublicado(string cpf, string idCliente)
        {
            try
            {
                string mensagem = "Beneficiário já cadastrado no banco de dados.";

                if (idCliente == null)
                {
                    var cpfEncontrado = new BoBeneficiario().VerificarExistencia(cpf, idCliente);
                    if (!cpfEncontrado)
                    {
                        mensagem = "";
                    }
                }
                else
                {
                    var BeneficarioEncontrado = new BoBeneficiario().Consultar(Convert.ToInt64(idCliente));

                    if (BeneficarioEncontrado.CPF == cpf && BeneficarioEncontrado.Id == Convert.ToInt64(idCliente))
                    {
                        mensagem = "";
                    }

                }

                return Json(new { Message = mensagem });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


    }
}