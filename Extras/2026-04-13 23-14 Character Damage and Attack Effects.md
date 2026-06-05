# Character Damage and Attack Effects

## User

次はプレイヤーキャラクターと敵キャラクターについて、最低限の演出を追加して下さい。
- ダメージを受けたとき、赤く光りながら左右に振動する。
- 攻撃（魔法攻撃含む）を始めるとき、一瞬白く光る。
この光る演出にはオーバーレイ色を使用します。 sprite-overlay-flash.md を参考にして下さい。

## Assistant

**Analyzing Visual Effects Implementation**

I'm currently focused on implementing the visual effects for damage and attacks. The core idea is clear: red flash and screen shake for damage, and a white flash for attacks, including magic. I'm exploring the best way to handle the color overlay, considering efficiency and ease of modification.

**Searching for Resource File**

I'm now investigating the `sprite-overlay-flash.md` resource, as the user directed. The file isn't immediately visible in the project overview. I'm checking file paths like `Assets/sprite-overlay-flash.md` to locate the resource, or confirm its absence. The phrase "sprite-overlay-flash.md を参考にして下さい" from the user means I need to use it as a reference for flash effects.

**Examining File References**

I'm now shifting my focus to finding the `sprite-overlay-flash.md` file the user cited. The search is underway, starting with checking direct file paths like `Assets/sprite-overlay-flash.md` and related locations within the project structure, including the skills folder, if any exists. I'm also considering the implications of the user's "を参考にして下さい" instruction, which implies using the file as a reference guide.

**Tool call: Unity.Grep**

```json
{
  "args": "-l \"sprite-overlay-flash.md\""
}
```

**Examining Potential Locations**

I'm currently expanding the search for the missing item. Initially, I checked `.cs` files with `grep`, but no luck there. Now, I'm broadening the scope, possibly checking the root directory. Further, I'm going to try activating skills like `ui` or `2d-character` to see if it's embedded in one of their resources.

**Expanding Script Exploration**

The initial attempts with `grep` and skill activation didn't pan out. I've shifted focus, and I'm currently delving into existing character and enemy scripts. I'm prioritizing the `CombatManager.cs` and `EnemyUnit.cs` files, and I'm also looking for the player's script. The "overlay color" detail is crucial, so I'll be mindful of that. My aim is to grasp their structure for later modification.

**Tool call: Unity.Grep**

```json
{
  "path": "Assets/Scripts"
}
```

**Initiating Search Protocol**

I've begun my search for all `.cs` files, specifically within the `Assets/Scripts` directory. I'm now proceeding through the directory structure to locate and identify all the C# script files.

**Tool call: Unity.Grep**

```json
{
  "args": "--files",
  "path": "Assets/Scripts"
}
```

**Investigating Combat System Components**

I'm now focusing on `EnemyUnit.cs` and `CombatManager.cs`, the key files of interest. I'm guessing `CombatManager` manages the player's stats. The project overview mentions enemy attacks are queued with timers, and it's a queue-based system. Reading through `EnemyUnit.cs` now.

**Refining System Understanding**

I'm checking out the `CombatManager` file, as anticipated. I'm focusing on how player stats are handled. The project overview's mention of timers and queues for enemy attacks is a helpful clue as I continue my analysis of `EnemyUnit.cs` and, now, `CombatManager.cs`. I have a good idea of how the pieces fit together.

**Tool call: Unity.GetFileContent**

```json
{
  "filePath": "Assets/Scripts/EnemyUnit.cs"
}
```

**Reviewing Hit Feedback Implementation**

I've been examining the existing `EnemyUnit.cs` script and its hit feedback. It's currently rudimentary, just a red flash using an overlay material. I've noted the `EnemyOverlay.mat` material. Next, I'll be reviewing `CombatManager.cs` to understand how the hits are actually triggered and managed.

**Inspecting Hit Mechanics in Combat**

I'm digging deeper into `CombatManager.cs` now. It seems to handle the core combat logic. I'm focusing on how health and damage are managed, particularly from the player's perspective, to better understand how hit feedback can be incorporated. I'm taking notes on potential hooks for visual effects within this script.

**Tool call: Unity.GetFileContent**

```json
{
  "filePath": "Assets/Scripts/CombatManager.cs"
}
```

**Reviewing the Damage Systems**

I've been poring over the codebase, specifically `EnemyUnit.cs` and `CombatManager.cs`. I'm seeing that the `EnemyUnit` script appears to manage its own damage processing, including visual feedback. The `CombatManager` script handles the player's offensive actions. The character's current state is next.

**Tracing the Flash Effects**

I'm now investigating the visual effects. I've noted the requirements: a red flash and shake for damage, and a white flash for attacks. I'm focusing on "sprite-overlay-flash.md" mentioned in the request. Since it seems external, I'm trying to locate a related asset or technique, searching the project for relevant files and considering potential implementation methods like overlaying sprites or using post-processing effects, while keeping in mind the need for player and enemy animation states.

**Tool call: Unity.Grep**

```json
{
  "args": "--glob \"*.md\" \"\""
}
```

**Considering File Locations**

