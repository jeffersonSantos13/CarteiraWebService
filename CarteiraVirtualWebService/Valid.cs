using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CarteiraVirtualWebService
{
    // Validações gerais
    public class Valid
    {
        #region Referentes a validação dos dados inseridos
        
        // Validação de E-mail
        #region Email
        public bool IsEmail(string email)
        {
            bool retorno = true;

            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (!rg.IsMatch(email))
            {
                retorno = false;
            }

            return retorno;
        }


        // Email unico
        public bool isUniqueEmail(string email)
        {
            CarteiraEntities db = new CarteiraEntities();
            var existe = db.Cliente.Where(m => m.emailCliente == email).Count();

            if (existe > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        // Validação de CFP
        #region CFP
        public bool IsCPFValido(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf, digito;
            int soma, resto;

            cpf = cpf.Trim();

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);

            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);

        }

        public bool IsUniqueCpF(string cpf)
        {
            CarteiraEntities db = new CarteiraEntities();
            var existe = db.Cliente.Where(m => m.cpfCliente == cpf).Count();
            if (existe == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region CEP
        public bool IsCEP(string cep)
        {
            bool retorno = true;

            try
            {
            } catch (Exception ex)
            {
                System.Console.WriteLine("Erro ao efetuar busca do CEP: {0}", ex.Message);
            }

            return retorno;
        }
        #endregion

        #region permissões do usuário
        public bool IsPermissoes(string permissoes)
        {
            bool retorono = false;

            CarteiraEntities db = new CarteiraEntities();

            


            return retorono;
        }
        #endregion

        #region chave estrangeiro do parceiro
        public bool ValidaChaveEstrangeira(int id)
        {
            bool erro = true;

            if(!(id > 0))
            {
                erro = false;
            }

            CarteiraEntities db = new CarteiraEntities();

            var par = db.Parceiro.Where(m => m.idParceiro == id).Count();
           
            if(par <= 0)
            {
                erro = false;
            }

            return erro;
        } 
        #endregion

        #endregion
    }
}