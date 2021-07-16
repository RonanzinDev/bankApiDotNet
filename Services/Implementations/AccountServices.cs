using System.Collections.Generic;
using bankApi.DAL;
using bankApi.Models;
using bankApi.Services.Interfaces;
using System.Linq;
using System;
using System.Text;

namespace bankApi.Services.Implementations
{
    public class AccountServices : IAccountServices
    {
        private BankApiContext _context { get; set; }
        public AccountServices(BankApiContext context)
        {
            _context = context;
        }
        //Funcao para fazer a autenticação
        public Account Authenticate(string AccountNumber, string Pin)
        {
            var accout = _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if (accout == null)
            {
                return null;
            }
            //verificando pinhash
            if (!VerifyPinHash(Pin, accout.PinStoredHash, accout.PinStoredSalt)) return null;
            return accout;

        }
        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin");
            //agora vamos verificar o pin
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }

        //Essa função ira servir para criarmos uma conta
        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            // Verificando se ja exite um email ja existente
            if (_context.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("An Account already exist with this email");
            // validando o Pin
            if (!Pin.Equals(ConfirmPin)) throw new ArgumentException("Pins do not match", "Pin");
            // Agora que validamos tudo, vamos criar um usuario
            // vamos encryptar o pin primeiro
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);
            account.PinStoredHash = pinHash;
            account.PinStoredSalt = pinSalt;
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }
        public void Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            var account = _context.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if (account == null) return null;
            return account;
        }

        public Account GetById(int id)
        {
            var account = _context.Accounts.Where(x => x.Id == id).FirstOrDefault();
            if (account == null) return null;
            return account;
        }
        // funcao para atualizar dados do usuario, queremos que o usuario atualize apenas o email, numero de celular e o Pin
        public void Update(Account account, string Pin = null)
        {
            var accounToBeUpdated = _context.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();

            if (accounToBeUpdated == null) throw new ApplicationException("Account does not exist");
            // Atualizar email
            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_context.Accounts.Any(x => x.Email == account.Email)) throw new ApplicationException("this email" + account.Email + " already exist");
                accounToBeUpdated.Email = account.Email;
            }


            //Atualizar o numero de celular
            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if (_context.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException("this phone number" + account.PhoneNumber + " already exist");
                accounToBeUpdated.PhoneNumber = account.PhoneNumber;
            }


            //Atualizar o Pin
            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);
                accounToBeUpdated.PinStoredHash = pinHash;
                accounToBeUpdated.PinStoredSalt = pinSalt;
            }
            accounToBeUpdated.DateLastUpdated = DateTime.Now;

            _context.Accounts.Update(accounToBeUpdated);
            _context.SaveChanges();
        }
    }
}