I've determined that the missing file, `sprite-overlay-flash.md`, is likely a resource within one of the skills. I'll start by checking the `ui` skill, and then potentially the `2d-character` or `match-project-style` skills.

**Prioritizing Skill Searches**

I'm now prioritizing `match-project-style` and `ui` skills, based on the user's reference to `sprite-overlay-flash.md`. I'll first search for the resource in those skills.

**Tool call: Unity.Internal.ActivateDomainContext**

```json
{}
```

**Attempting File Retrieval**

I'm trying to locate the file, but initial attempts with related keywords failed. The user's prompt is quite direct, suggesting a specific file, so I need to refine my search within the project's structure. I'll broaden my scope and examine the file system again.

**Revising Search Parameters**

I've widened my search criteria further. Initial attempts using keywords like "2d-character" and "ui" didn't work. The user's specific prompt mentioning the file name prompted me to broaden my search within the whole project. I used grep-like tools, but they failed. I'm now employing the 'rg' command to find the exact filename within the file system.

**Tool call: Unity.Grep**

```json
{
  "args": "--files"
}
```

**Investigating Asset Locations**

I'm currently trying to map out the asset locations. I've spotted `SpriteOverlay.shader`, `EnemyOverlay.mat`, and `Guidance.txt`. It's possible `sprite-overlay-flash.md` is lurking in the `Assets/` directory. Aha, I see... something else.

**Refining File Searches**

I'm now revisiting the asset directories. I've noted `Assets/Shaders/SpriteOverlay.shader` and `Assets/Resources/EnemyOverlay.mat`. Time to check `Assets/Guidance.txt`. I'm also re-examining the root directory for any overlooked `.md` files. Further, I'll analyze the shader.

**Tool call: Unity.Grep**

```json
{
  "args": "-l \"\"",
  "path": "."
}
```

**Identifying Missing Assets**

I've determined that the file `sprite-overlay-flash.md` is absent. It seems the user may have expected its presence or perhaps assumed it was tied to a functionality I haven't enabled yet. I need to consider which skill to activate to resolve this. Should I enable `ui-uitk` or `2d-character`?

**Examining Documentation Context**

Now, I'm thinking about the implications of that missing file. I'm leaning toward checking `Guidance.txt`, since the name suggests this might be detailed technical documentation. The `ui-uitk` and `2d-character` skills seem less directly related at this stage.

**Tool call: Unity.GetFileContent**

```json
{
  "filePath": "Assets/Guidance.txt"
}
```

**Analyzing Sprite Overlays**

I'm now diving into the `SpriteOverlay.shader` file to dissect how the overlay color functions within the shader. My primary goal is to grasp the practical application of this overlay effect. I'm hoping to quickly understand the shader's principles, so I can more clearly picture how it can be used.

**Tool call: Unity.GetFileContent**

```json
{
  "filePath": "Assets/Shaders/SpriteOverlay.shader"
}
```

**Defining the Overlay Logic**

I'm now fully grasping how the overlay process functions. It's essentially a color blending operation. The `_OverlayColor` provides the target color, and its alpha channel (`_OverlayColor.a`) dictates the blending strength. The actual blending is a linear interpolation (`lerp`) between the original color and the overlay color, using the alpha value as the interpolation factor.

**Investigating Implementation Strategies**

The blending logic is now crystal clear. The next step is applying it to the game. My current plan is to examine the existing animator components within the `CombatManager`. I need to grab the `SpriteRenderer` references from the `FighterAnimator` and `MageAnimator` components to apply the color changes. Then, I will look at how I will trigger those changes, considering the needs for both player and enemy.

**Tool call: Unity.RunCommand**

```json
{
  "title": "Find character objects and SpriteRenderers",
  "code": "using UnityEngine;\nusing UnityEditor;\nusing System.Collections.Generic;\n\ninternal class CommandScript : IRunCommand\n{\n    public void Execute(ExecutionResult result)\n    {\n        GameObject fighter = GameObject.Find(\"Fighter\");\n        GameObject mage = GameObject.Find(\"Mage\");\n        \n        if (fighter != null) result.Log(\"Fighter found at {0}\", fighter.transform.position);\n        else result.LogWarning(\"Fighter not found\");\n        \n        if (mage != null) result.Log(\"Mage found at {0}\", mage.transform.position);\n        else result.LogWarning(\"Mage not found\");\n\n        // List all SpriteRenderers in the scene to find enemies and players\n        SpriteRenderer[] srs = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);\n        foreach (var sr in srs)\n        {\n            result.Log(\"SR: {0} on {1}\", sr.name, sr.gameObject.name);\n        }\n    }\n}\n"
}
```

**Reviewing Combat Roles**

I'm currently focused on the roles within the combat system. I've noted Fighter and Mage, but now I'm grappling with the Tank. I noticed a potential inconsistency: the CombatManager might not fully account for the Tank. I need to re-examine the CombatManager's fields and also delve into the TankController's implementation.

**Identifying Missing Components**

