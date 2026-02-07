namespace BSDigital.Queries
{
    public static class OrderBookSnapshotQueries
    {
        public static string GetHistoricalDataByTimestamp = @"
            SELECT * 
            FROM ""ORDER_BOOK_SNAPSHOT"" 
            WHERE ""CODE"" = {0}
            ORDER BY ABS(EXTRACT(EPOCH FROM (""CREATED_ON"" - {1})))
            LIMIT 1";
    }
}
