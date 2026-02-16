package com.example.Services;

import com.example.models.Transaction;
import com.example.models.TransactionItem;
import com.lowagie.text.*;
import com.lowagie.text.pdf.*;

import org.springframework.stereotype.Service;

import java.io.ByteArrayOutputStream;
import java.math.BigDecimal;
import java.util.List;

@Service
public class TransactionPdfService {

    public byte[] generateInvoice(Transaction transaction,
                                  List<TransactionItem> items) {

        ByteArrayOutputStream out = new ByteArrayOutputStream();

        Document document = new Document(PageSize.A4);
        PdfWriter.getInstance(document, out);

        document.open();

        Font titleFont = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 18);
        Font normalFont = FontFactory.getFont(FontFactory.HELVETICA, 12);

        // Title
        document.add(new Paragraph("Bookworm - Transaction Invoice", titleFont));
        document.add(new Paragraph(" "));

        // Transaction details
        document.add(new Paragraph("Transaction ID: " + transaction.getTransactionId(), normalFont));
        document.add(new Paragraph("User: " + transaction.getUser().getUserName(), normalFont));
        document.add(new Paragraph("Type: " + transaction.getTransactionType(), normalFont));
        document.add(new Paragraph("Status: " + transaction.getStatus(), normalFont));
        document.add(new Paragraph("Date: " + transaction.getCreatedAt(), normalFont));
        document.add(new Paragraph(" "));

        // Table
        PdfPTable table = new PdfPTable(4);
        table.setWidthPercentage(100);

        table.addCell("Book");
        table.addCell("Price");
        table.addCell("Qty");
        table.addCell("Total");

        for (TransactionItem item : items) {
            table.addCell(item.getProduct().getProductName());
            table.addCell("₹" + item.getPrice());
            table.addCell(String.valueOf(item.getQuantity()));
            //table.addCell("₹" + (item.getPrice() * item.getQuantity()));
            BigDecimal lineTotal =
                    item.getPrice().multiply(BigDecimal.valueOf(item.getQuantity()));

            table.addCell("₹" + lineTotal);

        }

        document.add(table);

        document.add(new Paragraph(" "));
        document.add(new Paragraph("Total Amount: ₹" + transaction.getTotalAmount(), titleFont));

        document.close();

        return out.toByteArray();
    }
}