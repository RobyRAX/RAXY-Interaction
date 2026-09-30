# Interaction Sample

Ready-to-play scene for `Interactor` and `Interactable`: scan nearby targets, show a prompt list, and trigger the selected one.

## Contents

| Asset | Role |
|-------|------|
| `InteractionSample.unity` | Player scanner, three props, and the prompt list |
| `Prefabs/SampleInteractableRow.prefab` | One prompt row |
| `Scripts/SamplePlayerMover.cs` | WASD movement |
| `Scripts/SampleInteractInput.cs` | E interact, Tab cycle |
| `Scripts/SampleInteractableListUI.cs` | Spawns one row per scanned tag |
| `Scripts/SampleInteractableRowUI.cs` | Selects that row and calls `Interact()` |
| `Scripts/SampleInteractableFeedback.cs` | Color and world label on scan and interact |

## Controls

| Input | Action |
|-------|--------|
| WASD | Move |
| Tab | Cycle selection |
| E or click a row | Interact with the selected target |

## Setup

1. Package Manager → **RAXY Interaction System** → Samples → **Import** Interaction Sample.
2. Open `InteractionSample.unity` → Play.

Requires TextMeshPro, the Input System, and URP. Props and the scanner mask use layer 12 (named Interactable in Project Alice).
