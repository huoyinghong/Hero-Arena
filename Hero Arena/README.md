# Clash League: Hero Arena

A 3D card battle prototype built with Unity. Drag cards onto the battlefield to deploy units or cast spells, then destroy the opposing towers and base.

## Project overview

- **Unity version:** 6000.2.13f1
- **Primary language:** C#
- **Battle mode:** A local player battle against enemies spawned by the scene. The gameplay scripts do not include an online multiplayer flow.

## Getting started

1. Add the project root folder in Unity Hub and open it with **Unity 6000.2.13f1**.
2. Wait for Unity to import the assets and resolve dependencies from `Packages/manifest.json`.
3. Start Play mode from `Assets/Scenes/StartScene.unity`. The startup scene proceeds to the main menu; select the battle button there to enter the arena.

The following scenes are enabled in the build scene list, in this order:

1. `Assets/Scenes/StartScene.unity`
2. `Assets/Scenes/MainScene.unity`
3. `Assets/Scenes/GameScene.unity`

## Scene flow

| Scene       | Purpose |
| `StartScene`| Plays the opening screen and music, then loads the main menu. |
| `MainScene` | Shows the card collection, battle deck, and chest interface; cards can be moved between the deck and collection before entering battle. |
| `GameScene` | Contains the battlefield, both teams' buildings, cards, gold, timer, and end-game UI. |

## Gameplay

- The main menu starts with cards 1–8 in the battle deck and card 9 in the collection. The card panel lets you move cards between those lists.
- The battlefield shows **four cards in hand** plus a preview of the next card. Cards are drawn randomly from IDs 1–10, with no duplicate among the cards currently shown.
- Drag a card upward with the mouse or pointer to preview its unit model or spell range. Drop it on the battlefield plane to use it.
- Unit cards can only be deployed in the allowed area. Spell cards can be cast anywhere on the battlefield. Using a card spends gold and draws a replacement.
- Gold starts at 0, regenerates at 1 per second, and is capped at 10. Card costs are listed below.
- The battle UI counts down from **3:00**. The two enemy spawn points first create units after about 5 seconds, then each spawn one random unit every 15 seconds.
- Each side has one base and two side towers. Destroying a side tower opens that team's base to attack. Destroying a base triggers the win or loss screen.

### Card and unit IDs

| ID| Name        | Cost | Description |
| 1 | Elf Archer | 3 | Ranged unit. |
| 2 | Healing Angel | 4 | Attacks at range and periodically creates a healing aura. |
| 3 | Three Headed Wolf | 6 | Melee unit. |
| 4 | Fallen Angel | 6 | Fast-moving combat unit. |
| 5 | Lava Monster | 8 | High-health combat unit. |
| 6 | Archer Brothers | 5 | Ranged unit made up of multiple characters. |
| 7 | Bear | 7 | Melee unit. |
| 8 | Death | 6 | Combat unit that periodically summons skeletons. |
| 9 | Plaguebringer | 4 | Damage-over-time effect that can be placed anywhere. |
| 10 | Fireball | 4 | Area attack with a target point that can be placed anywhere. |
| 11 | Skeletons | 0 | Unit summoned by Death and other gameplay logic. |
| 12 | Healing Aura | 0 | Healing effect created by healing gameplay logic. |
| 13 | Building | 0 | Unit type used by the battlefield's defensive buildings. |

> IDs 11–13 are internal unit IDs. They are not part of the 10 cards randomly drawn during battle.

## Code map

The main gameplay scripts are in `Assets/Scripts/`:

| Script | Responsibility |
| `GameManager.cs` | Keeps card lists alive between scenes during a play session and plays music and sound effects. |
| `GameController.cs` | Initializes unit data, manages gold and time, creates units, and selects building targets. |
| `Unit.cs`, `Character.cs`, `Building.cs` | Unit movement, target selection, attacks, damage, death, and building behavior. |
| `Card.cs`, `UIManager.cs` | Battlefield card dragging and deployment, hand refill, gold display, and end-game UI. |
| `MainUIManager.cs`, `BtnCard.cs` | Main-menu card panel, moving cards between lists, and scene transitions. |
| `MagicFire.cs`, `MagicFog.cs` | Fireball and damage-over-time effects. |
| `HealingAngel.cs`, `Death.cs`, `EnemySpawner.cs` | Healing aura creation, skeleton summoning, and automatic enemy spawning. |
| `HPSlider.cs`, `UnitAnimationEvent.cs`, `Bullet.cs`, `Effect.cs` | Health bars, animation events, projectiles, and effect lifetimes. |

## Assets and configuration

```text
Assets/
├── Scenes/                   Startup, main-menu, and battle scenes
├── Scripts/                  Gameplay scripts
├── Resources/Prefabs/        Runtime-loaded unit and UI prefabs
├── GameResources/
│   ├── Characters/           Character models, materials, and animations
│   ├── Effects/              Battle effects, materials, and shaders
│   ├── Sprites/              Cards, scene art, and UI images
│   ├── AudioClips/           Music and sound effects
│   ├── Items/                Item assets
│   └── Fonts/                Fonts
├── Animation/                Animation assets
├── Animator/                 Animator controllers
├── Settings/                 Rendering assets
├── DOTween_1_2_705/          DOTween package included in the project
└── _Recovery/                Unity recovery scene files
Packages/                     Unity package manifest and lock file
ProjectSettings/              Unity editor and build settings
```

Numbered unit prefabs are in `Assets/Resources/Prefabs/Units/`. Unity `.meta` files store asset GUIDs; keep them alongside assets when moving or copying files within the project.

Package dependencies are listed in `Packages/manifest.json`. They include AI Navigation, Input System, UGUI, Universal RP, Timeline, and glTFast. DOTween is included as a project asset.

## Current implementation notes

- The main-menu card lists live in `GameManager` and persist through scene changes with `DontDestroyOnLoad`. The gameplay code does not include a save system that persists them between application launches.
- The battle draw logic currently chooses randomly from IDs 1–10 without reading the battle deck list. As a result, changing the deck in the main menu does not currently affect cards drawn in battle.
- The timer counts down and updates the UI, but the current scripts do not trigger a separate end-game result when it reaches zero. Win or loss is triggered by destroying a base.
- The chest flow plays an animation and displays a random card. The current script does not add that card to the collection list.
