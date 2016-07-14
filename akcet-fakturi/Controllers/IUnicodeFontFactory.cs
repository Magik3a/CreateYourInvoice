using iTextSharp.text;

namespace akcet_fakturi.Controllers
{
    public interface IUnicodeFontFactory
    {
        Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached);
    }
}