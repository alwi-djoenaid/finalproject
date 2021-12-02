using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FinalProject.Controllers;
using FinalProject.Data;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public PaymentController(ApiDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayment()
        {
            var payments = await _context.Payments.ToListAsync();
            return new OkObjectResult(new {
                Success = true,
                Message = "Success get all payment records",
                payments
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(x => x.Id == id);

            if (payment == null) return new NotFoundObjectResult(new
            {
                Success = false,
                Message = "Payment record not found"
            });

            return new OkObjectResult(new
            {
                Success = true,
                Message = "Success get payment record",
                payment
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(PaymentItem data)
        {
            if (ModelState.IsValid)
            {
                await _context.Payments.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAllPayment", new { data.Id }, data);
            }

            return new JsonResult("Something went wrong!") { StatusCode = 500 };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, PaymentItem data)
        {
            if (id != data.Id)
            {
                return new BadRequestObjectResult(new
                {
                    success = false,
                    Message = "Payload and URL doesn't match"
                });
            }

            var dataExist = await _context.Payments.FirstOrDefaultAsync(x => x.Id == id);

            if (dataExist == null)
            {
                return new NotFoundObjectResult(new
                {
                    success = false,
                    Message = "No payment record found"
                });
            }

            dataExist.cardNumber = data.cardNumber;
            dataExist.cardOwnerName = data.cardOwnerName;
            dataExist.expirationDate = data.expirationDate;
            dataExist.securityCode = data.securityCode;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var dataExist = await _context.Payments.FirstOrDefaultAsync(x => x.Id == id);

            if (dataExist == null)
            {
                return new NotFoundObjectResult(new
                {
                    success = false,
                    Message = "No payment record found"
                });
            }

            _context.Payments.Remove(dataExist);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new {
                success = true,
                message = "Success delete payment record",
                dataExist
            });
        }
    }
}
