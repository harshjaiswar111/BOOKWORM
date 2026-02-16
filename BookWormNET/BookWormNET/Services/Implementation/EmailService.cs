using BookWormNET.dto;
using BookWormNET.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BookWormNET.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public void SendTransactionSuccessEmail(
            string toEmail,
            Transaction transaction,
            byte[] invoicePdf)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _settings.SenderName,
                _settings.SenderEmail));

            message.To.Add(MailboxAddress.Parse(toEmail));

            message.Subject = "Bookworm - Transaction Successful";

            // Email body (TEXT)
            var body = new TextPart("plain")
            {
                Text =
                    $"Hello {transaction.User.UserName},\n\n" +
                    $"Your transaction was successful.\n\n" +
                    $"Transaction ID: {transaction.TransactionId}\n" +
                    $"Amount: ₹{transaction.TotalAmount}\n\n" +
                    $"Thank you for shopping with Bookworm!"
            };

            // PDF attachment
            var attachment = new MimePart("application", "pdf")
            {
                Content = new MimeContent(
                    new MemoryStream(invoicePdf)),
                ContentDisposition = new ContentDisposition(
                    ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = $"invoice_{transaction.TransactionId}.pdf"
            };

            // Combine body + attachment
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            message.Body = multipart;

            using var client = new SmtpClient();
            client.Connect(
                _settings.SmtpServer,
                _settings.Port,
                SecureSocketOptions.StartTls);

            client.Authenticate(
                _settings.Username,
                _settings.Password);

            client.Send(message);
            client.Disconnect(true);
        }
    }
}
