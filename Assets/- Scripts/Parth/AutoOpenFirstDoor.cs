using UnityEngine;

public class AutoOpenFirstDoor : MonoBehaviour
{
    [SerializeField] TatyaDoor tatyaDoor;

    bool fuseBoxCompleted = false;
    bool brokenFuseGiven = false;

    void Update()
    {
        if (fuseBoxCompleted && brokenFuseGiven)
        {
            tatyaDoor.EnableScript();
        }
    }

    public void FuseGivenToTatya()
    {
        brokenFuseGiven = true;
    }

    public void FuseBoxCompleted()
    {
        fuseBoxCompleted = true;
    }
}