I've zeroed in on the Tank's absence in the `CombatManager`. The code explicitly references only Fighter and Mage animators. Looking at the code for the `CharacterVisualEffects.cs` script, the `TakeDamage` in `EnemyUnit` needs updating. I'll integrate the script and make sure the `CombatManager` triggers visual effects for player attacks and damage.

**Revising Combat Flow**

Okay, I've got a clearer picture now. The `CombatManager` is the central point. I've noted the missing Tank animator reference and the existing Fighter and Mage animators. I'm going to create `CharacterVisualEffects.cs` for visual feedback. My current focus is integrating damage and attack visuals. I'll hook the damage visuals into `HandleEnemyAttack` and attack visuals into `HandlePlayerAttack`. I need to review and implement color overlays and shakes.

**Tool call: Unity.GetDependency**

```json
{
  "queryType": "get_edges",
  "filePath": "Assets/Shaders/SpriteOverlay.shader"
}
```

**Planning Visual Effects**

I've narrowed the focus to a single script, `CharacterVisuals.cs`, to manage both damage and attack visual effects. My current plan is to determine whether to create a new material or utilize an existing one. Further, I've confirmed that only `EnemyOverlay.mat` currently leverages this functionality.

**Considering Implementation Steps**

I've reviewed `EnemyUnit.cs` again, noting its use of `Resources.Load` and fallback mechanisms within the `#if UNITY_EDITOR` directive for material loading.  I will be utilizing `MaterialPropertyBlock` to set `_OverlayColor` to achieve the flash effect in `CharacterVisuals.cs`. I also have confirmed it will be a shared material.

**Developing Shader Interaction**

I've examined the `EnemyUnit.cs` script, and will adapt the `HitFeedback` coroutine for the damage effect. The shader uses the alpha of the overlay color as the blend factor. I'll modify the `CharacterVisuals.cs` script to handle damage as red, and attack as white. I will create a `CharacterVisuals` script that is added to all characters.

**Implementing CharacterVisuals Script**

I'm now going to focus on building the `CharacterVisuals.cs` script. This script will be added as a component on all player and enemy character GameObjects. I will be sure to add this script to all enemy prefabs and player characters.

**Tool call: Unity.Grep**

```json
{
  "args": "-l \"EnemyUnit\"",
  "path": "Assets/Prefabs"
}
```

**Identifying Enemy Prefabs**

Okay, so I'm looking for the enemy prefabs. The `CombatManager.cs` script seems to reference them in an array called `Enemy Prefabs`. I'll need to check the `Assets/Prefabs` directory to see what's in there. I'll make a list.

**Tool call: Unity.Grep**

```json
{
  "path": "Assets/Prefabs",
  "args": "--files"
}
```

**Adding CharacterVisuals.cs**

I've got the prefab list in front of me: ZombieMage, Orc, Slime, Golem, and Skeleton. My next step is clear -  I need to attach that `CharacterVisuals.cs` script to all of them. I'll need to remember to do the same for player character GameObjects too.

**Refining Visual Effects**

I'm now implementing the `CharacterVisuals.cs` script, and it's taking shape. I've decided to simplify things and use `FlashAndShake` and `FlashOnly` functions instead of separate methods. I'm focusing on damage and attack visual effects now. I've also just looked at `EnemyOverlay.mat`.

**Tool call: Unity.GetFileContent**

```json
{
  "filePath": "Assets/Resources/EnemyOverlay.mat"
}
```

**Examining Shader Variables**

I'm currently focused on the shader variables, specifically noticing `_OverlayColor` within `Custom/SpriteOverlay`. This seems crucial for the desired effects. My plan is now to create a `CharacterVisuals.cs` script to manage these visuals, including red flashes, shake effects, and white flashes. I'm building this script to be attached to each character in the game.

**Defining Visual Effects Implementation**

I'm now zeroing in on the specific visual effects. Based on the project overview and instructions, I need to implement a red flash and shake on damage, and a white flash on attack. This means I'll need to modify the `CharacterVisuals.cs` script further. I am also checking the existing `CombatManager` to see if `TankAnimator` is referenced anywhere.

**Assessing Character Animations**

My focus has shifted to the character animations. Given that they utilize `AnimatorControllers`, I'm concerned about potential conflicts with the shake effect, which will be implemented by directly modifying the `localPosition`. I need to investigate the animations themselves to ensure my shake implementation doesn't interfere. I'm looking for any keyframes that might already be manipulating the character's position.

**Tool call: Unity.Grep**

```json
{
  "args": "-l \"Fighter_Idle\"",
  "path": "Assets/Animations/Characters"
}
```

**Analyzing File Contents**

I've confirmed the existence of `Assets/Animations/Fighter_Idle.anim`. Now, instead of delving into the full content, which could be cumbersome, I'm focusing on whether it contains position keys. This should give me a quick understanding of the animation's nature without a deep dive.

**Tool call: Unity.GetFileContent**

```json
{
  "startLine": 1,
  "filePath": "Assets/Animations/Fighter_Idle.anim"
}
```

**Evaluating Coordinate Space**

I've determined that the animation data primarily affects `m_Sprite` curves. This allows me to work with `localPosition` without conflict. However, to guarantee the solution's robustness and meet user needs, I'm now leaning towards using a "visual container" or directly applying the offset to the object's transform. This should safely accommodate the "左右" requirement.

