﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiPaises.Models;

namespace WebApiPaises.Controllers
{
	[Produces("application/json")]
	[Route("api/Account")]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<AplicationUser> _userManager;
		private readonly SignInManager<AplicationUser> _signInManager;
		private readonly IConfiguration _configuration;

		public AccountController(UserManager<AplicationUser> useManager,
			SignInManager<AplicationUser> signInManager,
			IConfiguration configuration)
		{
			this._userManager = useManager;
			this._signInManager = signInManager;
			this._configuration = configuration;
		}

		[Route("Create")]
		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
		{
			if (ModelState.IsValid)
			{
				var user = new AplicationUser { UserName = model.Email, Email = model.Email };
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
					return BuildToken(model);
				else
					return BadRequest("Invalid user or password");
			}
			else
				return BadRequest(ModelState);
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

				if (result.Succeeded)
					return BuildToken(userInfo);
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt");
					return BadRequest(ModelState);
				}
			}
			else
				return BadRequest(ModelState);
		}

		private IActionResult BuildToken(UserInfo userInfo)
		{
			var claims = new[]
{
				new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["mykeytoken"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var expiration = DateTime.UtcNow.AddHours(1);

			JwtSecurityToken token = new JwtSecurityToken(
			   issuer: "yourdomain.com",
			   audience: "yourdomain.com",
			   claims: claims,
			   expires: expiration,
			   signingCredentials: creds);

			return Ok(new
			{
				token = new JwtSecurityTokenHandler().WriteToken(token),
				expiration = expiration
			});
		}
	}
}