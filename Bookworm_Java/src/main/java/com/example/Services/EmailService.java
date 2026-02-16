package com.example.Services;

import org.springframework.core.io.ByteArrayResource;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.stereotype.Service;

import com.example.models.Transaction;

import jakarta.mail.internet.MimeMessage;

@Service
public class EmailService {

    private final JavaMailSender mailSender;

    public EmailService(JavaMailSender mailSender) {
        this.mailSender = mailSender;
    }

    public void sendTransactionSuccessEmail(
            String toEmail,
            Transaction transaction,
            byte[] invoicePdf) {

        try {
            MimeMessage message = mailSender.createMimeMessage();
            MimeMessageHelper helper =
                    new MimeMessageHelper(message, true);

            helper.setTo(toEmail);
            helper.setSubject("Bookworm - Transaction Successful");
            helper.setText(
                    "Hello " + transaction.getUser().getUserName() + ",\n\n" +
                    "Your transaction was successful.\n\n" +
                    "Transaction ID: " + transaction.getTransactionId() + "\n" +
                    "Amount: ₹" + transaction.getTotalAmount() + "\n\n" +
                    "Thank you for shopping with Bookworm!",
                    false
            );

            helper.addAttachment(
                    "invoice_" + transaction.getTransactionId() + ".pdf",
                    new ByteArrayResource(invoicePdf)
            );

            mailSender.send(message);

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}