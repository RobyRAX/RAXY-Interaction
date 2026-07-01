# RAXY Interaction System

RAXY Interaction System provides a lightweight interaction foundation for Unity projects: scan nearby interactables, cycle selection, and trigger interactions via UnityEvents.

## Features

- **Interactable** — target component with tag, `OnScanEnter`, `OnScanExit`, and `OnInteracted` UnityEvents
- **Interactor** — periodic sphere scan, selection cycling, and `Interact()` dispatch
- **IInteractableTagProvider** — optional editor tag dropdown source for `Interactable`

## Setup

1. Add `Interactable` to objects the player can interact with.
2. Add `Interactor` to the player (or scanner object).
3. Configure `scanRange`, `interactableLayer`, and `scanDelay` on the interactor.
4. Subscribe to `Interactor.OnInteractableUpdated` for UI feedback.
5. Call `Interactor.Interact()` from input to trigger the selected target.

## Dependencies

- **Odin Inspector** (project plugin) — editor attributes on `Interactable` and `Interactor`; runtime works without Odin if attributes are stripped

## Notes

Game-specific UI bridges and tag providers (e.g. `GameConfigSO`) should live in your project, not in this package.
