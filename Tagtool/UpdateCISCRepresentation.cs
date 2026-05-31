using (var stream = Cache.OpenCacheReadWrite())
{
    foreach (var tag in Cache.TagCache.FindAllInGroup<TagTool.Tags.Definitions.CinematicScene>())
    {
        Console.WriteLine("Checking: " + tag);
        var cisc = Cache.Deserialize<TagTool.Tags.Definitions.CinematicScene>(stream, tag);
        foreach (var cinematic_object in cisc.Objects){
            if (cinematic_object.PuppetObject != null && !cinematic_object.ImportName.StartsWith("player")){
                if(cinematic_object.PuppetObject.Name.Equals("objects\\characters\\masterchief\\masterchief")){
                    cinematic_object.Flags |= (TagTool.Tags.Definitions.CinematicScene.ObjectBlock.ObjectFlags)(1 << 8);
                    Console.WriteLine("Updating flag for: " + cinematic_object.ImportName);
                }
                
                if(cinematic_object.PuppetObject.Name.Equals("objects\\characters\\dervish\\dervish")){
                    cinematic_object.Flags |= (TagTool.Tags.Definitions.CinematicScene.ObjectBlock.ObjectFlags)(1 << 9);
                    Console.WriteLine("Updating flag for: " + cinematic_object.ImportName);
                }
            }
        }
        Cache.Serialize(stream, tag, cisc);
    }
}