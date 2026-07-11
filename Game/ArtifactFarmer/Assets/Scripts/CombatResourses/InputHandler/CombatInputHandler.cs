using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatInputHandler : IDisposable
{
    private GameControls controls;
    private Camera camera;
    public event Action<Unit> OnTargetSelected;

    public CombatInputHandler()
    {
        camera = Camera.main;
        controls = new GameControls();
        controls.Combat.SelectTarget.performed += HandleTargetSelected;
    }

    public void StartListening() => controls.Combat.Enable();
    public void StopListening() => controls.Combat.Disable();

    private void HandleTargetSelected(InputAction.CallbackContext ctx)
    {
        Unit unit = ResolveUnderCursor();
        if (unit != null) OnTargetSelected?.Invoke(unit);
    }

    private Unit ResolveUnderCursor()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray ray = camera.ScreenPointToRay(screenPos);

        return Physics.Raycast(ray, out RaycastHit hit) ? hit.collider.GetComponent<Unit>() : null;
    }

    public void Dispose()
    {
        controls.Combat.SelectTarget.performed -= HandleTargetSelected;
        controls.Dispose();
    }
}