namespace BSDigital.Queries
{
    public static class OrderBookSnapshotQueries
    {
        public static string GetHistoricalDataByTimestamp = @"
            SELECT * 
            FROM ""ORDER_BOOK_SNAPSHOT"" 
            WHERE ""CODE"" = {0}
            AND ""CREATED_ON"" BETWEEN {1} - INTERVAL '1 hour' AND {1} + INTERVAL '1 hour'
            ORDER BY ABS(EXTRACT(EPOCH FROM (""CREATED_ON"" - {1})))
            LIMIT 1";
    }
}
