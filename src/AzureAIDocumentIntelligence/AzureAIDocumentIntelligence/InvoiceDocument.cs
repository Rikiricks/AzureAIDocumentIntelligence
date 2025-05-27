using Azure;
using Azure.AI.DocumentIntelligence;

namespace AzureAIDocumentIntelligence
{
    public class InvoiceDocument
    {
        private readonly string _fileUrl;
        public InvoiceDocument(string fileUrl)
        {
            _fileUrl = fileUrl;
        }
        public async Task ReadDocument(DocumentIntelligenceClient client)
        {
            Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", new Uri(_fileUrl));
            AnalyzeResult result = operation.Value;
            for (int i = 0; i < result.Documents.Count; i++)
            {
                Console.WriteLine($"Document {i}:");

                AnalyzedDocument document = result.Documents[i];

                if (document.Fields.TryGetValue("VendorName", out DocumentField vendorNameField)
                    && vendorNameField.FieldType == DocumentFieldType.String)
                {
                    string vendorName = vendorNameField.ValueString;
                    Console.WriteLine($"Vendor Name: '{vendorName}', with confidence {vendorNameField.Confidence}");
                }

                if (document.Fields.TryGetValue("CustomerName", out DocumentField customerNameField)
                    && customerNameField.FieldType == DocumentFieldType.String)
                {
                    string customerName = customerNameField.ValueString;
                    Console.WriteLine($"Customer Name: '{customerName}', with confidence {customerNameField.Confidence}");
                }

                if (document.Fields.TryGetValue("Items", out DocumentField itemsField)
                    && itemsField.FieldType == DocumentFieldType.List)
                {
                    foreach (DocumentField itemField in itemsField.ValueList)
                    {
                        Console.WriteLine("Item:");

                        if (itemField.FieldType == DocumentFieldType.Dictionary)
                        {
                            IReadOnlyDictionary<string, DocumentField> itemFields = itemField.ValueDictionary;

                            if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField)
                                && itemDescriptionField.FieldType == DocumentFieldType.String)
                            {
                                string itemDescription = itemDescriptionField.ValueString;
                                Console.WriteLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                            }

                            if (itemFields.TryGetValue("Amount", out DocumentField itemAmountField)
                                && itemAmountField.FieldType == DocumentFieldType.Currency)
                            {
                                CurrencyValue itemAmount = itemAmountField.ValueCurrency;
                                Console.WriteLine($"  Amount: '{itemAmount.CurrencySymbol}{itemAmount.Amount}', with confidence {itemAmountField.Confidence}");
                            }
                        }
                    }
                }

                if (document.Fields.TryGetValue("SubTotal", out DocumentField subTotalField)
                    && subTotalField.FieldType == DocumentFieldType.Currency)
                {
                    CurrencyValue subTotal = subTotalField.ValueCurrency;
                    Console.WriteLine($"Sub Total: '{subTotal.CurrencySymbol}{subTotal.Amount}', with confidence {subTotalField.Confidence}");
                }

                if (document.Fields.TryGetValue("TotalTax", out DocumentField totalTaxField)
                    && totalTaxField.FieldType == DocumentFieldType.Currency)
                {
                    CurrencyValue totalTax = totalTaxField.ValueCurrency;
                    Console.WriteLine($"Total Tax: '{totalTax.CurrencySymbol}{totalTax.Amount}', with confidence {totalTaxField.Confidence}");
                }

                if (document.Fields.TryGetValue("InvoiceTotal", out DocumentField invoiceTotalField)
                    && invoiceTotalField.FieldType == DocumentFieldType.Currency)
                {
                    CurrencyValue invoiceTotal = invoiceTotalField.ValueCurrency;
                    Console.WriteLine($"Invoice Total: '{invoiceTotal.CurrencySymbol}{invoiceTotal.Amount}', with confidence {invoiceTotalField.Confidence}");
                }
            }
        }
    }
}
