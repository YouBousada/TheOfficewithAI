using UnityEngine;

public class Services : MonoBehaviour
{
    public MapService mapService { get; private set; }
    public TimeGenerator timeGenerator { get; private set; }
    public JobArbiter jobArbiter  { get; private set; }
    void Awake()
    {
        mapService = new MapService();
        timeGenerator = new TimeGenerator();
        jobArbiter = new JobArbiter();
    }

}



