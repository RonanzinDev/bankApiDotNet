using bankApi.Models;
using System;

namespace bankApi.Services.Interfaces
{
    public interface ITransactionServices
    {
        // Criando uma nova transação, a classe Responsa servirar para nos dar os tipos de repostas
        Response CreateNewTransaction(Transaction transaction);
        // Encontrando transação pela data
        Response FindTransactionByDate(DateTime date);
        // Fazendo deposito
        Response MakeDeposit(string AccountNumber, decimal Amount, int TransactionPin);
        // Realizando Saque
        Response MakeWithdrawal(string AccountNumber, decimal Amount, int TransactionPin);
        // Transferencia
        Response MakeFundsTranfer(string FromAccount, string ToAccount, decimal Amount, int TransactionPin);
    }
}