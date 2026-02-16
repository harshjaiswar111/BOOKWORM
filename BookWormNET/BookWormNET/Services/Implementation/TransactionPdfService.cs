//using BookWormNET.Models;
//using iText.IO.Font.Constants;
//using iText.Kernel.Font;
//using iText.Kernel.Pdf;
//using iText.Layout.Element;
//using iText.Layout.Properties;
//using iText.Layout;


//namespace BookWormNET.Services.Implementation
//{
//    public class TransactionPdfService
//    {
//        public byte[] GenerateInvoice(Transaction transaction,
//                                      List<TransactionItem> items)
//        {
//            using var stream = new MemoryStream();

//            PdfWriter writer = new PdfWriter(stream);
//            PdfDocument pdf = new PdfDocument(writer);
//            Document document = new Document(pdf);

//            PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
//            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

//            // Title
//            document.Add(new Paragraph("Bookworm - Transaction Invoice")
//                .SetFont(titleFont)
//                .SetFontSize(18)
//                .SetTextAlignment(TextAlignment.CENTER));

//            document.Add(new Paragraph(" "));

//            // Transaction Details
//            document.Add(new Paragraph($"Transaction ID: {transaction.TransactionId}")
//                .SetFont(normalFont));

//            document.Add(new Paragraph($"User: {transaction.User.UserName}")
//                .SetFont(normalFont));

//            document.Add(new Paragraph($"Type: {transaction.TransactionType}")
//                .SetFont(normalFont));

//            document.Add(new Paragraph($"Status: {transaction.Status}")
//                .SetFont(normalFont));

//            document.Add(new Paragraph($"Date: {transaction.CreatedAt}")
//                .SetFont(normalFont));

//            document.Add(new Paragraph(" "));

//            // Table (4 columns)
//            Table table = new Table(4).UseAllAvailableWidth();

//            table.AddHeaderCell("Book");
//            table.AddHeaderCell("Price");
//            table.AddHeaderCell("Qty");
//            table.AddHeaderCell("Total");

//            foreach (var item in items)
//            {
//                decimal lineTotal = (item!.Price ?? 0m) * (item.Quantity ?? 0);

//                table.AddCell(item.Product?.ProductName ?? "N/A");
//                table.AddCell("₹" + (item.Price ?? 0m));
//                table.AddCell(item.Quantity.ToString());
//                table.AddCell("₹" + lineTotal);
//            }


//            document.Add(table);

//            document.Add(new Paragraph(" "));
//            document.Add(new Paragraph("Total Amount: ₹" + transaction.TotalAmount)
//                .SetFont(titleFont));

//            document.Close();

//            return stream.ToArray();
//        }
//    }
//}
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;


namespace BookWormNET.Services.Implementation
{
    public class TransactionPdfService
    {
        public byte[] GenerateInvoice(Transaction transaction, List<TransactionItem> items)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (items == null) throw new ArgumentNullException(nameof(items));

            using var ms = new MemoryStream();

            Document document = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            // 🎨 Colors
            BaseColor primary = new BaseColor(44, 62, 80);   // Charcoal
            BaseColor secondary = new BaseColor(127, 140, 141);
            BaseColor border = new BaseColor(200, 200, 200);

            // 🔤 Fonts
            Font titleFont = FontFactory.GetFont("Helvetica", 22, Font.BOLD, primary);
            Font headerFont = FontFactory.GetFont("Helvetica", 10, Font.BOLD, BaseColor.WHITE);
            Font normalFont = FontFactory.GetFont("Helvetica", 10, Font.NORMAL, primary);
            Font boldFont = FontFactory.GetFont("Helvetica", 10, Font.BOLD, primary);
            Font smallFont = FontFactory.GetFont("Helvetica", 9, Font.NORMAL, secondary);
            Font totalFont = FontFactory.GetFont("Helvetica", 13, Font.BOLD, primary);

