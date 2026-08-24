using System;
using System.Text.Json;
using Consumer.Data;
using Consumer.Models;

namespace Consumer.Services;

public class ProcessorService
{
    private readonly AppDbContext _context;
    public ProcessorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ProcessReportAsync(
        string json)
    {
        var report = JsonSerializer
            .Deserialize<Report>(json);

        if (report == null)
            return false;

        AssetStatus? created = report.AssetType switch
        {
            "UAV" => CreateFromUAV(report),
            "PerimeterSensor" => CreateFromSensor(report),
            _ => null
        };

        if (created == null)
            return false;

        var existReport = await _context.AssetLiveStatus.FindAsync(report.AssetId);
        if (existReport == null)
        {
            _context.Set<AssetStatus>().Add(created);
        }
        else
        {
            existReport.AssetType = created.AssetType;
            existReport.RawValue = created.RawValue;
            existReport.ProcessedStatus = created.ProcessedStatus;
            existReport.IsVerified = created.IsVerified;
            existReport.LastUpdate = created.LastUpdate;
        }
        

        await _context.SaveChangesAsync();

        return true;
    }



    public AssetStatus CreateFromUAV(Report report)
    {
        string processedStatus = "Warning";
        bool isVerified = false;

        var success = int.TryParse(
            report.RawValue, out int result);

        if (success)
        {
            if (result >= 20 && result <= 100)
            {
                processedStatus = "Stable";
                isVerified = true;
            }
            if (result >= 0 && result <= 19)
            {
                processedStatus = "Warning";
                isVerified = true;
            }
            
        }

        return new AssetStatus()
        {
            AssetId = report.AssetId,
            AssetType = report.AssetType,
            RawValue = report.RawValue,
            ProcessedStatus = processedStatus,
            IsVerified = isVerified,
            LastUpdate = report.Timestamp
        };
    }


    public AssetStatus CreateFromSensor(Report report)
    {
        string processedStatus = "Warning";
        bool isVerified = false;
        List<string> goodKeywords = new() { "GOOD", "Good", "good", "gud" };

        if (goodKeywords.Contains(report.RawValue))
        {
            processedStatus = "Stable";
            isVerified = true;
        }

        List<string> badKeywords = new() { "Bad", "BAD", "bad", "bed" };
        if (badKeywords.Contains(report.RawValue))
        {
            processedStatus = "Warning";
            isVerified = true;
        }

        return new AssetStatus()
        {
            AssetId = report.AssetId,
            AssetType = report.AssetType,
            RawValue = report.RawValue,
            ProcessedStatus = processedStatus,
            IsVerified = isVerified,
            LastUpdate = report.Timestamp
        };
    }
}
