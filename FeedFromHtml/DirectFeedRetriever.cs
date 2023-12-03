using System.Text;
using System.Xml;
using HtmlAgilityPack;

namespace FeedFromHtml;

public class DirectFeedRetriever : IFeedRetriever, IDisposable
{
    private readonly MemoryStream memoryStream = new();

    public long ByteCount => memoryStream.Length;

    public void Dispose()
    {
        memoryStream.Dispose();
    }

    public void Retrieve(FeedConfig feedConfig)
    {
        memoryStream.Position = 0;
        memoryStream.SetLength(0);

        using (XmlWriter xml = XmlWriter.Create(memoryStream, new XmlWriterSettings
        {
            CloseOutput = false,
            Encoding = Encoding.UTF8,
            Indent = true
        }))
        {
            xml.WriteStartDocument();

            xml.WriteStartElement("rss");
            xml.WriteAttributeString("version", "2.0");

            xml.WriteStartElement("channel");

            xml.WriteElementString("title", feedConfig.Title);
            xml.WriteElementString("link", feedConfig.Url);
            xml.WriteElementString("description", feedConfig.Description);
            xml.WriteElementString("language", feedConfig.Language);
            xml.WriteElementString("ttl", feedConfig.Ttl.ToString());
            xml.WriteStartElement("image");
            xml.WriteElementString("url", feedConfig.ImageUrl);
            xml.WriteElementString("title", feedConfig.Title);
            xml.WriteElementString("link", feedConfig.Url);
            xml.WriteEndElement(); //image

            HtmlDocument htmlDoc = (new HtmlWeb()).Load(feedConfig.Url);
            HtmlNodeCollection articleNodes = htmlDoc.DocumentNode.SelectNodes(feedConfig.XPathArticlesContainer);
            if (null == articleNodes)
            {
                throw new ApplicationException($"XPathArticlesContainer ({feedConfig.XPathArticlesContainer}) not found");
            }

            foreach (HtmlNode articleNode in articleNodes)
            {
                HtmlNode node;

                xml.WriteStartElement("item");

                node = articleNode.SelectSingleNode(feedConfig.XPathTitleContainer);
                if (null == node)
                {
                    throw new ApplicationException($"XPathTitleContainer ({feedConfig.XPathTitleContainer}) not found");
                }
                xml.WriteStartElement("title");
                xml.WriteCData(node.InnerText.Trim());
                xml.WriteEndElement();

                node = articleNode.SelectSingleNode(feedConfig.XPathHrefContainer);
                if (null == node)
                {
                    throw new ApplicationException($"XPathHrefContainer ({feedConfig.XPathHrefContainer}) not found");
                }
                xml.WriteElementString("link", node.Attributes["href"].Value.Trim());
                xml.WriteElementString("guid", node.Attributes["href"].Value.Trim());
                
                using (StringWriter sw = new())
                {
                    foreach (string xPathDescriptionComponent in feedConfig.XPathDescriptionComponents)
                    {
                        node = articleNode.SelectSingleNode(xPathDescriptionComponent);
                        if (null == node)
                        {
                            throw new ApplicationException($"XPathDescriptionComponent ({xPathDescriptionComponent}) not found");
                        }
                        sw.WriteLine(node.OuterHtml);
                    }

                    xml.WriteStartElement("description");
                    xml.WriteCData(sw.ToString());
                    xml.WriteEndElement();
                }

                xml.WriteEndElement(); //item
            }

            xml.WriteEndElement(); //channel
            xml.WriteEndElement(); //rss
            xml.WriteEndDocument();
        }

        memoryStream.Position = 0;
    }

    public async void Write(Stream stream)
    {
        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(stream);
        memoryStream.Position = 0;
    }
}