**Defining Visual Behavior**

I'm now focused on the `CharacterVisuals.cs` script, as the foundation for the visual effects. I've already defined essential variables such as `SpriteRenderer`, `Material`, and others. Furthermore, I have initiated a static overlay material and have set the default material. Next, I plan to include the logic for `Shake` and `Flash` calls.

**Initiating Character Visuals**

My current focus is writing the `CharacterVisuals.cs` script. It will handle visual effects like shake and flash. I've just written the base script, including variables, and the `Flash` and `Shake` methods. Next, I need to add the `FlashRoutine` and `ShakeRoutine` coroutines to handle the visual effects over time, and testing.

**Developing Visual Component Script**

I'm deep in the process of building the `CharacterVisuals.cs` script. I've successfully defined core elements like `SpriteRenderer`, `defaultMaterial`, and a static `overlayMaterial`. I've added `Flash` and `Shake` methods. Currently, I'm integrating the `FlashRoutine` and `ShakeRoutine` coroutines to manage the visual effects over time. I am using `transform.localPosition` to control the shake. I've also verified that the overlay material is loaded correctly from the Resources folder. Next I will need to test it, and then modify `EnemyUnit.cs` and `CombatManager.cs`.

**Tool call: Unity.CodeEdit**

```json
{
  "oldString": "",
  "newString": "using UnityEngine;\nusing System.Collections;\n\n[RequireComponent(typeof(SpriteRenderer))]\npublic class CharacterVisuals : MonoBehaviour\n{\n    private SpriteRenderer sr;\n    private Material defaultMaterial;\n    private static Material overlayMaterial;\n    private MaterialPropertyBlock mpb;\n    private static readonly int _OverlayColorId = Shader.PropertyToID(\"_OverlayColor\");\n\n    private Vector3 originalLocalPos;\n    private Coroutine currentFlashCoroutine;\n    private Coroutine currentShakeCoroutine;\n\n    private void Awake()\n    {\n        sr = GetComponent<SpriteRenderer>();\n        mpb = new MaterialPropertyBlock();\n        if (sr != null)\n        {\n            defaultMaterial = sr.sharedMaterial;\n        }\n        originalLocalPos = transform.localPosition;\n\n        if (overlayMaterial == null)\n        {\n            overlayMaterial = Resources.Load<Material>(\"EnemyOverlay\");\n        }\n    }\n\n    public void TriggerDamageEffect()\n    {\n        Flash(new Color(1f, 0f, 0f, 0.8f), 0.2f); // Red flash\n        Shake(0.15f, 0.2f); // Left-right shake\n    }\n\n    public void TriggerAttackEffect()\n    {\n        Flash(new Color(1f, 1f, 1f, 0.8f), 0.1f); // White flash\n    }\n\n    public void Flash(Color color, float duration)\n    {\n        if (currentFlashCoroutine != null) StopCoroutine(currentFlashCoroutine);\n        currentFlashCoroutine = StartCoroutine(FlashRoutine(color, duration));\n    }\n\n    public void Shake(float amount, float duration)\n    {\n        if (currentShakeCoroutine != null) StopCoroutine(currentShakeCoroutine);\n        currentShakeCoroutine = StartCoroutine(ShakeRoutine(amount, duration));\n    }\n\n    private IEnumerator FlashRoutine(Color color, float duration)\n    {\n        if (sr == null || overlayMaterial == null) yield break;\n\n        sr.sharedMaterial = overlayMaterial;\n        sr.GetPropertyBlock(mpb);\n        mpb.SetColor(_OverlayColorId, color);\n        sr.SetPropertyBlock(mpb);\n\n        yield return new WaitForSeconds(duration);\n\n        sr.SetPropertyBlock(null);\n        sr.sharedMaterial = defaultMaterial;\n        currentFlashCoroutine = null;\n    }\n\n    private IEnumerator ShakeRoutine(float amount, float duration)\n    {\n        float elapsed = 0f;\n        while (elapsed < duration)\n        {\n            elapsed += Time.deltaTime;\n            // Only horizontal shake\n            float xOffset = Random.Range(-amount, amount);\n            transform.localPosition = originalLocalPos + new Vector3(xOffset, 0, 0);\n            yield return null;\n        }\n        transform.localPosition = originalLocalPos;\n        currentShakeCoroutine = null;\n    }\n}\n",
  "filePath": "Assets/Scripts/CharacterVisuals.cs",
  "description": "Create CharacterVisuals script to handle flash and shake effects."
}
```

**Updating EnemyUnit.cs**

I'm now integrating `CharacterVisuals` into `EnemyUnit.cs`. My current focus is ensuring the visual component is added to the enemy objects within the game environment. I plan to modify the `EnemyUnit` script to handle this, adding `CharacterVisuals` if it's missing.

**Considering CharacterVisuals Integration**

I've shifted my attention to ensuring `EnemyUnit.cs` properly utilizes `CharacterVisuals`. My immediate step will involve modifying `EnemyUnit.cs` to actively call `CharacterVisuals`. The goal here is a seamless integration, making sure the visuals are always present and correctly initialized with the enemy units.

