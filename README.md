# Frontier Dispatch - DSA Unity Laboratory

A deliberately simple top-down/logistics sandbox used throughout the Data Structures and Algorithms course.

## Why this project exists

The course uses three layers:

1. **Reason** - paper, diagrams, group discussion.
2. **Implement** - headless C#/.NET labs and tests.
3. **Apply** - Unity visualization/integration/profiling.

The Unity layer is not required for the assessed algorithmic objectives. The reusable core under `Assets/DSA/Core` has **no UnityEngine dependency** and is also compiled by the standalone .NET solution under `Standalone/`.

## Game premise

You operate a frontier dispatch network. Outposts are connected by roads with travel costs. Couriers receive jobs, route through the world, and interact with entities identified by IDs.

This naturally supports the course topics:

- stacks: undo route/editor actions;
- queues: dispatch jobs;
- hashing: outpost/entity lookup by ID;
- trees: region/mission hierarchy;
- priority queues: urgent jobs and pathfinding frontier;
- graphs: road network;
- Dijkstra/A*: route planning;
- profiling: larger worlds, repeated queries, allocations, loading/preparation.

## Initial teacher setup

1. Create/open a normal Unity 6 project.
2. Copy this package's `Assets/DSA` folder into the project.
3. Let Unity compile.
4. Run **Tools > DSA > Build Course Scenes** once.
5. Press Play in `Assets/DSA/Scenes/Launcher.unity`.
6. Verify the standalone solution separately with `dotnet build/test` on a machine with .NET 10.
7. Commit the generated scenes and `.meta` files to the course Unity repository.

The distributed student repository should contain the generated scenes already. Students should not need to run the builder.
