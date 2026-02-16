package com.example.Services;

import com.example.models.LibraryPackagePurchase;
import com.example.models.LibraryPackagePurchaseItem;
import com.lowagie.text.*;
import com.lowagie.text.pdf.*;

import org.springframework.stereotype.Service;

import java.io.ByteArrayOutputStream;

@Service
public class LibraryInvoicePdfService {

    public byte[] generateLibraryInvoice(LibraryPackagePurchase purchase) {

        ByteArrayOutputStream out = new ByteArrayOutputStream();

        Document document = new Document(PageSize.A4);
        PdfWriter.getInstance(document, out);
        document.open();

        Font titleFont = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 18);
        Font normalFont = FontFactory.getFont(FontFactory.HELVETICA, 12);
        Font boldFont = FontFactory.getFont(FontFactory.HELVETICA_BOLD, 12);

        // ===== HEADER =====
        document.add(new Paragraph("Bookworm - Library Package Invoice", titleFont));
        document.add(new Paragraph(" "));

        document.add(new Paragraph(
                "Invoice ID: LIB-" + purchase.getPurchaseId(), normalFont));
        document.add(new Paragraph(
                "Transaction ID: " + purchase.getTransaction().getTransactionId(),
                normalFont));
        document.add(new Paragraph(
                "User: " + purchase.getUser().getUserName(),
                normalFont));
        document.add(new Paragraph(
                "Purchase Date: " + purchase.getPurchaseDate(),
                normalFont));

        document.add(new Paragraph(" "));

        // ===== PACKAGE DETAILS =====
        document.add(new Paragraph("Package Details", boldFont));
        document.add(new Paragraph(
                "Package Name: " + purchase.getLibraryPackage().getName(),
                normalFont));
        document.add(new Paragraph(
                "Validity: " + purchase.getLibraryPackage().getValidityDays() + " days",
                normalFont));
        document.add(new Paragraph(
                "Book Limit: " + purchase.getAllowedBooks(),
                normalFont));

        document.add(new Paragraph(" "));

        // ===== BOOK TABLE =====
        PdfPTable table = new PdfPTable(3);
        table.setWidthPercentage(100);

        table.addCell("Book");
        table.addCell("Avg Price");
        table.addCell("Royalty");

        for (LibraryPackagePurchaseItem item : purchase.getItems()) {
            table.addCell(item.getProduct().getProductName());
            table.addCell("₹" + purchase.getAvgBookPrice());
            table.addCell("₹" + item.getRoyaltyAmount());
        }

        document.add(table);

        document.add(new Paragraph(" "));
        document.add(new Paragraph(
                "Total Paid: ₹" + purchase.getPackagePrice(),
                titleFont));

        document.close();
        return out.toByteArray();
    }
}
