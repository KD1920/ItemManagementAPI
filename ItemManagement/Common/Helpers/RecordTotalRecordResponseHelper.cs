namespace ItemManagement.Common.Helpers;

public class RecordTotalRecordResponseHelper<T>
{
    public IEnumerable<T> Records { get; set; } = new List<T>();
    public int TotalRecords { get; set; }
}