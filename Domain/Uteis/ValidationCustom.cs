using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Uteis
{
    public class ValorMaiorQueZero : ValidationAttribute
    {
        /// <summary>
        /// Retorna true se valor for maior que zero
        /// </summary>
        /// <param name="value">O valor numérico</param>
        /// <returns>True se o valor for maior que zero</returns>
        /// 
    
        public override bool IsValid(object value)
        {
            ErrorMessage = string.Format("O valor do campo precisa ser maior que zero.");
            decimal valor;
            return value != null && decimal.TryParse(value.ToString(), out valor) && valor > 0;    
        }
    }
    public class ValorPositivo : ValidationAttribute
    {
        /// <summary>
        /// Retorna true se valor for maior que zero
        /// </summary>
        /// <param name="value">O valor numérico</param>
        /// <returns>True se o valor for maior que zero</returns>
        /// 

        public override bool IsValid(object value)
        {
            ErrorMessage = string.Format("O valor do campo precisa ser maior ou igual que zero.");
            decimal valor;
            return value != null && decimal.TryParse(value.ToString(), out valor) && valor >= 0;
        }
    }

    /// <summary>
    /// Retorna true se todos os elementos da estrutura de dados for maior que zero
    /// </summary>
    /// <remarks>Por favor conferir se o tipo da estrutura de dados já consta no validador.</remarks>
    /// <param name="value">O valor numérico</param>
    /// <returns>True se o valor de cada elemento da lista for maior que zero</returns>
    /// 
    public class SomaValoresListaIgualInteiro : ValidationAttribute
    {
        /// <summary>
        /// Retorna true se valor for maior que zero
        /// </summary>
        /// <param name="value">The o valor numérico</param>
        /// <returns>True se o valor for maior que zero</returns>
        /// 
        public long _ValorAlvo { get; set; }
        public SomaValoresListaIgualInteiro(long ValorAlvo)
        {
            _ValorAlvo = ValorAlvo;
            ErrorMessage = string.Format("A soma dos valores precisa ser igual à {0}.", ValorAlvo);
        }

    }

    public class CampoObrigatorio : RequiredAttribute
    {
        public CampoObrigatorio()
        {
            AllowEmptyStrings = false;
            ErrorMessage = "Campo obrigatório.";
        }
    }

    public class TamanhoFixoCaracteres : StringLengthAttribute
    {
        public TamanhoFixoCaracteres(int TamanhoFixo) : base(TamanhoFixo)
        {
             MinimumLength = TamanhoFixo;
             ErrorMessage = $"A sigla deve conter apenas {TamanhoFixo} caracteres.";
        }
    }

    public class TamanhoVariavelCaracteres : StringLengthAttribute
    {
        public TamanhoVariavelCaracteres(int TamanhoMinimo, int TamanhoMaximo) : base(TamanhoMaximo)
        {
            MinimumLength = TamanhoMinimo;
            ErrorMessage = $"A sigla deve conter entre {TamanhoMinimo} e {TamanhoMaximo} caracteres.";
        }
    }

    public class TamanhoVariavelCaracteresArray : StringLengthAttribute
    {
        public TamanhoVariavelCaracteresArray(int TamanhoMinimo, int TamanhoMaximo) : base(TamanhoMaximo)
        {
            MinimumLength = TamanhoMinimo;
            ErrorMessage = $"Cada elemento deve conter entre {TamanhoMinimo} e {TamanhoMaximo} caracteres.";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            else
            {
                List<string> lstObject = (List<string>)value;
                foreach(string strObject in lstObject)
                {
                    if (string.IsNullOrEmpty(strObject) || !base.IsValid(strObject))
                        return false;
                }

                return true;
            }
        }
    }

    public class TamanhoMinimoLista : MinLengthAttribute
    {
        public TamanhoMinimoLista(int TamanhoMinimo, string NomeCampo) : base(TamanhoMinimo)
        {
            ErrorMessage = $"Precisa conter ao menos um(a) {NomeCampo}.";
        }
}

    public class ValoresPossiveis : ValidationAttribute
    {
        private string[] _arrObjects { get; set; }
        public ValoresPossiveis(params string[] arrObjects)
        {
            _arrObjects = arrObjects;
            ErrorMessage = $"Valor deve respeitar a faixa especificada: {string.Join(",", arrObjects)} ";
        }

        public override bool IsValid(object value)
        {
            return _arrObjects.Contains(value?.ToString());
        }
    }

    public class EmailAddressListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string strLstEmail = value.ToString();

            EmailAddressAttribute oEmailAddressAttribute = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            foreach (string sDsEmail in strLstEmail.Split(new char[] { ';' }))
                if (!oEmailAddressAttribute.IsValid(sDsEmail))
                    return false;
            return true;
        }
 
    }

    public class CpfAttribute : ValidationAttribute
    {
     
    }

    public class ValorMaiorIgualaZero : ValidationAttribute
    {
        /// <summary>
        /// Retorna true se valor for maior ou igual a zero
        /// </summary>
        /// <param name="value">O valor numérico</param>
        /// <returns>True se o valor for maior ou igual a zero</returns>
        /// 

        public override bool IsValid(object value)
        {
            ErrorMessage = string.Format("O valor do campo precisa ser maior ou igual a zero.");
            decimal valor;
            return value != null && decimal.TryParse(value.ToString(), out valor) && valor >= 0;
        }
    }
}
