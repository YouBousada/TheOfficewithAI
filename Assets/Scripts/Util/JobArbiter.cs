using System.Collections.Generic;

public class JobArbiter
{
    public bool ApproveJobSwitch(World world, CharacterName character, JobName toJob)
    {

        var currentAssignments = new Dictionary<CharacterName, JobName>();

        var JobCounts = new Dictionary<JobName, int>();

        for(int i = 0; i < world.animatronicList.Count; i++)
        {
            var anim = world.animatronicList[i];
            currentAssignments[anim.GetCharacterName()] = anim.GetCurrentJobName();

            var job = anim.GetCurrentJobName();
            JobCounts[job] = JobCounts.GetValueOrDefault(job, 0) + 1;

        }

        var currentJob = currentAssignments[character];

        int windowCount = JobCounts.GetValueOrDefault(JobName.WindowWatcher, 0);
        // int rusherCount = JobCounts.GetValueOrDefault(JobName.WaitAndRusher, 0);

        if(currentJob == toJob)
        {
            return false;
        }

        if (currentJob == JobName.WindowWatcher && toJob != JobName.WindowWatcher && windowCount <= 1)
            return false;

        // if (currentJob == JobName.WaitAndRusher && toJob != JobName.WaitAndRusher && rusherCount <= 1)
        //     return false;

        if (currentJob != JobName.WindowWatcher && toJob == JobName.WindowWatcher && windowCount >= 2)
            return false;

        return true;
    }
}