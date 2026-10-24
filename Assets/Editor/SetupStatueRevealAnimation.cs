using SilentGallery.ClueSystem;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// One-time setup tool for the statue reveal animation (feature/anim-clue).
/// Run via Tools &gt; Silent Gallery &gt; Create Statue Reveal Animation with
/// Gallery.unity open. Creates the clip and controller through Unity's own
/// Animation API (not hand-authored files), then wires them onto GreekStatue.
/// Safe to delete this script after running it once.
/// </summary>
public static class SetupStatueRevealAnimation
{
    [MenuItem("Tools/Silent Gallery/Create Statue Reveal Animation")]
    public static void Create()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Animations"))
        {
            AssetDatabase.CreateFolder("Assets", "Animations");
        }

        AnimationClip clip = new AnimationClip { name = "StatueReveal" };

        AnimationCurve rotationY = new AnimationCurve();
        rotationY.AddKey(new Keyframe(0f, 180f));
        rotationY.AddKey(new Keyframe(1.5f, 270f));
        clip.SetCurve("", typeof(Transform), "localEulerAnglesRaw.y", rotationY);

        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = false;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        AssetDatabase.CreateAsset(clip, "Assets/Animations/StatueReveal.anim");

        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(
            "Assets/Animations/StatueReveal.controller");
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
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        GameObject statue = GameObject.Find("GreekStatue");
        if (statue == null)
        {
            Debug.LogError(
                "SetupStatueRevealAnimation: couldn't find a GameObject named "
                + "'GreekStatue' in the open scene. Open Gallery.unity, then run "
                + "this again - the clip and controller were still created.");
            return;
        }

        Animator animator = statue.GetComponent<Animator>();
        if (animator == null)
        {
            animator = statue.AddComponent<Animator>();
        }
        animator.runtimeAnimatorController = controller;

        ClueRevealAnimator revealScript = statue.GetComponent<ClueRevealAnimator>();
        if (revealScript == null)
        {
            revealScript = statue.AddComponent<ClueRevealAnimator>();
        }

        SerializedObject serializedScript = new SerializedObject(revealScript);
        serializedScript.FindProperty("clueId").stringValue = "clue_004";
        serializedScript.ApplyModifiedProperties();

        EditorUtility.SetDirty(statue);
        EditorSceneManager.MarkSceneDirty(statue.scene);

        Debug.Log(
            "Statue reveal animation created and wired onto GreekStatue. "
            + "Save the scene now (Ctrl+S) to keep it.");
    }
}
