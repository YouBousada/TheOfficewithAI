public static class JobFactory
{
    public static BaseJob Create(JobName jobName, JobNecessities necessities, bool switchedFromAnotherJob = false)
    {
        return jobName switch
        {
            JobName.Freeroamer => new Freeroamer(necessities, switchedFromAnotherJob),
            JobName.WaitAndRusher => new WaitAndRusher(necessities, switchedFromAnotherJob),
            JobName.WindowWatcher => new WindowWatcher(necessities, switchedFromAnotherJob),
            _ => throw new System.ArgumentOutOfRangeException(nameof(jobName), jobName, null)
        };
    }
}
