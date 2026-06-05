using System.Collections.Generic;

public static class JobArbiter
{
    public static bool ApproveJobSwitch(AnimatronicRegistry registry, CharacterName character, JobName toJob)
    {
        var jobCounts = new Dictionary<JobName, int>();
        JobName currentJob = JobName.NONE;

        foreach (Animatronic anim in registry.Animatronics)
        {
            var job = anim.GetCurrentJobName();
            jobCounts[job] = jobCounts.GetValueOrDefault(job, 0) + 1;

            if (anim.GetCharacterName() == character)
                currentJob = job;
        }

        if (currentJob == toJob) return false;

        int windowCount = jobCounts.GetValueOrDefault(JobName.WINDOW_WATCHER, 0);

        if (currentJob == JobName.WINDOW_WATCHER && toJob != JobName.WINDOW_WATCHER && windowCount <= 1)
            return false;

        if (currentJob != JobName.WINDOW_WATCHER && toJob == JobName.WINDOW_WATCHER && windowCount >= 2)
            return false;

        return true;
    }
}
