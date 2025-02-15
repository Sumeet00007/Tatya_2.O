using UnityEngine;

public class Combo1_PM : MonoBehaviour
{
    [Header("Puzzle Cubes")]
    [SerializeField] CubeRotation[] cubes; // References to the three cubes

    [Header("Drawer System")]
    [SerializeField] DrawerSlide drawer; // Reference to the drawer

    private bool puzzleSolved = false;

    void OnEnable()
    {
        CubeRotation.OnCubeRotated += CheckPuzzleCompletion;
    }

    void OnDisable()
    {
        CubeRotation.OnCubeRotated -= CheckPuzzleCompletion;
    }

    void CheckPuzzleCompletion()
    {
        if (puzzleSolved) return; // Prevent multiple triggers

        foreach (var cube in cubes)
        {
            if (!cube.IsCorrectlyAligned())
                return; // If one cube is incorrect, puzzle isn't solved
        }

        // If all cubes are correct, unlock and open the drawer
        puzzleSolved = true;
        drawer.UnlockDrawer();
    }
}
