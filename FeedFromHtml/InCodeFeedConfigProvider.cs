using System.Text.Json;

namespace FeedFromHtml;

public class InCodeFeedConfigProvider : IFeedConfigProvider
{
    private readonly string[] FEED_CONFIG_STRINGS =
    [
        """
            {
                "FeedId": "rtpa",
                "Enabled": true,
                "Title": "RTP Açores",
                "Url": "https://acores.rtp.pt/informacao/",
                "Description": "Notícias locais veículadas pela RTP Açores",
                "Language": "pt-pt",
                "Ttl": 240,
                "ImageUrl": "https://cdn-images.rtp.pt/common/img/channels/logos/color/horizontal/rtpacores.png",
                "XPathArticlesContainer": "(//div[contains(@class,'section-item')])",
                "XPathTitleContainer": "a/h2",
                "XPathHrefContainer": "a[@href and h2]",
                "XPathDescriptionComponents": [
                    "a/div/img"
                ]
            }
        """,
        """
            {
                "FeedId": "rl",
                "Enabled": true,
                "Title": "Rádio Lumena",
                "Url": "https://radiolumena.com/category/local/",
                "Description": "Notícias locais veículadas pela Rádio Lumena",
                "Language": "pt-pt",
                "Ttl": 240,
                "ImageUrl": "https://radiolumena.com/wp-content/uploads/2023/07/RADIO-LUMENA_TOP_SITE.png",
                "XPathArticlesContainer": "(//article[contains(@class,'post')])",
                "XPathTitleContainer": "div/h3[contains(@class,'news-title')]/a[@href]",
                "XPathHrefContainer": "div/h3[contains(@class,'news-title')]/a[@href]",
                "XPathDescriptionComponents": [
                    "figure/a/img",
                    "div/div[contains(@class,'news-block-content')]"
                ]
            }
        """
    ];

    private readonly ILogger<InCodeFeedConfigProvider> logger;
    private readonly Dictionary<string, FeedConfig> feedConfigs = [];

    public FeedConfig[] EnabledFeeds => [.. feedConfigs.Values.Where(feedConfig => feedConfig.Enabled).OrderBy(feedConfig => feedConfig.Title)];

    public InCodeFeedConfigProvider(ILogger<InCodeFeedConfigProvider> _logger)
    {
        logger = _logger;

        logger.LogInformation("Initializing");

        foreach (string feedConfigString in FEED_CONFIG_STRINGS)
        {
            try
            {
                FeedConfig? feedConfig = JsonSerializer.Deserialize<FeedConfig>(feedConfigString);

                if (null == feedConfig)
                {
                    throw new ApplicationException("Couldn't deserialize JSON");
                }

                feedConfigs.Add(feedConfig.FeedId, feedConfig);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to read {feedConfigString}: {ex.Message}", feedConfigString, ex.Message);
            }
        }

        logger.LogInformation("Initialized with {feedConfigs.Count} feed configs", feedConfigs.Count);
    }

    public FeedConfig? GetFeedConfig(string feedId)
    {
        if (false == feedConfigs.ContainsKey(feedId) || false == feedConfigs[feedId].Enabled)
        {
            return null;
        }

        return feedConfigs[feedId];
    }
}