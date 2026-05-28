using System.Collections.Generic;

public class JobArbiter
{
    private readonly AnimatronicRegistry animatronicRegistry;

    public JobArbiter(AnimatronicRegistry animatronicRegistry)
    {
        this.animatronicRegistry = animatronicRegistry;
    }

    public bool ApproveJobSwitch(CharacterName character, JobName toJob)
    {
        var jobCounts = new Dictionary<JobName, int>();
        JobName currentJob = JobName.NONE;

        foreach (Animatronic anim in animatronicRegistry.Animatronics)
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
