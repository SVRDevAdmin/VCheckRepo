using QuestPDF.Infrastructure;

namespace VCheckViewer.Lib.Interface
{
    public interface IDocument
    {
        DocumentMetadata GetMetdata();
        DocumentSettings GetSettings();
        void Compose(IDocumentContainer container);
    }
}
