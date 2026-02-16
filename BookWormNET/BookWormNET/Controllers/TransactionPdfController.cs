using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Implementation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/invoice")]
    [EnableCors("AllowFrontend")] // optional
    public class PdfController : ControllerBase
    {
        private readonly BookwormDbContext _context;
        private readonly TransactionPdfService _pdfService;

        public PdfController(BookwormDbContext context,
                             TransactionPdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        [HttpGet("{transactionId}")]
        public IActionResult DownloadInvoice(int transactionId)
        {
            // 1️⃣ Fetch transaction (like transactionRepo.findById)
            var transaction = _context.Transactions
                .Include(t => t.User)
                .FirstOrDefault(t => t.TransactionId == transactionId);

            if (transaction == null)
                return NotFound("Transaction not found");

            // 2️⃣ Status check (same as Java)
            if (transaction.Status != TransactionStatus.SUCCESS)
                return BadRequest("Invoice available only after success");

            // 3️⃣ Fetch items (like itemRepo.findByTransaction)
            var items = _context.TransactionItems
                .Include(i => i.Product)
                .Where(i => i.Transaction.TransactionId == transactionId)
                .ToList();

            // 4️⃣ Generate PDF
            byte[] pdf = _pdfService.GenerateInvoice(transaction, items);

            // 5️⃣ Return PDF (ResponseEntity<byte[]> equivalent)
            return File(
                pdf,
                "application/pdf",
                $"invoice_{transactionId}.pdf"
            );
        }
    }
}