            // =========================
            // 1️⃣ HEADER
            // =========================
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 60, 40 });

            PdfPCell brandCell = new PdfPCell(new Phrase("BOOKWORM", titleFont));
            brandCell.Border = Rectangle.NO_BORDER;
            brandCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            headerTable.AddCell(brandCell);

            PdfPCell invoiceCell = new PdfPCell();
            invoiceCell.Border = Rectangle.NO_BORDER;
            invoiceCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            invoiceCell.AddElement(new Paragraph("DIGITAL BOOK STORE", smallFont));
            invoiceCell.AddElement(new Paragraph("INVOICE", boldFont));
            headerTable.AddCell(invoiceCell);

            document.Add(headerTable);
            document.Add(new LineSeparator());
            document.Add(new Paragraph(" "));

            // =========================
            // 2️⃣ TRANSACTION DETAILS
            // =========================
            PdfPTable infoTable = new PdfPTable(2);
            infoTable.WidthPercentage = 100;
            infoTable.SetWidths(new float[] { 60, 40 });

            PdfPCell userCell = new PdfPCell();
            userCell.Border = Rectangle.NO_BORDER;
            userCell.AddElement(new Paragraph("BILL TO", smallFont));
            userCell.AddElement(new Paragraph(transaction.User?.UserName ?? "N/A", boldFont));
            userCell.AddElement(new Paragraph(transaction.User?.UserEmail ?? "N/A", normalFont));
            infoTable.AddCell(userCell);

            PdfPCell txnCell = new PdfPCell();
            txnCell.Border = Rectangle.NO_BORDER;
            txnCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            AddInfo(txnCell, "Transaction ID:", transaction.TransactionId.ToString(), smallFont, boldFont);
            AddInfo(txnCell, "Type:", transaction.TransactionType.ToString(), smallFont, normalFont);
            AddInfo(txnCell, "Status:", transaction.Status.ToString(), smallFont, normalFont);
            AddInfo(txnCell, "Date:", transaction.CreatedAt?.ToString("dd-MMM-yyyy") ?? "N/A", smallFont, normalFont);

            infoTable.AddCell(txnCell);
            document.Add(infoTable);
            document.Add(new Paragraph(" "));

            // =========================
            // 3️⃣ ITEMS TABLE
            // =========================
            PdfPTable itemTable = new PdfPTable(5);
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 40, 15, 10, 15, 20 });
            itemTable.HeaderRows = 1;

            AddHeader(itemTable, "BOOK", headerFont, primary);
            AddHeader(itemTable, "PRICE", headerFont, primary);
            AddHeader(itemTable, "QTY", headerFont, primary);
            AddHeader(itemTable, "TYPE", headerFont, primary);
            AddHeader(itemTable, "TOTAL", headerFont, primary);

            decimal grandTotal = 0;

            foreach (var item in items)
            {
                decimal price = item.Price ?? 0m;
                int qty = item.Quantity ?? 0;
                decimal total = price * qty;
                grandTotal += total;

                AddCell(itemTable, item.Product?.ProductName ?? "N/A", normalFont, border);
                AddCell(itemTable, "₹" + price.ToString("N2"), normalFont, border, Element.ALIGN_RIGHT);
                AddCell(itemTable, qty.ToString(), normalFont, border, Element.ALIGN_CENTER);
                AddCell(itemTable, transaction.TransactionType.ToString(), normalFont, border);
                AddCell(itemTable, "₹" + total.ToString("N2"), boldFont, border, Element.ALIGN_RIGHT);
            }

            document.Add(itemTable);
            document.Add(new Paragraph(" "));

            // =========================
            // 4️⃣ TOTAL SUMMARY
            // =========================
            PdfPTable totalTable = new PdfPTable(2);
            totalTable.WidthPercentage = 40;
            totalTable.HorizontalAlignment = Element.ALIGN_RIGHT;

            AddSummary(totalTable, "TOTAL AMOUNT:", "₹" + grandTotal.ToString("N2"), totalFont);

            document.Add(totalTable);

            // =========================
            // FOOTER
            // =========================
            document.Add(new Paragraph(" "));
            document.Add(new LineSeparator());
            Paragraph footer = new Paragraph(
                "Thank you for purchasing from BookWorm. Happy Reading!",
                FontFactory.GetFont("Helvetica", 9, Font.ITALIC, secondary)
            );
            footer.Alignment = Element.ALIGN_CENTER;
            document.Add(footer);

            document.Close();
            return ms.ToArray();
        }

        // =========================
        // 🔧 Helper Methods
        // =========================
        private void AddInfo(PdfPCell cell, string label, string value, Font labelFont, Font valueFont)
        {
            Paragraph p = new Paragraph();
            p.Add(new Chunk(label + " ", labelFont));
            p.Add(new Chunk(value, valueFont));
            cell.AddElement(p);
        }

        private void AddHeader(PdfPTable table, string text, Font font, BaseColor bg)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.BackgroundColor = bg;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Padding = 8;
            cell.Border = Rectangle.NO_BORDER;
            table.AddCell(cell);
        }

        private void AddCell(
            PdfPTable table,
            string text,
            Font font,
            BaseColor borderColor,
            int alignment = Element.ALIGN_LEFT)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.Padding = 8;
            cell.BorderColor = borderColor;
            cell.HorizontalAlignment = alignment;
            table.AddCell(cell);
        }

        private void AddSummary(PdfPTable table, string label, string value, Font font)
        {
            PdfPCell left = new PdfPCell(new Phrase(label, font));
            left.Border = Rectangle.NO_BORDER;
            table.AddCell(left);

            PdfPCell right = new PdfPCell(new Phrase(value, font));
            right.Border = Rectangle.NO_BORDER;
            right.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(right);
        }
    }
}
