using UnityEngine;

public class TheaterRun : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Transform door1;
    [SerializeField] Transform door2;
    [SerializeField] Transform door3;
    [SerializeField] GameObject ghostModel1;
    [SerializeField] Transform slamDoor1CheckPoint;
    [SerializeField] GameObject ghostModel2;
    [SerializeField] Transform slamDoor2CheckPoint;
    [SerializeField] GameObject ghostModel3;
    [SerializeField] Transform slamDoor3CheckPoint;
    [SerializeField] GameObject saltBag;
    [SerializeField] float doorPartialOpenAngel;
    [SerializeField] float closeDoor1Distance;
    [SerializeField] float closeDoor2Distance;
    [SerializeField] float closeDoor3Distance;
    [SerializeField] AudioClip doorSlamSound;
    [SerializeField] GameObject runningGhost;
    [SerializeField] Transform spawnRunningGhostCheckPoint;
    [SerializeField] float runningGhostTriggerDistance;

    Vector3 saltBagOriginalPosition;
    Animator runningGhostAnimator;
    bool doorsOpened = false;
    bool endDoor1JS = false;
    bool endDoor2JS = false;
    bool endDoor3JS = false;
    bool checkToDestroy = false;

    void Start()
    {
        saltBagOriginalPosition = saltBag.transform.position;
        runningGhostAnimator = runningGhost.GetComponent<Animator>();
    }

    void Update()
    {
        if (saltBag.transform.position != saltBagOriginalPosition && !doorsOpened)
        {
            OpenDoorsAndPlaceGhost();
            doorsOpened = true;
        }

        if (doorsOpened)
        {
            if (Vector3.Distance(slamDoor1CheckPoint.position, player.transform.position) < closeDoor1Distance && !endDoor1JS)
            {
                CloseDoor1();
            }

            if (Vector3.Distance(slamDoor2CheckPoint.position, player.transform.position) < closeDoor2Distance && !endDoor2JS)
            {
                CloseDoor2();
            }

            if (Vector3.Distance(slamDoor3CheckPoint.position, player.transform.position) < closeDoor3Distance && !endDoor3JS)
            {
                CloseDoor3();
            }
        }

        if (Vector3.Distance(spawnRunningGhostCheckPoint.position, player.transform.position) < runningGhostTriggerDistance && endDoor3JS)
        {
            runningGhost.SetActive(true);
            checkToDestroy = true;
        }

        if (checkToDestroy)
        {
            CheckIfCanDestroy();
        }
    }

    void OpenDoorsAndPlaceGhost()
    {
        door1.localRotation = Quaternion.Euler(0, 160, 0);
        door2.localRotation = Quaternion.Euler(0, -20, 0);
        door3.localRotation = Quaternion.Euler(0, -20, 0);
        ghostModel1.SetActive(true);
        ghostModel2.SetActive(true);
        ghostModel3.SetActive(true);
    }

    void CloseDoor1()
    {
        door1.localRotation = Quaternion.Euler(0, 180, 0);
        ghostModel1.SetActive(false);
        AudioSource.PlayClipAtPoint(doorSlamSound, door1.position);
        door1.GetComponentInChildren<DoorOpener>().isOpen = false;
        endDoor1JS = true;
    }

    void CloseDoor2()
    {
        door2.localRotation = Quaternion.Euler(0, 0, 0);
        ghostModel2.SetActive(false);
        AudioSource.PlayClipAtPoint(doorSlamSound, door2.position);
        door2.GetComponentInChildren<DoorOpener>().isOpen = false;
        endDoor2JS = true;
    }

    void CloseDoor3()
    {
        door3.localRotation = Quaternion.Euler(0, 0, 0);
        ghostModel3.SetActive(false);
        AudioSource.PlayClipAtPoint(doorSlamSound, door3.position);
        door3.GetComponentInChildren<DoorOpener>().isOpen = false;
        endDoor3JS = true;
    }

    void CheckIfCanDestroy()
    {
        AnimatorStateInfo info = runningGhostAnimator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= 1f)
        {
            Debug.Log("Animation finished!");
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(slamDoor1CheckPoint.position, closeDoor1Distance);
        Gizmos.DrawWireSphere(slamDoor2CheckPoint.position, closeDoor2Distance);
        Gizmos.DrawWireSphere(slamDoor3CheckPoint.position, closeDoor3Distance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnRunningGhostCheckPoint.position, runningGhostTriggerDistance);
    }
}