using UnityEngine;

public class Services : MonoBehaviour
{
    public MapService MapService { get; private set; }
    public TimeGenerator TimeGenerator { get; private set; }
    public JobArbiter JobArbiter  { get; private set; }
    void Awake()
    {
        MapService = new MapService();
        TimeGenerator = new TimeGenerator();
        JobArbiter = new JobArbiter();
    }

}