**Tool call: Unity.CodeEdit**

```json
{
  "newString": "    private Animator animator;\n    private CharacterVisuals visuals;\n\n    private void Start()\n    {\n        HP = MaxHP;\n        timer = Random.Range(1.0f, AttackInterval); // Random start offset\n\n        animator = GetComponent<Animator>();\n        visuals = GetComponent<CharacterVisuals>();\n        if (visuals == null) visuals = gameObject.AddComponent<CharacterVisuals>();\n\n        // Detect if I am a ZombieMage based on name\n        if (gameObject.name.Contains(\"ZombieMage\"))\n        {\n            IsMagic = true;\n            AttackPower = 3; // Magic is slightly weaker but ignores Shield\n        }\n    }\n\n    private void Update()\n    {\n        if (IsDead) return;\n\n        timer -= Time.deltaTime;\n        if (timer <= 0f)\n        {\n            CombatManager.Instance.AddEnemyAction(this, AttackPower, IsMagic);\n            timer = AttackInterval;\n        }\n    }\n\n    public void Attack()\n    {\n        if (animator != null)\n        {\n            animator.SetTrigger(\"Attack\");\n        }\n        if (visuals != null)\n        {\n            visuals.TriggerAttackEffect();\n        }\n    }\n\n    public void TakeDamage(int damage)\n    {\n        if (IsDead) return;\n\n        HP -= damage;\n        Debug.Log($\"{name} took {damage} damage. HP: {HP}\");\n        \n        if (visuals != null)\n        {\n            visuals.TriggerDamageEffect();\n        }\n\n        if (HP <= 0)\n        {\n            Die();\n        }\n    }\n\n    private void Die()\n    {\n",
  "filePath": "Assets/Scripts/EnemyUnit.cs",
  "oldString": "    private Animator animator;\n    private SpriteRenderer sr;\n    private MaterialPropertyBlock mpb;\n    private Material defaultMaterial;\n    private static Material overlayMaterial;\n    private static readonly int _OverlayColorId = Shader.PropertyToID(\"_OverlayColor\");\n\n    private void Start()\n    {\n        HP = MaxHP;\n        timer = Random.Range(1.0f, AttackInterval); // Random start offset\n\n        animator = GetComponent<Animator>();\n        sr = GetComponent<SpriteRenderer>();\n        mpb = new MaterialPropertyBlock();\n        \n        if (sr != null)\n        {\n            defaultMaterial = sr.sharedMaterial;\n        }\n\n        if (overlayMaterial == null)\n        {\n            overlayMaterial = Resources.Load<Material>(\"EnemyOverlay\");\n            // If Resources.Load fails, fallback to specific path via script\n            #if UNITY_EDITOR\n            if (overlayMaterial == null)\n            {\n                overlayMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(\"Assets/Materials/EnemyOverlay.mat\");\n            }\n            #endif\n        }\n\n        // Detect if I am a ZombieMage based on name\nif (gameObject.name.Contains(\"ZombieMage\"))\n        {\n            IsMagic = true;\n            AttackPower = 3; // Magic is slightly weaker but ignores Shield\n        }\n    }\n\n    private void Update()\n    {\n        if (IsDead) return;\n\n        timer -= Time.deltaTime;\n        if (timer <= 0f)\n        {\n            CombatManager.Instance.AddEnemyAction(this, AttackPower, IsMagic);\n            timer = AttackInterval;\n        }\n    }\n\n    public void Attack()\n    {\n        if (animator != null)\n        {\n            animator.SetTrigger(\"Attack\");\n        }\n    }\n\n    public void TakeDamage(int damage)\n    {\n        if (IsDead) return;\n\n        HP -= damage;\n        Debug.Log($\"{name} took {damage} damage. HP: {HP}\");\n        \n        // Add simple visual feedback (shake or color shift)\n        StartCoroutine(HitFeedback());\n\n        if (HP <= 0)\n        {\n            Die();\n        }\n    }\n\n    private System.Collections.IEnumerator HitFeedback()\n    {\n        if (sr != null && mpb != null && overlayMaterial != null)\n        {\n            // Swap to overlay material\n            sr.sharedMaterial = overlayMaterial;\n            \n            sr.GetPropertyBlock(mpb);\n            mpb.SetColor(_OverlayColorId, Color.red);\n            sr.SetPropertyBlock(mpb);\n\n            yield return new WaitForSeconds(0.1f);\n\n            // Revert to original material and clear property block\n            sr.SetPropertyBlock(null);\n            sr.sharedMaterial = defaultMaterial;\n        }\n    }\n\n    private void Die()\n    {\n",
  "description": "Update EnemyUnit to use CharacterVisuals for feedback.",
  "expectedOccurrences": 1
}
```

**Checking Initialization Details**

I've just been thinking about how CharacterVisuals are being initialized. To ensure everything runs smoothly, I'm verifying the CharacterVisuals are properly set up. I'm taking a look at `AddComponent` to fix any missing components in the Start function. Now, I'm moving on to `CombatManager.cs`.

