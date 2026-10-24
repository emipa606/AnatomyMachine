# GitHub Copilot Instructions for Anatomy Machine (Continued)

## Mod Overview and Purpose
Anatomy Machine (Continued) is an update to the LingLuos mod, which provides a machine that can dismember creatures based on various needs. The primary purpose of this mod is to add a dismemberment feature to RimWorld, allowing players to efficiently remove body parts from creatures for various purposes such as resource gathering or scientific research.

## Key Features and Systems
### Building_Dismemberment
- **BodyParts**: A collection of body parts that can be dismembered.
- **boPi**: A property or variable related to the dismemberment process.
- **ExposeData**: A method for exposing data during serialization.
- **GetGizmos**: Returns a list of gizmos for the building.
- **GetInspectString**: Provides an inspection string for the building.
- **ModeCBodyguards**: A method or property related to bodyguard modes.
- **Pan**: A method or property related to the pan or mechanism of the dismemberment process.

### ChooseBodypartWindow
- **ChooseBodypartWindow**: Constructor for the body part selection window.
- **DoWindowContents**: Draws the contents of the window.
- **InitialSize**: Sets the initial size of the window.

### DismembermentTieTu
A class related to the dismemberment process, possibly managing the ties or bonds between body parts.

### KeyValuePairsA
- **Key**: A key property.
- **SetValue**: A method to set a value.
- **Value**: A value property.

### WorkMode
A class or enum representing different work modes for the dismemberment machine.

### XML Integration
The mod integrates with the RimWorld XML def system to define new buildings, research projects, and other mod elements. Key XML files include:

- **Buildings_Misc.xml**: Defines the dismemberment machine building.
- **Research.xml**: Defines the research project required to unlock the dismemberment machine.

## Harmony Patching
This mod likely uses Harmony, a popular modding framework for RimWorld, to patch existing game code. Harmony allows developers to modify game behavior without altering the original codebase.

## Coding Patterns and Conventions
- **File Naming**: C# files are named using PascalCase with descriptive names.
- **Method Naming**: Methods are named using PascalCase, and are descriptive of their functionality.
- **Property Naming**: Properties are named using PascalCase.
- **XML Defs**: XML files are named in a way that reflects their content, and follow the RimWorld XML def conventions.

## Suggestions for Copilot
- **Provide XML Examples**: When generating XML code, ensure it follows the RimWorld XML def conventions and includes the necessary attributes and elements.
- **Integrate Harmony Patches**: When suggesting Harmony patches, provide a clear explanation of the patch's purpose and the specific game code it modifies.
- **Maintain Naming Conventions**: Ensure that generated code adheres to the established naming conventions used in the mod.
- **Document Changes**: When suggesting changes to existing code, include comments explaining the changes and their purpose.

By following these guidelines, GitHub Copilot can provide valuable assistance in developing and maintaining the Anatomy Machine (Continued) mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

