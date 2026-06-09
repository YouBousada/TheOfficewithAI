using System.Collections.Generic;

public static class JobArbiter
{
    public static bool ApproveJobSwitch(AnimatronicRegistry registry, CharacterName characterName, JobName toJob)
    {
        var jobCounts = new Dictionary<JobName, int>();
        JobName currentJob = JobName.NONE;

        foreach (Animatronic anim in registry.animatronicList)
        {
            var job = anim.GetCurrentJobName();
            jobCounts[job] = jobCounts.GetValueOrDefault(job, 0) + 1;

            if (anim.GetCharacterName() == characterName)
                currentJob = job;
        }

        if (currentJob == toJob) return false;

        int windowCount = jobCounts.GetValueOrDefault(JobName.WINDOW_WATCHER, 0);

        if (currentJob == JobName.WINDOW_WATCHER && toJob != JobName.WINDOW_WATCHER && windowCount <= 1)
            return false;

        if (currentJob != JobName.WINDOW_WATCHER && toJob == JobName.WINDOW_WATCHER && windowCount >= 2)
            return false;

        RoomName currentLoc = registry.animatronicDict[characterName].GetCurrentLocation();
        
        if (toJob == JobName.FREEROAMER &&
            ((currentLoc == RoomName.WEST_DOOR && registry.AnimatronicsInThisRoom(RoomName.WEST_HALL_FAR) > 1) ||
             (currentLoc == RoomName.EAST_DOOR && registry.AnimatronicsInThisRoom(RoomName.EAST_HALL_FAR) > 1)))
            return false;

        return true;
    }
}