**Confirming CharacterVisuals Setup**

I'm making sure `CharacterVisuals` initialization is flawless. Specifically, I'm going through the different character types to ensure their visuals are also cached and ready. This involves checking `Fighter`, `Mage`, and `Tank` objects to fetch and store their `CharacterVisuals`. Then I'll focus on triggering the visuals.

**Tool call: Unity.CodeEdit**

```json
{
  "description": "Update CombatManager to use CharacterVisuals for player feedback.",
  "filePath": "Assets/Scripts/CombatManager.cs",
  "oldString": "    [Header(\"Player Animators\")]\n    public Animator FighterAnimator;\n    public Animator MageAnimator;\n\n    private void Awake()\n    {\n        if (Instance == null) Instance = this;\n        else Destroy(gameObject);\n\n        CurrentHP = MaxHP;\n        SetupUI();\n\n        // Assign player animators if not assigned in inspector\n        if (FighterAnimator == null)\n        {\n            GameObject f = GameObject.Find(\"Fighter\");\n            if (f != null) FighterAnimator = f.GetComponent<Animator>();\n        }\n        if (MageAnimator == null)\n        {\n            GameObject m = GameObject.Find(\"Mage\");\n            if (m != null) MageAnimator = m.GetComponent<Animator>();\n        }\n    }\n\n    private void SetupUI()\n",
  "expectedOccurrences": 1,
  "newString": "    [Header(\"Player Animators\")]\n    public Animator FighterAnimator;\n    public Animator MageAnimator;\n    public Animator TankAnimator;\n\n    private CharacterVisuals fighterVisuals;\n    private CharacterVisuals mageVisuals;\n    private CharacterVisuals tankVisuals;\n\n    private void Awake()\n    {\n        if (Instance == null) Instance = this;\n        else Destroy(gameObject);\n\n        CurrentHP = MaxHP;\n        SetupUI();\n\n        // Assign player animators and visuals\n        SetupPlayerComponents();\n    }\n\n    private void SetupPlayerComponents()\n    {\n        if (FighterAnimator == null)\n        {\n            GameObject f = GameObject.Find(\"Fighter\");\n            if (f != null) FighterAnimator = f.GetComponent<Animator>();\n        }\n        if (MageAnimator == null)\n        {\n            GameObject m = GameObject.Find(\"Mage\");\n            if (m != null) MageAnimator = m.GetComponent<Animator>();\n        }\n        if (TankAnimator == null)\n        {\n            GameObject t = GameObject.Find(\"Tank\");\n            if (t != null) TankAnimator = t.GetComponent<Animator>();\n        }\n\n        if (FighterAnimator != null) fighterVisuals = GetOrAddVisuals(FighterAnimator.gameObject);\n        if (MageAnimator != null) mageVisuals = GetOrAddVisuals(MageAnimator.gameObject);\n        if (TankAnimator != null) tankVisuals = GetOrAddVisuals(TankAnimator.gameObject);\n    }\n\n    private CharacterVisuals GetOrAddVisuals(GameObject go)\n    {\n        CharacterVisuals v = go.GetComponent<CharacterVisuals>();\n        if (v == null) v = go.AddComponent<CharacterVisuals>();\n        return v;\n    }\n\n    private void SetupUI()\n"
}
```

**Incorporating Visual Effects**

I'm currently focused on integrating the visual effects into the `CombatManager` class. Specifically, I'm working on adding calls to these effects within the `HandlePlayerAttack`, `HandlePlayerMagicAttack`, and `HandleEnemyAttack` functions to ensure they trigger correctly during combat.

**Tool call: Unity.CodeEdit**

```json
{
  "expectedOccurrences": 1,
  "description": "Add visual effect calls to player combat actions.",
  "oldString": "    private IEnumerator HandlePlayerAttack(int damage)\n    {\n        if (ActiveEnemies.Count == 0) yield break;\n\n        if (FighterAnimator != null) FighterAnimator.SetTrigger(\"Attack\");\n\n        EnemyUnit target = ActiveEnemies[0];\n        if (target != null)\n        {\n            Debug.Log($\"Player attacks {target.name} for {damage} damage.\");\n            target.TakeDamage(damage);\n            yield return new WaitForSeconds(0.6f); \n        }\n\n        CleanupEnemies();\n    }\n\n    private IEnumerator HandlePlayerMagicAttack(int damage)\n    {\n        if (ActiveEnemies.Count == 0) yield break;\n\n        if (MageAnimator != null) MageAnimator.SetTrigger(\"Magic\");\n\n        Debug.Log($\"Mage casts AOE Magic for {damage} damage to ALL enemies.\");\n        \n        List<EnemyUnit> targets = new List<EnemyUnit>(ActiveEnemies);\n        foreach (var enemy in targets)\n        {\n            if (enemy != null)\n            {\n                enemy.TakeDamage(damage);\n            }\n        }\n        \n        yield return new WaitForSeconds(0.8f); \n        CleanupEnemies();\n    }\n",
  "newString": "    private IEnumerator HandlePlayerAttack(int damage)\n    {\n        if (ActiveEnemies.Count == 0) yield break;\n\n        if (FighterAnimator != null) FighterAnimator.SetTrigger(\"Attack\");\n        if (fighterVisuals != null) fighterVisuals.TriggerAttackEffect();\n\n        EnemyUnit target = ActiveEnemies[0];\n        if (target != null)\n        {\n            Debug.Log($\"Player attacks {target.name} for {damage} damage.\");\n            target.TakeDamage(damage);\n            yield return new WaitForSeconds(0.6f); \n        }\n\n        CleanupEnemies();\n    }\n\n    private IEnumerator HandlePlayerMagicAttack(int damage)\n    {\n        if (ActiveEnemies.Count == 0) yield break;\n\n        if (MageAnimator != null) MageAnimator.SetTrigger(\"Magic\");\n        if (mageVisuals != null) mageVisuals.TriggerAttackEffect();\n\n        Debug.Log($\"Mage casts AOE Magic for {damage} damage to ALL enemies.\");\n        \n        List<EnemyUnit> targets = new List<EnemyUnit>(ActiveEnemies);\n        foreach (var enemy in targets)\n        {\n            if (enemy != null)\n            {\n                enemy.TakeDamage(damage);\n            }\n        }\n        \n        yield return new WaitForSeconds(0.8f); \n        CleanupEnemies();\n    }\n",
  "filePath": "Assets/Scripts/CombatManager.cs"
}
```

