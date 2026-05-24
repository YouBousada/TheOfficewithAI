using System.Collections.Generic;

public class JobArbiter
{
    private readonly GameContext gameContext;

    public JobArbiter(GameContext gameContext)
    {
        this.gameContext = gameContext;
    }

    public bool ApproveJobSwitch(CharacterName character, JobName toJob)
    {
        var jobCounts = new Dictionary<JobName, int>();
        JobName currentJob = JobName.None;

        foreach (Animatronic anim in gameContext.Animatronics)
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
