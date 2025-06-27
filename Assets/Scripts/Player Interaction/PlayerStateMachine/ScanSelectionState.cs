using UnityEngine;

public class ScanSelectionState : PlayerControllerInteractionState
{
    public override void OnStateEnter()
    {
        // set up hand menu
        refs.scanAddObjectsMenu.gameObject.SetActive(true);
        // Hookup to the menu callbacks state change to this menu?
        refs.scanAddObjectsMenu.OnAddFurnitureClickedCallback += OnMenuAddFurnitureClicked;
        refs.scanAddObjectsMenu.OnAddWallClickedCallback += OnMenuCreateWallClicked;
        refs.scanAddObjectsMenu.OnDeleteWallsClickedCallback += OnDeleteWallsClicked;
    }

    public override void OnStateExit()
    {
        runtimeData.hoveredOverFurniture?.OnHoverEnd();
        runtimeData.hoveredOverFurniture = null;

        // loose  hand menu
        refs.scanAddObjectsMenu.gameObject.SetActive(false);
        refs.scanAddObjectsMenu.OnAddFurnitureClickedCallback -= OnMenuAddFurnitureClicked;
        refs.scanAddObjectsMenu.OnAddWallClickedCallback -= OnMenuCreateWallClicked;
        refs.scanAddObjectsMenu.OnDeleteWallsClickedCallback -= OnDeleteWallsClicked;

        refs.inGameMenu.HideMenu();
    }

    public override void UpdateState()
    {
        HandleRightHandRay(RaycastType.HitBothPriorityOnUi);
        HandleRayVisuals();

        HandleUiInteraction();
        HandleHoverOverFurniture();


        // If we press the select button on a furniture we select it and enter the scan edit state
        if (HandleFurnitureSelect())
        {
            sm.scanEditDetails.SetCurrentFurniture(runtimeData.selectedFurniture as ScanModeFurniture);
            sm.SetState(sm.scanEditDetails);
        }

        HandleInGameMenuEnableByButton();

    }

    void OnMenuAddFurnitureClicked()
    {
        sm.SetState(sm.scanRegisterNew);
    }

    void OnMenuCreateWallClicked()
    {
        sm.SetState(sm.scanCreateWall);
    }

    void OnDeleteWallsClicked()
    {
        Debug.Log("[Walls] on Delete clicked");

        // very hacked but works
        WallPlacedByPlayer[] walls = GameObject.FindObjectsByType<WallPlacedByPlayer>(FindObjectsSortMode.None);

        for (int i = 0; i < walls.Length; i++)
        {
            GameObject.Destroy(walls[i].gameObject);
        }

    }
}