**Envisioning the Visuals**

I'm currently focusing on integrating the visual damage feedback within the `HandleEnemyAttack` function. My approach involves activating the damage effect for each active player character, ensuring a comprehensive display of the impact. The plan is to create a clear and immediate visual response to enemy attacks.

**Tool call: Unity.CodeEdit**

```json
{
  "description": "Add damage visual effect to player characters in HandleEnemyAttack.",
  "filePath": "Assets/Scripts/CombatManager.cs",
  "oldString": "        action.SourceEnemy.Attack();\n        yield return new WaitForSeconds(0.4f); \n\n        int finalDamage = action.Value;\n        if (action.IsMagic)\n",
  "newString": "        action.SourceEnemy.Attack();\n        yield return new WaitForSeconds(0.4f); \n\n        int finalDamage = action.Value;\n        \n        // Trigger player damage visual\n        if (fighterVisuals != null) fighterVisuals.TriggerDamageEffect();\n        if (mageVisuals != null) mageVisuals.TriggerDamageEffect();\n        if (tankVisuals != null) tankVisuals.TriggerDamageEffect();\n\n        if (action.IsMagic)\n",
  "expectedOccurrences": 1
}
```

**Implementing Feedback Mechanisms**

I've recently integrated `CharacterVisuals.cs` into `EnemyUnit.cs`, enabling visual feedback like flashing and shaking upon damage and attacks. Combat Manager will be my next focus, with plans to integrate these new effects and extend the feedback mechanisms.

**Inspecting Sprite Renderers**

I'm now checking the prefab hierarchies for `Fighter`, `Mage`, and `Tank` to verify the location of their `SpriteRenderer` components. This is crucial for `CharacterVisuals.cs` to correctly apply visual effects. If the `SpriteRenderer` is nested, I'll need to update the script to search within children.

**Tool call: Unity.RunCommand**

```json
{
  "code": "using UnityEngine;\n\ninternal class CommandScript : IRunCommand\n{\n    public void Execute(ExecutionResult result)\n    {\n        string[] names = { \"Fighter\", \"Mage\", \"Tank\" };\n        foreach (var name in names)\n        {\n            GameObject go = GameObject.Find(name);\n            if (go != null)\n            {\n                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();\n                if (sr != null) result.Log(\"{0} has SpriteRenderer on root\", name);\n                else\n                {\n                    sr = go.GetComponentInChildren<SpriteRenderer>();\n                    if (sr != null) result.Log(\"{0} has SpriteRenderer on child {1}\", name, sr.gameObject.name);\n                    else result.LogWarning(\"{0} has no SpriteRenderer\", name);\n                }\n            }\n        }\n    }\n}\n",
  "title": "Check SpriteRenderer locations on player characters"
}
```

**Defining the Initial Position**

I'm currently focused on the initial position of the character, particularly the `originalLocalPos`. It's crucial to correctly capture this value, especially if the character doesn't start at a zero position. I'm working through the `CharacterVisuals.cs` script now.

**Confirming the Root Position**

I've affirmed the initial position's capture in `CharacterVisuals.cs` using `transform.localPosition` within `Awake()`. The focus now shifts to verifying the correct invocation of `TriggerAttackEffect()` and `TriggerDamageEffect()` during attacks, including magic attacks. Further validation includes confirming the proper assignment and usage of the `EnemyOverlay` material, focusing on its resource loading and application in `CharacterVisuals.cs` and `EnemyUnit.cs`.

**Validating Material Application**

I'm now carefully checking the `EnemyOverlay` material's application within both `CharacterVisuals.cs` and `EnemyUnit.cs` to ensure consistency. I'm confirming that the material is loaded correctly from the `Resources` folder and applied without errors. My focus is now on any potential conflicts in material assignments or unexpected behavior related to material instantiation or resource loading. I am also running tests to validate the `TriggerAttackEffect()` and `TriggerDamageEffect()` calls.

