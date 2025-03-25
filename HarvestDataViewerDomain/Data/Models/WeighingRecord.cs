namespace HarvestDataViewerDomain.Data.Models;

public class WeighingRecord
{
  public int Id { get; init; }
  public string? CarNumber { get; init; }
  public int? GrossWeight { get; init; }
  public int? TareWeight { get; init; }
  public int? NetWeight => GrossWeight - TareWeight;
  public DateTime? TareDate { get; init; }
  public DateTime? GrossDate { get; init; }
}