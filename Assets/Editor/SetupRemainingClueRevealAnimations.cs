using SilentGallery.ClueSystem;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// One-time setup tool that extends the statue reveal animation pattern to
/// the rest of the clue objects. Run via Tools &gt; Silent Gallery &gt;
/// Create Remaining Clue Reveal Animations with Gallery.unity open. Builds
/// one clip/controller pair per item through Unity's Animation API, each
/// rotating that object 90 degrees around Y (from its current rotation)
/// over 1.5 seconds, Loop Time off. Safe to delete after running once.
/// </summary>
public static class SetupRemainingClueRevealAnimations
{
    private struct Target
    {
        public string objectName;
        public string clueId;
        public string clipBaseName;

        public Target(string objectName, string clueId, string clipBaseName)
        {
            this.objectName = objectName;
            this.clueId = clueId;
            this.clipBaseName = clipBaseName;
        }
    }

    private static readonly Target[] Targets =
    {
        new Target("rust_key", "clue_005", "KeyReveal"),
        new Target("pot_2", "clue_06_sun", "SunReveal"),
        new Target("SM_dino_midpoly", "clue_07_moon", "MoonReveal"),
        new Target("Clepsydra", "clue_08_star", "StarReveal"),
    };

    [MenuItem("Tools/Silent Gallery/Create Remaining Clue Reveal Animations")]
    public static void CreateAll()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Animations"))
        {
            AssetDatabase.CreateFolder("Assets", "Animations");
        }

        int succeeded = 0;
        foreach (Target target in Targets)
        {
            if (CreateOne(target))
            {
                succeeded++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Scene activeScene = SceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(activeScene);

        Debug.Log(
            $"Created reveal animations for {succeeded}/{Targets.Length} "
            + "remaining clue items. Save the scene now (Ctrl+S) to keep it.");
    }

    private static bool CreateOne(Target target)
    {
        GameObject targetObject = FindIncludingInactive(target.objectName);
        if (targetObject == null)
        {
            Debug.LogError(
                $"SetupRemainingClueRevealAnimations: couldn't find a "
                + $"GameObject named '{target.objectName}' in the open scene. "
                + "Open Gallery.unity, then run this again.");
            return false;
        }

        float startY = targetObject.transform.eulerAngles.y;
        float endY = startY + 90f;

        AnimationClip clip = new AnimationClip { name = target.clipBaseName };

        AnimationCurve rotationY = new AnimationCurve();
        rotationY.AddKey(new Keyframe(0f, startY));
        rotationY.AddKey(new Keyframe(1.5f, endY));
        clip.SetCurve("", typeof(Transform), "localEulerAnglesRaw.y", rotationY);

        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = false;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        string clipPath = $"Assets/Animations/{target.clipBaseName}.anim";
        AssetDatabase.CreateAsset(clip, clipPath);

        string controllerPath = $"Assets/Animations/{target.clipBaseName}.controller";
        AnimatorController controller =
            AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        controller.AddParameter("Reveal", AnimatorControllerParameterType.Trigger);

        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;
        AnimatorState idleState = rootStateMachine.AddState("Idle", new Vector3(50, 50, 0));
        AnimatorState revealState = rootStateMachine.AddState("Reveal", new Vector3(310, 50, 0));
        revealState.motion = clip;
        rootStateMachine.defaultState = idleState;

        AnimatorStateTransition transition = idleState.AddTransition(revealState);
        transition.hasExitTime = false;
        transition.duration = 0.1f;
        transition.AddCondition(AnimatorConditionMode.If, 0, "Reveal");

        EditorUtility.SetDirty(controller);

        Animator animator = targetObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = targetObject.AddComponent<Animator>();
        }
        animator.runtimeAnimatorController = controller;

        ClueRevealAnimator revealScript = targetObject.GetComponent<ClueRevealAnimator>();
        if (revealScript == null)
        {
            revealScript = targetObject.AddComponent<ClueRevealAnimator>();
        }

        SerializedObject serializedScript = new SerializedObject(revealScript);
        serializedScript.FindProperty("clueId").stringValue = target.clueId;
        serializedScript.ApplyModifiedProperties();

        EditorUtility.SetDirty(targetObject);

        Debug.Log($"Wired {target.clipBaseName} onto '{target.objectName}' ({target.clueId}).");
        return true;
    }

    /// <summary>
    /// GameObject.Find only searches active objects - the key starts
    /// disabled until the combination lock is solved, so this walks every
    /// root object's full hierarchy instead, inactive children included.
    /// </summary>
    private static GameObject FindIncludingInactive(string name)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        foreach (GameObject root in activeScene.GetRootGameObjects())
        {
            GameObject match = FindInChildren(root.transform, name);
            if (match != null)
            {
                return match;
            }
        }
        return null;
    }

    private static GameObject FindInChildren(Transform parent, string name)
    {
        if (parent.name == name)
        {
            return parent.gameObject;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject match = FindInChildren(parent.GetChild(i), name);
            if (match != null)
            {
                return match;
            }
        }

        return null;
    }
}
