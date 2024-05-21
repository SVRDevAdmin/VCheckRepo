using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCheckViewer.Lib.Interface
{
    public interface IDocument
    {
        DocumentMetadata GetMetdata();
        DocumentSettings GetSettings();
        void Compose(IDocumentContainer container);
    }
}
