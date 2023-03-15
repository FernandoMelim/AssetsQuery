using System.Net;

namespace Domain.DTOs;

public class AssetDataDTO
{
    public ChartData Chart { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public class ChartData
    {
        public ResultData[] Result { get; set; }

        public ErrorDTO Error { get; set; }
    }

    public class ResultData
    {
        public long[] Timestamp { get; set; }

        public IndicatorsData Indicators { get; set; }
    }

    public class IndicatorsData
    {
        public QuoteData[] Quote { get; set; }
    }

    public class QuoteData
    {
        public decimal?[] Open { get; set; }
    }
}


