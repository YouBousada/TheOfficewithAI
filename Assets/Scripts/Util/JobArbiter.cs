using System.Collections.Generic;

public static class JobArbiter
{
    public static bool ApproveJobSwitch(World world, CharacterName character, JobName toJob)
    {
        var jobCounts = new Dictionary<JobName, int>();
        JobName currentJob = JobName.None;

        foreach (Animatronic anim in world.animatronicDictionary.Values)
        {
            var job = anim.GetCurrentJobName();
            jobCounts[job] = jobCounts.GetValueOrDefault(job, 0) + 1;

            if (anim.GetCharacterName() == character)
                currentJob = job;
        }

        if (currentJob == toJob) return false;

        int windowCount = jobCounts.GetValueOrDefault(JobName.WindowWatcher, 0);

        if (currentJob == JobName.WindowWatcher && toJob != JobName.WindowWatcher && windowCount <= 1)
            return false;

        if (currentJob != JobName.WindowWatcher && toJob == JobName.WindowWatcher && windowCount >= 2)
            return false;

        return true;
    }
}
