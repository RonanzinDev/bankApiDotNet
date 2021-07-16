using System.Collections.Generic;
using System.Text.RegularExpressions;
using AutoMapper;
using bankApi.Models;
using bankApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bankApi.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountServices _accountservices;
        IMapper _mapper;
        public AccountController(IAccountServices accountServices, IMapper mapper)
        {
            _accountservices = accountServices;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register_new_account")]
        public IActionResult RegisterNewAccount([FromBody] RegisterNewAccountModel newAccount)
        {
            if (!ModelState.IsValid) return BadRequest(newAccount);
            // Vamo lá, pelo o que eu entendi do codigo, você esta passando essa nova conta pra o Account
            var account = _mapper.Map<Account>(newAccount);
            return Ok(_accountservices.Create(account, newAccount.Pin, newAccount.ConfirmPin));
        }
        [HttpGet]
        [Route("get_all_accounts")]

        public IActionResult GetAllAccounts()
        {
            var accounts = _accountservices.GetAllAccounts();
            var cleanedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);
            return Ok(cleanedAccounts);
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            return Ok(_accountservices.Authenticate(model.AccountNumber, model.Pin));
        }
        [HttpGet]
        [Route("get_by_account_number")]
        public IActionResult GetByAccountNumber(string AccountNumber)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Your account Number can only be 10 digits");
            var account = _accountservices.GetByAccountNumber(AccountNumber);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedAccount);
        }
        [HttpGet]
        [Route("get_account_by_id")]
        public IActionResult GetAccountById(int Id)
        {

            var account = _accountservices.GetById(Id);
            var cleanedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(cleanedAccount);
        }
        [HttpPut]
        [Route("update_account")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var account = _mapper.Map<Account>(model);
            _accountservices.Update(account, model.Pin);
            return Ok();
        }
    }
}