using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data.Entity.Infrastructure;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;

namespace CarteiraVirtualWebService
{
    
    /// <summary>
    /// PJVWebServiceCliente - Esse WebService vai ser responsável pela inclusão dos registros dos clientes
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PJVWebServiceCliente : System.Web.Services.WebService
    {

        private CarteiraEntities db = new CarteiraEntities();
        private RetornoMensagem retorno = new RetornoMensagem("");

        // Validações gerais
        private Valid _valid = new Valid();

        // Mensagem de retorno
        private string mensagem = "";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Inserir(string param)
        {
            try
            {
                Cliente cliente = new Cliente();
                cliente = JsonConvert.DeserializeObject<Cliente>(param);

                var _this = cliente;

                if (_this != null)
                {
                    if (IsClienteValid(_this, "I"))
                    {
                        Encrypt encrypt = new Encrypt();

                        string senha = encrypt.RetornarMD5(_this.senha);

                        if (!senha.Equals(""))
                            _this.senha = senha;

                        DateTime dateValue;

                        if (DateTime.TryParseExact(_this.data, "dd/MM/yyyy",
                                new CultureInfo("pt-BR"),
                                DateTimeStyles.None,
                                out dateValue))
                        {
                            _this.dtNasc = dateValue;
                        }

                        DateTime date = DateTime.Today;
                        _this.dtRegistro = date;

                        db.Cliente.Add(_this);
                        db.SaveChanges();
                    }
                }

            } catch (DbUpdateException ex)
            {
                mensagem = "ERRO: Falha na inclusão do registro: " + ex.Message;
            } catch (ArgumentNullException arg)
            {
                mensagem = "ERRO: Parâmetro inválido: " + arg.Message;
            } catch (NullReferenceException ex)
            {
                mensagem = "ERRO: Parâmetro com valor inválido: " + ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Response(mensagem);

            Context.Response.Write(mensagem);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Alterar(string param)
        {
            try {

                Cliente cliente = new Cliente();
                cliente = JsonConvert.DeserializeObject<Cliente>(param);

                var _this = cliente;

                if (_this != null)
                {
                    if (_this.idCliente > 0 && !_this.idCliente.Equals(0))
                    {
                        // Atualiza os valores do cliente
                        var clienteUpdate = db.Cliente.Where(m => m.idCliente == _this.idCliente).FirstOrDefault();

                        if (IsClienteValid(_this, "A"))
                        {
                            clienteUpdate.nomeCliente   = _this.nomeCliente;
                            clienteUpdate.emailCliente  = _this.emailCliente;
                            clienteUpdate.cepCliente    = _this.cepCliente;
                            clienteUpdate.sexo          = _this.sexo;
                            clienteUpdate.cpfCliente    = _this.cpfCliente;
                            clienteUpdate.cepCliente    = _this.cepCliente;

                            DateTime dateValue;

                            if (DateTime.TryParseExact(_this.data, "dd/MM/yyyy",
                                    new CultureInfo("pt-BR"),
                                    DateTimeStyles.None,
                                    out dateValue))
                            {
                                clienteUpdate.dtNasc = dateValue;
                            }

                            if (_this.sobreCliente != null && !_this.sobreCliente.Equals(""))
                                clienteUpdate.sobreCliente = _this.sobreCliente;
                            
                            if(_this.telCliente != null && !_this.telCliente.Equals(""))                            
                                clienteUpdate.telCliente = _this.telCliente;

                            if (_this.celCliente != null && !_this.celCliente.Equals(""))
                                clienteUpdate.celCliente = _this.celCliente;

                            if (_this.rgCliente != null && !_this.rgCliente.Equals(""))
                                clienteUpdate.rgCliente = _this.rgCliente;

                            //string senha = Encrypt.GetMD5Hash(_this.senha);

                            if (db.SaveChanges() <= 0) mensagem = "Não foi possível alterar os dados";
                        }
                    } else{
                        mensagem = "Código do cliente não informado.";
                    }
                }
            }catch (Exception ex)
            {
                mensagem = "ERRO: Ocorreu um problema na alteração do registro: " + ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Response(mensagem);

            Context.Response.Write(mensagem);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Deletar(int idCliente)
        {
            try
            {
                if (idCliente > 0 && !idCliente.Equals(0))
                {
                    var cliente = db.Cliente.Where(m => m.idCliente == idCliente).FirstOrDefault();
                    db.Cliente.Remove(cliente);
                    db.SaveChanges();
                }

            } catch (DbUpdateException ex)
            {
                mensagem = "ERRO: Ocorreu um problema na alteração do registro: " + ex.Message;
            }

            // Retorno das mensagens em Json
            mensagem = retorno.Response(mensagem);

            Context.Response.Write(mensagem);
        }

        [WebMethod]
        public void Consultar(int idCliente)
        {
            string retorno = "";
            Cliente cliente = new Cliente();

            //HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://www.contoso.com");
            //httpReq.AllowAutoRedirect = false;

            //HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse();

            try
            {
                // Retornar todos os clients
                if (idCliente > 0)
                {
                    var lista = db.Cliente.Where(m => m.idCliente == idCliente).FirstOrDefault();
                    retorno = JsonConvert.SerializeObject(lista);

                } else if (idCliente <= 0)
                {
                    var lista = db.Cliente.OrderBy(m => m.idCliente).ToList();
                    retorno = JsonConvert.SerializeObject(lista);
                }
            } catch (NullReferenceException ex)
            {
                Console.WriteLine("ERRO: Parâmetro com valor inválido: {0}", ex.Message);
            } catch (ArgumentNullException ex)
            {
                Console.WriteLine("ERRO: fonte ou chaveSelector é nulo.: { 0}", ex.Message);
            }

            // Retorno em Json
            Context.Response.Write(retorno);
        }

        public enum HttpStatusCode
        {
            Accepted = 202,
            Ambiguous = 300,
            BadGateway = 502,
            BadRequest = 400,
            Moved = 301,
            OK = 200,
            Redirect = 302
        }

        // Alterar Senha
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void AlterarSenha(int idCliente, string novaSenha)
        {
            if (idCliente > 0)
            {
                if (!novaSenha.Equals("") && novaSenha != null)
                {
                    var cliente = db.Cliente.Where(m => m.idCliente == idCliente).FirstOrDefault(); ;

                    var _this = cliente;

                    if (_this != null)
                    {
                        Encrypt encrypt = new Encrypt();

                        string senha = encrypt.RetornarMD5(novaSenha);

                        if (!senha.Equals(""))
                        {
                            _this.senha = senha;
                            db.SaveChanges();
                        } else
                            mensagem = "Problema na alteração da senha do cliente";
                    }
                    else
                        mensagem = "Cliente informado não existe";

                } else
                    mensagem = "Senha não informado";
            } else
                mensagem = "Cliente não informado";

            // Retorno da mensagem em Json
            mensagem = retorno.Response(mensagem);

            Context.Response.Write(mensagem);
        }

        // Login
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Login(string email, string senha)
        {
            int idRetorno = 0;

            if (!email.Equals(""))
            {
                if (!senha.Equals(""))
                {
                    Encrypt encrypt = new Encrypt();

                    string senhaHash = encrypt.RetornarMD5(senha);

                    var login = db.Cliente.Where(m => m.emailCliente == email && m.senha == senhaHash).FirstOrDefault();

                    var _this = login;

                    if (_this == null)
                        mensagem = "E-mail ou senha inválidos";
                    else
                        idRetorno = _this.idCliente;
                }
                else
                    mensagem = "Senha não informado.";
            }
            else
                mensagem = "E-mail não informado.";

            // Retorno da mensagem em Json
            mensagem = retorno.Login(mensagem, idRetorno);

            Context.Response.Write(mensagem);
        }

        // Validações do cliente
        public bool IsClienteValid(Cliente cliente, string acao)
        {
            bool retorno = true;

            var _this = cliente;

            if (_this != null)
            {

                if (_this.nomeCliente == null || _this.nomeCliente.Equals(""))
                {
                    mensagem = "Nome do Cliente não informado.";
                    retorno = false;
                }
                else if (_this.emailCliente == null || _this.emailCliente.Equals(""))
                {
                    mensagem = "E-mail do Cliente não informado.";
                    retorno = false;
                }
                else if (!_valid.IsEmail(_this.emailCliente.ToString()))
                {
                    mensagem = "E-mail informado inválido.";
                    retorno = false;
                }   
                else if(_this.sexo == null || _this.sexo.Equals(""))
                {
                    mensagem = "Informe o sexo";
                    retorno = false;
                }
                else if (_this.cpfCliente == null ||_this.cpfCliente.Equals(""))
                {
                    mensagem = "CPF não informado";
                    retorno = false;
                }
                else if (!_this.cpfCliente.Equals(""))
                {
                    if (!_valid.IsCPFValido(_this.cpfCliente))
                    {
                        mensagem = "CPF informado inválido." + _this.cpfCliente;
                        retorno = false;
                    }
                }
                else if (acao == "I")
                {
                    if (_valid.IsUniqueCpF(_this.cpfCliente))
                    {
                        mensagem = "CPF informado já existe.";
                        retorno = false;
                    }
                    else if (_this.senha == null || _this.senha.Equals(""))
                    {
                        mensagem = "Senha não informado.";
                        retorno = false;
                    }
                    else if (_this.senha.Length <= 2)
                    {
                        mensagem = "Senha informada muito curta.";
                        retorno = false;
                    }
                }
                else if (_this.data.Equals("") || _this.data == null)
                {
                    mensagem = "Data de nascimento não informado";
                    retorno = false;
                }
                else if (_this.cepCliente.Equals("") || _this.cepCliente == null)
                {
                    mensagem = "CEP não informado";
                    retorno = false;
                }
            }

            return retorno;
        }

    }
}
