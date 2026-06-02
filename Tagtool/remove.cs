// DeleteUnicStrings.cs  
// Usage: CS < "DeleteUnicStrings.cs" <substring>  
// Must be run from inside a unic tag editing context (EditTag <tag>)  
  
using System;  
using System.Collections.Generic;  
using System.Linq;  
using TagTool.Cache;  
using TagTool.Common;  
using TagTool.Tags.Definitions;  
  
if (Args.Count < 1)  
{  
    Console.WriteLine("Usage: CS < \"DeleteUnicStrings.cs\" <substring>");  
    return;  
}  
  
var substring = Args[0];  
var unic = (MultilingualUnicodeStringList)Definition;  
var languages = Enum.GetValues(typeof(GameLanguage)).Cast<GameLanguage>().ToArray();  
  
// Collect all LocalizedString blocks where any language value OR the StringIDStr contains the substring  
var toRemove = new List<LocalizedString>();  
foreach (var entry in unic.Strings)  
{  
    bool matches = entry.StringIDStr != null &&  
                   entry.StringIDStr.Contains(substring, StringComparison.OrdinalIgnoreCase);  
  
    if (!matches)  
    {  
        foreach (var lang in languages)  
        {  
            var value = unic.GetString(entry, lang);  
            if (value != null && value.Contains(substring, StringComparison.OrdinalIgnoreCase))  
            {  
                matches = true;  
                break;  
            }  
        }  
    }  
  
    if (matches)  
        toRemove.Add(entry);  
}  
  
Console.WriteLine($"Found {toRemove.Count} matching string block(s) for substring \"{substring}\".");  
  
if (toRemove.Count == 0)  
    return;  
  
// For each match: null out all 12 language data entries (this keeps Data offsets consistent),  
// then remove the block from the Strings list  
foreach (var entry in toRemove)  
{  
    foreach (var lang in languages)  
        unic.SetString(entry, lang, null);  
  
    unic.Strings.Remove(entry);  
    Console.WriteLine($"  Removed: [{entry.StringIDStr}]");  
}  
  
// Write the modified definition back to the cache  
using (var stream = Cache.OpenCacheReadWrite())  
    Cache.Serialize(stream, Tag, unic);  
  
Console.WriteLine("Done. Tag saved.");