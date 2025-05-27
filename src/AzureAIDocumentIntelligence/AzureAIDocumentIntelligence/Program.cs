using Azure;
using Azure.AI.DocumentIntelligence;
using AzureAIDocumentIntelligence;

string endpoint = "https://riki-document-ai.cognitiveservices.azure.com/";
string key = "KEY";
var layoutFileUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/sample-layout.pdf";
var invoiceFileUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/sample-invoice.pdf";

AzureKeyCredential cred = new AzureKeyCredential(key);
DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(endpoint), cred);


LayoutDocument layoutDocument = new LayoutDocument(layoutFileUrl);
await layoutDocument.ReadDocument(client);

InvoiceDocument invoiceDocument = new InvoiceDocument(invoiceFileUrl);
await invoiceDocument.ReadDocument(client);


