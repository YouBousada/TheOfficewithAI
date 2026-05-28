public static class JobFactory
{
    public static BaseJob Create(JobName jobName, JobNecessities necessities, bool switchedFromAnotherJob = false)
    {
        return jobName switch
        {
            JobName.FREEROAMER => new Freeroamer(necessities, switchedFromAnotherJob),
            JobName.WAIT_AND_RUSHER => new WaitAndRusher(necessities, switchedFromAnotherJob),
            JobName.WINDOW_WATCHER => new WindowWatcher(necessities, switchedFromAnotherJob),
            _ => throw new System.ArgumentOutOfRangeException(nameof(jobName), jobName, null)
        };
    }
}
