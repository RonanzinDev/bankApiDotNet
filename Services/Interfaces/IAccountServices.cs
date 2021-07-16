using System.Collections.Generic;
using bankApi.Models;

namespace bankApi.Services.Interfaces
{
    public interface IAccountServices
    {
        // Autenticação de conta
        Account Authenticate(string AccountNumber, string Pin);
        //Pegando todas as contas
        IEnumerable<Account> GetAllAccounts();
        // Criação de conta
        Account Create(Account account, string Pin, string ConfirmPin);
        // atualizar conta
        void Update(Account account, string Pin = null);
        // Deletar conta
        void Delete(int id);
        // Pegar conta pelo id
        Account GetById(int id);
        //Pegando a conta pelo numero
        Account GetByAccountNumber(string AccountNumber);

    }
}