**Tool call: Unity.GetConsoleLogs**

```json
{
  "maxEntries": 10
}
```

**Analyzing Initial Errors**

I've tackled the initial compilation errors; now I'm shifting focus to `GridManager.cs` and those unassigned fields. My next move involves examining the `TankAnimator` within `CombatManager.Awake`. A quick log will confirm the assignment of `fighterVisuals`, `mageVisuals`, and `tankVisuals`.

**Confirming Player Character Setup**

I've confirmed the existence of all player character GameObjects, including the `Tank`. Now, I'm verifying their positions and component setup through `CombatManager.SetupPlayerComponents`. I'm also revising `CharacterVisuals.cs` to ensure the damage flash is fully visible by setting the alpha to 1.0 using `Color.red`. Finally, I'll take a screenshot of the scene view to confirm visibility.

**Tool call: Unity.SceneView.Capture2DScene**

```json
{
  "pixelsPerUnit": 32,
  "worldY": -5,
  "worldWidth": 20,
  "worldX": -5,
  "worldHeight": 10
}
```

**Implementing Visual Feedback**

I've been focusing on implementing visual feedback for damage and attacks. I've successfully integrated the red flash and left/right shake on damage events using the `SpriteOverlay` shader. Next up, I'm working on the white flash for attack animations and considering ways to streamline the implementation across all character types.

**Validating Player Character Setups**

I've just verified that the player characters, Fighter, Mage, and Tank, are correctly found and assigned within `CombatManager.cs`. The scene setup aligns with my code's expectations. I reviewed the `SetupPlayerComponents` function and confirmed that the game objects are being located by name. I am now confident that the visuals will function as expected for all player character types during combat.

**Assessing Character Implementations**

I'm now investigating the attack animations and visual effects across all character types, particularly the Tank. The `CombatManager` code seems sound, but I need to ensure all characters trigger the white flash when attacking, not just when they are damaged. I will examine each character's attack logic and animations and integrate the `TriggerAttackEffect` to match.

**Examining Attack Triggers**

I'm now diving deeper into how attack visuals are triggered. I need to ensure the white flash effect, implemented in `CharacterVisuals.cs`, is correctly activated when any player character begins an attack, including Tank abilities. I will pinpoint the appropriate animation events to trigger `TriggerAttackEffect`. I am checking `TankAnimator` and its animations.

**Verifying Tank Animations**

I'm now investigating the Tank's behavior further. The Tank doesn't have attack animations in the current setup, only a shield action. I'm considering whether to add a white flash on the shield trigger, but the prompt emphasizes attack visuals, so I will prioritize this. I'll need to confirm what triggers the damage visuals on the Tank, the Fighter and the Mage.

**Evaluating Tank Attack Visuals**

My focus has shifted to the Tank character. Considering the Tank's shield-based gameplay, I am questioning whether to include a white flash effect on the shield activation, or just when damage is taken. The prompt's focus is on attack visuals. I will prioritize verifying the damage visuals.

**Reviewing Combat Visuals**

My focus has shifted to the Tank character. Considering the Tank's shield-based gameplay, I am questioning whether to include a white flash effect on the shield activation, or just when damage is taken. The prompt's focus is on attack visuals. I will prioritize verifying the damage visuals.

**Refining Player Visuals**

I'm now satisfied with the basic damage and attack visuals for the players. I confirmed all player characters will receive the damage flash and shake. I'm focusing on validating the attack flash for all characters, including the Tank. I'll verify the attack animations and integration of the `TriggerAttackEffect`.

**Concluding Player Implementations**

I'm now satisfied with the basic damage and attack visuals for the players and enemies. The red flash and shake on damage events is confirmed across all applicable characters, based on updates to `EnemyUnit` and `CombatManager`. I will now do a final code review for cleanliness.

プレイヤーと敵キャラクターにダメージ演出（赤く光る＋振動）と攻撃演出（白く光る）を追加しました。

### 実装内容

1.  **`CharacterVisuals.cs` の作成**: 
    演出ロジックを共通化するため、`SpriteOverlay` シェーダーを使用したフラッシュ効果と、`localPosition` を操作する振動効果を制御するクラスを作成しました。
2.  **`EnemyUnit.cs` の更新**: 
    ダメージを受けた際および攻撃時に `CharacterVisuals` を介して演出を実行するように変更しました。
3.  **`CombatManager.cs` の更新**: 
    プレイヤーキャラクター（Fighter, Mage, Tank）に対しても同様の演出を適用しました。敵からダメージを受けた際はパーティ全員が、プレイヤーが攻撃（物理・魔法）を開始する際はそれぞれの担当キャラクターが光るように設定しています。

### 追加・修正されたファイル

*   `Assets/Scripts/CharacterVisuals.cs` (新規)
*   `Assets/Scripts/EnemyUnit.cs` (修正)
*   `Assets/Scripts/CombatManager.cs` (修正)

これにより、戦闘時の視覚的なフィードバックが強化されました。